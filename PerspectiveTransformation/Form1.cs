using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
namespace PerspectiveTransformation
{
    public partial class 透视变换器 : Form
    {
        private Mat dst = new Mat();
        private string _name;
        private Mat src;
        private Bitmap _nowImage;
        private Bitmap _sourceImage;
        private Bitmap _backImage;
        private  int Margins = 40;
        private MyPoint _fromPoint;
        private MyPoint _toPoint;
        private List<MyPoint> _myPoints = new List<MyPoint>();
        private List<MyPoint> _storedPoints = new List<MyPoint>();
        private Point _mouseFrom, _mouseTo;
        public bool Drawcord = false;
        public int ShowScale = 1;
        public int OffsetX, OffsetY;
        private List<Point2f> _tranPoints = new List<Point2f>();
        private List<Point> _points = new List<Point>();
        private int? _changeNumber;

        public 透视变换器()
        {
            InitializeComponent();
            UpdateSize();
            //使窗体大小初始全屏化
            KeyPreview = true;
        //    StartPosition = FormStartPosition.Manual;
            Rectangle rect = Screen.GetWorkingArea(this);
            this.Width = rect.Width;
            this.Height = rect.Height;
        }
        private void BOpen_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Jpg 图片|*.jpg|Bmp 图片|*.bmp|Gif 图片|*.gif|Png 图片|*.png|Wmf  图片|*.wmf",
                FilterIndex = 0
            };
            if (dialog.ShowDialog() != DialogResult.OK) return;
            var name = dialog.SafeFileName;
            _name = Roll(name);
            _name = _name.Substring(name.Length - name.LastIndexOf("."));
            _name = Roll(_name);
            _sourceImage = (Bitmap)Image.FromFile(dialog.FileName); _fromPoint = null;
            _toPoint = null;
            UpdateSize();
            UpdateMap();
        }
        private void BtTran_Click(object sender, EventArgs e)
        {
            if (_backImage == null) return;
            tranRadioButton.Enabled = true;
            drawRadioButton.Enabled = true;
        }
        private void BClear_Click(object sender, EventArgs e)
        {
            _fromPoint = null;
            _toPoint = null;
            _tranPoints = null;
            _storedPoints = null;

            tranRadioButton.Checked = false;
            drawRadioButton.Checked = true;
            tranRadioButton.Enabled = false;
            drawRadioButton.Enabled = false;
            UpdateMap();
        }

        private void BSave_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Jpg 图片|*.jpg|Bmp 图片|*.bmp|Gif 图片|*.gif|Png 图片|*.png|Wmf  图片|*.wmf",
                FileName = _name + "（副本）",
                FilterIndex = 0,
                CheckFileExists = false,
                RestoreDirectory = true,
                AddExtension = true
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;
            Image saveImage;
            if (_nowImage == null)
            {
                saveImage = _sourceImage;
            }
            else
            {
                saveImage = _nowImage;
            }
            saveImage.Save(dialog.FileName);
            MessageBox.Show(this, "图片保存成功！", "信息提示");
        }
        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            LocationLabel.Text = e.Location.ToString();
            if (e.Button != MouseButtons.Left) return;
            _mouseTo = e.Location;
            PanelOrigin.Invalidate();
        }
        private static string Roll(string inputString)
        {
            string returnString = "";
            int n = inputString.Length;
            for (int i = 0; i < n; i++)
            {
                returnString += inputString[n - i - 1];
            }

            return returnString;
        }
        

        private void UpdateMap()
        {
            _backImage?.Dispose();
            if (_sourceImage == null) return;
            _backImage = new Bitmap(PanelOrigin.Width, PanelOrigin.Height);
            var g = Graphics.FromImage(_backImage);
            g.DrawImage(_sourceImage, new Rectangle((1 - ShowScale) * PanelOrigin.Width / 50 + OffsetX, (1 - ShowScale) * PanelOrigin.Height / 50 + OffsetY,
                (ShowScale - 1) * 2 * PanelOrigin.Width / 50 + PanelOrigin.Width, (ShowScale - 1) * 2 * PanelOrigin.Height / 50 + PanelOrigin.Height));
            if (_fromPoint != null)
            {
                if(_fromPoint.X != _toPoint.X && _fromPoint.Y != _toPoint.Y) { 
                var startpoint = ToScreenPoint(_fromPoint, ShowScale, OffsetX, OffsetY);
                var endpoint = ToScreenPoint(_toPoint, ShowScale, OffsetX, OffsetY);
                DrawAll(startpoint, endpoint, g);
                }
            }
            g.Dispose();
            PanelOrigin.Invalidate();
        }
        private void UpdateSize()
        {
            PanelControls.Width = 80;
            var panelWidth = (ClientRectangle.Width - PanelControls.Width) / 2 - Margins * 2;
            var panelHeight = ClientRectangle.Height - statusStrip1.Height - Margins * 2;

            if (_sourceImage != null)
            {
                var forBitmap = _nowImage ?? _sourceImage;
                var imageHeight = forBitmap.Height * panelWidth / forBitmap.Width;

                switch (imageHeight >= panelHeight)
                {
                    case true:
                        panelWidth = forBitmap.Width * panelHeight / forBitmap.Height;
                        Margins = 20;
                        break;
                    default:
                        panelHeight = imageHeight;
                        break;
                }
            }

            PanelOrigin.Width = PanelShow.Width = panelWidth;
            PanelOrigin.Height = PanelShow.Height = panelHeight;
            PanelShow.Top = PanelOrigin.Top = (ClientRectangle.Height - panelHeight) / 2;

            PanelOrigin.Left = PanelControls.Width + Margins;
            PanelShow.Left = PanelOrigin.Left + Margins * 2 + panelWidth;


        }
        private Point ToScreenPoint(MyPoint onePoint, int scale, int offsetX, int offsetY)
        {
            var X = (int)(onePoint.X * ((scale - 1) * 2 * PanelOrigin.Width / 50 + PanelOrigin.Width)) +
                    offsetX + (1 - scale) * PanelOrigin.Width / 50;
            var Y = (int)(onePoint.Y * ((scale - 1) * 2 * PanelOrigin.Height / 50 + PanelOrigin.Height)) +
                    offsetY + (1 - scale) * PanelOrigin.Height / 50;
            return new Point(X, Y);
        }
        public MyPoint ToTruePoint(Point onePoint, int scale, int offsetX, int offsetY)
        {
            var X = (float)(onePoint.X - offsetX - (1 - scale) * PanelOrigin.Width / 50) /
                    (float)((scale - 1) * 2 * PanelOrigin.Width / 50 + PanelOrigin.Width);
            var Y = (float)(onePoint.Y - offsetY - (1 - scale) * PanelOrigin.Height / 50) /
                    (float)((scale - 1) * 2 * PanelOrigin.Height / 50 + PanelOrigin.Height);
            return new MyPoint(X, Y);
        }

        //绘制变形框
        public void DrawAll(Point startPoint, Point endPoint, Graphics g)
        {
            if (drawRadioButton.Checked == true && drawRadioButton.Enabled == true)
            {
                DrawBank(startPoint, endPoint, g);
            }
            if (tranRadioButton.Checked == true && tranRadioButton.Enabled == true)
            {
                
                TranBank(startPoint, endPoint, g);
                
            }
        }
        private void DrawPoints(List<MyPoint> myPoints, Graphics g)
        {
            var points = new Point[4];
            foreach (var point in myPoints)
            {
                Rectangle rect = new Rectangle((int)(point.X - 5), (int)(point.Y - 5), 10, 10);
                g.DrawEllipse(new Pen(Color.SteelBlue), rect);
                g.FillEllipse(new SolidBrush(Color.SteelBlue), rect);
                var i = myPoints.IndexOf(point);
                points[i] = new Point
                {
                    X = (int)(point.X),
                    Y = (int)(point.Y)
                };
            }
            g.DrawPolygon(new Pen(Color.SteelBlue, 2), points);
            g.DrawLine(new Pen(Color.SteelBlue), new Point(Convert.ToInt32(points[0].X / 3 + points[3].X * 2 / 3), Convert.ToInt32(points[0].Y * 2 / 3 + points[3].Y / 3)),
                new Point(Convert.ToInt32(points[1].X / 3 + points[2].X * 2 / 3), Convert.ToInt32(points[1].Y * 2 / 3 + points[2].Y / 3)));
            g.DrawLine(new Pen(Color.SteelBlue), new Point(Convert.ToInt32(points[0].X * 2 / 3 + points[3].X / 3), Convert.ToInt32(points[0].Y / 3 + points[3].Y * 2 / 3)),
                new Point(Convert.ToInt32(points[1].X * 2 / 3 + points[2].X / 3), Convert.ToInt32(points[1].Y / 3 + points[2].Y * 2 / 3)));
            g.DrawLine(new Pen(Color.SteelBlue), new Point(Convert.ToInt32(points[0].X * 2 / 3 + points[1].X / 3), Convert.ToInt32(points[0].Y / 3 + points[1].Y * 2 / 3)),
                new Point(Convert.ToInt32(points[3].X * 2 / 3 + points[2].X / 3), Convert.ToInt32(points[3].Y / 3 + points[2].Y * 2 / 3)));
            g.DrawLine(new Pen(Color.SteelBlue), new Point(Convert.ToInt32(points[0].X / 3 + points[1].X * 2 / 3), Convert.ToInt32(points[0].Y * 2 / 3 + points[1].Y / 3)),
                new Point(Convert.ToInt32(points[3].X / 3 + points[2].X * 2 / 3), Convert.ToInt32(points[3].Y * 2 / 3 + points[2].Y / 3)));
            
        }
        private void DrawBank(Point startPoint, Point endPoint, Graphics g)
        {
            List<MyPoint> mypoints = new List<MyPoint>();
            var width = Math.Abs(startPoint.X - endPoint.X);
            var height = Math.Abs(startPoint.Y - endPoint.Y);
            var left = Math.Min(startPoint.X, endPoint.X);
            var top = Math.Min(startPoint.Y, endPoint.Y);
            
            mypoints.Add(new MyPoint(left, top));
            mypoints.Add(new MyPoint(left, top + height));
            mypoints.Add(new MyPoint(left + width, top + height));
            mypoints.Add(new MyPoint(left + width, top));
            //绘制边角的四个顶点
            DrawPoints(mypoints,g);
            _myPoints = mypoints;
        }

        private void TranBank(Point startPoint, Point endPoint, Graphics g)
        {
            foreach (MyPoint myPoint in _myPoints)
            {
                _tranPoints.Add(new Point2f(myPoint.X, myPoint.Y));
            }
            List<MyPoint> _mypoints = _myPoints;
            List<Point2f> nowPoints = _tranPoints;
            
            //绘制边角的四个顶点
            Point[] points = new Point[4];
            for (int i = 0; i < _mypoints.Count; i++)
            {
                points[i] = new Point
                {
                    X = (int)(_mypoints[i].X),
                    Y = (int)(_mypoints[i].Y)
                };
                var distance = Math.Pow((_mypoints[i].X - startPoint.X), 2) 
                               + Math.Pow((_mypoints[i].Y - startPoint.Y), 2);
                if (distance <= 25)
                {
                    points[i] = endPoint;
                    _changeNumber = i;
                    nowPoints[i] = new Point2f(
                        endPoint.X, 
                        endPoint.Y
                        );
                    _mypoints[i] = new MyPoint(
                        startPoint.X,
                        startPoint.Y
                        );
                }
                
            }
            DrawPoints(_mypoints,g);
            _myPoints = _mypoints;
            Point2f[] warpFromPoints = { _tranPoints[0], _tranPoints[1], _tranPoints[2], _tranPoints[3] };
            Point2f[] warpToPoints = { nowPoints[0], nowPoints[1], nowPoints[2], nowPoints[3] };
       //     src = OpenCvSharp.Extensions.BitmapConverter.ToMat(_sourceImage);
         //   dst = src;
      //      Mat Trans = Cv2.GetAffineTransform(warpFromPoints, warpToPoints);
      //      Cv2.WarpPerspective(src,dst,Trans,src.Size());
        }

        private void PanelOrigin_Paint(object sender, PaintEventArgs e)
        {
            if (_backImage != null)
            {
                e.Graphics.DrawImage(_backImage, 0, 0);
                if (Drawcord)
                {
                    DrawAll(_mouseFrom, _mouseTo, e.Graphics);
                }
            }
            Invalidate();
        }

        private void PanelOrigin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (_fromPoint != null)
            {
                _fromPoint = null;
               // UpdateMap();
            }
            _mouseFrom = e.Location;
            _fromPoint = new MyPoint(e.X, e.Y);
            Drawcord = true;
        }

        private void PanelOrigin_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _toPoint = new MyPoint(e.X, e.Y);
            Drawcord = false;
            if (tranRadioButton.Checked == true && tranRadioButton.Enabled == true)
            {
                if(_changeNumber != null) _myPoints[_changeNumber.Value] = _toPoint;
            }
            _changeNumber = null;
         //   if(dst!=null)Cv2.ImShow("dst",dst);
        }

        
        private void Form1_Size(object sender, EventArgs e)
        {
            UpdateSize();
            if (_sourceImage != null) UpdateMap();
        }

        
    }
    public sealed class MyPanel : Control
    {
        public MyPanel()
        {
            DoubleBuffered = true;
        }
    }

    public class MyPoint
    {
        public float X;
        public float Y;
        public MyPoint(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Point ConvertToPoint(MyPoint myPoint)
        {
            var returnPoint = new Point((int)myPoint.X, (int)myPoint.Y);
            return returnPoint;
        }
    }
}
