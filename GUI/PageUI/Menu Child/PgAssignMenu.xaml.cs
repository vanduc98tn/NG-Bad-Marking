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
    /// Interaction logic for PgAssignMenu.xaml
    /// </summary>
    public partial class PgAssignMenu : Page
    {
        private LoggerDebug logger = new LoggerDebug("PgAssignMenu");
        private List<string> lstRadio;
        private List<string> lstGroupName;

        private int itemsPerPage = 10;
        private int currentPage = 1;

        public PgAssignMenu()
        {
            InitializeComponent();
            this.Loaded += PgAssignMenu_Loaded;
            this.btnPrevious.Click += BtnPrevious_Click;
            this.btnPrevious.TouchDown += BtnPrevious_Click;

            this.btnNext.Click += BtnNext_Click;
            this.btnNext.TouchDown+= BtnNext_Click;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.currentPage < (UserManagers.Instance.lstItemAssign.lstAssignOperater.Count + this.itemsPerPage - 1) / this.itemsPerPage)
                {
                    this.currentPage++;
                    this.AddItemsAssignToUI();
                }
            }
            catch(Exception ex)
            {
                logger.Create("BtnNext_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.currentPage > 1)
                {
                    this.currentPage--;
                    this.AddItemsAssignToUI();
                }
            }
            catch(Exception ex)
            {
                logger.Create("BtnPrevious_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void PgAssignMenu_Loaded(object sender, RoutedEventArgs e)
        {
            this.AddItemsAssignToUI();
        }
        private void AddItemsAssignToUI()
        {
            try
            {
                this.lsbItems.Items.Clear();
                this.lstRadio = new List<string>();
                this.lstGroupName = new List<string>();

                this.lblCurrentPage.Content = this.currentPage;

                int startIndex = (this.currentPage - 1) * this.itemsPerPage;
                int endIndex = Math.Min(startIndex + this.itemsPerPage, UserManagers.Instance.lstItemAssign.lstAssignOperater.Count);

                for (int i = startIndex; i < endIndex; i++)
                {
                    var x = UserManagers.Instance.lstItemAssign.lstAssignOperater[i];
                    this.lsbItems.Items.Add(x.AssignName);
                    this.lstRadio.Add("View");
                    this.lstRadio.Add("Control");
                    this.lstGroupName.Add(x.AssignName);
                }
                this.AddRadioButton();
            }
            catch(Exception ex)
            {
                logger.Create("AddItemsAssignToUI: " + ex.Message, LogLevel.Error);
            }
        }
        private void AddRadioButton()
        {
            try
            {
                this.wrapPanelOperater.Children.Clear();
                this.wrapPanelManager.Children.Clear();

                int startIndex = (this.currentPage - 1) * this.itemsPerPage;

                int itemsPerRow = 2;
                int rowCount = (int)Math.Ceiling((double)this.lstRadio.Count / itemsPerRow);

                for (int i = 0; i < rowCount; i++)
                {
                    WrapPanel rowWrapPanelOperater = new WrapPanel { Orientation = Orientation.Horizontal };
                    WrapPanel rowWrapPanelManager = new WrapPanel { Orientation = Orientation.Horizontal };

                    for (int j = 0; j < itemsPerRow && (i * itemsPerRow + j) < this.lstRadio.Count; j++)
                    {
                        RadioButton radioButton = new RadioButton
                        {
                            FontSize = 20,
                            FontWeight = FontWeights.Bold,
                            VerticalContentAlignment = VerticalAlignment.Center,
                            Content = this.lstRadio[i * itemsPerRow + j],
                            GroupName = this.lstGroupName[i] + "_Operator"
                        };
                        RadioButton radioButtonManager = new RadioButton
                        {
                            FontSize = 20,
                            FontWeight = FontWeights.Bold,
                            VerticalContentAlignment = VerticalAlignment.Center,
                            Content = this.lstRadio[i * itemsPerRow + j],
                            GroupName = this.lstGroupName[i] + "_Manager"
                        };
                        if (j % 2 == 0)
                        {
                            radioButton.Name = "rd1" + this.lstGroupName[i] + "_Operator";
                            radioButton.IsChecked = UserManagers.Instance.lstItemAssign.lstAssignOperater[i + startIndex].Isview;
                            radioButton.Margin = new Thickness(30, 8, 0, 3.5);

                            radioButtonManager.Name = "rd1" + this.lstGroupName[i] + "_Manager";
                            radioButtonManager.IsChecked = UserManagers.Instance.lstItemAssign.lstAssignManager[i + startIndex].Isview;
                            radioButtonManager.Margin = new Thickness(30, 8, 0, 3.5);
                        }
                        else
                        {
                            radioButton.Name = "rd2" + this.lstGroupName[i] + "_Operator";
                            radioButton.IsChecked = !UserManagers.Instance.lstItemAssign.lstAssignOperater[i + startIndex].Isview;
                            radioButton.Margin = new Thickness(200, 8, 0, 3.5);

                            radioButtonManager.Name = "rd2" + this.lstGroupName[i] + "_Manager";
                            radioButtonManager.IsChecked = !UserManagers.Instance.lstItemAssign.lstAssignManager[i + startIndex].Isview;
                            radioButtonManager.Margin = new Thickness(200, 8, 0, 3.5);
                        }

                        radioButton.Checked += RadioButton_Checked;
                        radioButtonManager.Checked += RadioButtonManager_Checked;


                        rowWrapPanelOperater.Children.Add(radioButton);
                        rowWrapPanelManager.Children.Add(radioButtonManager);
                    }

                    this.wrapPanelOperater.Children.Add(rowWrapPanelOperater);
                    this.wrapPanelManager.Children.Add(rowWrapPanelManager);
                }
            }
            catch(Exception ex)
            {
                logger.Create("AddRadioButton: " + ex.Message, LogLevel.Error);
            }
        }

        private void RadioButtonManager_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton rdButton = sender as RadioButton;
                var name = rdButton.Name;
                var assign = name.Replace("_Manager", "").Remove(0, 3);

                var foundIndex = UserManagers.Instance.lstItemAssign.lstAssignManager.FindIndex(x => x.AssignName.Contains(assign));
                if (foundIndex != -1)
                {
                    if (rdButton.Content.ToString() == "View")
                    {
                        UserManagers.Instance.lstItemAssign.lstAssignManager[foundIndex].Isview = (bool)rdButton.IsChecked;
                    }
                    else
                    {
                        UserManagers.Instance.lstItemAssign.lstAssignManager[foundIndex].Isview = !(bool)rdButton.IsChecked;
                    }
                }
                SystemsManager.Instance.SaveAppSettings();
            }
            catch(Exception ex)
            {
                logger.Create("RadioButtonManager_Checked: " + ex.Message, LogLevel.Error);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rdButton = sender as RadioButton;
            var name = rdButton.Name;
            var assign = name.Replace("_Operator", "").Remove(0,3);

            var foundIndex = UserManagers.Instance.lstItemAssign.lstAssignOperater.FindIndex(x => x.AssignName.Contains(assign));
            if(foundIndex!=-1)
            {
                if(rdButton.Content.ToString()=="View")
                {
                    UserManagers.Instance.lstItemAssign.lstAssignOperater[foundIndex].Isview = (bool)rdButton.IsChecked;
                }   
                else
                {
                    UserManagers.Instance.lstItemAssign.lstAssignOperater[foundIndex].Isview = !(bool)rdButton.IsChecked;
                }
            }
            SystemsManager.Instance.SaveAppSettings();
        }
    }
}
