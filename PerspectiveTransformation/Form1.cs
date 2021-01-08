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
        private Mat showMat;
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
            _tempPoint = new MyPoint(e.X, e.Y);
            
        }

        private void BOpen_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;
            _name = dialog.SafeFileName;
            src = Cv2.ImRead(dialog.FileName);
            showMat = src;
            UpdateSize();
            UpdateMap();
        }
        public static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {

            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                var bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
        public static Bitmap MatToBitmap(Mat dst)

        {
            return new Bitmap(dst.Cols, dst.Rows, (int)dst.Step(), PixelFormat.Format24bppRgb, dst.Data);

        }

        public static Mat BitmapToMat(Bitmap src)

        {
            int srcWidth = src.Width;

            int iheight = src.Height;

            int srcByte = srcWidth * iheight * 3;

            byte[] result = new byte[srcByte];

            int step;

            Rectangle rect = new Rectangle(0, 0, srcWidth, iheight);

            BitmapData bmpData = src.LockBits(rect, ImageLockMode.ReadWrite, src.PixelFormat);

            IntPtr iPtr = bmpData.Scan0;

            Marshal.Copy(iPtr, result, 0, srcByte);

            step = bmpData.Stride;

            src.UnlockBits(bmpData);

            return new Mat(src.Height, src.Width, new MatType(MatType.CV_8UC3), result, step);

        }
        public static Bitmap MatrixToBitmap(Mat dst)

        {
            return new Bitmap(dst.Cols, dst.Rows, (int)dst.Step(), PixelFormat.Format24bppRgb, dst.Data);

        }
        private void UpdateMap()
        {
            if (!showMat.Empty())
            {
                Bitmap showBitmap = new Bitmap(PanelOrigin.Width, PanelOrigin.Height);

                Mat mat;
                if (dst.Empty())
                {
                    mat = showMat;
                }
                else
                {
                    mat = dst;
                }
      /*          var mem = mat.ToMemoryStream();
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = mem;
                bmp.EndInit();*/
          //      PanelOrigin.BackgroundImage = BitmapImageToBitmap(bmp);
      //    Image showImage = BitmapConverter.ToBitmap(mat);
                
                Image showImage = MatrixToBitmap(mat);
            //    PanelOrigin.BackgroundImage = showImage;
              //         Image showImage= BitmapImageToBitmap(bmp);
                var g = Graphics.FromImage(showBitmap);
                   g.DrawImage(showImage, new Rectangle(0, 0, PanelOrigin.Width, PanelOrigin.Height));

                if (_storedPoints != null)
                {
                    DrawFrame(_storedPoints, g);

                    //   DrawFrame(_fromPoint,_toPoint,g);
                }

                g.Dispose();
                PanelOrigin.Invalidate();
            }
        }

        private void DrawFrame(List<MyPoint> storedPoints, Graphics g)
        {
           
        }

        private void UpdateSize()
        {
            var panelWidth = ClientRectangle.Width - Margins * 2; ;
            var panelHeight = ClientRectangle.Height - PanelControls.Height - statusStrip1.Height - Margins * 2;

            if (showMat != null)
            {
                if (!showMat.Empty())
                {
                    Mat mat;
                    if (dst.Empty())
                    {
                        mat = showMat;
                    }
                    else
                    {
                        mat = dst;
                    }
                    //    Image showImage = BitmapConverter.ToBitmap(mat);
                    Image showImage = MatrixToBitmap(mat);
                    var newHeight = showImage.Height * panelWidth / showImage.Width;
                    if (newHeight >= panelHeight)
                    {
                        panelWidth = showImage.Width * panelHeight / showImage.Height;
                    }
                    else
                    {
                        panelHeight = newHeight;
                    }
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
                List<MyPoint> myPoints = _storedPoints;

                for (int i = 0; i < myPoints.Count; i++)
                {
                    var distance = Math.Pow((myPoints[i].X - fromPoint.X), 2) 
                                   + Math.Pow((myPoints[i].Y - fromPoint.Y), 2);
                    if (distance <= 25)
                    {
                        myPoints[i] = new MyPoint(toPoint.X, toPoint.Y);
                    }
                }
                TranBank(myPoints, g);
                _storedPoints = myPoints;
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
        private void PanelOrigin_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            switch (showMat)
            {
                case null:
                    break;
                default:
                {
                    if (!showMat.Empty())
                        UpdateMap();
                    break;
                }
            }
            Invalidate();
        }

        private void PanelOrigin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _fromPoint = new MyPoint(e.X, e.Y);
                var g = Graphics.FromImage(PanelOrigin.BackgroundImage);
                Rectangle rect = new Rectangle((int)(e.X - 5), (int)(e.Y - 5), 10, 10);
                g.DrawEllipse(new Pen(Color.SteelBlue), rect);
                g.FillEllipse(new SolidBrush(Color.SteelBlue), rect);
                g.Dispose();
            }
        }

        private void PanelOrigin_MouseUp(object sender, MouseEventArgs e)
        {
            _toPoint = new MyPoint(e.X, e.Y);
            UpdateMap();
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

        private void BSave_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Jpg 图片|*.jpg|Bmp 图片|*.bmp|Gif 图片|*.gif|Png 图片|*.png|Wmf  图片|*.wmf";
            dialog.FileName = _name+"（副本）";
            dialog.FilterIndex = 0;
            dialog.CheckFileExists = true;
            dialog.RestoreDirectory = true;
            dialog.AddExtension = true;
            Image saveImage;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dst != null)
                {
                    if (dst.Empty())
                    {
                        saveImage = BitmapConverter.ToBitmap(src);
                    }
                    else
                    {
                        saveImage = BitmapConverter.ToBitmap(dst);
                    }
                }
                else
                {
                    saveImage = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(dst);
                }
                saveImage.Save(dialog.FileName);
                MessageBox.Show(this, "图片保存成功！", "信息提示");
            }
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
