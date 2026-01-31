using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DTO;
using BLL;
using System.Threading;
using System.Collections.ObjectModel;

namespace DevicePLC
{
    /// <summary>
    /// Interaction logic for LogFileUI.xaml
    /// </summary>
    public partial class LogFileUI : UserControl, IObserverChangeBits
    {
        private LoggerDebug logger = new LoggerDebug("LogFileUI");

        public static readonly DependencyProperty IsShowInWindowProperty = DependencyProperty.Register(
           "IsShowDialog", typeof(bool), typeof(LogFileUI), new PropertyMetadata(false));

        public static new readonly DependencyProperty ActionProperty = DependencyProperty.Register(
    "Action", typeof(ActionType), typeof(LogFileUI), new PropertyMetadata(ActionType.Alternative));

        public static new readonly DependencyProperty DeviceLampProperty = DependencyProperty.Register(
            "DeviceLamp", typeof(DeviceName), typeof(LogFileUI), new PropertyMetadata(DeviceName.M));

        public static new readonly DependencyProperty DeviceWriteProperty = DependencyProperty.Register(
            "DeviceWrite", typeof(DeviceName), typeof(LogFileUI), new PropertyMetadata(DeviceName.M));

        public static new readonly DependencyProperty AddressWriteProperty = DependencyProperty.Register(
            "AddressWrite", typeof(object), typeof(LogFileUI), new PropertyMetadata(null));

        public static readonly DependencyProperty AddressLampProperty = DependencyProperty.Register(
            "AddressLamp", typeof(object), typeof(LogFileUI), new PropertyMetadata(null));

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

        public bool IsShowDialog
        {
            get { return (bool)GetValue(IsShowInWindowProperty); }
            set { SetValue(IsShowInWindowProperty, value); }
        }
        private CancellationTokenSource ioMonitorCancellation;
        public ObservableCollection<LogItem> LogItems = new ObservableCollection<LogItem>();
        private List<KeyValuePair<int, FileAlarm>> alarmList;
        private NotifyPLCBits notifyPLCBits = new NotifyPLCBits();

        public LogFileUI()
        {
            InitializeComponent();
            this.Loaded += LogFileUI_Loaded;
            this.Unloaded += LogFileUI_Unloaded;

            this.btnUp.PreviewMouseDown += BtnUp_PreviewMouseDown;
            this.btnDown.PreviewMouseDown += BtnDown_PreviewMouseDown;
        }
        private void BtnDown_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ScrollListView(1);
        }

        private void BtnUp_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ScrollListView(-1);
        }
        private void ScrollListView(int direction)
        {
            var scrollViewer = GetScrollViewer(lsvLogFile);
            if (scrollViewer != null)
            {
                if (direction < 0)
                {
                    scrollViewer.LineUp();
                }
                else
                {
                    scrollViewer.LineDown();
                }
            }
        }

        private ScrollViewer GetScrollViewer(DependencyObject depObj)
        {
            if (depObj is ScrollViewer viewer) return viewer;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = GetScrollViewer(child);
                if (result != null) return result;
            }

            return null;
        }
        private void LogFileUI_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                {
                    // Bỏ qua các hành động trong chế độ Design
                    return;
                }
                UnregisterNotifyBits();
                this.ioMonitorCancellation?.Cancel();
                this.UnregisterPLCMap();
            }
            catch (Exception ex)
            {
                logger.Create("LogFileUI_Unloaded: " + ex.Message, LogLevel.Error);
            }
        }
        private async void LogFileUI_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                {
                    // Bỏ qua các hành động trong chế độ Design
                    return;
                }
                RegisterNotifyBits();
                this.alarmList = new List<KeyValuePair<int, FileAlarm>>();
                this.alarmList = BLLManager.Instance.AlarmListService.GetAll().ToList();
                this.lsvLogFile.ItemsSource = LogItems;
                this.ioMonitorCancellation = new CancellationTokenSource();
                this.RegisterPLCMap();
                await UpdatePLCTOUI();
            }
            catch(Exception ex)
            {
                logger.Create("LogFileUI_Loaded: " + ex.Message, LogLevel.Error);
            }
        }
        private void RegisterNotifyBits()
        {
            this.notifyPLCBits = SystemsManager.Instance.NotifyPLCBits;
            this.notifyPLCBits.Attach(this);
        }
        private void UnregisterNotifyBits()
        {
            this.notifyPLCBits.Detach(this);
        }
        private async Task UpdatePLCTOUI()
        {
            Dictionary<string, bool> previousState = new Dictionary<string, bool>(BLLManager.Instance.PLC.monitorDeviceBits_M);
            
            while (!this.ioMonitorCancellation.IsCancellationRequested)
            {
                CheckChanged(previousState, BLLManager.Instance.PLC.monitorDeviceBits_M);
                
                await Task.Delay(1);
            }
        }
        private void UpdateContent(string message,string key)
        {
            try
            {
                var newLog = new LogItem();
                newLog.Time = DateTime.Now.ToString();
                newLog.Message = message;
                newLog.Frequense = "1";
                newLog.Status = true;
                newLog.Key = key;

                // Kiểm tra xem có LogItem nào có Message trùng lặp hay không
                var existingLog = LogItems.FirstOrDefault(log => log.Message == newLog.Message);

                if (existingLog != null)
                {
                    // Nếu có, xóa nó khỏi vị trí hiện tại
                    LogItems.Remove(existingLog);

                    // Cập nhật thông tin
                    var newFrequense = int.Parse(existingLog.Frequense);
                    newFrequense++;
                    existingLog.Time = newLog.Time;
                    existingLog.Frequense = newFrequense.ToString();
                    existingLog.Key = newLog.Key;
                    existingLog.Status = newLog.Status;
                    existingLog.Key = newLog.Key;
                    // Đẩy nó lên vị trí phía trên đầu danh sách
                    LogItems.Insert(0, existingLog);
                    //LogItems.Add(existingLog);
                }
                else
                {
                    // Nếu không, thêm một mục mới
                    LogItems.Insert(0, newLog);
                    //LogItems.Add(newLog);
                }
            }
            catch(Exception ex)
            {
                logger.Create("UpdateContent: " + ex.Message, LogLevel.Error);
            }
        }
        private void RegisterPLCMap()
        {
            for (int i = 0; i < this.alarmList.Count; i++)
            {
                BLLManager.Instance.PLC.AddBitAddress(this.alarmList[i].Value.Device, (ushort)this.alarmList[i].Value.DeviceCode);
            }
        }
        private void UnregisterPLCMap()
        {
            for (int i = 0; i < this.alarmList.Count; i++)
            {
                BLLManager.Instance.PLC.RemoveBitAddress(this.alarmList[i].Value.Device, (ushort)this.alarmList[i].Value.DeviceCode);
            }
        }
        private void UpdateBrushRow()
        {
            try
            {
                foreach (LogItem newItem in this.LogItems)
                {
                    if (newItem.Status)
                    {
                        newItem.RowColor = new SolidColorBrush(Colors.Red);
                        continue;
                    }
                    newItem.RowColor = new SolidColorBrush(Colors.White);
                }
                SortLogItems();
                this.Dispatcher.Invoke(() =>
                {
                    this.lsvLogFile.Items.Refresh();
                });
            }
            catch(Exception ex)
            {
                logger.Create("UpdateBrushRow: " + ex.Message,LogLevel.Error);
            }
        }
        private void SortLogItems()
        {
            try
            {
                // Sắp xếp danh sách theo thời gian giảm dần, dòng màu vàng được đặt lên đầu
                var sortedLogItems = LogItems
                    .OrderByDescending(log => log.RowColor.Color == Colors.Yellow)
                    .ThenByDescending(log => DateTime.Parse(log.Time))
                    .ToList();

                LogItems.Clear();

                foreach (var logItem in sortedLogItems)
                {
                    LogItems.Add(logItem);
                }
            }
            catch(Exception ex)
            {
                logger.Create("SortLogItems: " + ex.Message, LogLevel.Error);
            }
        }
        private void CheckChanged(Dictionary<string, bool> previousState, Dictionary<string, bool> currentState)
        {
            try
            {
                List<string> changedKeys = GetChangedKeys(previousState, currentState);
                var alarmList = BLLManager.Instance.AlarmListService.GetAll();
                if (changedKeys.Count > 0)
                {
                    foreach (var key in changedKeys)
                    {
                        var x = key.Remove(0, 1);
                        if (!alarmList.ContainsKey(Convert.ToInt32(x))) continue;
                        var message = alarmList[Convert.ToInt32(x)].Message;
                        var deviceCode = alarmList[Convert.ToInt32(x)].DeviceCode;
                        var device = alarmList[Convert.ToInt32(x)].Device;
                        var solution = alarmList[Convert.ToInt32(x)].Solution;
                        if (!currentState[key])
                        {
                            this.UpdateKeyStatus(x);
                            continue;
                        }
                        this.ShowDialog(message, solution, device + deviceCode);
                        this.UpdateContent(message, x);
                        this.AlarmLog(deviceCode.ToString(), message, solution, "");
                    }
                    this.UpdateBrushRow();
                }
                // Cập nhật previousState
                previousState.Clear();
                foreach (var kvp in currentState)
                {
                    previousState[kvp.Key] = kvp.Value;
                }
            }
            catch(Exception ex)
            {
                logger.Create("CheckChanged: " + ex.Message,LogLevel.Error);
            }
        }
        private List<string> GetChangedKeys(Dictionary<string, bool> previousState, Dictionary<string, bool> currentState)
        {
            List<string> changedKeys = new List<string>();
            try
            {
                foreach (var kvp in currentState)
                {
                    if (previousState.TryGetValue(kvp.Key, out var prevValue) && kvp.Value != prevValue)
                    {
                        changedKeys.Add(kvp.Key);
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Create("GetChangedKeys: " + ex.Message, LogLevel.Error);
            }
            
            return changedKeys;
        }
        private void UpdateKeyStatus(string key)
        {
            try
            {
                for (int i = 0; i < LogItems.Count; i++)
                {
                    if (LogItems[i].Key == key)
                    {
                        LogItems[i].Status = false;
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Create("UpdateKeyStatus: " + ex.Message,LogLevel.Error);
            }  
        }
        private void AlarmLog(string alarmCode, string message, string solution, string mode)
        {
            try
            {
                var alarmService = new SqlLiteAlarmLog();
                var alarm = new AlarmLog(alarmCode, message, solution, mode);
                var alarmLog = new AlarmLogService(alarmService.AlarmLogRepository);
                alarmLog.CreateAlarm(alarm);
            }
            catch(Exception ex)
            {
                logger.Create("AlarmLog: " + ex.Message, LogLevel.Error);
            }
        }

        public void NotifyChangeBits(string key, bool status)
        {
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true; // Hủy bỏ sự kiện chọn
        }

        private void ShowDialog(String msg = "", String solution = "", string code = "")
        {
            if (!this.IsShowDialog) return;
            var circle = FindCodeAlarmCircle(code);
            var image = FindCodeAlarmImage(code);
            AlarmShow wnd = new AlarmShow(circle, image, this.DeviceWrite, this.DeviceLamp, this.Action, ushort.Parse(this.AddressWrite.ToString()), ushort.Parse(this.AddressLamp.ToString()), msg, solution, code);
            Task.Run(() =>
            {
                this.Dispatcher.Invoke(() => { wnd.Show(); });
            });
        }
        private List<CircleInfor> FindCodeAlarmCircle(string code)
        {
            List<CircleInfor> newCircles = new List<CircleInfor>();
            var alarms = SystemsManager.Instance.AppSettings.CircleInfors;
            foreach (CircleInfor circleInfor in alarms)
            {
                string codeAlarm = circleInfor.ID_Alarm.ToString();
                if (codeAlarm != code) continue;
                newCircles.Add(circleInfor);
            }
            if (newCircles.Count <= 0) return null;
            return newCircles;
        }
        private ImageInfor FindCodeAlarmImage(string code)
        {
            var alarms = SystemsManager.Instance.AppSettings.ImageInfors;
            foreach (ImageInfor circleInfor in alarms)
            {
                string codeAlarm = circleInfor.ID_Alarm.ToString();
                if (codeAlarm != code) continue;
                return circleInfor;
            }
            return null;
        }
    }
    public class LogItem
    {
        public string Time { get; set; }
        public bool Status { get; set; }
        public string Key { get; set; }
        public string Message { get; set; }
        public string Frequense { get; set; }
        public SolidColorBrush RowColor { get; set; }
    }
}
