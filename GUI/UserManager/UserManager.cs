using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using BLL;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace GUI
{
    public class UserManagers
    {
        private static UserManagers instance = new UserManagers();
        public static UserManagers Instance = instance;
        //Debug Log.
        private static LoggerDebug logger = new LoggerDebug("UserManagers");
        //List Assign.
        public ItemAssign lstItemAssign;

        public string CurrentUser { get; set; }
        public bool isLogin { get; set; }

        //Event Log.
        protected UserLoginService eventLogService;
        public UserLogin UserLogin;

        public void Startup()
        {
            this.LoadUserLogin();
            this.AddlstItemAssign();
        }
        private void LoadUserLogin()
        {
            SqlLiteUserLogin sqlLiteUserLogin = new SqlLiteUserLogin();
            this.eventLogService = new UserLoginService(sqlLiteUserLogin.UserLoginRepository);
            this.UserLogin = new UserLogin();
        }
        public async Task<bool> CheckPassword(string userName , string Password)
        {
            var result = await this.eventLogService.VerifyPassword(userName, Password);
            this.isLogin = result;
            return result;
        }
        public async Task<bool> ChangePassword(string userName,string newPassword)
        {
            var result = await this.eventLogService.ChangePassword(userName, newPassword);
            return result;
        }
        private void AddlstItemAssign()
        {
            var itemAssign = SystemsManager.Instance.AppSettings.ItemAssign;
            this.lstItemAssign = new ItemAssign();
            
            if (itemAssign.lstAssignManager.Count!= UIManager.Instance.pageTable.Count || itemAssign.lstAssignOperater.Count != UIManager.Instance.pageTable.Count)
            {
                this.ClearAssign(itemAssign);
                foreach (var value in UIManager.Instance.pageTable.Keys)
                {
                    var item = new Item();
                    item.AssignName = value.ToString();
                    item.Isview = false;
                    itemAssign.lstAssignManager.Add(item);
                    itemAssign.lstAssignOperater.Add(item);
                }
                SystemsManager.Instance.SaveAppSettings();
            }
            this.lstItemAssign = itemAssign;
        }
        private void ClearAssign(ItemAssign itemAssign)
        {
            if (itemAssign.lstAssignManager == null || itemAssign.lstAssignOperater == null) return;
            itemAssign.lstAssignManager = new List<Item>();
            itemAssign.lstAssignOperater = new List<Item>();
        }
        public bool CheckAssignLevel(string nameItems)
        {
            if(this.CurrentUser == "AutoTeams")
            {
                return true;
            }
            else if(this.CurrentUser == "Operator")
            {
                var item = this.lstItemAssign.lstAssignOperater.FirstOrDefault(x => x.AssignName == nameItems);
                if (item == null) return false;
                return !item.Isview;
            }
            else if (this.CurrentUser == "Manager")
            {
                var item = this.lstItemAssign.lstAssignManager.FirstOrDefault(x => x.AssignName == nameItems);
                if (item == null) return false;
                return !item.Isview;
            }
            return false;
        }
        public void Logout()
        {
            this.isLogin = false;
            this.CurrentUser = "";
        }
        public void DisableAllControls(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is Control)
                {
                    ((Control)child).IsEnabled = false;
                }

                DisableAllControls(child); // Đệ quy để xử lý các đối tượng con
            }
        }
        public void EnableAllControls(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is Control)
                {
                    ((Control)child).IsEnabled = true;
                }

                EnableAllControls(child); // Đệ quy để xử lý các đối tượng con
            }
        }
    }
}
