using BLL;
using DTO;
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
using System.Xml.Linq;

namespace GUI
{
    /// <summary>
    /// Interaction logic for PgMechanicalBarcode.xaml
    /// </summary>
    public partial class PgMechanicalBarcode : Page, IObserverTCPClient
    {
        private LoggerDebug logger = new LoggerDebug("PgMechanicalBarcode");
        private TCPSetting scannerTCPSetting;
        private NotifyEvenTCPClient notifyEvenTCP;
        public PgMechanicalBarcode()
        {
            InitializeComponent();

            this.Loaded += PgMechanicalDelayTimeMenu_Loaded;
            this.Unloaded += PgMechanicalDelayTimeMenu_Unloaded;

            this.btnSave.Click += BtnSave_Click;
            this.btnSave.TouchDown += BtnSave_Click;

            this.btnSetting.Click += BtnSetting_Click;
            this.btnSetting.TouchDown += BtnSetting_Click;

            this.btnOpen.Click += BtnOpen_Click;
            this.btnOpen.TouchDown += BtnOpen_Click;

            this.btnClose.Click += BtnClose_Click;
            this.btnClose.TouchDown += BtnClose_Click;

            this.btnReadQrCode.Click += BtnReadQrCode_Click;
            this.btnReadQrCode.TouchDown += BtnReadQrCode_Click;

            this.btnTurning.Click += BtnTurning_Click;
            this.btnTurning.TouchDown += BtnTurning_Click;

            this.btnForcus.Click += BtnForcus_Click;
            this.btnForcus.TouchDown += BtnForcus_Click;
        }
        private void BtnForcus_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                this.logger.Create("BtnForcus_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtnTurning_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                this.logger.Create("BtnTurning_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private async void BtnReadQrCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var barcode = await BLLManager.Instance.scannerTCP.ReadQrCode();
                UpdateLogs(barcode);
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnReadQrCode_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLLManager.Instance.scannerTCP.Close();
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnClose_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private async void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLLManager.Instance.scannerTCP.Close();
                // Open Scanner;
                if (this.scannerTCPSetting == null)
                {
                    this.UpdateLogs("Scanner config is NULL!!!");
                    return;
                }
                await BLLManager.Instance.LoadScannerTCP();
                if (!BLLManager.Instance.scannerTCP.IsConnected) this.UpdateLogs("Open Scanner Faild!!!");
                else this.UpdateLogs("Open Scanner Sucessful!!!");
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnOpen_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WndTCPSetting wndCom = new WndTCPSetting();
                var settingNew = wndCom.DoSettings(Window.GetWindow(this), this.scannerTCPSetting);
                if (settingNew == null) return;
                this.scannerTCPSetting.Ip = settingNew.Ip;
                this.scannerTCPSetting.Port = settingNew.Port;
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnSetting_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WndComfirm comfirmYesNo = new WndComfirm();
                if (!comfirmYesNo.DoComfirmYesNo("You Want Save Setting?")) return;
                //if (this.cbSelectBank.SelectedValue != null)
                //{
                //    this.scannerComSetting.BankID = this.cbSelectBank.SelectedValue?.ToString();
                //}
                SystemsManager.Instance.AppSettings.SettingDevice.ScannerTCPSetting = this.scannerTCPSetting;
                SystemsManager.Instance.SaveAppSettings();
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnSave_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void PgMechanicalDelayTimeMenu_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.UnregisterNotify();
            }
            catch (Exception ex)
            {
                this.logger.Create("PgMechanicalDelayTimeMenu_Unloaded: " + ex.Message, LogLevel.Error);
            }
        }

        private void PgMechanicalDelayTimeMenu_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.RegisterNotify();
                this.CheckLock();
                this.LoadSettingScanner();
                this.LoadBankCurrent();
            }
            catch (Exception ex)
            {
                this.logger.Create("PgMechanicalDelayTimeMenu_Loaded: " + ex.Message, LogLevel.Error);
            }
        }

        private void LoadSettingScanner()
        {
            this.scannerTCPSetting = SystemsManager.Instance.AppSettings.SettingDevice.ScannerTCPSetting;
        }
        private void LoadBankCurrent()
        {
            try
            {
                //this.cbSelectBank.SelectedValue = this.scannerComSetting.BankID;
            }
            catch (Exception ex)
            {
                this.logger.Create("LoadBankCurrent: " + ex.Message, LogLevel.Error);
            }
        }
        private void CheckLock()
        {
            if (!UserManagers.Instance.CheckAssignLevel(PAGE_ID.PAGE_MENU_MECHANICAL_BARCODE.ToString()))
            {
                UserManagers.Instance.DisableAllControls(this);
            }
            else
            {
                UserManagers.Instance.EnableAllControls(this);
            }
        }
        private void RegisterNotify()
        {
            this.notifyEvenTCP = SystemsManager.Instance.NotifyEvenTCPClient;
            this.notifyEvenTCP.Attach(this);
        }
        private void UnregisterNotify()
        {
            this.notifyEvenTCP?.Detach(this);
        }


        private void UpdateLogs(string notify)
        {
            this.Dispatcher.Invoke(() => {
                this.txtLogs.Text += "\r\n" + notify;
                this.txtLogs.ScrollToEnd();
            });
        }

        public void UpdateResultToUI(string name, string qr)
        {
            if (name == "Scanner TCP")
            {
                this.UpdateLogs(this.Name + qr);
            }

        }

        public void CheckConnectChange(string name, bool connected)
        {
            try
            {
                if (name == "Scanner TCP")
                {
                    this.UpdateLogs(this.Name + "Connect Is " + connected);
                }

            }
            catch (Exception ex)
            {
                this.logger.Create("CheckConnectChange: " + ex.Message, LogLevel.Error);
            }
        }

        public void UpdateNotifyToUI(string Notify)
        {
            try
            {


            }
            catch (Exception ex)
            {
                this.logger.Create("UpdateNotifyToUI: " + ex.Message, LogLevel.Error);
            }
        }
    }
}
