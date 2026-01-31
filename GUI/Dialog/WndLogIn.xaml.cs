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
using System.Diagnostics;

namespace GUI
{
    /// <summary>
    /// Interaction logic for WndLogIn.xaml
    /// </summary>
    public partial class WndLogIn : Window
    {
        private LoggerDebug logger = new LoggerDebug("WndLogIn");
        public WndLogIn()
        {
            InitializeComponent();

            this.Loaded += WndLogIn_Loaded;
            this.Unloaded += WndLogIn_Unloaded;

            this.btnEnter.Click += BtnEnter_Click;
            this.btnChangePassword.Click += BtnChangePassword_Click;
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            WndChangePassword wndChangePassword = new WndChangePassword();
            wndChangePassword.ShowDialog();
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            this.CheckPassword();
        }

        private void WndLogIn_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void WndLogIn_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private async void CheckPassword()
        {
            string userName = this.cboUserName.Text;
            string passWord = this.txtPassword.Password;
            if(!await UserManagers.Instance.CheckPassword(userName, passWord))
            {
                MessageBox.Show("Password wrong!!!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (UserManagers.Instance.isLogin) UserManagers.Instance.CurrentUser = userName;
            this.Close();
        }
        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            //ShowOnScreenKeyboard();
        }
        private void ShowOnScreenKeyboard()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = @"C:\Windows\System32\osk.exe",
                    UseShellExecute = true,
                    CreateNoWindow = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
