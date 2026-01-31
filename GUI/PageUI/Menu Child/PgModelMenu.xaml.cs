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
using DTO;
using BLL;
using System.Diagnostics.SymbolStore;

namespace GUI
{
    /// <summary>
    /// Interaction logic for PgModelMenu.xaml
    /// </summary>
    public partial class PgModelMenu : Page
    {
        LoggerDebug logger = new LoggerDebug("PgModelMenu");
        public PgModelMenu()
        {
            InitializeComponent();
            this.Loaded += this.PgMenuSave_Loaded;
            this.btPkgSave.Click += this.BtPkgSave_Click;
            this.btPkgDelete.Click += this.BtPkgDelete_Click;
            this.btPkgDeleteAll.Click += this.BtPkgDeleteAll_Click;

            this.dgridModels.SelectionChanged += this.DgridModels_SelectionChanged;
        }
        private void PgMenuSave_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                this.txtCurrentPkg.Text = SystemsManager.Instance.AppSettings.currentModel;
                this.txtSelectPkg.Text = "";
                this.dgridModels.UnselectAll();
                this.dgridModels.ItemsSource = BLLManager.Instance.ServiceModel.GetModelInfoList();
                CheckLock();
            }
            catch (Exception ex)
            {
                logger.Create("PgMenuSave_Loaded:" + ex.Message,LogLevel.Error);
            }
        }
        private void CheckLock()
        {
            if (!UserManagers.Instance.CheckAssignLevel(PAGE_ID.PAGE_MENU_MODEL.ToString()))
            {
                UserManagers.Instance.DisableAllControls(this);
            }
            else
            {
                UserManagers.Instance.EnableAllControls(this);
            }
        }
        private void DgridModels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = this.dgridModels.SelectedItem;
                if (item != null)
                {
                    this.txtSelectPkg.Text = ((ModelInfo)item).Name;
                }
            }
            catch (Exception ex)
            {
                logger.Create("DgridModels_SelectionChanged:" + ex.Message, LogLevel.Error);
            }
        }

        private void BtPkgSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if ((bool)this.rdSave.IsChecked)
                {
                    // Confirm:
                    if (MessageBox.Show("Are you sure to saving (override) all changes of the current Package?", "Note",
                            MessageBoxButton.YesNo, MessageBoxImage.Question,
                            MessageBoxResult.No) != MessageBoxResult.Yes)
                    {
                        return;
                    }

                    // Save:
                    SystemsManager.Instance.currentModel.UpdatedTime = DateTime.Now;
                    SystemsManager.Instance.currentModel.ModelName = txtPkgName.Text;
                    SystemsManager.Instance.AppSettings.currentModel = SystemsManager.Instance.currentModel.ModelName;
                    BLLManager.Instance.SaveModel();
                    SystemsManager.Instance.SaveAppSettings();
                }
                else if ((bool)this.rdSaveAs.IsChecked)
                {
                    // Check if the new Model name existing:
                    if (BLLManager.Instance.ServiceModel.GetModelSettings(this.txtPkgName.Text) != null)
                    {
                        MessageBox.Show("The new Package name already existed! Please choose another name!");
                        return;
                    }

                    // Save:
                    var model = SystemsManager.Instance.currentModel;
                    model.ModelName = this.txtPkgName.Text;
                    model.UpdatedTime = DateTime.Now;
                    BLLManager.Instance.ServiceModel.UpdateModelSettings(model);
                }

                // Reload models:
                this.PgMenuSave_Loaded(this, null);
            }
            catch (Exception ex)
            {
                logger.Create("BtPkgSave_Click:" + ex.Message, LogLevel.Error);
            }
        }

        private void BtPkgDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // Confirm:
                if (MessageBox.Show("Are you sure to delete the selected Package?", "Note",
                        MessageBoxButton.YesNo, MessageBoxImage.Question,
                        MessageBoxResult.No) != MessageBoxResult.Yes)
                {
                    return;
                }

                // Delete:
                var model = this.txtSelectPkg.Text;
                if (this.txtCurrentPkg.Text.Equals(model))
                {
                    MessageBox.Show("You cannot delete current model!");
                    return;
                }

                BLLManager.Instance.ServiceModel.DeleteModel(model);

                // Reload models:
                this.PgMenuSave_Loaded(this, null);
            }
            catch (Exception ex)
            {
                logger.Create("BtPkgDelete_Click:" + ex.Message, LogLevel.Error);
            }
        }

        private void BtPkgDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // Confirm:
                if (MessageBox.Show("Are you sure to delete ALL Packages?", "Note",
                        MessageBoxButton.YesNo, MessageBoxImage.Question,
                        MessageBoxResult.No) != MessageBoxResult.Yes)
                {
                    return;
                }

                // Delete
                BLLManager.Instance.ServiceModel.DeleteAll();

                // Reload models:
                this.PgMenuSave_Loaded(this, null);
            }
            catch (Exception ex)
            {
                logger.Create("BtPkgDeleteAll_Click:" + ex.Message, LogLevel.Error);
            }
        }
    }
}
