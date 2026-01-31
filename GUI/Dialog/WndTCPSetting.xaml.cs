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
    public partial class WndTCPSetting : Window
    {
        private LoggerDebug logger = new LoggerDebug("WndTCPSetting");
        private TCPSetting tcpSetting;

        public WndTCPSetting()
        {
            InitializeComponent();
            this.btnOkTCP.Click += BtnOk_Click;
            this.btnOkTCP.TouchDown += BtnOk_Click;

            this.btnCancleTCP.Click += BtnCancle_Click;
            this.btnCancleTCP.TouchDown += BtnCancle_Click;
        }

        private void BtnCancle_Click(object sender, RoutedEventArgs e)
        {
            this.tcpSetting = null;
            this.Close();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.tcpSetting.Ip = this.txtIpTCP.Text;
                this.tcpSetting.Port = ushort.Parse(this.txtPortTCP.Text);
                this.Close();
            }
            catch (Exception ex)
            {
                logger.Create("BtnOk_Click: " + ex.Message, LogLevel.Error);
            }
        }

        public TCPSetting DoSettings(Window owner, TCPSetting oldSettings)
        {
            this.tcpSetting = oldSettings;
            try
            {
                this.txtIpTCP.Text = this.tcpSetting.Ip.ToString();
                this.txtPortTCP.Text = this.tcpSetting.Port.ToString();

                this.ShowDialog();
            }
            catch (Exception ex)
            {
                logger.Create("DoSettings: " + ex.Message, LogLevel.Error);
            }
            return this.tcpSetting;
        }
    }
}
