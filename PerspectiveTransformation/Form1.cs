using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerspectiveTransformation
{
    public partial class 透视变换器 : Form
    {
        private Bitmap _sourceImage;
        private Bitmap _backImage;
        private const int Margins = 20;
        private MyPoint _startPoint;
        private MyPoint _endPoint;
        private List<MyPoint> _myPoints = new List<MyPoint>();
        private Point _mouseFrom, _mouseTo;
        private bool _drawCord = false;
        private bool _moveCord = false;
        public int ShowScale = 1;
        public int OffsetX, OffsetY;
        public int MoveOffsetX, MoveOffsetY;
        private int? _changeNumber;

        public 透视变换器()
        {
            InitializeComponent();
            UpdateSize();
            PanelOrigin.MouseWheel += Panel1_MouseWheel;
        }
        //控件操作
        private void BOpen_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;
            _sourceImage = (Bitmap)Image.FromFile(dialog.FileName);
            _startPoint = null;
            _endPoint = null;
            UpdateSize();
            UpdateMap(ShowScale, OffsetX, OffsetY);
        }

        private void BTran_Click(object sender, EventArgs e)
        {
            if (_backImage == null) return;
            RB变形.Enabled = true;
            RB版面.Enabled = true;
        }

        private void BClear_Click(object sender, EventArgs e)
        {
            _startPoint = null;
            _endPoint = null;
            UpdateMap(ShowScale, OffsetX, OffsetY);
        }

        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            ShowScale = (int)trackBar1.Value;
            UpdateMap(ShowScale, OffsetX, OffsetY);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            UpdateSize();
            if (_sourceImage != null) UpdateMap(ShowScale, OffsetX, OffsetY);
        }


        //更新底图
        private void UpdateMap(int showScale, int offsetX, int offsetY)
        {
            _backImage?.Dispose();
            if (_sourceImage == null) return;
            _backImage = new Bitmap(PanelOrigin.Width, PanelOrigin.Height);
            var g = Graphics.FromImage(_backImage);
            g.DrawImage(_sourceImage, new Rectangle((1 - showScale) * PanelOrigin.Width / 50 + offsetX, (1 - showScale) * PanelOrigin.Height / 50 + offsetY,
                (showScale - 1) * 2 * PanelOrigin.Width / 50 + PanelOrigin.Width, (showScale - 1) * 2 * PanelOrigin.Height / 50 + PanelOrigin.Height));
            if (_startPoint != null)
            {
                var startpoint = ToScreenPoint(_startPoint, showScale, offsetX, offsetY);
                var endpoint = ToScreenPoint(_endPoint, showScale, offsetX, offsetY);
                DrawAll(startpoint, endpoint, g);
            }
            g.Dispose();
            PanelOrigin.Invalidate();
        }
        //更新尺寸
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
     

        //绘制变形框
        public void DrawAll(Point startPoint, Point endPoint, Graphics g)
        {
            if (RB版面.Checked == true && RB版面.Enabled == true)
            {
                DrawBank(startPoint, endPoint, g);
            }
            if (RB变形.Checked == true && RB变形.Enabled == true)
            {
                TranBank(startPoint, endPoint, g);
            }
        }

        private void DrawBank(Point startPoint, Point endPoint, Graphics g)
        {
            List<MyPoint> mypoints = new List<MyPoint>();
            var width = Math.Abs(startPoint.X - endPoint.X);
            var height = Math.Abs(startPoint.Y - endPoint.Y);
            var left = Math.Min(startPoint.X, endPoint.X);
            var top = Math.Min(startPoint.Y, endPoint.Y);
            var points = new Point[4];
            mypoints.Add(new MyPoint(left, top));
            mypoints.Add(new MyPoint(left, top + height));
            mypoints.Add(new MyPoint(left + width, top + height));
            mypoints.Add(new MyPoint(left + width, top));
            //绘制边角的四个顶点
            foreach (var point in mypoints)
            {
                Rectangle rect = new Rectangle((int)(point.X - 5), (int)(point.Y - 5), 10, 10);
                g.DrawEllipse(new Pen(Color.SteelBlue), rect);
                g.FillEllipse(new SolidBrush(Color.SteelBlue), rect);
                var i = mypoints.IndexOf(point);
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
            _myPoints = mypoints;
        }

        private void TranBank(Point startPoint, Point endPoint, Graphics g)
        {
            List<MyPoint> _mypoints = _myPoints;
            Point[] points = new Point[4];
            //绘制边角的四个顶点
            for (int i = 0; i < _mypoints.Count; i++)
            {
                points[i] = new Point
                {
                    X = (int)(_mypoints[i].X),
                    Y = (int)(_mypoints[i].Y)
                };
                var distance = Math.Pow((_mypoints[i].X - startPoint.X), 2) + Math.Pow((_mypoints[i].Y - startPoint.Y), 2);
                if (distance <= 25)
                {
                    points[i] = endPoint;
                    _changeNumber = i;
                }
                Rectangle rect = new Rectangle(points[i].X - 5, points[i].Y - 5, 10, 10);
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
        
        //重绘窗口
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            if (_backImage != null)
            {
                e.Graphics.DrawImage(_backImage, 0, 0);
                if (_drawCord)
                {
                    DrawAll(_mouseFrom, _mouseTo, e.Graphics);
                }
            }
            Invalidate();
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                {
                    if (_startPoint != null)
                    {
                 //       _startPoint = null;
                        UpdateMap(ShowScale, OffsetX, OffsetY);
                    }
                    _mouseFrom = e.Location;
                    _startPoint = new MyPoint(e.X, e.Y);
                    _drawCord = true;
                    break;
                }
                case MouseButtons.Middle:
                    Cursor.Current = Cursors.SizeAll;
                    _mouseFrom = e.Location;
                    _moveCord = true;
                    break;
            }
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                {
                    _endPoint = new MyPoint(e.X, e.Y);
                    _drawCord = false;
                    if (RB变形.Checked == true && RB变形.Enabled == true)
                    {
                        if (_changeNumber != null) _myPoints[_changeNumber.Value] = _endPoint;
                    }
                    _startPoint = null;
                    _endPoint = null;
                    _changeNumber = null;
                    break;
                }
                case MouseButtons.Middle:
                    Cursor.Current = Cursors.Default;
                    OffsetX = MoveOffsetX;
                    OffsetY = MoveOffsetY;
                    UpdateMap(ShowScale, OffsetX, OffsetY);
                    _moveCord = false;
                    break;
            }
        }

        private void Panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            bool check = false;
            switch (e.Delta > 0)
            {
                case true when ShowScale < 50:
                {
                    ShowScale++;
                    check = true;
                    break;
                    }
            }
            switch (e.Delta <= 0)
            {
                case true when ShowScale > 1:
                {
                    ShowScale--;
                    check = true;
                    
                    break;
                    }
            }
            
            if (!check) return;
            Point currentMiddle = MidPoint(ShowScale, OffsetX, OffsetY);
            OffsetX += (e.Location.X - currentMiddle.X) / 50;
            OffsetY += (e.Location.Y - currentMiddle.Y) / 50;
            UpdateMap(ShowScale, OffsetX, OffsetY);
            trackBar1.Value = ShowScale;
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel1.Text = e.Location.ToString();
            switch (e.Button)
            {
                case MouseButtons.Left:
                    _mouseTo = e.Location;
                    panel1.Invalidate();
                    break;
                case MouseButtons.Middle:
                {
                    if (_moveCord)
                    {
                        _mouseTo = e.Location;
                        MoveOffsetX = OffsetX + _mouseTo.X - _mouseFrom.X;
                        MoveOffsetY = OffsetY + _mouseTo.Y - _mouseFrom.Y;
                        UpdateMap(ShowScale, MoveOffsetX, MoveOffsetY);
                    }
                    break;
                }
            }
        }

        //屏幕与图像坐标转换
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
            var X = (double)(onePoint.X - offsetX - (1 - scale) * PanelOrigin.Width / 50) /
                    (double)((scale - 1) * 2 * PanelOrigin.Width / 50 + PanelOrigin.Width);
            var Y = (double)(onePoint.Y - offsetY - (1 - scale) * PanelOrigin.Height / 50) /
                    (double)((scale - 1) * 2 * PanelOrigin.Height / 50 + PanelOrigin.Height);
            return new MyPoint(X, Y);
        }
        public Point MidPoint(int scale, int offsetX, int offsetY)
        {
            int X = (1 - scale) * PanelOrigin.Width / 50 + offsetX
                                                         + ((scale - 1) * 2 * PanelOrigin.Width / 50 + PanelOrigin.Width) / 2;
            int Y = (1 - scale) * PanelOrigin.Height / 50 + offsetY
                                                          + ((scale - 1) * 2 * PanelOrigin.Height / 50 + PanelOrigin.Height) / 2;
            return new Point(X, Y);
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
        public double X;
        public double Y;
        public MyPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}