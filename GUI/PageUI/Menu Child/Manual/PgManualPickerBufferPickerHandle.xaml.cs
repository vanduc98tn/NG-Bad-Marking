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
    public partial class PgManualPickerBufferPickerHandle : Page
    {
        public PgManualPickerBufferPickerHandle()
        {
            InitializeComponent();

            this.Loaded += PgManualPickerBufferPickerHandle_Loaded;
        }

        private void PgManualPickerBufferPickerHandle_Loaded(object sender, RoutedEventArgs e)
        {
            CheckLock();
        }
        private void CheckLock()
        {
            if (!UserManagers.Instance.CheckAssignLevel(PAGE_ID.PAGE_MENU_MANUAL_BUFFER_PICKER_HANDLE.ToString()))
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
