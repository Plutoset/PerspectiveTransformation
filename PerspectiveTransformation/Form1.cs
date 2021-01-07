using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using Point = System.Drawing.Point;

namespace PerspectiveTransformation
{
    public partial class 透视变换器 : Form
    {
        private Mat dst = new Mat();
        private Mat src;
        private Bitmap _sourceImage;
        private Bitmap _backImage;
        private const int Margins = 20;
        private MyPoint _tempPoint;
        private MyPoint _fromPoint;
        private MyPoint _toPoint;
        private List<MyPoint> _myPoints = new List<MyPoint>();
        private List<MyPoint> _storedPoints = new List<MyPoint>();
        private Point _mouseFrom, _mouseTo;
        public bool Drawcord = false;
        public List<Point2f> TranPoints = new List<Point2f>();
        private List<Point> _points = new List<Point>();
        private int? _changeNumber;

        public 透视变换器()
        {
            InitializeComponent();
            UpdateSize();
            //使窗体大小初始全屏化
            KeyPreview = true;
            StartPosition = FormStartPosition.Manual;
            Rectangle rect = Screen.GetWorkingArea(this);
            this.Width = rect.Width;
            this.Height = rect.Height;
        }

        private void BtTran_Click(object sender, EventArgs e)
        {
            if (_backImage == null) return;
            tranRadioButton.Enabled = true;
            drawRadioButton.Enabled = true;
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            LocationLabel.Text = e.Location.ToString();
           
        }

        private void BOpen_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;
            _sourceImage = (Bitmap)Image.FromFile(dialog.FileName);
            src = OpenCvSharp.Extensions.BitmapConverter.ToMat(_sourceImage);
            UpdateSize();
            UpdateMap();
        }

        private void UpdateMap()
        {
            _backImage?.Dispose();
          
        }
        private void UpdateSize()
        {
            var panelWidth = ClientRectangle.Width - Margins * 2; ;
            var panelHeight = ClientRectangle.Height - PanelControls.Height - statusStrip1.Height - Margins * 2;

            if (_sourceImage != null)
            {
                var newHeight = _sourceImage.Height * panelWidth / _sourceImage.Width;
                if (newHeight >= panelHeight)
                {
                    panelWidth = _sourceImage.Width * panelHeight / _sourceImage.Height;
                }
                else
                {
                    panelHeight = newHeight;
                }
            }
            PanelOrigin.Width = panelWidth;
            PanelOrigin.Height = panelHeight;
            PanelOrigin.Left = (ClientRectangle.Width - panelWidth) / 2;
            PanelOrigin.Top = (ClientRectangle.Height - panelHeight) / 2;
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
            switch (drawRadioButton.Checked)
            {
                case true when drawRadioButton.Enabled == true:
                    DrawFrame( MyPoint.ConvertToPoint(_fromPoint), MyPoint.ConvertToPoint(_toPoint), g);
                    break;
            }

            switch (tranRadioButton.Checked)
            {
                case true when tranRadioButton.Enabled == true:
                {
                    if (_storedPoints == null)
                    {
                        MessageBox.Show("请先创建转换框架！");
                    }
                    else
                    {
                        DrawFrame(MyPoint.ConvertToPoint(_fromPoint), MyPoint.ConvertToPoint(_toPoint), g);
                        WarpPerspective(TranPoints);
                    }

                    break;
                }
            }
        }

        private void DrawFrame(Point fromPoint, Point toPoint, Graphics g)
        {
            if (_storedPoints == null)
            {
                DrawBank(fromPoint, toPoint, g);
            }
            else
            {
                List<MyPoint> myPoints = _myPoints;

                for (int i = 0; i < myPoints.Count; i++)
                {
                    var distance = Math.Pow((myPoints[i].X - fromPoint.X), 2) + Math.Pow((myPoints[i].Y - fromPoint.Y), 2);
                    if (distance <= 25)
                    {
                        myPoints[i] = new MyPoint(toPoint.X, toPoint.Y);
                    }
                }
                TranBank(_myPoints, g);
                _myPoints = myPoints;
            }
        }
        //绘制矩形框
        private void DrawBank(Point fromPoint, Point toPoint, Graphics g)
        {
            List<MyPoint> myPoints = new List<MyPoint>();
            var width = Math.Abs(fromPoint.X - toPoint.X);
            var height = Math.Abs(fromPoint.Y - toPoint.Y);
            var left = Math.Min(fromPoint.X, toPoint.X);
            var top = Math.Min(fromPoint.Y, toPoint.Y);
            
            myPoints.Add(new MyPoint(left, top));
            myPoints.Add(new MyPoint(left, top + height));
            myPoints.Add(new MyPoint(left + width, top + height));
            myPoints.Add(new MyPoint(left + width, top));
            //绘制边角的四个顶点
            DrawPoints(myPoints, g);
            _storedPoints = myPoints;
        }
        //绘制框和点
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
            g.Dispose();

        }
        //绘制变形框
        private void TranBank(List<MyPoint> myPoints, Graphics g)
        {
            //     List<MyPoint> myPoints = _storedPoints;
            //绘制边角的四个顶点
            DrawPoints(myPoints, g);

            foreach (MyPoint myPoint in _myPoints)
            {
                TranPoints.Add(new Point2f(myPoint.X, myPoint.Y));
            }

            _storedPoints = myPoints;
            _myPoints = myPoints;
        }

        private void WarpPerspective(List<Point2f> nowPoints)
        {
            Mat Trans = Cv2.GetPerspectiveTransform(TranPoints, nowPoints);
            Trans.ConvertTo(Trans, MatType.CV_32F);
            Cv2.WarpPerspective(src, dst, Trans, new OpenCvSharp.Size() { Height = src.Cols, Width = src.Rows });
        }
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

            Invalidate();
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
           
            }
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
           
        }

        private void BClear_Click(object sender, EventArgs e)
        {
            _fromPoint = null;
            _toPoint = null;
            TranPoints = null;
            _storedPoints = null;

            dst = new Mat();
            tranRadioButton.Checked = false;
            drawRadioButton.Checked = true;
            tranRadioButton.Enabled = false;
            drawRadioButton.Enabled = false;
            UpdateMap();
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
            var returnPoint = new Point((int) myPoint.X, (int) myPoint.Y);
            return returnPoint;
        }
    }
}
