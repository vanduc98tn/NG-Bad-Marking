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
    public partial class PgManualMotor : Page
    {
        public PgManualMotor()
        {
            InitializeComponent();

            this.Loaded += PgManualMotor_Loaded;
        }

        private void PgManualMotor_Loaded(object sender, RoutedEventArgs e)
        {
            CheckLock();
        }
        private void CheckLock()
        {
            if (!UserManagers.Instance.CheckAssignLevel(PAGE_ID.PAGE_MENU_MANUAL_MOTOR.ToString()))
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
