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
using DTO;
using BLL;

namespace GUI
{
    /// <summary>
    /// Interaction logic for PgMESSettingMenu.xaml
    /// </summary>
    public partial class PgAOISettingMenu : Page, IObserverMES
    {
        private LoggerDebug logger = new LoggerDebug("PgMechanicalMESSettingMenu");
        private MESSetting mesSetting;
        private NotifyEvenMES notifyEvenMES;

        public PgAOISettingMenu()
        {
            InitializeComponent();
            this.Loaded += PgMechanicalMESSettingMenu_Loaded;
            this.Unloaded += PgMechanicalMESSettingMenu_Unloaded;

            this.btnSave.Click += BtnSave_Click;
            this.btnSave.TouchDown += BtnSave_Click;

            this.btClose.Click += BtClose_Click;
            this.btClose.TouchDown += BtClose_Click;

            this.btOpen.Click += BtOpen_Click;
            this.btOpen.TouchDown += BtOpen_Click;

            this.btSend001.Click += BtSend001_Click;
            this.btSend001.TouchDown += BtSend001_Click;

            this.btSend031.Click += BtSend031_Click;
            this.btSend031.TouchDown += BtSend031_Click;

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

        private void BtSend031_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                this.logger.Create("BtSend031_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtSend001_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                this.logger.Create("BtSend001_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private async void BtOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await BLLManager.Instance.LoadMES();
                this.chkListen.IsChecked = true;
                this.UpdateCheckAccept(false);
            }
            catch (Exception ex)
            {
                this.logger.Create("BtOpen_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                this.logger.Create("BtClose_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WndComfirm comfirmYesNo = new WndComfirm();
                if (!comfirmYesNo.DoComfirmYesNo("You Want Save Setting?")) return;
                this.SaveSetting();
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnSave_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void PgMechanicalMESSettingMenu_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.UnregisterNotifyMES();
            }
            catch (Exception ex)
            {
                this.logger.Create("PgMechanicalMESSettingMenu_Unloaded: " + ex.Message, LogLevel.Error);
            }
        }

        private void PgMechanicalMESSettingMenu_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.RegisterNotifyMES();
                this.LoadMesSetting();
                this.UpdateMesSettingToUI();
                this.UpdateCheckAccept(BLLManager.Instance.MES.isAccept);
            }
            catch (Exception ex)
            {
                this.logger.Create("PgMechanicalMESSettingMenu_Loaded: " + ex.Message, LogLevel.Error);
            }
        }
        private void LoadMesSetting()
        {
            this.mesSetting = SystemsManager.Instance.AppSettings.MESSetting;
        }
        private void UpdateMesSettingToUI()
        {
            var arrIP = this.mesSetting.Ip.Split('.');
            if (arrIP.Length == 4)
            {
                this.txtLocalIp1.Text = arrIP[0];
                this.txtLocalIp2.Text = arrIP[1];
                this.txtLocalIp3.Text = arrIP[2];
                this.txtLocalIp4.Text = arrIP[3];
            }
            this.txtLocalPort.Text = this.mesSetting.Port.ToString();
            this.txtEquiment.Text = this.mesSetting.EquimentID;
            this.txtBincodeOK.Text = SystemsManager.Instance.AppSettings.SupperOptions.BinCode_OK;
            this.txtBincodeNG.Text = SystemsManager.Instance.AppSettings.SupperOptions.BinCode_NG;
        }
        private void SaveSetting()
        {
            this.mesSetting.Ip = String.Format("{0}.{1}.{2}.{3}",
                    txtLocalIp1.Text, txtLocalIp2.Text, txtLocalIp3.Text, txtLocalIp4.Text);
            this.mesSetting.Port = int.Parse(txtLocalPort.Text);
            this.mesSetting.EquimentID = this.txtEquiment.Text;
            SystemsManager.Instance.AppSettings.SupperOptions.BinCode_OK = this.txtBincodeOK.Text;
            SystemsManager.Instance.AppSettings.SupperOptions.BinCode_NG = this.txtBincodeNG.Text;
            SystemsManager.Instance.SaveAppSettings();
        }
        private void UpdateLogs(string notify)
        {
            this.Dispatcher.Invoke(() => {
                this.txtLog.Text += "\r\n" + notify;
                this.txtLog.ScrollToEnd();
            });
        }
        private void UpdateCheckAccept(bool status)
        {
            if (this.chkAccepted.Dispatcher.CheckAccess())
            {
                this.chkAccepted.IsChecked = status;
                return;
            }
            this.Dispatcher.Invoke(() => {
                this.chkAccepted.IsChecked = status;
            });
        }

        public void CheckConnectionMES(bool connected)
        {
            this.UpdateLogs("Connect: " + connected);
            this.UpdateCheckAccept(connected);
        }

        public void FollowingDataMES(string MESResult)
        {
            this.UpdateLogs("MES GET BACK: " + MESResult);
        }

        public void GetInformationFromClientConnect(string clientIP, int clientPort)
        {
            this.UpdateLogs("Information Client Connect: " + clientIP + " - " + clientPort);
        }

        public void UpdateNotifyToUI(string Notify)
        {
            this.UpdateLogs(Notify);
        }
    }
}
