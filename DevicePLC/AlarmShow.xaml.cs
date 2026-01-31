using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DTO;
using BLL;
using System.Windows.Media.Animation;

namespace DevicePLC
{
    /// <summary>
    /// Interaction logic for WndAlarmShow.xaml
    /// </summary>
    public partial class AlarmShow : Window
    {
        private static int seqId = 0;
        private bool isAnimating = false;
        private List<CircleInfor> CircleInforCurrent;
        private ImageInfor ImageInforCurrent;
        public AlarmShow(List<CircleInfor> circleInfors, ImageInfor imageInfor, DeviceName deviceWrite, DeviceName deviceLamp, ActionType action, ushort addressWrite, ushort addressLamp, String msg = "", String solution = "", string code = "", string mode = "AUTO")
        {
            InitializeComponent();
            this.btnClose.Click += BtnClose_Click;
            this.Closed += WndAlarmShow_Closed;
            this.Loaded += AlarmShow_Loaded;

            CircleInforCurrent = circleInfors;
            ImageInforCurrent = imageInfor;


            this.lblMessage.Text = msg;
            this.lblSolution.Text = solution;
            this.lblCode.Content = code.ToString();
            this.lblMode.Content = mode.ToString();
            this.lblTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            this.plcButtonBuzzer.DeviceWrite = deviceWrite;
            this.plcButtonBuzzer.DeviceLamp = deviceLamp;
            this.plcButtonBuzzer.Action = action;
            this.plcButtonBuzzer.AddressWrite = addressWrite;
            this.plcButtonBuzzer.AddressLamp = addressLamp;

            // Increase SeqId:
            seqId++;
            this.lblSeqId.Content = seqId.ToString();

            this.Topmost = true;
        }

        private void AlarmShow_Loaded(object sender, RoutedEventArgs e)
        {
            isAnimating = false;
            LoadCircle();
            LoadImage();
            Animation();
        }

        private void LoadCircle()
        {
            if (CircleInforCurrent == null) return;
            foreach (var x in CircleInforCurrent)
            {
                DrawCircle(x.StartX, x.StartY, x.EndX, x.EndY, x.Color);
            }
        }
        private void LoadImage()
        {
            if (ImageInforCurrent == null) return;
            InsertImageToUI(ImageInforCurrent.Path);
        }
        private void InsertImageToUI(string imagePath)
        {
            // Tạo một ImageBrush từ đường dẫn của tệp hình ảnh đã chọn
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(imagePath));

            // Tạo một hình chữ nhật có kích thước bằng kích thước của Canvas
            Rectangle rect = new Rectangle();
            rect.Width = canvas.ActualWidth;
            rect.Height = canvas.ActualHeight;
            rect.Fill = imageBrush;

            // Thêm hình chữ nhật vào Canvas
            canvas.Children.Insert(0, rect);
        }
        private void DrawCircle(double startX, double startY, double endX, double endY, string color)
        {
            double radius = Math.Max(Math.Abs(endX - startX), Math.Abs(endY - startY)) / 2;
            double centerX = (startX + endX) / 2;
            double centerY = (startY + endY) / 2;

            Ellipse ellipse = new Ellipse
            {
                Width = 2 * radius,
                Height = 2 * radius,
                Stroke = Brushes.Red,
                StrokeThickness = 4,
                Fill = (Brush)new BrushConverter().ConvertFromString(color)
            };
            Canvas.SetLeft(ellipse, centerX - radius);
            Canvas.SetTop(ellipse, centerY - radius);
            canvas.Children.Add(ellipse);
        }
        private void Animation()
        {
            if (!isAnimating)
            {
                foreach (var child in canvas.Children)
                {
                    if (child is Ellipse)
                    {
                        Ellipse ellipse = child as Ellipse;

                        ScaleTransform scaleTransform = new ScaleTransform(1, 1);

                        DoubleAnimation widthAnimation = new DoubleAnimation();
                        widthAnimation.From = 1;
                        widthAnimation.To = 2;
                        widthAnimation.Duration = TimeSpan.FromSeconds(0.2);
                        widthAnimation.AutoReverse = true;
                        widthAnimation.RepeatBehavior = RepeatBehavior.Forever;

                        DoubleAnimation heightAnimation = new DoubleAnimation();
                        heightAnimation.From = 1;
                        heightAnimation.To = 2;
                        heightAnimation.Duration = TimeSpan.FromSeconds(0.2);
                        heightAnimation.AutoReverse = true;
                        heightAnimation.RepeatBehavior = RepeatBehavior.Forever;

                        ellipse.RenderTransformOrigin = new Point(0.5, 0.5);
                        ellipse.RenderTransform = scaleTransform;

                        scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, widthAnimation);
                        scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, heightAnimation);
                    }
                }
                isAnimating = true;
            }
            else
            {
                foreach (var child in canvas.Children)
                {
                    if (child is Ellipse)
                    {
                        Ellipse ellipse = child as Ellipse;
                        ellipse.RenderTransform = null;
                    }
                }
                isAnimating = false;
            }
        }
        private void WndAlarmShow_Closed(object sender, EventArgs e)
        {
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
