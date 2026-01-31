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

namespace GUI
{
    /// <summary>
    /// Interaction logic for PgSystemMenu.xaml
    /// </summary>
    public partial class PgSystemMenu : Page
    {
        public PgSystemMenu()
        {
            InitializeComponent();
            this.Loaded += PgSystemMenu_Loaded;

            this.btAOIOnline.Click += BtAOIOnline_Click;
            this.btAOIOnline.TouchDown+= BtAOIOnline_Click;

            this.btAOIOffline.Click += BtAOIOffline_Click;
            this.btAOIOffline.TouchDown += BtAOIOffline_Click;

            this.btSelectDataAOI.Click += BtSelectDataAOI_Click;
            this.btSelectDataAOI.TouchDown+= BtSelectDataAOI_Click;

            this.btUseE011.Click += BtUseE011_Click;
            this.btUseE011.TouchDown += BtUseE011_Click;

            this.btUseE021.Click += BtUseE021_Click;
            this.btUseE021.TouchDown += BtUseE021_Click;
        }

        private void BtUseE021_Click(object sender, RoutedEventArgs e)
        {
            SystemsManager.Instance.AppSettings.RunSetting.UseE011 = false;
            SystemsManager.Instance.SaveAppSettings();
            this.UpdateStatus();
        }

        private void BtUseE011_Click(object sender, RoutedEventArgs e)
        {
            SystemsManager.Instance.AppSettings.RunSetting.UseE011 = true;
            SystemsManager.Instance.SaveAppSettings();
            this.UpdateStatus();
        }

        private void BtSelectDataAOI_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new wndJigOffline();
            var mmm = wnd.DoSettings(Window.GetWindow(this), SystemsManager.Instance.AppSettings.JigAoiOffline);
        }

        private void PgSystemMenu_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateStatus();
            CheckLock();
        }
        private void CheckLock()
        {
            if (!UserManagers.Instance.CheckAssignLevel(PAGE_ID.PAGE_MENU_SYSTEM.ToString()))
            {
                UserManagers.Instance.DisableAllControls(this);
            }
            else
            {
                UserManagers.Instance.EnableAllControls(this);
            }
        }
        private void UpdateStatus()
        {
            if(SystemsManager.Instance.AppSettings.RunSetting.AOIOnline)
            {
                this.btAOIOnline.Background = Brushes.LightGreen;
                this.btAOIOffline.Background = Brushes.LightGray;
            }
            else
            {
                this.btAOIOnline.Background = Brushes.LightGray;
                this.btAOIOffline.Background = Brushes.LightGreen;
            }

            if (SystemsManager.Instance.AppSettings.RunSetting.UseE011)
            {
                this.btUseE011.Background = Brushes.LightGreen;
                this.btUseE021.Background = Brushes.LightGray;
            }
            else
            {
                this.btUseE011.Background = Brushes.LightGray;
                this.btUseE021.Background = Brushes.LightGreen;
            }
        }

        private void BtAOIOffline_Click(object sender, RoutedEventArgs e)
        {
            SystemsManager.Instance.AppSettings.RunSetting.AOIOnline = false;
            SystemsManager.Instance.SaveAppSettings();
            this.UpdateStatus();
        }

        private void BtAOIOnline_Click(object sender, RoutedEventArgs e)
        {
            SystemsManager.Instance.AppSettings.RunSetting.AOIOnline = true;
            SystemsManager.Instance.SaveAppSettings();
            this.UpdateStatus();
        }
    }
}
