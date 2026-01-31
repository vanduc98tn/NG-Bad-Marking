using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTO;
using Microsoft.Win32;
using BLL;

namespace GUI
{
    /// <summary>
    /// Interaction logic for PgSuperUserMenu.xaml
    /// </summary>
    public partial class PgSuperUserMenu : Page
    {
        private bool isDrawing = false;
        private Point startPoint;
        private Ellipse currentEllipse;
        private Point originalPosition;
        private Point originalEllipsePosition;
        private bool isEllipseSelected = false;
        private bool isAnimating = false;
        private List<CircleInfor> circles;
        private List<ImageInfor> imageInfors;
        private bool isRemove = false;
        private List<string> alarms = new List<string>();
        private List<Rectangle> recCodeButton;
        private int selectedIndex = 0;
        private string selectedIndexAlarm;
        private Rectangle currentImage;
        public PgSuperUserMenu()
        {
            InitializeComponent();
            this.Loaded += PgSuperUserMenu_Loaded;
            this.KeyDown += MainWindow_KeyDown;

        }
        private void GetCirclePoints(double centerX, double centerY, double radius, out Point startPoint, out Point endPoint)
        {

            startPoint = new Point(centerX - radius, centerY);
            endPoint = new Point(centerX + radius, centerY);
        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.RightButton == MouseButtonState.Pressed)
            {
                currentEllipse = e.Source as Ellipse;

                if (currentEllipse != null && canvas.CaptureMouse())
                {

                    originalPosition = e.GetPosition(canvas);
                    originalEllipsePosition = new Point(Canvas.GetLeft(currentEllipse), Canvas.GetTop(currentEllipse));
                    isEllipseSelected = true;
                }
            }
            else if (!isDrawing)
            {
                startPoint = e.GetPosition(canvas);
                currentEllipse = new Ellipse
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 4,
                    Fill = Brushes.Transparent
                };
                Canvas.SetLeft(currentEllipse, startPoint.X);
                Canvas.SetTop(currentEllipse, startPoint.Y);
                canvas.Children.Add(currentEllipse);
                originalPosition = startPoint;
                isDrawing = true;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

            if (isDrawing && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(canvas);
                double width = Math.Abs(currentPosition.X - startPoint.X);
                double height = Math.Abs(currentPosition.Y - startPoint.Y);
                double diameter = Math.Min(width, height);
                if (currentEllipse != null)
                {
                    currentEllipse.Width = diameter;
                    currentEllipse.Height = diameter;
                }
            }
            else if (isEllipseSelected && canvas.IsMouseCaptured && e.RightButton == MouseButtonState.Pressed)
            {
                if (currentEllipse != null)
                {
                    Point mousePos = e.GetPosition(canvas);
                    double offsetX = mousePos.X - originalPosition.X;
                    double offsetY = mousePos.Y - originalPosition.Y;

                    double newLeft = originalEllipsePosition.X + offsetX;
                    double newTop = originalEllipsePosition.Y + offsetY;

                    Canvas.SetLeft(currentEllipse, newLeft);
                    Canvas.SetTop(currentEllipse, newTop);
                }
            }
        }
        private void UpdateValueCircle()
        {
            for (int i = 0; i < circles.Count; i++)
            {
                double radius1 = Math.Max(Math.Abs(circles[i].EndX - circles[i].StartX), Math.Abs(circles[i].EndY - circles[i].StartY)) / 2;
                if ((currentEllipse.Width / 2) == radius1 && circles[i].ID_Alarm == selectedIndexAlarm)
                {
                    double left = Canvas.GetLeft(currentEllipse);
                    double top = Canvas.GetTop(currentEllipse);
                    double width = currentEllipse.Width;
                    double height = currentEllipse.Height;

                    // Tính toán tọa độ của tâm và bán kính của hình tròn
                    double centerX = left + width / 2;
                    double centerY = top + height / 2;
                    double radius = Math.Min(width, height) / 2;

                    circles[i].StartX = centerX - radius;
                    circles[i].StartY = centerY - radius;
                    circles[i].EndX = centerX + radius;
                    circles[i].EndY = centerY + radius;
                    circles[i].Color = "Transparent";
                    SaveCircles();
                }
            }
        }
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDrawing = false;
            Point startPoint, endPoint;
            if (currentEllipse != null && e.ChangedButton == MouseButton.Left)
            {
                GetCirclePoints(Canvas.GetLeft(currentEllipse) + currentEllipse.Width / 2, Canvas.GetTop(currentEllipse) + currentEllipse.Height / 2, currentEllipse.Width / 2, out startPoint, out endPoint);
                AddCircleInfo(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, "Transparent");
            }

            if (currentEllipse != null && e.ChangedButton == MouseButton.Right)
            {
                UpdateValueCircle();
            }

            if (currentEllipse != null)
            {

                canvas.ReleaseMouseCapture();
                currentEllipse = null;
            }
            isEllipseSelected = false;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && currentEllipse != null)
            {
                canvas.Children.Remove(currentEllipse);
                this.isRemove = true;
                RemoveCircle();
                SaveCircles();
                //currentEllipse = null;
                isDrawing = false; // Đặt isDrawing thành false sau khi xóa
                isEllipseSelected = false;
            }
        }
        private void RemoveCircle()
        {

            for (int i = 0; i < circles.Count; i++)
            {
                double radius = Math.Max(Math.Abs(circles[i].EndX - circles[i].StartX), Math.Abs(circles[i].EndY - circles[i].StartY)) / 2;
                if ((currentEllipse.Width / 2) == radius)
                {
                    circles.RemoveAt(i);
                }
            }
        }
        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            string imageAlarmDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image Alarm");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = imageAlarmDirectory;
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImageFileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                string fileSave = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image Alarm", selectedImageFileName);
                AddImagePath(fileSave);
                string imagePath = openFileDialog.FileName;
                RemoveImage();
                InsertImageToUI(imagePath);
            }
        }
        private void RemoveImage()
        {
            if (currentImage != null)
            {
                canvas.Children.Remove(currentImage);
            }
        }
        private void AddImagePath(string fileSave)
        {
            ImageInfor imageInfor = new ImageInfor();
            imageInfor.ID_Alarm = selectedIndexAlarm;
            imageInfor.Path = fileSave;

            if (this.imageInfors != null || this.imageInfors.Count > 0)
            {
                for (int i = 0; i < imageInfors.Count; i++)
                {
                    if (imageInfors[i].ID_Alarm != imageInfor.ID_Alarm) continue;
                    imageInfors.RemoveAt(i);
                }
            }
            this.imageInfors.Add(imageInfor);
        }
        private bool DirectoryContainsImage(string directoryPath, string imageName)
        {
            // Kiểm tra xem thư mục có tồn tại không
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"Thư mục '{directoryPath}' không tồn tại.");
            }

            // Kiểm tra xem file ảnh có tồn tại trong thư mục không
            string imagePath = System.IO.Path.Combine(directoryPath, imageName);
            return File.Exists(imagePath);
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

            currentImage = rect;

            // Thêm hình chữ nhật vào Canvas
            canvas.Children.Insert(0, currentImage);
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Save ảnh thành công!!!");
            SystemsManager.Instance.SaveAppSettings();
        }
        private void LoadImageInfor()
        {
            foreach (var image in imageInfors)
            {
                if (image.ID_Alarm != selectedIndexAlarm) continue;
                if (!File.Exists(image.Path))
                {
                    imageInfors.Remove(image);
                    SystemsManager.Instance.SaveAppSettings();
                    return;
                }
                InsertImageToUI(image.Path);
            }
        }
        private void AnimationButton_Click(object sender, RoutedEventArgs e)
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

                        ellipse.RenderTransformOrigin = new Point(0.5, 0.5); // Đặt tâm biến đổi là trung tâm của hình tròn
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
        private void AddCircleInfo(double startX, double startY, double endX, double endY, string color)
        {
            CircleInfor circle = new CircleInfor
            {
                StartX = startX,
                StartY = startY,
                EndX = endX,
                EndY = endY,
                Color = color,
                ID_Alarm = selectedIndexAlarm,
            };
            circles.Add(circle);
            SaveCircles();
        }
        private void LoadCircles()
        {
            try
            {
                foreach (var circle in circles)
                {
                    if (circle.ID_Alarm != selectedIndexAlarm) continue;
                    DrawCircle(circle.StartX, circle.StartY, circle.EndX, circle.EndY, circle.Color);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading circles: " + ex.Message);
            }
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
            CircleInfor circle = new CircleInfor
            {
                Color = color,
                StartX = startX,
                StartY = startY,
                EndX = endX,
                EndY = endY
            };

            canvas.Children.Add(ellipse);
        }

        private void SaveCircles()
        {
            SystemsManager.Instance.SaveAppSettings();
        }

        private void CreateButtonAlarm()
        {
            var alarmList = BLLManager.Instance.AlarmListService.GetAll();
            alarms = new List<string>();
            foreach (var x in alarmList)
            {
                var deviceCode = x.Value.Device + x.Value.DeviceCode;
                alarms.Add(deviceCode);
            }
            this.ugridJamList.Children.Clear();
            this.recCodeButton = new List<Rectangle>();
            for (int i = 0; i < this.alarms.Count; i++)
            {
                var alarm = this.alarms[i];
                Button myButton = new Button();

                // Tạo một Rectangle và thiết lập các thuộc tính
                //Rectangle myRectangle = new Rectangle();
                //myRectangle.Width = 15;
                //myRectangle.Height = 48;
                //myRectangle.Fill = new SolidColorBrush(Colors.LightGray);
                //myRectangle.Margin = new Thickness(-100, 0, 0, 0);
                //myRectangle.Tag = i;
                //this.recCodeButton.Add(myRectangle);
                // Tạo một StackPanel và thiết lập Orientation là Horizontal
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;

                // Thêm Rectangle và TextBlock vào StackPanel
                //stackPanel.Children.Add(myRectangle);
                stackPanel.Children.Add(CreateTextBlock(alarm));

                // Đặt StackPanel làm nội dung cho Button
                myButton.Content = stackPanel;
                myButton.Margin = new Thickness(5);
                myButton.Tag = i;
                myButton.Click += Bt_Click;

                ugridJamList.Children.Add(myButton);
            }
            selectedIndex = 0;
            selectedIndexAlarm = alarms[0];
            if (this.alarms.Count > 0)
            {
                updateAlarm();

                LoadCircles();
                LoadImageInfor();
            }

        }
        private TextBlock CreateTextBlock(string text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Padding = new Thickness(10, 0, 0, 0);
            textBlock.Text = text;
            return textBlock;
        }
        private void updateAlarm()
        {
            foreach (var obj in ugridJamList.Children)
            {
                var bt = obj as Button;
                if (bt != null)
                {
                    if ((int)bt.Tag == selectedIndex)
                    {
                        bt.Background = Brushes.Red;
                        //var rec = this.recCodeButton.FirstOrDefault(x => (int)x.Tag == selectedIndex);
                        //rec.Fill = new SolidColorBrush(Colors.DarkRed);
                    }
                    else
                    {
                        //var rec = this.recCodeButton[(int)bt.Tag];
                        //rec.Fill = new SolidColorBrush(Colors.LightGray);
                        bt.ClearValue(Button.BackgroundProperty);
                    }
                }
            }
        }
        private void Bt_Click(object sender, RoutedEventArgs e)
        {
            this.canvas.Children.Clear();
            var bt = (Button)sender;
            selectedIndex = (int)bt.Tag;
            selectedIndexAlarm = alarms[(int)bt.Tag];

            this.updateAlarm();
            LoadCircles();
            LoadImageInfor();
        }
        private void PgSuperUserMenu_Loaded(object sender, RoutedEventArgs e)
        {
            circles = SystemsManager.Instance.AppSettings.CircleInfors;
            imageInfors = SystemsManager.Instance.AppSettings.ImageInfors;
            CreateButtonAlarm();
        }
    }
}
