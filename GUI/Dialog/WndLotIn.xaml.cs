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
    /// Interaction logic for WndLotIn.xaml
    /// </summary>
    public partial class WndLotIn : Window
    {
        private LoggerDebug logger = new LoggerDebug("WndLotIn");
        private LotInData lotInData;
        private DateTime lastInputTime = DateTime.Now;
        public WndLotIn()
        {
            InitializeComponent();
            this.btnOK.Click += BtnOK_Click;
            this.btnOK.TouchDown+= BtnOK_Click;

            this.btnCancel.Click += BtnCancel_Click;
            this.btnCancel.TouchDown+= BtnCancel_Click;

            this.txtWorkGroup.PreviewKeyDown += TxtWorkGroup_PreviewKeyDown;
            this.txtDeviceId.PreviewKeyDown+= TxtWorkGroup_PreviewKeyDown;
            this.txtLotId.PreviewKeyDown+= TxtWorkGroup_PreviewKeyDown;
            this.txtLotQty.PreviewKeyDown+= TxtWorkGroup_PreviewKeyDown;
        }

        private void TxtWorkGroup_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (!IsFromScanner(e))
                //{
                //    e.Handled = true;
                //}
                if (e.Key != Key.Enter) return;
                TextBox currentTextBox = sender as TextBox;
                if (currentTextBox == null) return;
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                currentTextBox.MoveFocus(request);

                
                e.Handled = true;
            }
            catch(Exception ex)
            {
                this.logger.Create("TxtWorkGroup_PreviewKeyDown: " + ex.Message, LogLevel.Error);
            }
        }
        private bool IsFromScanner(KeyEventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan elapsedTime = currentTime - lastInputTime;

            if (elapsedTime.TotalMilliseconds < 100)
            {
                lastInputTime = currentTime;
                return true;
            }

            return false;
        }
        public LotInData DoSettings(Window owner, LotInData oldSettings)
        {
            this.lotInData = oldSettings;

            txtWorkGroup.Text = this.lotInData.WorkGroup;
            txtDeviceId.Text = this.lotInData.DeviceId;
            txtLotId.Text = this.lotInData.LotId;
            if (this.lotInData.LotQty > 0)
            {
                txtLotQty.Text = this.lotInData.LotQty.ToString();
            }

            this.ShowDialog();
            return this.lotInData;
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.lotInData = null;
                this.Close();
            }
            catch(Exception ex)
            {
                this.logger.Create("BtnCancel_Click: "+ex.Message, LogLevel.Error);
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate data:
                if (this.txtLotId.Text.Length < 1)
                {
                    MessageBox.Show("Invalid LOT ID: it must has more 1 characters!", "PARAMETER ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (String.IsNullOrEmpty(this.txtDeviceId.Text))
                {
                    MessageBox.Show("Invalid Device ID: it must has atleast 1 character!", "PARAMETER ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                int lotQty = 0;
                try
                {
                    lotQty = int.Parse(txtLotQty.Text);
                }
                catch
                {
                    lotQty = 0;
                }
                const int MAX_QTY = 99999;
                if (lotQty == 0 || lotQty > MAX_QTY)
                {
                    var msg = String.Format("Invalid LOT QTY: it must be a positive number and not over {0}!", MAX_QTY);
                    MessageBox.Show(msg, "PARAMETER ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                this.lotInData = new LotInData();
                this.lotInData.WorkGroup = this.txtWorkGroup.Text;
                this.lotInData.DeviceId = this.txtDeviceId.Text;
                this.lotInData.LotId = this.txtLotId.Text;
                this.lotInData.LotQty = int.Parse(this.txtLotQty.Text);

                this.Close();
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnOK_Click: " + ex.Message, LogLevel.Error);
            }
        }
    }
}
