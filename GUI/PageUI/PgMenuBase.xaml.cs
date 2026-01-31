using System;
using System.Windows;
using System.Windows.Controls;
using DTO;

namespace GUI
{
    /// <summary>
    /// Interaction logic for PgMenuBase.xaml
    /// </summary>
    public partial class PgMenuBase : Page
    {
        private LoggerDebug logger = new LoggerDebug("PgMenuBase");

        public PgMenuBase()
        {
            InitializeComponent();

            this.Loaded += PgMenuBase_Loaded;
            this.Unloaded += PgMenuBase_Unloaded;

            this.btLogin.Click += BtLogin_Click;
            this.btLogin.TouchDown+= BtLogin_Click;

            this.btMechanical.Click += BtMechanical_Click;
            this.btMechanical.TouchDown += BtMechanical_Click;

            this.btTeaching.Click += BtTeaching_Click;
            this.btTeaching.TouchDown+= BtTeaching_Click;

            this.btSystem.Click += BtSystem_Click;
            this.btSystem.TouchDown+= BtSystem_Click;

            this.btManual.Click += BtManual_Click;
            this.btManual.TouchDown += BtManual_Click;

            this.btStatus.Click += BtStatus_Click;
            this.btStatus.TouchDown+= BtStatus_Click;

            this.btLoadSave.Click += BtLoadSave_Click;
            this.btLoadSave.TouchDown+= BtLoadSave_Click;

            this.btSuperUser.Click += BtSuperUser_Click;
            this.btSuperUser.TouchDown+= BtSuperUser_Click;

            this.btAssign.Click += BtAssign_Click;
            this.btAssign.TouchDown+= BtAssign_Click;

        }

        private void BtAssign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!UserManagers.Instance.isLogin) return;
                if (this.imgAssignMenu.Visibility == Visibility.Visible) return;
                LogsManager.Instance.EventLogs.CreateEventLog("BtAssign Clicked", UserManagers.Instance.CurrentUser);
                UIManager.Instance.SwitchPage(PAGE_ID.PAGE_MENU_ASSIGN);
            }
            catch (Exception ex)
            {
                logger.Create("BtAssign_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WndLogIn login = new WndLogIn();
                login.ShowDialog();
                this.UpdateNameUserLoginToUI();
                this.CheckLock();
            }
            catch(Exception ex)
            {
                logger.Create("BtLogin_Click: " + ex.Message,LogLevel.Error);
            }
        }

        private void PgMenuBase_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void PgMenuBase_Loaded(object sender, RoutedEventArgs e)
        {
            this.CheckLock();
        }

        private void BtSuperUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!UserManagers.Instance.isLogin) return;
                UIManager.Instance.SwitchPage(PAGE_ID.PAGE_MENU_SUPER_USER);
                LogsManager.Instance.EventLogs.CreateEventLog("BtSuperUser Clicked", UserManagers.Instance.CurrentUser);
            }
            catch (Exception ex)
            {
                logger.Create("BtSuperUser_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtLoadSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!UserManagers.Instance.isLogin) return;
                UIManager.Instance.SwitchPage(PAGE_ID.PAGE_MENU_MODEL);
                LogsManager.Instance.EventLogs.CreateEventLog("BtLoadSave Clicked", UserManagers.Instance.CurrentUser);
            }
            catch (Exception ex)
            {
                logger.Create("BtLoadSave_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!UserManagers.Instance.isLogin) return;
                //UIManager.Instance.SwitchPage(PAGE_ID.PAGE_MENU_TEACHING);
                LogsManager.Instance.EventLogs.CreateEventLog("BtStatus Clicked", UserManagers.Instance.CurrentUser);
            }
            catch (Exception ex)
            {
                logger.Create("BtStatus_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtManual_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!UserManagers.Instance.isLogin) return;
                UIManager.Instance.SwitchPage(PAGE_ID.PAGE_MENU_MANUAL_PICKER_TRANSFER);
                LogsManager.Instance.EventLogs.CreateEventLog("BtManual Clicked", UserManagers.Instance.CurrentUser);
            }
            catch (Exception ex)
            {
                logger.Create("BtManual_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtSystem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!UserManagers.Instance.isLogin) return;
                UIManager.Instance.SwitchPage(PAGE_ID.PAGE_MENU_SYSTEM);
                LogsManager.Instance.EventLogs.CreateEventLog("BtSystem Clicked", UserManagers.Instance.CurrentUser);
            }
            catch (Exception ex)
            {
                logger.Create("BtSystem_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtTeaching_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!UserManagers.Instance.isLogin) return;
                UIManager.Instance.SwitchPage(PAGE_ID.PAGE_MENU_TEACHING);
                LogsManager.Instance.EventLogs.CreateEventLog("BtTeaching Clicked", UserManagers.Instance.CurrentUser);
            }
            catch (Exception ex)
            {
                logger.Create("BtTeaching_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private  void BtMechanical_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!UserManagers.Instance.isLogin) return;
                UIManager.Instance.SwitchPage(PAGE_ID.PAGE_MENU_MECHANICAL_DELAY_TIME);
                LogsManager.Instance.EventLogs.CreateEventLog("BtMechanical Clicked", UserManagers.Instance.CurrentUser);
            }
            catch(Exception ex)
            {
                logger.Create("BtMechanical_Click: " + ex.Message, LogLevel.Error);
            }
        }
        private void CheckLock()
        {
            if (!UserManagers.Instance.isLogin)
            {
                this.imgLockTeachingMenu.Visibility = Visibility.Visible;
                this.imgManualMenu.Visibility = Visibility.Visible;
                this.imgMechanicalMenu.Visibility = Visibility.Visible;
                this.imgModelMenu.Visibility = Visibility.Visible;
                this.imgStatusMenu.Visibility = Visibility.Visible;
                this.imgSupperUserMenu.Visibility = Visibility.Visible;
                this.imgSystemMenu.Visibility = Visibility.Visible;
                this.imgAssignMenu.Visibility = Visibility.Visible;
                return;
            }
            this.imgLockTeachingMenu.Visibility = Visibility.Hidden;
            this.imgManualMenu.Visibility = Visibility.Hidden;
            this.imgMechanicalMenu.Visibility = Visibility.Hidden;
            this.imgModelMenu.Visibility = Visibility.Hidden;
            this.imgStatusMenu.Visibility = Visibility.Hidden;
            this.imgSupperUserMenu.Visibility = Visibility.Hidden;
            this.imgSystemMenu.Visibility = Visibility.Hidden;
            if(UserManagers.Instance.CurrentUser== "AutoTeams")
            {
                this.imgAssignMenu.Visibility = Visibility.Hidden;
            }
            else
            {
                this.imgAssignMenu.Visibility = Visibility.Visible;
            }
        }
        private void UpdateNameUserLoginToUI()
        {
            if (UserManagers.Instance.CurrentUser == null) return;
            this.txtID.Content = UserManagers.Instance.CurrentUser;
            this.txtTime.Content = DateTime.Now.ToString();
        }
    }
}
