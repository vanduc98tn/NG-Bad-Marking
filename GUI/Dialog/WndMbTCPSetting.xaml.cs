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
using System.Windows.Shapes;
using DTO;
using BLL;

namespace GUI
{
    /// <summary>
    /// Interaction logic for WndMbTCPSetting.xaml
    /// </summary>
    public partial class WndMbTCPSetting : Window
    {
        private LoggerDebug logger = new LoggerDebug("WndMbTCPSetting");
        private ModbusTCPSetting mbSetting;

        public WndMbTCPSetting()
        {
            InitializeComponent();
            this.btnOk.Click += BtnOk_Click;
            this.btnOk.TouchDown+= BtnOk_Click;

            this.btnCancle.Click += BtnCancle_Click;
            this.btnCancle.TouchDown+= BtnCancle_Click;
        }

        private void BtnCancle_Click(object sender, RoutedEventArgs e)
        {
            this.mbSetting = null;
            this.Close();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.mbSetting.Ip = this.txtIp.Text;
                this.mbSetting.Port = ushort.Parse(this.txtPort.Text);
                this.mbSetting.Address = ushort.Parse(this.txtAddress.Text);
                this.Close();
            }
            catch(Exception ex)
            {
                logger.Create("BtnOk_Click: " + ex.Message, LogLevel.Error);
            }
        }

        public ModbusTCPSetting DoSettings(Window owner, ModbusTCPSetting oldSettings)
        {
            this.mbSetting = oldSettings;
            try
            {
                this.txtAddress.Text = this.mbSetting.Address.ToString();
                this.txtIp.Text = this.mbSetting.Ip.ToString();
                this.txtPort.Text = this.mbSetting.Port.ToString();

                this.ShowDialog();
            }
            catch(Exception ex)
            {
                logger.Create("DoSettings: " + ex.Message, LogLevel.Error);
            }
            return this.mbSetting;
        }
    }
}
