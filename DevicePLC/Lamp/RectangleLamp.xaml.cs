using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Shapes;
using DTO;
using BLL;
using System;

namespace DevicePLC
{
    /// <summary>
    /// Interaction logic for RectangleLamp.xaml
    /// </summary>
    public partial class RectangleLamp : UserControl, IObserverChangeBits
    {
        private LoggerDebug logger = new LoggerDebug("RectangleLamp");

        public static new readonly DependencyProperty DeviceLampProperty = DependencyProperty.Register(
            "DeviceLamp", typeof(DeviceName), typeof(RectangleLamp), new PropertyMetadata(DeviceName.M));

        public static readonly DependencyProperty AddressLampProperty = DependencyProperty.Register(
            "AddressLamp", typeof(object), typeof(RectangleLamp), new PropertyMetadata(null));

        public static readonly DependencyProperty BackgroundLampONProperty = DependencyProperty.Register(
            "BackgroundLampON", typeof(Brush), typeof(RectangleLamp), new PropertyMetadata(Brushes.Green));

        public static readonly DependencyProperty BackgroundLampOFFProperty = DependencyProperty.Register(
            "BackgroundLampOFF", typeof(Brush), typeof(RectangleLamp), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty TextOFFProperty = DependencyProperty.Register(
            "TextOFF", typeof(string), typeof(RectangleLamp), new PropertyMetadata(""));
        public static readonly DependencyProperty TextONProperty = DependencyProperty.Register(
            "TextON", typeof(string), typeof(RectangleLamp), new PropertyMetadata(""));

        public static readonly DependencyProperty IsShowInWindowProperty = DependencyProperty.Register(
            "IsShowInWindow", typeof(bool), typeof(RectangleLamp), new PropertyMetadata(false));

        public static readonly DependencyProperty IsTabItemProperty = DependencyProperty.Register(
            "IsTabItem", typeof(bool), typeof(RectangleLamp), new PropertyMetadata(false));

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

        public DeviceName DeviceLamp
        {
            get { return (DeviceName)GetValue(DeviceLampProperty); }
            set { SetValue(DeviceLampProperty, value); }
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
        public object AddressLamp
        {
            get { return GetValue(AddressLampProperty); }
            set { SetValue(AddressLampProperty, value); }
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

        private NotifyPLCBits notifyPLCBits = new NotifyPLCBits();
        private bool isInTabItem;

        public RectangleLamp()
        {
            InitializeComponent();
            this.Loaded += RectangleLamp_Loaded;
            this.Unloaded += RectangleLamp_Unloaded;
        }

        private void RectangleLamp_Unloaded(object sender, RoutedEventArgs e)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                // Bỏ qua các hành động trong chế độ Design
                return;
            }
            if (this.isInTabItem) return;
            UnregisterNotifyBits();
            
            if (this.IsShowInWindow) return;
            this.RemoveAddress();
        }

        private void RectangleLamp_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                // Bỏ qua các hành động trong chế độ Design
                return;
            }
            if (this.isInTabItem) return;
            this.RemoveAddress();
            this.RegisterNotifyBits();
            this.Initial();
            this.AddAddress();
            this.isInTabItem = this.IsTabItem;
        }
        private void Initial()
        {
            if (this.txt == null) return;
            if (this.rec == null) return;
            this.rec.Fill= BackgroundLampOFF;
            this.txt.Text = this.TextOFF.ToString();
        }
        private void RegisterNotifyBits()
        {
            try
            {
                this.notifyPLCBits = SystemsManager.Instance.NotifyPLCBits;
                if (this.notifyPLCBits == null) return;
                this.notifyPLCBits.Attach(this);
            }
            catch(Exception ex)
            {
                logger.Create("RegisterNotifyBits: " + ex.Message, LogLevel.Error);
            }
        }
        private void UnregisterNotifyBits()
        {
            if (this.notifyPLCBits == null) return;
            this.notifyPLCBits.Detach(this);
        }
        private void ChangeBrushLamp(bool status, Rectangle rec)
        {
            if (this.rec == null) return;
            if (this.txt == null) return;
            this.Dispatcher.Invoke(() =>
            {
                if (!status)
                {
                    rec.Fill = BackgroundLampOFF;
                    this.txt.Text = this.TextOFF.ToString();
                }
                else
                {
                    rec.Fill = BackgroundLampON;
                    this.txt.Text = this.TextON.ToString();
                }
            });
        }

        public void NotifyChangeBits(string key, bool status)
        {
            if (this.AddressLamp == null) return;
            if (this.DeviceLamp.ToString()+this.AddressLamp.ToString() != key) return;
            this.ChangeBrushLamp(status, this.rec);
        }
        private void AddAddress()
        {
            if (this.AddressLamp == null) return;
            var address = ushort.Parse(this.AddressLamp.ToString());
            BLLManager.Instance.PLC.AddBitAddress(this.DeviceLamp.ToString(), address);
        }
        private void RemoveAddress()
        {
            if (this.AddressLamp == null) return;
            var address = ushort.Parse(this.AddressLamp.ToString());
            BLLManager.Instance.PLC.RemoveBitAddress(this.DeviceLamp.ToString(), address);
        }
    }
}
