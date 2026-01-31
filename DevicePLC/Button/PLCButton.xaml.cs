using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DTO;
using BLL;
using System.Windows.Input;
using System.Windows.Threading;

namespace DevicePLC
{
    /// <summary>
    /// Interaction logic for AlternativeButtonUserControl.xaml
    /// </summary>
    public partial class PLCButton : UserControl, IObserverChangeBits
    {
        public static new readonly DependencyProperty ActionProperty = DependencyProperty.Register(
            "Action", typeof(ActionType), typeof(PLCButton), new PropertyMetadata(ActionType.Alternative));

        public static new readonly DependencyProperty DeviceLampProperty = DependencyProperty.Register(
            "DeviceLamp", typeof(DeviceName), typeof(PLCButton), new PropertyMetadata(DeviceName.M));

        public static new readonly DependencyProperty DeviceWriteProperty = DependencyProperty.Register(
            "DeviceWrite", typeof(DeviceName), typeof(PLCButton), new PropertyMetadata(DeviceName.M));

        public static new readonly DependencyProperty AddressWriteProperty = DependencyProperty.Register(
            "AddressWrite", typeof(object), typeof(PLCButton), new PropertyMetadata(null));

        public static readonly DependencyProperty AddressLampProperty = DependencyProperty.Register(
            "AddressLamp", typeof(object), typeof(PLCButton), new PropertyMetadata(null));

        public static new readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
            "FontSize", typeof(int), typeof(PLCButton), new PropertyMetadata(20));

        public static new readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(object), typeof(PLCButton), new PropertyMetadata("Name Button"));

        public static new readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            "Foreground", typeof(Brush), typeof(PLCButton), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty UseLampCoditionProperty =
            DependencyProperty.Register("UseLampCodition", typeof(bool), typeof(PLCButton), new PropertyMetadata(false));

        public static readonly DependencyProperty BackgroundLampONProperty = DependencyProperty.Register(
            "BackgroundLampON", typeof(Brush), typeof(PLCButton), new PropertyMetadata(Brushes.Green));

        public static readonly DependencyProperty BackgroundLampOFFProperty = DependencyProperty.Register(
            "BackgroundLampOFF", typeof(Brush), typeof(PLCButton), new PropertyMetadata(Brushes.LightGray));

        public static readonly DependencyProperty UseImageProperty = DependencyProperty.Register(
            "UseImage", typeof(bool), typeof(PLCButton), new PropertyMetadata(false));

        public static readonly DependencyProperty ImageOFFProperty = DependencyProperty.Register(
            "ImageOFF", typeof(string), typeof(PLCButton), new PropertyMetadata(""));

        public static readonly DependencyProperty ImageONProperty = DependencyProperty.Register(
            "ImageON", typeof(string), typeof(PLCButton), new PropertyMetadata(""));

        public static readonly DependencyProperty TextOFFProperty = DependencyProperty.Register(
            "TextOFF", typeof(string), typeof(PLCButton), new PropertyMetadata(""));

        public static readonly DependencyProperty TextONProperty = DependencyProperty.Register(
            "TextON", typeof(string), typeof(PLCButton), new PropertyMetadata(""));

        public static readonly DependencyProperty ImageSourceProperty =
        DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(PLCButton));

        public static readonly DependencyProperty IsShowInWindowProperty = DependencyProperty.Register(
            "IsShowInWindow", typeof(bool), typeof(PLCButton), new PropertyMetadata(false));

        public static readonly DependencyProperty IsTabItemProperty = DependencyProperty.Register(
            "IsTabItem", typeof(bool), typeof(PLCButton), new PropertyMetadata(false));

        public bool IsTabItem
        {
            get { return (bool)GetValue(IsTabItemProperty); }
            set { SetValue(IsTabItemProperty, value); }
        }

        public bool IsShowInWindow
        {
            get { return (bool)GetValue(IsShowInWindowProperty); }
            set { SetValue(IsShowInWindowProperty, value); }
        }

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public ActionType Action
        {
            get { return (ActionType)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        public DeviceName DeviceLamp
        {
            get { return (DeviceName)GetValue(DeviceLampProperty); }
            set { SetValue(DeviceLampProperty, value); }
        }

        public DeviceName DeviceWrite
        {
            get { return (DeviceName)GetValue(DeviceWriteProperty); }
            set { SetValue(DeviceWriteProperty, value); }
        }

        public object TextOFF
        {
            get { return GetValue(TextOFFProperty); }
            set { SetValue(TextOFFProperty, value); }
        }
        public object TextON
        {
            get { return GetValue(TextONProperty); }
            set { SetValue(TextONProperty, value); }
        }
        public bool UseImage
        {
            get { return (bool)GetValue(UseImageProperty); }
            set { SetValue(UseImageProperty, value); }
        }
        
        public string ImageON
        {
            get { return (string)GetValue(ImageONProperty); }
            set { SetValue(ImageONProperty, value); }
        }
        public string ImageOFF
        {
            get { return (string)GetValue(ImageOFFProperty); }
            set { SetValue(ImageOFFProperty, value); }
        }
        public new int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public Brush BackgroundLampON
        {
            get { return (Brush)GetValue(BackgroundLampONProperty); }
            set { SetValue(BackgroundLampONProperty, value); }
        }

        public Brush BackgroundLampOFF
        {
            get { return (Brush)GetValue(BackgroundLampOFFProperty); }
            set { SetValue(BackgroundLampOFFProperty, value); }
        }

        public new Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public new object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public bool UseLampCodition
        {
            get { return (bool)GetValue(UseLampCoditionProperty); }
            set { SetValue(UseLampCoditionProperty,value); }
        }

        public object AddressWrite
        {
            get { return GetValue(AddressWriteProperty); }
            set { SetValue(AddressWriteProperty, value); }
        }

        public object AddressLamp
        {
            get { return GetValue(AddressLampProperty); }
            set { SetValue(AddressLampProperty, value); }
        }

        private bool isToggled = false;
        private NotifyPLCBits notifyPLCBits = new NotifyPLCBits();
        private CancellationTokenSource monitorCancellation;
        private DispatcherTimer timer;
        private bool isInTabItem;
        private LoggerDebug logger = new LoggerDebug("AlternativeBtn");

        public PLCButton()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += Timer_Tick;
            InitializeComponent();
            this.Loaded += AlternativeButtonUserControl_Loaded;
            this.Unloaded += AlternativeButtonUserControl_Unloaded;
        }

        private void AlternativeButtonUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                // Bỏ qua các hành động trong chế độ Design
                return;
            }
            if (this.isInTabItem) return;
            this.DeleteDevice(this.IsShowInWindow);
            this.StopTimer();
            this.UnregisterNotifyBits();
        }
        private void DeleteDevice(bool cmd)
        {
            if (this.UseLampCodition)
            {
                this.monitorCancellation?.Cancel();
                if (this.AddressLamp == null) return;
                var address = ushort.Parse(this.AddressLamp.ToString());
                if (!cmd)
                {
                    this.RemoveAddress(address);
                }
            }
            if (Action == ActionType.Alternative)
            {
                this.monitorCancellation?.Cancel();
                if (this.AddressWrite == null) return;
                var address = ushort.Parse(this.AddressWrite.ToString());
                if (!cmd)
                {
                    BLLManager.Instance.PLC.RemoveBitAddress(this.DeviceWrite.ToString(), address);
                }
            }
        }
        private void AlternativeButtonUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                // Bỏ qua các hành động trong chế độ Design
                return;
            }
            if (this.isInTabItem) return;
            this.DeleteDevice(false);
            Initial();
            this.RegisterNotifyBits();
            this.monitorCancellation = new CancellationTokenSource();
            if (this.UseLampCodition)
            {
                if (this.AddressLamp == null) return;
                var address = ushort.Parse(this.AddressLamp.ToString());
                this.AddAddress(address);
            }
            if(Action == ActionType.Alternative)
            {
                if (this.AddressWrite == null) return;
                var address = ushort.Parse(this.AddressWrite.ToString());
                BLLManager.Instance.PLC.AddBitAddress(this.DeviceWrite.ToString(), address);
            }
            this.isInTabItem = this.IsTabItem;
        }
        private void Initial()
        {
            this.btn.Background = BackgroundLampOFF;
            this.btn.Content = TextOFF;
            this.SetImagePath(ImageOFF);
        }
        private void RegisterNotifyBits()
        {
            this.notifyPLCBits = SystemsManager.Instance.NotifyPLCBits;
            if (this.notifyPLCBits == null) return;
            this.notifyPLCBits.Attach(this);
        }
        private void UnregisterNotifyBits()
        {
            this.notifyPLCBits.Detach(this);
        }
        private void ChangeBrushLamp(bool status,Button btn)
        {
            if (!status)
            {
                this.btn.Background = BackgroundLampOFF;
                this.btn.Content = TextOFF;
                this.SetImagePath(ImageOFF);
            }
            else
            {
                this.btn.Background = BackgroundLampON;
                this.btn.Content = TextON;
                this.SetImagePath(ImageON);
            }
        }

        private void SetImagePath(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;
            if (!this.UseImage) return;
            // Tạo một Uri từ đường dẫn ảnh
            Uri imageUri = new Uri(imagePath, UriKind.RelativeOrAbsolute);

            // Tạo một BitmapImage và gán vào Image
            BitmapImage bitmapImage = new BitmapImage(imageUri);
            this.img.Source = bitmapImage;
        }
        private void EventLog(string message, string type)
        {
            //try
            //{
            //    var log = new EventLog(message, type);
            //    SqlLiteEventLog eventLog = new SqlLiteEventLog();
            //    var eventLogService = new EventLogService(eventLog.EventLogRepository);
            //    eventLogService.CreateEvent(log);
            //}
            //catch (Exception ex)
            //{
            //    logger.Create("AlarmLog: " + ex.Message, LogLevel.Error);
            //}
        }
        public void NotifyChangeBits(string key, bool status)
        {
            if (this.UseLampCodition)
            {
                if (this.AddressLamp != null)
                {
                    var device = this.DeviceLamp.ToString() + this.AddressLamp.ToString();   
                    if (device == key)
                    {
                        ChangeBrushLamp(status, this.btn);
                    }
                }
            }
            if (this.AddressWrite == null) return;
            if ("M"+this.AddressWrite.ToString() != key) return;
            this.isToggled = status ? true : false;
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Action == ActionType.Momentary) return;
            if(this.Action==ActionType.Alternative)
            {
                this.Alternative_Click();
            }
            else if(this.Action == ActionType.Set)
            {
                this.Set_Click();
            }
            else if(this.Action == ActionType.Reset)
            {
                this.Reset_Click();
            }
        }
        private void Button_TouchDown(object sender, TouchEventArgs e)
        {
            if (this.Action == ActionType.Momentary) return;
            if (this.Action == ActionType.Alternative)
            {
                this.Alternative_TouchDown();
            }
            else if (this.Action == ActionType.Set)
            {
                this.Set_TouchDown();
            }
            else if (this.Action == ActionType.Reset)
            {
                this.Reset_TouchDown();
            }
        }

        // Alternative.
        private async void Alternative_Click()
        {
            if (this.AddressWrite == null) return;
            var address = ushort.Parse(this.AddressWrite.ToString());
            if (!isToggled)
            {
                await BLLManager.Instance.PLC.Device.WriteBit(address, true, this.DeviceWrite.ToString());
                this.EventLog("AlternativeBtn Address('" + address + "') - " + this.Content, "true");
            }
            else
            {
                await BLLManager.Instance.PLC.Device.WriteBit(address, false, this.DeviceWrite.ToString());
                this.EventLog("AlternativeBtn Address('" + address + "') - " + this.Content, "false");
            }
        }
        private void Alternative_TouchDown()
        {
            this.Alternative_Click();
        }
        //SET.
        private async void Set_Click()
        {
            if (this.AddressWrite == null) return;
            var address = ushort.Parse(this.AddressWrite.ToString());
            await BLLManager.Instance.PLC.Device.WriteBit(address, true, this.DeviceWrite.ToString());
            this.EventLog("OnBtn Address('" + address + "') - " + this.Content, "true");
        }
        private void Set_TouchDown()
        {
            this.Set_Click();
        }
        //Reset
        private async void Reset_Click()
        {
            if (this.AddressWrite == null) return;
            var address = ushort.Parse(this.AddressWrite.ToString());
            await BLLManager.Instance.PLC.Device.WriteBit(address, false, this.DeviceWrite.ToString());
            this.EventLog("OffBtn Address('" + address + "') - " + this.Content, "false");
        }
        private void Reset_TouchDown()
        {
            this.Reset_Click();
        }
        //Momentary
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            // Cập nhật vị trí chuột mỗi lần chuột di chuyển
            MousePosition = e.GetPosition(this);
        }
        private async void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Action != ActionType.Momentary) return;
            if (this.AddressWrite == null) return;
            var address = ushort.Parse(this.AddressWrite.ToString());
            await BLLManager.Instance.PLC.Device.WriteBit(address, true, this.DeviceWrite.ToString());
            this.EventLog("MomentaryBtn Address('" + address + "') - " + this.Content, "true");
        }
        private async void Button_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.Action != ActionType.Momentary) return;
            Point position = e.GetPosition(btn);
            if (position.X < 0 || position.Y < 0 || position.X > btn.ActualWidth || position.Y > btn.ActualHeight)
            {
                // Chuột đã rời khỏi Button
                if (this.AddressWrite == null) return;
                var address = ushort.Parse(this.AddressWrite.ToString());
                await BLLManager.Instance.PLC.Device.WriteBit(address, false, this.DeviceWrite.ToString());
                this.EventLog("MomentaryBtn Address('" + address + "') - " + this.Content, "false");
                Mouse.Capture(null);
            }
        }
        private async void Button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.Action != ActionType.Momentary) return;
            if (this.AddressWrite == null) return;
            var address = ushort.Parse(this.AddressWrite.ToString());
            await BLLManager.Instance.PLC.Device.WriteBit(address, false, this.DeviceWrite.ToString());
            this.EventLog("MomentaryBtn Address('" + address + "') - " + this.Content, "false");
        }
        private void Button_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            if (this.Action != ActionType.Momentary) return;
            StartTimer();
            Button_PreviewMouseDown(sender, null);
        }
        private void Button_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (this.Action != ActionType.Momentary) return;
            this.Button_PreviewMouseUp(sender, null);
        }
        private void Button_LostTouchCapture(object sender, TouchEventArgs e)
        {
            if (this.Action != ActionType.Momentary) return;
            // Kiểm tra xem liệu tay có rời khỏi hit test của nút hay không
            if (!IsTouchOverButton(e.TouchDevice, (UIElement)sender))
            {
                // Nếu tay đã rời khỏi hit test của nút, gọi PreviewTouchUp
                Button_PreviewTouchUp(sender, e);
            }
        }
        private bool IsMouseOverElement()
        {
            // Kiểm tra xem vị trí chuột có nằm trong phạm vi của UserControl không
            return new Rect(0, 0, ActualWidth, ActualHeight).Contains(MousePosition);
        }
        private Point MousePosition { get; set; }
        private bool IsTouchOverButton(TouchDevice touchDevice, UIElement element)
        {
            // Kiểm tra xem tay có nằm trong hit test của phần tử hay không
            Point touchPoint = touchDevice.GetTouchPoint(element).Position;
            return element.InputHitTest(touchPoint) != null;
        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (this.Action != ActionType.Momentary) return;
            // Kiểm tra vị trí chuột so với UserControl
            if (!IsMouseOverElement())
            {
                // Chuột rời khỏi UserControl
                // Thực hiện các xử lý ở đây
                if (this.AddressWrite == null) return;
                var address = ushort.Parse(this.AddressWrite.ToString());
                await BLLManager.Instance.PLC.Device.WriteBit(address, false, this.DeviceWrite.ToString());
                Mouse.Capture(null);
                StopTimer();
            }
        }
        private void StartTimer()
        {
            // Bắt đầu theo dõi chuột khi UserControl được loaded
            MouseMove += UserControl_MouseMove;
            timer?.Start();
        }
        private void StopTimer()
        {
            // Dừng theo dõi chuột khi UserControl bị unloaded
            MouseMove -= UserControl_MouseMove;
            timer?.Stop();
        }
        private void AddAddress(ushort address)
        {
            BLLManager.Instance.PLC.AddBitAddress(this.DeviceLamp.ToString(), address);
        }
        private void RemoveAddress(ushort address)
        {
            BLLManager.Instance.PLC.RemoveBitAddress(this.DeviceLamp.ToString(), address);
        }
    }
    public enum ActionType
    {
        Alternative,
        Momentary,
        Set,
        Reset
    }
}
