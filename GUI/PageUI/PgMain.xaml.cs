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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using DevicePLC;
using BLL;
using DTO;
using System.Windows.Markup;
using System.Reflection.Emit;
using System.Timers;

namespace GUI
{
    /// <summary>
    /// Interaction logic for pgMain.xaml
    /// </summary>
    public partial class PgMain : Page,IObserverChangeBits,IObserverCOM,IObserverTime, IObserverMES
    {
        private LoggerDebug logger = new LoggerDebug("PgMain");
        private NotifyEvenCOM notifyEvenCOM;
        private bool isRunning = false;
        private bool isShowReverse = false;
        private NotifyEvenMES notifyEvenMES;

        private LotInData lotInData;
        //MAP Write PLC
        private ushort plcBitErrorReadCode_W = 1119;
        private ushort plcBitAutoMode_W = 328;
        private ushort plcBitManualMode_W = 329;
        private ushort PC_AOI_OK_PASS_W = 1121;
        private ushort PC_AOI_NG_W = 1122;
        private ushort PC_AOI_ERROR_W = 1118;
        private ushort plcRequestReadAOI_W = 1120; //M700
        private ushort PLC_READY_POS_W = 1104; // M690
        private ushort PC_COMPLETE_WRITE_POS_W = 1123; // M703
        private ushort PC_WRITE_ALL_POS_COMPLETE_W = 1124; // M704
        private ushort PLC_READY_LASER_W = 1125;// M705
        private ushort PLC_REVERSE_COMPLETE_W = 1126;// M706
        private ushort PLC_READY_POS_Reveser_W = 1105; // M691
        private ushort X_Data_D4010_W = 4010;
        private ushort Y_Data_D4012_W = 4012;
        //MAP Read PLC
        private ushort PLC_AUTO_RUN = 176; //M110
        private ushort plcRequestReadBarcode_M700 = 1120; //M700
        private ushort plcRequestPOSNGNormal_M690 = 1104; // M690
        private ushort plcRequestPOSNGReveser_R_M691 = 1105; // M691
        private ushort PLC_READY_LASER_R = 1125;// M705
        private ushort PLC_REVERSE_COMPLETE_R = 1126;// M70F
        private ushort PC_ON_Start = 1135; //M70F
        private ushort PC_Machine_Error = 1136; //M710
        private ushort PC_Machine_Not_Home = 1137; //M711
        private ushort PC_Machine_Not_Ready = 1138; //M712
        private ushort PC_Machine_Is_Coming_Home = 1139; //M713
        private ushort PC_Machine_Is_Coming_Ready = 1140; //M714
        private ushort PC_JIG_SUPPLY = 1141; //M715
        private ushort PC_Conf_Data_AOI = 1142; //M716
        private ushort PC_Reverse_JIG = 1143; //M717
        private ushort PC_Trigger_Laser = 1144; //M718
        private ushort PC_Out_Jig = 1145; //M719
        private ushort PC_Machine_Runing = 1146; //M71A
        

        public PgMain()
        {
            InitializeComponent();
            this.Loaded += PgMain_Loaded;
            this.Unloaded += PgMain_Unloaded;

            this.btnLotIn.Click += BtnLotIn_Click;
            this.btnLotOut.Click += BtnLotOut_Click;

            //this.txtTest.Click += TxtTest_Click;
        }

        private void BtnLotOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //string a = "BAMARK01 E001";
                //bool x = a.Contains("E001");


                WndComfirm wndComfirm = new WndComfirm();
                if (!wndComfirm.DoComfirmYesNo("You want lot end , parameter will clear?")) return;
                this.lotInData = new LotInData();
                SystemsManager.Instance.AppSettings.LotInData = this.lotInData;
                SystemsManager.Instance.SaveAppSettings();
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnLotOut_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtnLotIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WndLotIn wndLot = new WndLotIn();
                var newLot = wndLot.DoSettings(Window.GetWindow(this), this.lotInData);
                if (newLot == null) return;
                this.lotInData = newLot;
                SystemsManager.Instance.AppSettings.LotInData = this.lotInData;
                UpdateInformationLOTIN(this.lotInData);
                SystemsManager.Instance.SaveAppSettings();
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnLotIn_Click: " + ex.Message, LogLevel.Error);
            }
        }
        private void UpdateInformationLOTIN(LotInData lotin)
        {
            this.lblLotNo.Content = lotin.LotId?.ToString();
            this.lblDevice.Content = lotin.DeviceId?.ToString();
            this.lblInputCount.Content = lotin.LotQty.ToString();
            this.lblOkCount.Content = lotin.OKCount.ToString();
            this.lblNgCount.Content = lotin.NGCount.ToString();
            this.lblTotalCount.Content = lotin.TotalCount.ToString();
        }
        private void PgMain_Unloaded(object sender, RoutedEventArgs e)
        {
            this.SetModeManual();
            this.UnregisterNotifyCOM();
            this.UnregisterDevicePLC();
            this.UnregisterNotifyMES();
            this.UnregisterTimeMachine();
            
        }
        private void LoadInformationMES()
        {
            var arr = SystemsManager.Instance.AppSettings.MESSetting.Ip.Split('.');
            if (arr.Length == 4)
            {
                this.txtLocalIp1.Text = arr[0];
                this.txtLocalIp2.Text = arr[1];
                this.txtLocalIp3.Text = arr[2];
                this.txtLocalIp4.Text = arr[3];
            }
            this.txtMcsLocalPort.Text = SystemsManager.Instance.AppSettings.MESSetting.Port.ToString();
            this.UpdateCheckAccept(BLLManager.Instance.MES.isAccept);
            if (BLLManager.Instance.MES.InformationClient != null)
                UpdateInformationClientMES("Client Connected: " + BLLManager.Instance.MES.InformationClient);
            else this.UpdateInformationClientMES("Listen...");
        }
        private void LoadStatusDevice()
        {
            if(SystemsManager.Instance.AppSettings.RunSetting.AOIOnline)
            {
                this.lblAOIMode.Content = "Online";
                this.lblAOIMode.Background = Brushes.Green;
            }
            else
            {
                this.lblAOIMode.Content = "Offline";
                this.lblAOIMode.Foreground = Brushes.Black;
                this.lblAOIMode.Background = Brushes.Yellow;
            }
            this.UpdateStatusConnectMES(BLLManager.Instance.MES.isAccept);
            this.UpdateStatusConnectSCANNER(BLLManager.Instance.scannerTCP.IsConnected);
        }
        private void LoadLotData()
        {
            this.lotInData = SystemsManager.Instance.AppSettings.LotInData;
        }
        private void RegisterNotifySCANNER()
        {
            this.notifyEvenCOM = SystemsManager.Instance.NotifyEvenCOM;
            this.notifyEvenCOM.Attach(this);
        }
        private void UnregisterNotifyCOM()
        {
            this.notifyEvenCOM?.Detach(this);
        }
        private void PgMain_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadLotData();
            this.UpdateInformationLOTIN(this.lotInData);
            this.LoadTimeRunMachine();
            this.RegisterNotifySCANNER();
            this.RegisterNotifyMES();
            this.RegisterDevicePLC();
            this.RegisterTimeMachine();
            this.LoadInformationMES();
            this.SetModeAuto();
            this.generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, SystemsManager.Instance.currentModel.Pattern.CurrentPatern, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
            this.isShowMESOK = false;
            this.LoadStatusDevice();
            this.LoadCurrentNameModel();
            
        }
        private void LoadTimeRunMachine()
        {
            this.TotalTime(FormatElapsedTime(SystemsManager.Instance.AppSettings.TimeInfor.TotalTime));
            this.NormalTime(FormatElapsedTime(SystemsManager.Instance.AppSettings.TimeInfor.NormalRunTime));
            this.StopTime(FormatElapsedTime(SystemsManager.Instance.AppSettings.TimeInfor.StopTime));
            this.AlarmTime(FormatElapsedTime(SystemsManager.Instance.AppSettings.TimeInfor.AlarmTime));
        }
        private string FormatElapsedTime(TimeSpan elapsed)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds);
        }
        private void LoadCurrentNameModel()
        {
            this.lblModelName.Content = SystemsManager.Instance.currentModel.ModelName;
        }
        private async void SetModeAuto()
        {
            await BLLManager.Instance.PLC.Device.WriteBit(plcBitAutoMode_W, true, "M");
            await BLLManager.Instance.PLC.Device.WriteBit(plcBitManualMode_W, false, "M");
        }
        private async void SetModeManual()
        {
            await BLLManager.Instance.PLC.Device.WriteBit(plcBitManualMode_W, true, "M");
            await BLLManager.Instance.PLC.Device.WriteBit(plcBitAutoMode_W, false, "M");
        }
        public bool IsRunningAuto()
        {
            return this.isRunning;
        }
        private void Logout()
        {
            UserManagers.Instance.Logout();
        }
        private void RegisterDevicePLC()
        {
            SystemsManager.Instance.NotifyPLCBits.Attach(this);
            this.AddDevicePLC();
        }
        private void UnregisterTimeMachine()
        {
            SystemsManager.Instance.NotifyEvenTime.Detach(this);
        }
        private void RegisterNotifyMES()
        {
            this.notifyEvenMES = SystemsManager.Instance.NotifyEvenMES;
            this.notifyEvenMES.Attach(this);
        }
        private void UnregisterNotifyMES()
        {
            this.notifyEvenMES?.Detach(this);
        }
        private void RegisterTimeMachine()
        {
            SystemsManager.Instance.NotifyEvenTime.Attach(this);
        }
        private void UnregisterDevicePLC()
        {
            SystemsManager.Instance.NotifyPLCBits.Detach(this);
            this.RemoveDevicePLC();
        }
        private void AddDevicePLC()
        {
            BLLManager.Instance.PLC.AddBitAddress("M",this.plcRequestReadBarcode_M700);
            BLLManager.Instance.PLC.AddBitAddress("M", this.plcRequestPOSNGNormal_M690);
            BLLManager.Instance.PLC.AddBitAddress("M", this.plcRequestPOSNGReveser_R_M691);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PLC_READY_LASER_R);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PLC_REVERSE_COMPLETE_R);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PC_ON_Start);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PC_Machine_Error);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PC_Machine_Not_Home);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PC_Machine_Not_Ready);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PC_Machine_Is_Coming_Home);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PC_Machine_Is_Coming_Ready);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PC_JIG_SUPPLY);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PC_Conf_Data_AOI);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PC_Reverse_JIG);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PC_Trigger_Laser);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PC_Out_Jig);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PC_Machine_Runing);
            BLLManager.Instance.PLC.AddBitAddress("M", this.PLC_AUTO_RUN);
        }
        private void RemoveDevicePLC()
        {
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.plcRequestReadBarcode_M700);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.plcRequestPOSNGNormal_M690);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PLC_READY_LASER_R);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PLC_REVERSE_COMPLETE_R);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.plcRequestPOSNGReveser_R_M691);

            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PC_ON_Start);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PC_Machine_Error);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PC_Machine_Not_Home);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PC_Machine_Not_Ready);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PC_Machine_Is_Coming_Home);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PC_Machine_Is_Coming_Ready);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PC_JIG_SUPPLY);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PC_Conf_Data_AOI);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PC_Reverse_JIG);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PC_Trigger_Laser);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PC_Out_Jig);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PC_Machine_Runing);
            BLLManager.Instance.PLC.RemoveBitAddress("M", this.PLC_AUTO_RUN);
        }
        private void UpdateLogs(string notify)
        {
            this.Dispatcher.Invoke(() => {
                this.txtLog.Text += "\r\n" + DateTime.Now.ToString() + ": " + notify;
                this.txtLog.ScrollToEnd();
            });
            this.logger.Create(notify, LogLevel.Debug);
        }
        private void UpdateStatusMachine(string key, bool status)
        {
            this.Dispatcher.Invoke(() => {
                if (key == "M" + PC_ON_Start.ToString() && status)
                {
                    lblSttRunning.Content = "NHẤN NÚT START TRÊN MÀN HÌNH ĐỂ CHẠY";
                }
                if (key == "M" + PC_Machine_Error.ToString() && status)
                {
                    lblSttRunning.Content = "MÁY ĐANG LỖI, ẤN NÚT RESET ĐỂ XÓA LỖI";
                }
                if (key == "M" + PC_Machine_Not_Home.ToString() && status)
                {
                    lblSttRunning.Content = "MÁY CHƯA VỀ HOME, ẤN NÚT INITIAL TRÊN PHẦN MỀM 2S ĐỂ MÁY VỀ HOME";
                }
                if (key == "M" + PC_Machine_Not_Ready.ToString() && status)
                {
                    lblSttRunning.Content = "MÁY CHƯA VỀ SẴN SÀNG, ẤN NÚT NÚT START BÊN NGOÀI 2S";
                }
                if (key == "M" + PC_Machine_Is_Coming_Home.ToString() && status)
                {
                    lblSttRunning.Content = "MÁY ĐANG VỀ HOME!";
                }
                if (key == "M" + PC_Machine_Is_Coming_Ready.ToString() && status)
                {
                    lblSttRunning.Content = "MÁY ĐANG VỀ CHẾ ĐỘ READY!";
                }
                if (key == "M" + PC_JIG_SUPPLY.ToString() && status)
                {
                    lblSttRunning.Content = "CHẾ ĐỘ AUTO , YÊU CẦU CUNG CẤP JIG!";
                }
                if (key == "M" + PC_Conf_Data_AOI.ToString() && status)
                {
                    lblSttRunning.Content = "CHẾ ĐỘ AUTO , GỬI DỮ LIỆU SANG MÁY AOI!";
                }
                if (key == "M" + PC_Reverse_JIG.ToString() && status)
                {
                    lblSttRunning.Content = "CHẾ ĐỘ AUTO , ĐANG LẬT NGƯỢC JIG!";
                }
                if (key == "M" + PC_Trigger_Laser.ToString() && status)
                {
                    lblSttRunning.Content = "CHẾ ĐỘ AUTO , ĐANG TIẾN HÀNH KHẮC NG CHO JIG!";
                }
                if (key == "M" + PC_Out_Jig.ToString() && status)
                {
                    lblSttRunning.Content = "CHẾ ĐỘ AUTO , ĐANG OUT JIG RA BÊN NGOÀI!";
                }
                if (key == "M" + PC_Machine_Runing.ToString() && status)
                {
                    lblSttRunning.Content = "CHẾ ĐỘ AUTO , MÁY ĐANG CHẠY!";
                }
            });
        }
        private void CheckPLCAutoRunning(string key, bool status)
        {
            if(key=="M"+this.PLC_AUTO_RUN && status)
            {
                this.isRunning = true;
            }
            else if(key == "M" + this.PLC_AUTO_RUN && !status)
            { this.isRunning = false; }
        }
        private async Task ProcessReadAOI()
        {
            this.PaternAoiResult = SystemsManager.Instance.currentModel.Pattern.CurrentPatern;
            this.UpdateLogs("Start Process Read MES...");
            this.isShowReverse = false;
            //Reset bit trigger.
            await BLLManager.Instance.PLC.Device.WriteBit(this.plcRequestReadAOI_W, false, "M");
            var barcode = await BLLManager.Instance.scannerTCP.ReadQrCode(SystemsManager.Instance.AppSettings.ScannerSetting.BankID);
            barcode = barcode.Replace("\r", "");
            barcode = barcode.Replace("\n", "");
            barcode = barcode.Replace("\r\n", "");
            this.lblBarcode.Content = barcode;
            if ((barcode =="ERROR"|| barcode == "ERROR\r" || barcode == "ERROR\n" || barcode == "ERROR\r\n" || string.IsNullOrEmpty(barcode)) && SystemsManager.Instance.AppSettings.RunSetting.AOIOnline)
            {
                UpdateLogs("PC Write Bits Error Read Code!");
                await BLLManager.Instance.PLC.Device.WriteBit(this.plcBitErrorReadCode_W, true, "M");
                return;
            } 
            this.UpdateLogs("Read Data From MES...");
            if(SystemsManager.Instance.AppSettings.RunSetting.AOIOnline)
            {
                UpdateLogs("CHECK ONLINE!!!");
                await this.MESCHECK(barcode);
            }
            else
            {
                UpdateLogs("CHECK OFFLINE!!!");
                await this.MESCHECKOFFLINE(barcode);
            }
        }
        private async Task ProcessSendDataTriggerLaserToPLC()
        {
           
            this.UpdateLogs("Start Process Send Data Trigger Laser...");
            this.isShowReverse = false;
            if (lstDataAoiResultNormal == null) return;
            UpdateLogs(lstDataAoiResultNormal.Count.ToString());
            if (lstDataAoiResultNormal.Count<=0 || lstDataAoiResultNormal == null)
            {
                await BLLManager.Instance.PLC.Device.WriteBit(this.PC_WRITE_ALL_POS_COMPLETE_W, true, "M");
                await BLLManager.Instance.PLC.Device.WriteBit(this.PC_COMPLETE_WRITE_POS_W, false, "M");
                await BLLManager.Instance.PLC.Device.WriteBit(this.plcRequestPOSNGNormal_M690, false, "M");
                UpdateLogs("PC Write Complete Pos NG!");
                return;
            }    
            var x = findPos(lstDataAoiResultNormal[0], this.PaternAoiResult, "X");
            var y = findPos(lstDataAoiResultNormal[0], this.PaternAoiResult, "Y");
            await BLLManager.Instance.PLC.Device.WriteMultiDWords(X_Data_D4010_W, new int[] { x, y }, "D");
            //logger.Create("Position X: " + x,LogLevel.Debug);
            //logger.Create("Position Y: " + y, LogLevel.Debug);
            UpdateLogs("Position X: " + x);
            UpdateLogs("Position Y: " + y);
            await BLLManager.Instance.PLC.Device.WriteMultiBits(this.plcRequestPOSNGNormal_M690, new bool[] { false }, "M");
            await BLLManager.Instance.PLC.Device.WriteMultiBits(this.PC_COMPLETE_WRITE_POS_W, new bool[] { true }, "M");
            UpdateLogs("Find Pos Start");
            var tsk = Task.Run(async () => {
                Dispatcher.Invoke(() => {
                    var lbl = findCellControl(gridResults, lstDataAoiResultNormal[0]);
                    lbl.Content = "NG";
                });
            });
            lstDataAoiResultNormal.RemoveAt(0);
            UpdateLogs("Process End");
        }
        private async Task ProcessSendDataTriggerLaserToPLCReverse()
        {
            
            if (lstDataAoiResultReverse == null) return;
            this.UpdateLogs("Start Process Send Data Trigger Laser Reversed...");
            await BLLManager.Instance.PLC.Device.WriteBit(this.plcRequestPOSNGNormal_M690, false, "M");
            // show Ui Reverse.
            int patternReverse = ShowUiReverse();
            UpdateLogs("PC Send Bits AOI NG Reverse!");
            this.isShowReverse = true;
            if (lstDataAoiResultReverse.Count <= 0 || lstDataAoiResultReverse == null)
            {
                await BLLManager.Instance.PLC.Device.WriteBit(this.PLC_READY_POS_Reveser_W, false, "M");
                await BLLManager.Instance.PLC.Device.WriteBit(this.PC_WRITE_ALL_POS_COMPLETE_W, true, "M");
                UpdateLogs("PC Write Complete Pos Reverse NG!");
                return;
            }
            var x = findPos(lstDataAoiResultReverse[0], patternReverse, "X");
            var y = findPos(lstDataAoiResultReverse[0], patternReverse, "Y");
            await BLLManager.Instance.PLC.Device.WriteMultiDWords(X_Data_D4010_W, new int[] { x, y }, "D");
            //logger.Create("Position X Reverse: " + x, LogLevel.Debug);
            //logger.Create("Position Y Reverse: " + y, LogLevel.Debug);
            UpdateLogs("Position X Reverse: " + x);
            UpdateLogs("Position Y Reverse: " + y);
            await BLLManager.Instance.PLC.Device.WriteMultiBits(this.PLC_READY_POS_Reveser_W, new bool[] { false }, "M");
            await BLLManager.Instance.PLC.Device.WriteMultiBits(this.PC_COMPLETE_WRITE_POS_W, new bool[] { true }, "M");
            UpdateLogs("Find Pos Start");
            var tsk = Task.Run(async () => {
                this.Dispatcher.Invoke(() => {
                    var lbl = findCellControl(gridResults, lstDataAoiResultReverse[0]);
                    lbl.Content = "NG";
                });
            });
            lstDataAoiResultReverse.RemoveAt(0);
            UpdateLogs("Process End");
        }

        public async void NotifyChangeBits(string key, bool status)
        {
            UpdateStatusMachine(key,status);
            CheckPLCAutoRunning(key, status);
            if (key=="M"+this.plcRequestReadBarcode_M700&& status)
            {
                await ProcessReadAOI();
            }
            else if(key == "M" + this.plcRequestPOSNGNormal_M690 && status)
            {
                await ProcessSendDataTriggerLaserToPLC();
            }
            else if (key == "M" + this.plcRequestPOSNGReveser_R_M691 && status)
            {
                await ProcessSendDataTriggerLaserToPLCReverse();
            }
        }

        public void UpdateResultToUI(string name, string notify)
        { 
        }

        public void CheckConnectChange(string name, bool connected)
        {
            if (name == "ScannerCOM")
            {
                this.UpdateStatusConnectSCANNER(connected);
            }
            //if(name== "Scanner TCP")
            //{
            //    this.UpdateStatusConnectSCANNER(connected);
            //}
        }
        public void UpdateNotifyToUI(string Notify)
        {
            
        }

        public void UpdateTimeTotal(string time)
        {
            this.TotalTime(time);
        }
        private void UpdateStatusConnectSCANNER(bool status)
        {
            if (this.lblScannerStatus.Dispatcher.CheckAccess())
            {
                if (status)
                {
                    this.lblScannerStatus.Content = "Connected";
                    this.lblScannerStatus.Background = Brushes.Green;
                    return;
                }
                this.lblScannerStatus.Content = "Disconnect";
                this.lblScannerStatus.Background = Brushes.Red;

            }
            this.Dispatcher.Invoke(() => {
                if (status)
                {
                    this.lblScannerStatus.Content = "Connected";
                    this.lblScannerStatus.Background = Brushes.Green;
                    return;
                }
                this.lblScannerStatus.Content = "Disconnect";
                this.lblScannerStatus.Background = Brushes.Red;

            });
        }
        private void NormalTime(string time)
        {
            this.Dispatcher.Invoke(() => {
                this.lblRunNormalTime.Content = time;
            });
        }
        private void StopTime(string time)
        {
            this.Dispatcher.Invoke(() => {
                this.lblStopTime.Content = time;
            });
        }
        private void AlarmTime(string time)
        {
            this.Dispatcher.Invoke(() => {
                this.lblAlarmTime.Content = time;
            });
        }
        private void TotalTime(string time)
        {
            this.Dispatcher.Invoke(() => {
                this.lblTotalTimeRun.Content = time;
            });
        }
        public void UpdateTimeNormal(string time)
        {
            NormalTime(time);
        }

        public void UpdateTimeStop(string time)
        {
            StopTime(time);
        }

        public void UpdateTimeAlarm(string time)
        {
            AlarmTime(time);
        }
        private void UpdateStatusConnectMES(bool status)
        {
            if (this.lblMESConnect.Dispatcher.CheckAccess())
            {
                if (status)
                {
                    this.lblMESConnect.Content = "Connected";
                    this.lblMESConnect.Background = Brushes.Green;
                    return;
                }
                this.lblMESConnect.Content = "Disconnect";
                this.lblMESConnect.Background = Brushes.Red;

            }
            this.Dispatcher.Invoke(() => {
                if (status)
                {
                    this.lblMESConnect.Content = "Connected";
                    this.lblMESConnect.Background = Brushes.Green;
                    return;
                }
                this.lblMESConnect.Content = "Disconnect";
                this.lblMESConnect.Background = Brushes.Red;
            });
        }
        private void UpdateCheckAccept(bool status)
        {
            if (this.chkMcsAccepted.Dispatcher.CheckAccess())
            {
                this.chkMcsAccepted.IsChecked = status;
                this.chkMcsListen.IsChecked = !status;
                return;
            }
            this.Dispatcher.Invoke(() => {
                this.chkMcsAccepted.IsChecked = status;
                this.chkMcsListen.IsChecked = !status;
            });
        }
        private void UpdateInformationClientMES(string content)
        {
            if (this.lblStatusMES.Dispatcher.CheckAccess())
            {
                this.lblStatusMES.Content = content;
                return;
            }
            this.Dispatcher.Invoke(() => {
                this.lblStatusMES.Content = content;
            });
        }
        public void CheckConnectionMES(bool connected)
        {
            this.UpdateCheckAccept(connected);
            this.UpdateStatusConnectMES(connected);
            if (!connected)
            {
                this.UpdateInformationClientMES("Listen...");
            }
        }

        public void FollowingDataMES(string MESResult)
        {
            this.UpdateLogs("MES RESPONSE: " + MESResult);
        }

        public void GetInformationFromClientConnect(string clientIP, int clientPort)
        {
            this.UpdateInformationClientMES("Client Connected: " + clientIP + "-" + clientPort);
        }
    }
}
