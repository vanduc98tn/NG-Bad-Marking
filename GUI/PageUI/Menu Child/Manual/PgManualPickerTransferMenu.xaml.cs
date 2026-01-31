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
    /// Interaction logic for PgManualOperationMenu.xaml
    /// </summary>
    public partial class PgManualPickerTransferMenu : Page
    {
        public PgManualPickerTransferMenu()
        {
            InitializeComponent();
            this.Loaded += PgManualPickerTransferMenu_Loaded;
        }

        private void PgManualPickerTransferMenu_Loaded(object sender, RoutedEventArgs e)
        {
            CheckLock();
        }
        private void CheckLock()
        {
            if (!UserManagers.Instance.CheckAssignLevel(PAGE_ID.PAGE_MENU_MANUAL_PICKER_TRANSFER.ToString()))
            {
                UserManagers.Instance.DisableAllControls(this);
            }
            else
            {
                UserManagers.Instance.EnableAllControls(this);
            }
        }
    }
}
