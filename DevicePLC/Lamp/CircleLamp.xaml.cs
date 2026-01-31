using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Shapes;
using DTO;
using BLL;

namespace DevicePLC
{
    /// <summary>
    /// Interaction logic for CircleLamp.xaml
    /// </summary>
    public partial class CircleLamp : UserControl,IObserverChangeBits
    {
        public static new readonly DependencyProperty DeviceProperty = DependencyProperty.Register(
            "DeviceLamp", typeof(DeviceName), typeof(CircleLamp), new PropertyMetadata(DeviceName.M));

        public static readonly DependencyProperty AddressLampProperty = DependencyProperty.Register(
            "AddressLamp", typeof(object), typeof(CircleLamp), new PropertyMetadata(null));

        public static readonly DependencyProperty BackgroundLampONProperty = DependencyProperty.Register(
            "BackgroundLampON", typeof(Brush), typeof(CircleLamp), new PropertyMetadata(Brushes.Green));

        public static readonly DependencyProperty BackgroundLampOFFProperty = DependencyProperty.Register(
            "BackgroundLampOFF", typeof(Brush), typeof(CircleLamp), new PropertyMetadata(Brushes.LightGray));

        public static readonly DependencyProperty TextOFFProperty = DependencyProperty.Register(
            "TextOFF", typeof(string), typeof(CircleLamp), new PropertyMetadata(""));
        public static readonly DependencyProperty TextONProperty = DependencyProperty.Register(
            "TextON", typeof(string), typeof(CircleLamp), new PropertyMetadata(""));

        public static readonly DependencyProperty IsShowInWindowProperty = DependencyProperty.Register(
            "IsShowInWindow", typeof(bool), typeof(CircleLamp), new PropertyMetadata(false));

        public static readonly DependencyProperty IsTabItemProperty = DependencyProperty.Register(
            "IsTabItem", typeof(bool), typeof(CircleLamp), new PropertyMetadata(false));

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
            get { return (DeviceName)GetValue(DeviceProperty); }
            set { SetValue(DeviceProperty, value); }
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

        public new object AddressLamp
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

        private CancellationTokenSource monitorCancellation;
        private NotifyPLCBits notifyPLCBits = new NotifyPLCBits();
        private bool isInTabItem;

        public CircleLamp()
        {
            InitializeComponent();

            this.Loaded += CircleLamp_Loaded;
            this.Unloaded += CircleLamp_Unloaded;
        }

        private void CircleLamp_Unloaded(object sender, RoutedEventArgs e)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                // Bỏ qua các hành động trong chế độ Design
                return;
            }
            if (this.isInTabItem) return;
            this.UnregisterNotifyBits();
            this.monitorCancellation?.Cancel();
            
            if (this.IsShowInWindow) return;
            this.RemoveAddress();
        }

        private void CircleLamp_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                // Bỏ qua các hành động trong chế độ Design
                return;
            }
            if (this.isInTabItem) return;
            this.RemoveAddress();
            this.Initial();
            this.RegisterNotifyBits();
            this.monitorCancellation = new CancellationTokenSource();
            this.AddAddress();
            this.isInTabItem = this.IsTabItem;
        }
        private void Initial()
        {
            this.cir.Fill = BackgroundLampOFF;
            this.txt.Text = this.TextOFF.ToString();
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
        private void ChangeBrushLamp(bool status, Ellipse ell)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (!status)
                {
                    ell.Fill = BackgroundLampOFF;
                    this.txt.Text = this.TextOFF.ToString();
                }
                else
                {
                    ell.Fill = BackgroundLampON;
                    this.txt.Text = this.TextON.ToString();
                }
            });
        }

        public void NotifyChangeBits(string key, bool status)
        {
            if (this.AddressLamp == null) return;
            if (this.DeviceLamp.ToString()+this.AddressLamp.ToString() != key) return;
            this.ChangeBrushLamp(status, this.cir);
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
