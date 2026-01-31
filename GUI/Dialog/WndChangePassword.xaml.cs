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

namespace GUI
{
    /// <summary>
    /// Interaction logic for WndChangePassword.xaml
    /// </summary>
    public partial class WndChangePassword : Window
    {
        public WndChangePassword()
        {
            InitializeComponent();
            this.btnChange.Click += BtnChange_Click;
        }

        private async void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            string userName = this.cboUserName.Text;
            string passwordOld = this.txtPasswordOld.Password;
            string passwordNew = this.txtPasswordNew.Password;

            if(!await UserManagers.Instance.CheckPassword(userName, passwordOld))
            {
                MessageBox.Show("Password Old Wrong!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(!await UserManagers.Instance.ChangePassword(userName, passwordNew))
            {
                MessageBox.Show("Change Password Error!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Change Password Complete!!!", "Notify", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
