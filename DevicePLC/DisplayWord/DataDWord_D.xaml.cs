using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DevicePLC
{
    /// <summary>
    /// Interaction logic for TxtReadOnlyDWord.xaml
    /// </summary>
    public partial class DataDWord_D : UserControl, IObserverChangeDWord
    {
        private CancellationTokenSource monitorCancellation;
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background", typeof(Brush), typeof(DataDWord_D), new PropertyMetadata(Brushes.White));
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            "Foreground", typeof(Brush), typeof(DataDWord_D), new PropertyMetadata(Brushes.White));
        public static readonly DependencyProperty DeviceProperty = DependencyProperty.Register(
            "Device", typeof(object), typeof(DataDWord_D), new PropertyMetadata(null));
        public static readonly DependencyProperty NoOfDisplayProperty =
            DependencyProperty.Register("NoOfDisplay", typeof(int), typeof(DataDWord_D), new PropertyMetadata(8));
        public static readonly DependencyProperty NoOfDecimalDigitsProperty =
            DependencyProperty.Register("NoOfDecimalDigits", typeof(int), typeof(DataDWord_D), new PropertyMetadata(0));
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
        "IsReadOnly", typeof(bool), typeof(DataDWord_D), new PropertyMetadata(false));
        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
            "FontSize", typeof(int), typeof(DataDWord_D), new PropertyMetadata(20));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(DataDWord_D), new PropertyMetadata(""));

        public static readonly DependencyProperty IsShowInWindowProperty = DependencyProperty.Register(
            "IsShowInWindow", typeof(bool), typeof(DataDWord_D), new PropertyMetadata(false));

        public static readonly DependencyProperty IsTabItemProperty = DependencyProperty.Register(
            "IsTabItem", typeof(bool), typeof(DataDWord_D), new PropertyMetadata(false));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
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

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public int NoOfDisplay
        {
            get { return (int)GetValue(NoOfDisplayProperty); }
            set { SetValue(NoOfDisplayProperty, value); }
        }
        public int NoOfDecimalDigits
        {
            get { return (int)GetValue(NoOfDecimalDigitsProperty); }
            set { SetValue(NoOfDecimalDigitsProperty, value); }
        }
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        public object Device
        {
            get { return GetValue(DeviceProperty); }
            set { SetValue(DeviceProperty, value); }
        }
        private bool isShowKeypad;
        private bool isInTabItem;
        private NotifyPLCDWord notifyPLCDWord = new NotifyPLCDWord();
        private LoggerDebug logger = new LoggerDebug("DataDWord_D");

        public DataDWord_D()
        {
            InitializeComponent();
            this.Loaded += TxtReadOnlyDWord_Loaded;
            this.Unloaded += TxtReadOnlyDWord_Unloaded;
        }

        private void TxtReadOnlyDWord_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                {
                    // Bỏ qua các hành động trong chế độ Design
                    return;
                }
                if (this.isInTabItem) return;
                this.UnregisterNotify();
                this.monitorCancellation?.Cancel();
                if (this.IsShowInWindow) return;
                this.RemoveDevice();
            }
            catch (Exception ex)
            {
                this.logger.Create("TxtReadOnlyWord_Unloaded: " + ex.Message, LogLevel.Error);
            }
        }

        private void TxtReadOnlyDWord_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.isInTabItem) return;
                this.RemoveDevice();
                this.Initial();
                this.monitorCancellation = new CancellationTokenSource();
                this.AddDevice();
                this.isInTabItem = this.IsTabItem;
            }
            catch (Exception ex)
            {
                this.logger.Create("TxtReadOnlyWord_Loaded: " + ex.Message, LogLevel.Error);
            }
        }

        private void AddDevice()
        {
            if (this.Device == null) return;
            var address = ushort.Parse(this.Device.ToString());
            BLLManager.Instance.PLC.AddAddressDeviceDWord_D(address);
        }
        private void RemoveDevice()
        {
            if (this.Device == null) return;
            var address = ushort.Parse(this.Device.ToString());
            BLLManager.Instance.PLC.RemoveAddressDeviceDWord_D(address);
        }
        private void Initial()
        {
            var value = this.convertValue(Convert.ToDouble(0), NoOfDisplay, NoOfDecimalDigits);
            txt.Text = value;
            this.notifyPLCDWord = SystemsManager.Instance.NotifyPLCDWord;
            this.RegisterNotify();
        }
        private void RegisterNotify()
        {
            this.notifyPLCDWord.Attach(this);
        }
        private void UnregisterNotify()
        {
            this.notifyPLCDWord.Detach(this);
        }
        public string convertValue(double value, int totalDigitsBeforeDecimal, int totalDigitsAfterDecimal)
        {
            string formatString = "";
            try
            {
                string numberString = value.ToString();
                string numberStringSource = value.ToString();
                numberString = numberString.Replace('-', ' ');
                numberString = numberString.Trim();
                formatString = numberString.PadLeft(totalDigitsBeforeDecimal + totalDigitsAfterDecimal, '0');

                // Tìm vị trí của dấu thập phân và chèn dấu thập phân
                int decimalPointIndex = formatString.Length - totalDigitsAfterDecimal;
                if (decimalPointIndex >= 0 && decimalPointIndex < formatString.Length)
                {
                    formatString = formatString.Insert(decimalPointIndex, ".");
                }
                if (numberStringSource.Contains("-"))
                {
                    formatString = "-" + formatString;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("convertValue: " + ex.Message, LogLevel.Error);
            }
            return this.FillString(formatString);
        }
        private string FillString(string input)
        {
            int indexOfMinus = input.IndexOf('-');
            if (indexOfMinus != -1)
            {
                return input.Substring(0, indexOfMinus + 1) + input.Substring(indexOfMinus + 1).TrimStart('0');
            }
            else
            {
                return input;
            }
        }
        private async void txt_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (txt.IsReadOnly) return;
            this.isShowKeypad = true;
            KeyPadNum keypad = new KeyPadNum();
            var number = keypad.ShowKeypad(txt.Text, this.NoOfDecimalDigits, this.NoOfDisplay);
            string x = "1";
            string y = x.PadRight(this.NoOfDecimalDigits + 1, '0');
            var numberData = number.TrimStart('0');
            txt.Text = number;
            if (string.IsNullOrEmpty(numberData)) return;
            int value = (int)(Convert.ToDouble(numberData) * Convert.ToInt32(y));
            var address = ushort.Parse(this.Device.ToString());
            await BLLManager.Instance.PLC.Device.WriteMultiDWords(address,new int[] { value },"D");
            this.EventLog(address + " Changed value " , value.ToString());
            await Task.Delay(100);
            this.isShowKeypad = false;
        }
        private void EventLog(string message, string type)
        {
            try
            {
                var log = new EventLog(message, type);
                SqlLiteEventLog eventLog = new SqlLiteEventLog();
                var eventLogService = new EventLogService(eventLog.EventLogRepository);
                eventLogService.CreateEvent(log);
            }
            catch (Exception ex)
            {
                logger.Create("AlarmLog: " + ex.Message, LogLevel.Error);
            }
        }
        public void NotifyChangeDWord(string key, int value)
        {
            if (this.Device == null) return;
            if (key != this.Device.ToString()) return;
            var data = this.convertValue(Convert.ToDouble(value), NoOfDisplay, NoOfDecimalDigits);
            if (!isShowKeypad)
            {
                txt.Text = data;
                Text = data;
            }
        }
    }
}
