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
    /// Interaction logic for PgMechanicalMenu.xaml
    /// </summary>
    public partial class PgMechanicalPLCMenu : Page, IObserverTCPClient
    {
        private LoggerDebug logger = new LoggerDebug("PgMechanicalPLCMenu");
        private SettingDevice settingDevice;
        public PgMechanicalPLCMenu()
        {
            InitializeComponent();
            this.Loaded += PgMechanicalPLCMenu_Loaded;
            this.Unloaded += PgMechanicalPLCMenu_Unloaded;

            this.btLogClear.Click += BtLogClear_Click;

            this.btnSave.Click += BtnSave_Click;
            this.btnOpen.Click += BtnOpen_Click;
            this.btnClose.Click += BtnClose_Click;

            this.btnSettingDevice.Click += BtnSettingDevice_Click;

            this.btnReadBit.Click += BtnReadBit_Click;
            this.btnWriteBit.Click += BtnWriteBit_Click;
            this.btnReadWord.Click += BtnReadWord_Click;
            this.btnWriteWord.Click += BtnWriteWord_Click;

        }

        private void BtnSettingDevice_Click(object sender, RoutedEventArgs e)
        {
            if (this.cbSelectDeviceType.SelectedValue == null) return;
            if(this.cbSelectDeviceType.SelectedValue.ToString() == "ModbusRTU")
            {
                WndMbComSetting wndMB = new WndMbComSetting();
                var settingNew = wndMB.DoSettings(Window.GetWindow(this), this.settingDevice.modbusComSetting);
                if(settingNew!=null)
                {
                    this.settingDevice.modbusComSetting = settingNew;
                }
            }
            else if (this.cbSelectDeviceType.SelectedValue.ToString() == "ModbusTCP")
            {
                WndMbTCPSetting wndMB = new WndMbTCPSetting();
                var settingNew = wndMB.DoSettings(Window.GetWindow(this), this.settingDevice.modbusTCPSetting);
                if (settingNew != null)
                {
                    this.settingDevice.modbusTCPSetting = settingNew;
                }
            }
            else if (this.cbSelectDeviceType.SelectedValue.ToString() == "MCProtocolBinaryEthernet")
            {
                wndMCESetting wndMC = new wndMCESetting();
                var settingNew = wndMC.DoSettings(Window.GetWindow(this), this.settingDevice.MCEProtocolBinarySetting);
                if (settingNew != null)
                {
                    this.settingDevice.MCEProtocolBinarySetting = settingNew;
                }
            }
        }

        private void BtLogClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.txtLogs.Text = string.Empty;
            }
            catch(Exception ex)
            {
                this.logger.Create("BtLogClear_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private async void BtnWriteWord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ushort address;
                if (string.IsNullOrEmpty(this.txtAddressReadBit.Text) || !ushort.TryParse(this.txtAddressReadBit.Text, out address))
                {
                    MessageBox.Show("Address Invalid!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                short value;
                if (string.IsNullOrEmpty(this.txtValueWriteWord.Text) || !short.TryParse(this.txtValueWriteWord.Text, out value))
                {
                    MessageBox.Show("Value Invalid!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var result = await BLLManager.Instance.PLC.Device.WriteSignedWord(address, value);
                this.UpdateLogs("Write Word: " + result.ToString());
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnWriteWord_Click: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("WriteWord: " + ex.Message);
            }
        }

        private async void BtnReadWord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ushort address;
                if (string.IsNullOrEmpty(this.txtAddressReadBit.Text) || !ushort.TryParse(this.txtAddressReadBit.Text, out address))
                {
                    MessageBox.Show("Address Invalid!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var result = await BLLManager.Instance.PLC.Device.ReadSignedWord(address);
                this.UpdateLogs("Read Word: " + result.ToString());
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnReadWord_Click: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("ReadWord: " + ex.Message);
            }
        }

        private async void BtnWriteBit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ushort address;
                if (string.IsNullOrEmpty(this.txtAddressReadBit.Text) || !ushort.TryParse(this.txtAddressReadBit.Text, out address))
                {
                    MessageBox.Show("Address Invalid!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if(cboValueWriteBit.SelectedItem==null)
                {
                    this.UpdateLogs("Value Is Not Select!!!");
                    return;
                }
                bool value;
                if (cboValueWriteBit.SelectedItem.ToString()=="false")
                {
                    value = false;
                }
                else
                {
                    value = true;
                }
                var result = await BLLManager.Instance.PLC.Device.WriteBit(address, value);
                this.UpdateLogs("Write Bit: " + result.ToString());
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnWriteBit_Click: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("WriteBit: " + ex.Message);
            }
        }

        private async void BtnReadBit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ushort address;
                if (string.IsNullOrEmpty(this.txtAddressReadBit.Text) || !ushort.TryParse(this.txtAddressReadBit.Text, out address))
                {
                    MessageBox.Show("Address Invalid!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var result = await BLLManager.Instance.PLC.Device.ReadBit(address);
                this.UpdateLogs("Read Bit: " + result.ToString());
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnReadBit_Click: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("ReadBit: " + ex.Message);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLLManager.Instance.PLC.Device.Close();
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnClose_Click: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("Close: " + ex.Message);
            }
        }

        private async void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await BLLManager.Instance.PLC.Device.Open();
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnOpen_Click: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("Open_Click: " + ex.Message);
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
                this.UpdateLogs("Save: " + ex.Message);
            }
        }

        private async void PgMechanicalPLCMenu_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.UnregisterNotify();
                var conneted = await BLLManager.Instance.PLC.Device.Open();
                UIManager.Instance.isConnectDevice = conneted;
            }
            catch (Exception ex)
            {
                this.logger.Create("PgMechanicalPLCMenu_Unloaded: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("PgMechanicalPLCMenu_Unloaded: " + ex.Message);
            }
        }

        private void PgMechanicalPLCMenu_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                this.LoadSetting();
                this.RegisterNotify();
                CheckLock();
            }
            catch (Exception ex)
            {
                this.logger.Create("PgMechanicalPLCMenu_Loaded: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("PgMechanicalPLCMenu_Loaded: " + ex.Message);
            }
        }
        private void CheckLock()
        {
            if (!UserManagers.Instance.CheckAssignLevel(PAGE_ID.PAGE_MENU_MECHANICAL_DEVICE.ToString()))
            {
                UserManagers.Instance.DisableAllControls(this);
            }
            else
            {
                UserManagers.Instance.EnableAllControls(this);
            }
        }
        public void CheckConnectChange(string name, bool connected)
        {
            try
            {
                this.UpdateLogs("Connect: "+connected.ToString());
            }
            catch (Exception ex)
            {
                this.logger.Create("CheckConnectChange: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("CheckConnectChange: " + ex.Message);
            }
        }

        public void UpdateNotifyToUI(string Notify)
        {
            try
            {
                this.UpdateLogs(Notify);
            }
            catch (Exception ex)
            {
                this.logger.Create("UpdateNotifyToUI: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("UpdateNotifyToUI: " + ex.Message);
            }
        }

        public void UpdateResultToUI(string name, string notify)
        {
            try
            {
                this.UpdateLogs(notify);
            }
            catch (Exception ex)
            {
                this.logger.Create("UpdateResultToUI: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("UpdateResultToUI: " + ex.Message);
            }
        }
        private void UpdateLogs(string notify)
        {
            this.Dispatcher.Invoke(() => {
                this.txtLogs.Text += "\r\n" +notify;
                this.txtLogs.ScrollToEnd();
            });
        }
        private void LoadSetting()
        {
            try
            {
                this.LoadDeviceSetting();
            }
            catch(Exception ex)
            {
                this.logger.Create("LoadSetting: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("LoadSetting: " + ex.Message);
            }
        }
        private void SaveSetting()
        {
            try
            {
                this.SaveDeviceTypeSetting();
                SystemsManager.Instance.SaveAppSettings();
            }
            catch(Exception ex)
            {
                this.logger.Create("SaveSetting: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("SaveSetting: " + ex.Message);
            }  
        }
        private void SaveDeviceTypeSetting()
        {
            try
            {
                if (this.cbSelectDeviceType.SelectedValue == null) return;
                if (this.cbSelectDeviceType.SelectedValue.ToString() == "ModbusRTU")
                {
                    SystemsManager.Instance.AppSettings.CurrentDeviceType = DeviceType.ModbusRTU;
                }
                else if(this.cbSelectDeviceType.SelectedValue.ToString() == "ModbusTCP")
                {
                    SystemsManager.Instance.AppSettings.CurrentDeviceType = DeviceType.ModbusTCP;
                }
                else if (this.cbSelectDeviceType.SelectedValue.ToString() == "MCProtocolBinaryEthernet")
                {
                    SystemsManager.Instance.AppSettings.CurrentDeviceType = DeviceType.MCProtocolBinaryEthernet;
                }
                else if (this.cbSelectDeviceType.SelectedValue.ToString() == "MCProtocolAsciiEthernet")
                {
                    SystemsManager.Instance.AppSettings.CurrentDeviceType = DeviceType.MCProtocolAsciiEthernet;
                }
                else if (this.cbSelectDeviceType.SelectedValue.ToString() == "MCProtocolBinarySerial")
                {
                    SystemsManager.Instance.AppSettings.CurrentDeviceType = DeviceType.MCProtocolBinarySerial;
                }
                else if (this.cbSelectDeviceType.SelectedValue.ToString() == "MCProtocolAsciiSerial")
                {
                    SystemsManager.Instance.AppSettings.CurrentDeviceType = DeviceType.MCProtocolAsciiSerial;
                }
                else if (this.cbSelectDeviceType.SelectedValue.ToString() == "XGTLS")
                {
                    SystemsManager.Instance.AppSettings.CurrentDeviceType = DeviceType.XGTLS;
                }
            }
            catch(Exception ex)
            {

            }
        }
        private void LoadDeviceSetting()
        {
            this.settingDevice = SystemsManager.Instance.AppSettings.SettingDevice;
            this.cbSelectDeviceType.SelectedValue = SystemsManager.Instance.AppSettings.CurrentDeviceType.ToString();
        }
        private void RegisterNotify()
        {
            try
            {
                SystemsManager.Instance.NotifyEvenTCPClient.Attach(this);
            }
            catch(Exception ex)
            {
                this.logger.Create("RegisterNotify: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("RegisterNotify: " + ex.Message);
            }
        }
        private void UnregisterNotify()
        {
            try
            {
                SystemsManager.Instance.NotifyEvenTCPClient.Detach(this);
            }
            catch (Exception ex)
            {
                this.logger.Create("UnregisterNotify: " + ex.Message, LogLevel.Error);
                this.UpdateLogs("UnregisterNotify: " + ex.Message);
            }
        }
    }
}
