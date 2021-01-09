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
        private string _name;
        private Bitmap _nowImage;
        private Bitmap _sourceImage;
        private Bitmap _backImage;
        private  int _margins = 40;
        private MyPoint _fromPoint;
        private MyPoint _toPoint;
        private List<MyPoint> _myPoints = new List<MyPoint>();
        private List<Point2f> _storedPoints = new List<Point2f>();
        private Point _mouseFrom, _mouseTo;
        public bool Drawcord = false;
        public int ShowScale = 1;
        public int OffsetX, OffsetY;
        private List<Point2f> _tranPoints = new List<Point2f>();
        private List<Point> _points = new List<Point>();
        private int? _changeNumber;
        private int _width;
        private int _height;

        public 透视变换器()
        {
            InitializeComponent();
            UpdateSize();
            //使窗体大小初始全屏化
            KeyPreview = true;
            StartPosition = FormStartPosition.Manual;
            var rect = Screen.GetWorkingArea(this);
            this.Width = rect.Width;
            this.Height = rect.Height;
        }
        /// <summary>
        /// /打开图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BOpen_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Jpg 图片|*.jpg|Bmp 图片|*.bmp|Gif 图片|*.gif|Png 图片|*.png|Wmf  图片|*.wmf",
                FilterIndex = 0
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var name = dialog.SafeFileName;
                _name = Roll(name);
                _name = _name.Substring(name.Length - name.LastIndexOf("."));
                _name = Roll(_name);
                _sourceImage = (Bitmap) Image.FromFile(dialog.FileName);
                _fromPoint = null;
                _nowImage = _sourceImage;
                _toPoint = null;
                this.PanelOrigin.Cursor = System.Windows.Forms.Cursors.Default;
                if (!(_storedPoints == null || _storedPoints.Count == 0)) _storedPoints.Clear();
                if (!(_tranPoints == null || _tranPoints.Count == 0)) _tranPoints.Clear();
                UpdateSize();
                UpdateMap();
                BTran.Enabled = true;
                BClear.Enabled = true;
                BSave.Enabled = true;
                BStore.Enabled = true;
                BShow.Enabled = true;
            }
        }
        /// <summary>
        /// 允许开始转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtTran_Click(object sender, EventArgs e)
        {
            if (_backImage == null) return;
            this.PanelOrigin.Cursor = System.Windows.Forms.Cursors.Cross;
            tranRadioButton.Enabled = true;
            drawRadioButton.Enabled = true;
        }
        /// <summary>
        /// 清除控制框和控制点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (!(_storedPoints == null || _storedPoints.Count == 0)) _storedPoints.Clear();
            if (!(_tranPoints == null || _tranPoints.Count == 0)) _tranPoints.Clear();
            UpdateMap();
        }
        /// <summary>
        /// 展示变换效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BShow_Click(object sender, EventArgs e)
        {
            if (_sourceImage != null)
            {
                WarpPerspective();
            
                var drawImage = new Bitmap(_width,_height);
                var g = Graphics.FromImage(drawImage);
                g.DrawImage(_nowImage, new Rectangle(0, 0, _width, _height));
                g.Dispose();
                PanelShown.BackgroundImage = drawImage;
                PanelShown.Invalidate();
            }
            else
            {
                MessageBox.Show("请先加载图片！");
            }
        }
        /// <summary>
        /// 存储转换点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BStore_Click(object sender, EventArgs e)
        {
         
            if(_storedPoints !=null && _storedPoints.Count !=0)
            {
                if (_tranPoints == null) return;
                if (_tranPoints.Count != 0)
                {
                    _tranPoints.Clear();
                }
                foreach (var point in _storedPoints)
                {
                    _tranPoints.Add(point);
                }

            }
            else
            {
                MessageBox.Show("请先创建变换框！");
            }

        }
        /// <summary>
        /// 存储图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        
        /// <summary>
        /// 提取文件名
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private static string Roll(string inputString)
        {
            var returnString = "";
            var n = inputString.Length;
            for (var i = 0; i < n; i++)
            {
                returnString += inputString[n - i - 1];
            }

            return returnString;
        }
        /// <summary>
        /// 对图片进行变换
        /// </summary>
        private  void WarpPerspective()
        {
            Bitmap nowImage;
            if (_tranPoints != null && _tranPoints.Count != 0)
            {
                var  warpFromPoints = new List<Point2f>();
                var warpToPoints = new List<Point2f>();
                for (var index = 0; index < _storedPoints.Count; index++)
                {
                    warpToPoints.Add(ToCanvasPoint(_storedPoints[index]));
                    warpFromPoints.Add(ToCanvasPoint(_tranPoints[index])); 
                }
                var src = _sourceImage.ToMat();
                var dst = new Mat();
                var trans = Cv2.GetPerspectiveTransform(warpFromPoints, warpToPoints);
                trans.ConvertTo(trans, MatType.CV_32F);
                Cv2.WarpPerspective(src, dst, trans, src.Size());
                nowImage = dst.ToBitmap();
            }
            else
            {
                nowImage = _sourceImage;
            }

            _nowImage = nowImage;
        }
        
        /// <summary>
        /// 对绘制框的显示
        /// </summary>
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
        /// <summary>
        /// 窗口大小的设定
        /// </summary>
        private void UpdateSize()
        {
            PanelControls.Width = 80;
            var panelWidth = (ClientRectangle.Width - PanelControls.Width) / 2 - _margins * 2;
            var panelHeight = ClientRectangle.Height - statusStrip1.Height - _margins * 2;

            if (_sourceImage != null)
            {
                var forBitmap = _nowImage ?? _sourceImage;
                var imageHeight = forBitmap.Height * panelWidth / forBitmap.Width;

                switch (imageHeight >= panelHeight)
                {
                    case true:
                        panelWidth = forBitmap.Width * panelHeight / forBitmap.Height;
                        _margins = 20;
                        break;
                    default:
                        panelHeight = imageHeight;
                        break;
                }
            }

            PanelOrigin.Width = PanelShown.Width = _width = panelWidth;
            PanelOrigin.Height = PanelShown.Height = _height = panelHeight;
            PanelShown.Top = PanelOrigin.Top = (ClientRectangle.Height - panelHeight) / 2;

            PanelOrigin.Left = PanelControls.Width + _margins;
            PanelShown.Left = PanelOrigin.Left + _margins * 2 + panelWidth;


        }
        /// <summary>
        /// 屏幕坐标和绘制坐标转换
        /// </summary>
        /// <param name="onePoint"></param>
        /// <param name="scale"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <returns></returns>
        private Point ToScreenPoint(MyPoint onePoint, int scale, int offsetX, int offsetY)
        {
            var X = (int)(onePoint.X * ((scale - 1) * 2 * PanelOrigin.Width / 50 + PanelOrigin.Width)) +
                    offsetX + (1 - scale) * PanelOrigin.Width / 50;
            var Y = (int)(onePoint.Y * ((scale - 1) * 2 * PanelOrigin.Height / 50 + PanelOrigin.Height)) +
                    offsetY + (1 - scale) * PanelOrigin.Height / 50;
            return new Point(X, Y);
        }
        /// <summary>
        /// 绘制坐标和图像坐标转换
        /// </summary>
        /// <param name="onePoint"></param>
        /// <returns></returns>
        private Point2f ToCanvasPoint(Point2f onePoint)
        {
            var scale = _sourceImage.Width / PanelOrigin.Width;
            var X = (float)(onePoint.X * scale);
            var Y = (float)(onePoint.Y * scale);
            return new Point2f(X, Y);
        }


        /// <summary>
        /// 绘制变形框
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="g"></param>
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
        /// <summary>
        /// 绘制的具体代码
        /// </summary>
        /// <param name="myPoints"></param>
        /// <param name="g"></param>
        private static void DrawPoints(List<MyPoint> myPoints, Graphics g)
        {
            if (myPoints == null) throw new ArgumentNullException(nameof(myPoints));
            if (g == null) throw new ArgumentNullException(nameof(g));
            var points = new Point[4];
            foreach (var point in myPoints)
            {
                var rect = new Rectangle((int)(point.X - 5), (int)(point.Y - 5), 10, 10);
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
        /// <summary>
        /// 绘制矩形框
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="g"></param>
        private void DrawBank(Point startPoint, Point endPoint, Graphics g)
        {
            var mypoints = new List<MyPoint>();
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
            _storedPoints.Clear();
            foreach (var myPoint in mypoints)
            {
                _storedPoints.Add(new Point2f(myPoint.X, myPoint.Y));
            }
            
        }
        /// <summary>
        /// 绘制变形框
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="g"></param>
        private void TranBank(Point startPoint, Point endPoint, Graphics g)
        {
            if (_storedPoints != null && _storedPoints.Count != 0)
            {
                var mypoints = _myPoints;
                var points = new Point[4];
                //绘制边角的四个顶点
                for (var i = 0; i < mypoints.Count; i++)
                {
                    points[i] = new Point
                    {
                        X = (int)(mypoints[i].X),
                        Y = (int)(mypoints[i].Y)
                    };
                    var distance = Math.Pow((mypoints[i].X - startPoint.X), 2) + Math.Pow((mypoints[i].Y - startPoint.Y), 2);
                    if (distance <= 25)
                    {
                        points[i] = endPoint;
                        _changeNumber = i;
                    }
                    var rect = new Rectangle(points[i].X - 5, points[i].Y - 5, 10, 10);
                    g.DrawEllipse(new Pen(Color.SteelBlue), rect);
                    g.FillEllipse(new SolidBrush(Color.SteelBlue), rect);
                }
                g.DrawPolygon(new Pen(Color.SteelBlue, 2), points);
                g.DrawLine(new Pen(Color.SteelBlue), new Point(Convert.ToInt32(points[0].X * 2 / 3 + points[3].X / 3), Convert.ToInt32(points[0].Y * 2 / 3 + points[3].Y / 3)),
                       new Point(Convert.ToInt32(points[1].X * 2 / 3 + points[2].X / 3), Convert.ToInt32(points[1].Y * 2 / 3 + points[2].Y / 3)));
                g.DrawLine(new Pen(Color.SteelBlue), new Point(Convert.ToInt32(points[0].X / 3 + points[3].X * 2 / 3), Convert.ToInt32(points[0].Y / 3 + points[3].Y * 2 / 3)),
                       new Point(Convert.ToInt32(points[1].X / 3 + points[2].X * 2 / 3), Convert.ToInt32(points[1].Y / 3 + points[2].Y * 2 / 3)));
                g.DrawLine(new Pen(Color.SteelBlue), new Point(Convert.ToInt32(points[0].X * 2 / 3 + points[1].X / 3), Convert.ToInt32(points[0].Y * 2 / 3 + points[1].Y / 3)),
                       new Point(Convert.ToInt32(points[3].X * 2 / 3 + points[2].X / 3), Convert.ToInt32(points[3].Y * 2 / 3 + points[2].Y / 3)));
                g.DrawLine(new Pen(Color.SteelBlue), new Point(Convert.ToInt32(points[0].X / 3 + points[1].X * 2 / 3), Convert.ToInt32(points[0].Y / 3 + points[1].Y * 2 / 3)),
                       new Point(Convert.ToInt32(points[3].X / 3 + points[2].X * 2 / 3), Convert.ToInt32(points[3].Y / 3 + points[2].Y * 2 / 3)));


            }
            else
            {
                MessageBox.Show("请创建变形框！");
            }
               
        }

        /// <summary>
        /// 在PanelOrigin控件上绘制图案和变形框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 在PanelShown控件上绘制展示效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelShown_Paint(object sender, PaintEventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// 按下鼠标触发开始描绘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelOrigin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (_fromPoint != null)
            {
                _fromPoint = null;
            }
            _mouseFrom = e.Location;
            _fromPoint = new MyPoint(e.X, e.Y);
            Drawcord = true;
        }
        /// <summary>
        /// 鼠标抬起结束描绘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelOrigin_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _toPoint = new MyPoint(e.X, e.Y);
            Drawcord = false;
            if (tranRadioButton.Checked == true && tranRadioButton.Enabled == true)
            {
                if (_changeNumber != null)
                {
                    _storedPoints[_changeNumber.Value] = new Point2f(e.X,e.Y);
                    _myPoints[_changeNumber.Value] = new MyPoint(e.X, e.Y);
                }
            }
            _changeNumber = null;
        }
        /// <summary>
        /// 对两个绘制窗口的重绘，确保不发生闪烁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
           
            Invalidate();
        }
        /// <summary>
        /// 显示鼠标在PanelShown控件上的坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelShown_MouseMove(object sender, MouseEventArgs e)
        {
            LocationLabel.Text = e.Location.ToString();
            if (e.Button != MouseButtons.Left) return;
            PanelShown.Invalidate();
        }
        /// <summary>
        /// 显示鼠标在PanelOrigin控件上的坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelOrigin_MouseMove(object sender, MouseEventArgs e)
        {
            LocationLabel.Text = e.Location.ToString();
            if (e.Button != MouseButtons.Left) return;
            _mouseTo = e.Location;
            PanelOrigin.Invalidate();
        }
        /// <summary>
        /// 处理窗体大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Size(object sender, EventArgs e)
        {
            UpdateSize();
            if (_sourceImage != null) UpdateMap();
        }

        
    }
    /// <summary>
    /// 对Panel使用双缓冲，防止窗口闪烁
    /// </summary>
    public sealed class MyPanel : Control
    {
        public MyPanel()
        {
            DoubleBuffered = true;
        }
    }
    /// <summary>
    /// 对Point2f定义可以为空
    /// </summary>
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
