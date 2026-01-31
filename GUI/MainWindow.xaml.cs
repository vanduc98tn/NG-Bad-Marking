using BLL;
using DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Text;
using System.Windows.Controls;
using System;
using System.Windows.Media.Animation;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window , IObserverTCPClient,IObserverCOM, IObserverChangeBits
    {
        public const string AppVersionNumber = "1.0.0";
        public const string AppVersionTime = "12-Dec-2023";
        private LoggerDebug logger = new LoggerDebug("MainWindow");
        private NotifyEvenTCPClient notifyEvenTCPClient = new NotifyEvenTCPClient();
        private NotifyEvenCOM notifyEvenCOM = new NotifyEvenCOM();
        private System.Timers.Timer clock = new System.Timers.Timer(1000);

        private ushort PLC_RUN_TIME = 176;
        private ushort PLC_STOP_TIME = 183;
        private ushort PLC_ALARM_TIME = 99;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;

            this.btPower.Click += BtPower_Click;
            this.btMain.Click += BtMain_Click;
            this.btMenu.Click += BtMenu_Click;
            this.btIO.Click += BtIO_Click;
            this.btLastJam.Click += BtLastJam_Click;
            
        }

        private void BtLastJam_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (UIManager.Instance.IsRunningAuto())
                //{
                //    new WndComfirm().DoComfirmYesNo("Is Running...\r\nStop and Push menu.", this);
                //    return;
                //}
                LogsManager.Instance.EventLogs.CreateEventLog("BtLastJam Clicked");
                UIManager.Instance.SwitchPage(PAGE_ID.PAGE_LAST_JAM);
            }
            catch(Exception ex)
            {
                logger.Create("BtLastJam_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtIO_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (UIManager.Instance.IsRunningAuto())
                //{
                //    new WndComfirm().DoComfirmYesNo("Is Running...\r\nStop and Push menu.", this);
                //    return;
                //}
                LogsManager.Instance.EventLogs.CreateEventLog("BtIO Clicked");
                UIManager.Instance.SwitchPage(PAGE_ID.PAGE_IO);
            }
            catch (Exception ex)
            {
                logger.Create("BtIO_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (UIManager.Instance.IsRunningAuto())
                //{
                //    new WndComfirm().DoComfirmYesNo("Is Running...\r\nStop and Push menu.", this);
                //    return;
                //}
                LogsManager.Instance.EventLogs.CreateEventLog("BtMenu Clicked");
                UIManager.Instance.SwitchPage(PAGE_ID.PAGE_MENU);
            }
            catch (Exception ex)
            {
                logger.Create("BtMenu_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtMain_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LogsManager.Instance.EventLogs.CreateEventLog("BtMain Clicked");
                UIManager.Instance.SwitchPage(PAGE_ID.PAGE_MAIN);
            }
            catch (Exception ex)
            {
                logger.Create("BtMain_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtPower_Click(object sender, RoutedEventArgs e)
        {
            LogsManager.Instance.EventLogs.CreateEventLog("BtPower Clicked");
            this.MainWindow_Closed(this, null);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            this.UnregisterNotifyDevice();
            this.UnregisterNotify();
            this.StopMonitorDevice();
            this.RemoveDevicePLC();
            BLLManager.Instance.ServiceTime.StopTotalTime();
            BLLManager.Instance.ServiceTime.StopNormalTime();
            BLLManager.Instance.ServiceTime.StopPauseTime();
            BLLManager.Instance.ServiceTime.StopAlarmTime();
            Environment.Exit(0);
        }
        private void StopMonitorDevice()
        {
            BLLManager.Instance.PLC.NotUseDevice();
        }
        public void UpdateMainContent(object obj)
        {
            this.mainContent.Navigate(obj);
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.UpdateStatusPLC(BLLManager.Instance.isConnectPLC);
                this.RegisterNotifyDevice();
                this.RegisterDevicePLC();
                BLLManager.Instance.ServiceTime.StartTotalTime();
                clock.AutoReset = true;
                clock.Elapsed += this.Clock_Elapsed;
                clock.Start();

                this.Title = String.Format("Tray Loader - v{0}", AppVersionNumber);
                this.lblVersion.Content = String.Format("Ver {0} ({1})", AppVersionNumber, AppVersionTime);
                UIManager.Instance.SwitchPage(PAGE_ID.PAGE_MAIN);
            }
            catch(Exception ex)
            {
                logger.Create("MainWindow_Loaded: " + ex.Message, LogLevel.Error);
            } 
        }
        private void Clock_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() => {
                try
                {
                    var date = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    var time = string.Format("{0:HH:mm:ss}", DateTime.Now);
                    this.lblCurrentTime.Content = date+ " " +time;
                }
                catch (Exception ex)
                {
                    logger.Create("Clock_Elapsed error:" + ex.Message,LogLevel.Error);
                }
            });
        }
        private void MoveTitle()
        {
            lblNameMachine.Margin = new Thickness(0, 100, 0, 0);

            // Tạo DoubleAnimation để di chuyển Label từ phải sang trái
            DoubleAnimation moveRightAnimation = new DoubleAnimation
            {
                From = this.Width,   // Vị trí bắt đầu (phải)
                To = -lblNameMachine.ActualWidth,  // Vị trí kết thúc (trái)
                Duration = TimeSpan.FromSeconds(2),
                AutoReverse = true,  // Cho phép quay lại
            };

            // Tạo DoubleAnimation để di chuyển Label từ trái sang phải
            DoubleAnimation moveLeftAnimation = new DoubleAnimation
            {
                From = -lblNameMachine.ActualWidth,   // Vị trí bắt đầu (trái)
                To = this.Width,  // Vị trí kết thúc (phải)
                Duration = TimeSpan.FromSeconds(2),
                AutoReverse = true,  // Cho phép quay lại
            };

            // Tạo một Storyboard và thêm cả hai Animation vào đó
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(moveRightAnimation);
            storyboard.Children.Add(moveLeftAnimation);

            // Đặt cài đặt bắt đầu cho Animation
            Storyboard.SetTarget(moveRightAnimation, lblNameMachine);
            Storyboard.SetTargetProperty(moveRightAnimation, new PropertyPath(Canvas.LeftProperty));

            Storyboard.SetTarget(moveLeftAnimation, lblNameMachine);
            Storyboard.SetTargetProperty(moveLeftAnimation, new PropertyPath(Canvas.LeftProperty));

            // Bắt đầu Storyboard khi ứng dụng khởi động và lặp lại vô hạn
            storyboard.RepeatBehavior = RepeatBehavior.Forever;
            storyboard.Begin();
        }
        private void RegisterNotifyDevice()
        {
            if(SystemsManager.Instance.AppSettings.CurrentDeviceType==DeviceType.ModbusTCP || SystemsManager.Instance.AppSettings.CurrentDeviceType == DeviceType.MCProtocolBinaryEthernet)
            {
                this.notifyEvenTCPClient = SystemsManager.Instance.NotifyEvenTCPClient;
                this.notifyEvenTCPClient.Attach(this);
            }
            else
            {
                this.RegisterNotify();
            }
        }
        private void UnregisterNotifyDevice()
        {
            this.notifyEvenTCPClient?.Detach(this);
        }
        private void RegisterNotify()
        {
            this.notifyEvenCOM = SystemsManager.Instance.NotifyEvenCOM;
            this.notifyEvenCOM.Attach(this);
        }
        private void UnregisterNotify()
        {
            this.notifyEvenCOM?.Detach(this);
        }

        private void CheckConnectPLC(string name , bool status)
        {
            if(name== "ModbusRTUCOM" || name == "Modbus TCP" || name == "MCProtocolBinary")
            {
                this.Dispatcher.Invoke(() => {
                    if (!status)
                    {
                        this.lblPLCTimeout.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.lblPLCTimeout.Visibility = Visibility.Hidden;
                    }
                });
            }
        }
        private void UpdateStatusPLC(bool status)
        {
            this.Dispatcher.Invoke(() => {
                if (!status)
                {
                    this.lblPLCTimeout.Visibility = Visibility.Visible;
                }
                else
                {
                    this.lblPLCTimeout.Visibility = Visibility.Hidden;
                }
            });
        }
        public void UpdateResultToUI(string name, string notify)
        {
            
        }
        private void AddDevicePLC()
        {
            BLLManager.Instance.PLC.AddBitAddress("M", this.PLC_RUN_TIME);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PLC_STOP_TIME);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PLC_ALARM_TIME);
        }
        private void RemoveDevicePLC()
        {
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PLC_RUN_TIME);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PLC_STOP_TIME);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PLC_ALARM_TIME);
        }
        private void RegisterDevicePLC()
        {
            SystemsManager.Instance.NotifyPLCBits.Attach(this);
            this.AddDevicePLC();
        }

        public void CheckConnectChange(string name, bool connected)
        {
            this.CheckConnectPLC(name, connected);
        }

        public void UpdateNotifyToUI(string Notify)
        {
            
        }

        public void NotifyChangeBits(string key, bool status)
        {
            if(key=="M"+ this.PLC_RUN_TIME && status)
            {
                BLLManager.Instance.ServiceTime.StartNormalTime();
            }   
            if(key == "M" + this.PLC_RUN_TIME && !status)
            {
                BLLManager.Instance.ServiceTime.StopNormalTime();
            }
            if(key == "M" + this.PLC_STOP_TIME && status)
            {
                BLLManager.Instance.ServiceTime.StartPauseTime();
            }
            if(key == "M" + this.PLC_STOP_TIME && !status)
            {
                BLLManager.Instance.ServiceTime.StopPauseTime();
            }
            if (key == "M" + this.PLC_ALARM_TIME && status)
            {
                BLLManager.Instance.ServiceTime.StartAlarmTime();
            }
            if (key == "M" + this.PLC_ALARM_TIME && !status)
            {
                BLLManager.Instance.ServiceTime.StopAlarmTime();
            }
        }
    }
}
