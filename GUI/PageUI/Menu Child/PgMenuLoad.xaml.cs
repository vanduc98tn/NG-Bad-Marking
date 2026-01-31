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
using DTO;
using BLL;

namespace GUI
{
    /// <summary>
    /// Interaction logic for PgMenuLoad.xaml
    /// </summary>
    public partial class PgMenuLoad : Page
    {
        private static LoggerDebug logger = new LoggerDebug("PgMenuLoad");

        private List<ModelInfo> modelInfoList = new List<ModelInfo>();
        public PgMenuLoad()
        {
            InitializeComponent();
            this.dgridModels.ItemsSource = modelInfoList;
            this.Loaded += this.PgMenuLoad_Loaded;
            this.btLoading.Click += this.BtLoading_Click;
            this.dgridModels.SelectionChanged += this.DgridModels_SelectionChanged;
        }
        private void PgMenuLoad_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.tvLoad.Text = "Load";
                this.txtCurrentPkg.Text = SystemsManager.Instance.AppSettings.currentModel;
                this.txtSelectPkg.Text = "";
                this.dgridModels.UnselectAll();
                this.dgridModels.ItemsSource = BLLManager.Instance.ServiceModel.GetModelInfoList();
                CheckLock();
            }
            catch (Exception ex)
            {
                logger.Create("PgMenuLoad_Loaded:" + ex.Message,LogLevel.Error);
            }
        }
        private void CheckLock()
        {
            if (!UserManagers.Instance.CheckAssignLevel(PAGE_ID.PAGE_MENU_LOAD.ToString()))
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

        private async void BtLoading_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (MessageBox.Show("Are you sure to load new Model?", "Note",
                        MessageBoxButton.YesNo, MessageBoxImage.Question,
                        MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    var loadedModel = BLLManager.Instance.ServiceModel.GetModelSettings(this.txtSelectPkg.Text);
                    if (loadedModel != null)
                    {

                        tvLoad.Text = "Loading...";
                        await Task.Delay(50);

                        ReplaceModel(loadedModel);
                        this.PgMenuLoad_Loaded(this, null);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Create("BtLoading_Click:" + ex.Message, LogLevel.Error);
            }
        }
        public void ReplaceModel(ModelSetting model)
        {
            try
            {
                if (model != null)
                {
                    SystemsManager.Instance.currentModel = model;
                    BLLManager.Instance.SaveModel();
                    SystemsManager.Instance.AppSettings.currentModel = model.ModelName;
                    SystemsManager.Instance.SaveAppSettings();
                }
            }
            catch (Exception ex)
            {
                logger.Create(String.Format("ReplaceModel:" + ex.Message),LogLevel.Error);
            }
        }
    }
}
