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
    /// Interaction logic for WndAlarmShow.xaml
    /// </summary>
    public partial class WndAlarmShow : Window
    {
        private static int seqId = 0;
        public WndAlarmShow(String msg = "", String solution = "", string code = "", string mode = "AUTO")
        {
            InitializeComponent();
            this.btnClose.Click += BtnClose_Click;
            this.Closed += WndAlarmShow_Closed;

            UIManager.Instance.isShowAlarm = true;

            this.lblMessage.Text = msg;
            this.lblSolution.Text = solution;
            this.lblCode.Content = code.ToString();
            this.lblMode.Content = mode.ToString();
            this.lblTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Increase SeqId:
            seqId++;
            this.lblSeqId.Content = seqId.ToString();

            this.Topmost = true;
        }

        private void WndAlarmShow_Closed(object sender, EventArgs e)
        {
            UIManager.Instance.isShowAlarm = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
