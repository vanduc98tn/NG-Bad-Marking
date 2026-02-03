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
using System.Windows.Markup;
using System.Threading;
using System.Diagnostics;

namespace GUI
{
    /// <summary>
    /// Interaction logic for PgTeachingMenu.xaml
    /// </summary>
    public partial class PgTeachingMenu : Page
    {
        private LoggerDebug logger = new LoggerDebug("PgTeachingMenu");
        private List<Button> lstButtonPos;
        private ModelSetting modelSetting;
        Int32 dataX;
        Int32 dataY;
        bool flag;
        public PgTeachingMenu()
        {
            InitializeComponent();
            this.Loaded += PgTeachingMenu_Loaded;
            this.Unloaded += PgTeachingMenu_Unloaded;

            this.btSave.Click += BtSave_Click;
            //this.btSave.TouchDown += BtSave_Click;

            this.btnSelectPatern.Click += BtnSelectPatern_Click;
            //this.btnSelectPatern.TouchDown += BtnSelectPatern_Click;

            this.btnChangePos.Click += BtnChangePos_Click;
            //this.btnChangePos.TouchDown += BtnChangePos_Click;

            this.btnSavePos.Click += BtnSavePos_Click;
            //this.btnSavePos.TouchDown += BtnSavePos_Click;

            this.btnAutoGenerate.Click += BtnAutoGenerate_Click;
            //this.btnAutoGenerate.TouchDown += BtnAutoGenerate_Click;

            this.cbUseWithTargetX.Checked += CbUseWithTargetX_Checked;
            this.cbUseWithTargetY.Checked += CbUseWithTargetY_Checked;
        }

        private void CbUseWithTargetY_Checked(object sender, RoutedEventArgs e)
        {
            txtChangePosY.Text = lblPosY.Content.ToString();
        }

        private void CbUseWithTargetX_Checked(object sender, RoutedEventArgs e)
        {
            txtChangePosX.Text = lblPosX.Content.ToString();
        }
        private void LoadAC()
        {

            var patter = new Pattern();
            patter.pos.X = 0; patter.pos.Y = 0;
            var modelSt = modelSetting.Teaching;

            if (modelSt.pt1.Count == 0) modelSt.pt1.Add(patter);
            if (modelSt.pt2.Count == 0) modelSt.pt2.Add(patter);
            if (modelSt.pt3.Count == 0) modelSt.pt3.Add(patter);
            if (modelSt.pt4.Count == 0) modelSt.pt4.Add(patter);
            if (modelSt.pt5.Count == 0) modelSt.pt5.Add(patter);
            if (modelSt.pt6.Count == 0) modelSt.pt6.Add(patter);
            if (modelSt.pt7.Count == 0) modelSt.pt7.Add(patter);
            if (modelSt.pt8.Count == 0) modelSt.pt8.Add(patter);
            if (modelSt.pt9.Count == 0) modelSt.pt9.Add(patter);
            if (modelSt.pt10.Count == 0) modelSt.pt10.Add(patter);
            if (modelSt.pt11.Count == 0) modelSt.pt11.Add(patter);
            if (modelSt.pt12.Count == 0) modelSt.pt12.Add(patter);
            if (modelSt.pt13.Count == 0) modelSt.pt13.Add(patter);
            if (modelSt.pt14.Count == 0) modelSt.pt14.Add(patter);
            if (modelSt.pt15.Count == 0) modelSt.pt15.Add(patter);
            if (modelSt.pt16.Count == 0) modelSt.pt16.Add(patter);


        }
        private void BtnAutoGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAC();
                WndComfirm wnd = new WndComfirm();
                if (wnd.DoComfirmYesNo("Auto Generate ?"))
                {
                    double startX = 0;
                    double startY = 0;
                    if (modelSetting.Pattern.CurrentPatern == 1)
                    {
                        startX = modelSetting.Teaching.pt1[0].pos.X;
                        startY = modelSetting.Teaching.pt1[0].pos.Y;
                        
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 2)
                    {
                        startX = modelSetting.Teaching.pt2[0].pos.X;
                        startY = modelSetting.Teaching.pt2[0].pos.Y;
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 3)
                    {
                        startX = modelSetting.Teaching.pt3[0].pos.X;
                        startY = modelSetting.Teaching.pt3[0].pos.Y;
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 4)
                    {
                        startX = modelSetting.Teaching.pt4[0].pos.X;
                        startY = modelSetting.Teaching.pt4[0].pos.Y;
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 5)
                    {
                        startX = modelSetting.Teaching.pt5[0].pos.X;
                        startY = modelSetting.Teaching.pt5[0].pos.Y;
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 6)
                    {
                        startX = modelSetting.Teaching.pt6[0].pos.X;
                        startY = modelSetting.Teaching.pt6[0].pos.Y;
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 7)
                    {
                        startX = modelSetting.Teaching.pt7[0].pos.X;
                        startY = modelSetting.Teaching.pt7[0].pos.Y;
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 8)
                    {
                        startX = modelSetting.Teaching.pt8[0].pos.X;
                        startY = modelSetting.Teaching.pt8[0].pos.Y;
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 9)
                    {
                        startX = modelSetting.Teaching.pt9[0].pos.X;
                        startY = modelSetting.Teaching.pt9[0].pos.Y;
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 10)
                    {
                        startX = modelSetting.Teaching.pt10[0].pos.X;
                        startY = modelSetting.Teaching.pt10[0].pos.Y;
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 11)
                    {
                        startX = modelSetting.Teaching.pt11[0].pos.X;
                        startY = modelSetting.Teaching.pt11[0].pos.Y;
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 12)
                    {
                        startX = modelSetting.Teaching.pt12[0].pos.X;
                        startY = modelSetting.Teaching.pt12[0].pos.Y;
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 13)
                    {
                        startX = modelSetting.Teaching.pt13[0].pos.X;
                        startY = modelSetting.Teaching.pt13[0].pos.Y;
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 14)
                    {
                        startX = modelSetting.Teaching.pt14[0].pos.X;
                        startY = modelSetting.Teaching.pt14[0].pos.Y;
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 15)
                    {
                        startX = modelSetting.Teaching.pt15[0].pos.X;
                        startY = modelSetting.Teaching.pt15[0].pos.Y;
                    }
                    else if (modelSetting.Pattern.CurrentPatern == 16)
                    {
                        startX = modelSetting.Teaching.pt16[0].pos.X;
                        startY = modelSetting.Teaching.pt16[0].pos.Y;
                    }
                    //var startX = Convert.ToDouble(lblPosX.Content.ToString());
                    //var startY = Convert.ToDouble(lblPosY.Content.ToString());
                    Autogene(modelSetting.Pattern.CurrentPatern, modelSetting.Pattern.PitchX, modelSetting.Pattern.PitchY, startX, startY, modelSetting.Pattern.yColumn, modelSetting.Pattern.xRow, modelSetting.Pattern.Use2Matrix);
                    UpdateNotify("Auto Generate OK!");
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnAutoGenerate_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtnSavePos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(lblId.Content.ToString());
                EditPositon(modelSetting.Pattern.CurrentPatern, id, Convert.ToDouble(lblPosX.Content.ToString()), Convert.ToDouble(lblPosY.Content.ToString()));
                UpdateNotify("Save OK!");
            }
            catch (Exception ex)
            {
                UpdateNotify("Save Faild!");
                this.logger.Create("BtnSavePos_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtnChangePos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.lblPosX.Content = this.txtChangePosX.Text;
                this.lblPosY.Content = this.txtChangePosY.Text;
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnChangePos_Click: " + ex.Message, LogLevel.Error);
            }
        }

        private void BtnSelectPatern_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WndPatern uiPatern = new WndPatern();
                uiPatern.DoConfirmYesNo();
                generateCells(modelSetting.Pattern.xRow, modelSetting.Pattern.yColumn, modelSetting.Pattern.CurrentPatern,modelSetting.Pattern.Use2Matrix);
                string filePath = string.Format("/Resource/Image/{0}.PNG", modelSetting.Pattern.CurrentPatern);
                imgPattern.Source = new BitmapImage(new Uri(filePath, UriKind.Relative));
            }
            catch (Exception ex)
            {
                this.logger.Create("BtnSelectPatern_Click: " + ex.Message, LogLevel.Error);
            }
        }
        private void UpdateNotify(string notify)
        {
            this.Dispatcher.Invoke(() => {
                lblNotification.Content = notify;
            });
            this.clearLogThread();
        }
        private void clearLogThread(int timeout = 1000)
        {
            new Thread(new ThreadStart(() => {
                Thread.Sleep(timeout);
                this.Dispatcher.Invoke(() => {
                    lblNotification.Content = "";
                });
            })).Start();
        }
        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WndComfirm wnd = new WndComfirm();
                if (wnd.DoComfirmYesNo("Save All Setting ?"))
                {
                    BLLManager.Instance.SaveModel();
                    MessageBox.Show("Save setting OK!");
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("BtSave_Click: " + ex.Message, LogLevel.Error);
            }
        }
        private void generateCells1(int rowCnt, int colCnt, int pattern, bool Use2Matrix)
        {
            lstButtonPos = new List<Button>();
            this.gridPos.Children.Clear();
            this.gridPos.RowDefinitions.Clear();
            for (int r = 0; r < rowCnt; r++)
            {
                var rowDef = new RowDefinition();
                rowDef.Height = new GridLength(1, GridUnitType.Star);
                this.gridPos.RowDefinitions.Add(rowDef);
            }
            this.gridPos.ColumnDefinitions.Clear();
            for (int c = 0; c < colCnt; c++)
            {
                var colDef = new ColumnDefinition();
                colDef.Width = new GridLength(1, GridUnitType.Star);
                this.gridPos.ColumnDefinitions.Add(colDef);
            }
            for (int id = 1; id <= rowCnt * colCnt; id++)
            {
                var r = GetCellRow(id, pattern, rowCnt, colCnt, Use2Matrix);
                var c = GetCellColumn(id, pattern, rowCnt, colCnt, Use2Matrix);
                var cell = createCell(id);
                gridPos.Children.Add(cell);
                Grid.SetRow(cell, r);
                Grid.SetColumn(cell, c);
            }
        }
        private void generateCells(int rowCnt, int colCnt, int pattern, bool Use2Matrix)
        {
            lstButtonPos = new List<Button>();
            gridPos.Children.Clear();
            gridPos.RowDefinitions.Clear();
            gridPos.ColumnDefinitions.Clear();

            // Rows
            for (int r = 0; r < rowCnt; r++)
            {
                gridPos.RowDefinitions.Add(
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                );
            }

            int matrixCols = Use2Matrix ? colCnt / 2 : colCnt;
            int gapCols = Use2Matrix ? 1 : 0;
            int totalCols = Use2Matrix ? matrixCols * 2 + gapCols : colCnt;

            // Columns (có GAP)
            for (int c = 0; c < totalCols; c++)
            {
                if (Use2Matrix && c == matrixCols)
                {
                    // Cột trống giữa 2 matrix
                    gridPos.ColumnDefinitions.Add(
                        new ColumnDefinition { Width = new GridLength(5) } // px
                    );
                }
                else
                {
                    gridPos.ColumnDefinitions.Add(
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                    );
                }
            }

            int matrixSize = rowCnt * matrixCols;

            // Create cells
            for (int id = 1; id <= rowCnt * colCnt; id++)
            {
                int r = GetCellRow(id, pattern, rowCnt, colCnt, Use2Matrix);
                int c = GetCellColumn(id, pattern, rowCnt, colCnt, Use2Matrix);

                var cell = createCell(id);
                gridPos.Children.Add(cell);
                Grid.SetRow(cell, r);
                Grid.SetColumn(cell, c);
            }
        }
        private Button createCell(int number)
        {
            var cell = new Button();
            cell.Content = String.Format("{0}", number);
            cell.FontWeight = FontWeights.Bold;
            cell.FontSize = 12;
            //cell.Margin = new Thickness(1, 1, 1, 1);
            cell.Name = String.Format("btCell{0:00}", number);
            cell.Background = Brushes.LightGray;
            cell.Click += this.Cell_Click;
            cell.TouchDown += this.Cell_Click;
            lstButtonPos.Add(cell);
            return cell;
        }
        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearBackGroundButton();
                var btCell = (Button)sender;
                btCell.Background = Brushes.OrangeRed;
                lblId.Content = Convert.ToInt32(btCell.Content.ToString());
                int id = Convert.ToInt32(btCell.Content.ToString());
                this.UpdatePosToUi(id, modelSetting.Pattern.CurrentPatern);
                this.SendDataTrigger();
                if(cbUseWithTargetX.IsChecked==true)
                {
                    txtChangePosX.Text = lblPosX.Content.ToString();
                }
                if (cbUseWithTargetY.IsChecked == true)
                {
                    txtChangePosY.Text = lblPosY.Content.ToString();
                }
            }
            catch (Exception ex)
            {
                logger.Create("Cell_Click:" + ex.Message, LogLevel.Error);
            }
        }
        private async void SendDataTrigger()
        {
            await BLLManager.Instance.PLC.Device.WriteMultiDWords(3010, new int[] { dataX, dataY }, "D");
        }
        private void UpdatePosToUi(int id, int patern)
        {

            if (patern == 1)
            {
                if (modelSetting.Teaching.pt1 == null || modelSetting.Teaching.pt1.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt1[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt1[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt1[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt1[id - 1].pos.Y + "";
            }
            else if (patern == 2)
            {
                if (modelSetting.Teaching.pt2 == null || modelSetting.Teaching.pt2.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt2[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt2[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt2[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt2[id - 1].pos.Y + "";
            }
            else if (patern == 3)
            {
                if (modelSetting.Teaching.pt3 == null || modelSetting.Teaching.pt3.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt3[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt3[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt3[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt3[id - 1].pos.Y + "";
            }
            else if (patern == 4)
            {
                if (modelSetting.Teaching.pt4 == null || modelSetting.Teaching.pt4.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt4[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt4[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt4[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt4[id - 1].pos.Y + "";
            }
            else if (patern == 5)
            {
                if (modelSetting.Teaching.pt5 == null || modelSetting.Teaching.pt5.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt5[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt5[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt5[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt5[id - 1].pos.Y + "";
            }
            else if (patern == 6)
            {
                if (modelSetting.Teaching.pt6 == null || modelSetting.Teaching.pt6.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt6[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt6[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt6[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt6[id - 1].pos.Y + "";
            }
            else if (patern == 7)
            {
                if (modelSetting.Teaching.pt7 == null || modelSetting.Teaching.pt7.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt7[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt7[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt7[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt7[id - 1].pos.Y + "";
            }
            else if (patern == 8)
            {
                if (modelSetting.Teaching.pt8 == null || modelSetting.Teaching.pt8.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt8[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt8[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt8[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt8[id - 1].pos.Y + "";
            }
            else if (patern == 9)
            {
                if (modelSetting.Teaching.pt9 == null || modelSetting.Teaching.pt9.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt9[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt9[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt9[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt9[id - 1].pos.Y + "";
            }
            else if (patern == 10)
            {
                if (modelSetting.Teaching.pt10 == null || modelSetting.Teaching.pt10.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt10[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt10[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt10[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt10[id - 1].pos.Y + "";
            }
            else if (patern == 11)
            {
                if (modelSetting.Teaching.pt11 == null || modelSetting.Teaching.pt11.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt11[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt11[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt11[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt11[id - 1].pos.Y + "";
            }
            else if (patern == 12)
            {
                if (modelSetting.Teaching.pt12 == null || modelSetting.Teaching.pt12.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt12[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt12[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt12[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt12[id - 1].pos.Y + "";
            }
            else if (patern == 13)
            {
                if (modelSetting.Teaching.pt13 == null || modelSetting.Teaching.pt13.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt13[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt13[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt13[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt13[id - 1].pos.Y + "";
            }
            else if (patern == 14)
            {
                if (modelSetting.Teaching.pt14 == null || modelSetting.Teaching.pt14.Count<=0) return;
                lblPosX.Content = modelSetting.Teaching.pt14[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt14[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt14[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt14[id - 1].pos.Y + "";
            }
            else if (patern == 15)
            {
                if (modelSetting.Teaching.pt15 == null || modelSetting.Teaching.pt15.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt15[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt15[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt15[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt15[id - 1].pos.Y + "";
            }
            else if (patern == 16)
            {
                if (modelSetting.Teaching.pt16 == null || modelSetting.Teaching.pt16.Count <= 0) return;
                lblPosX.Content = modelSetting.Teaching.pt16[id - 1].pos.X;
                lblPosY.Content = modelSetting.Teaching.pt16[id - 1].pos.Y;
                txtPositionDataX.Text = modelSetting.Teaching.pt16[id - 1].pos.X + "";
                txtPositionDataY.Text = modelSetting.Teaching.pt16[id - 1].pos.Y + "";
            }
            dataX = (int)(Convert.ToDouble(txtPositionDataX.Text) * 1000);
            dataY = (int)(Convert.ToDouble(txtPositionDataY.Text) * 1000);
            flag = true;

        }
        private void ClearBackGroundButton()
        {
            foreach (var c in lstButtonPos)
            {
                c.Background = Brushes.LightGray;
            }
        }
        private int GetCellRow(int cellId, int pattern, int row, int column, bool use2Matrix)
        {
            if (!use2Matrix)
                return GetCellRowCaculator(cellId, pattern, row, column);

            int matrixCols = column / 2;
            int matrixSize = row * matrixCols;

            int localCellId = (cellId - 1) % matrixSize + 1;

            return GetCellRowCaculator(localCellId, pattern, row, matrixCols);
        }
        private int GetCellColumn(int cellId, int pattern, int row, int column, bool use2Matrix)
        {
            if (!use2Matrix)
                return GetCellColumnCaculator(cellId, pattern, row, column);

            int matrixCols = column / 2;
            int matrixSize = row * matrixCols;
            int gapCols = 1;

            int matrixIndex = (cellId - 1) / matrixSize; // 0 hoặc 1
            int localCellId = (cellId - 1) % matrixSize + 1;

            int col = GetCellColumnCaculator(localCellId, pattern, row, matrixCols);

            return col + matrixIndex * (matrixCols + gapCols);
        }
        private int GetCellRowCaculator(int cellId, int pattern, int row, int column)
        {
            int xSize = column;
            int ySize = row;
            // new
            if (pattern == 1 || pattern == 2)
            {
                return ySize - 1 - (cellId - 1) / xSize;
            }
            else if (pattern == 3 || pattern == 4)
            {
                return (cellId - 1) / xSize;
            }
            else if (pattern == 5)
            {
                if (((cellId - 1) / ySize) % 2 == 0)
                {
                    return ySize - 1 - ((cellId - 1) % ySize);
                }
                else
                {
                    return (cellId - 1) % ySize;
                }
            }
            else if (pattern == 6)
            {
                if (((cellId - 1) / ySize) % 2 != 0)
                {
                    return ySize - 1 - ((cellId - 1) % ySize);
                }
                else
                {
                    return (cellId - 1) % ySize;
                }
            }
            else if (pattern == 7)
            {
                if ((column - 1) % 2 != 0)
                {
                    if ((xSize - 1 - (cellId - 1) / ySize) % 2 != 0)
                    {
                        return (cellId - 1) % ySize;
                    }
                    else
                    {
                        return ySize - 1 - ((cellId - 1) % ySize);
                    }
                }
                else
                {
                    if ((xSize - 1 - (cellId - 1) / ySize) % 2 == 0)
                    {
                        return (cellId - 1) % ySize;
                    }
                    else
                    {
                        return ySize - 1 - ((cellId - 1) % ySize);
                    }
                }
            }
            else if (pattern == 8)
            {
                if ((column - 1) % 2 != 0)
                {
                    if ((xSize - 1 - (cellId - 1) / ySize) % 2 == 0)
                    {
                        return (cellId - 1) % ySize;
                    }
                    else
                    {
                        return ySize - 1 - ((cellId - 1) % ySize);
                    }
                }
                else
                {
                    if ((xSize - 1 - (cellId - 1) / ySize) % 2 != 0)
                    {
                        return (cellId - 1) % ySize;
                    }
                    else
                    {
                        return ySize - 1 - ((cellId - 1) % ySize);
                    }
                }
            }
            else if (pattern == 9 || pattern == 10)
            {
                return ySize - 1 - (cellId - 1) / xSize;
            }
            else if (pattern == 11 || pattern == 12)
            {
                return (cellId - 1) / xSize;
            }
            else if (pattern == 13 || pattern == 15)
            {
                return ySize - 1 - ((cellId - 1) % ySize);
            }
            else if (pattern == 14 || pattern == 16)
            {
                return (cellId - 1) % ySize;
            }
            return 0;
        }
        private int GetCellColumnCaculator(int cellId, int pattern, int row, int column)
        {
            int xSize = column;
            int ySize = row;

            if (pattern == 1)
            {
                if ((row - 1) % 2 != 0)
                {
                    if ((ySize - 1 - (cellId - 1) / xSize) % 2 != 0)
                    {
                        return (cellId - 1) % xSize;
                    }
                    else
                    {
                        return xSize - 1 - ((cellId - 1) % xSize);
                    }
                }
                else
                {
                    if ((ySize - 1 - (cellId - 1) / xSize) % 2 == 0)
                    {
                        return (cellId - 1) % xSize;
                    }
                    else
                    {
                        return xSize - 1 - ((cellId - 1) % xSize);
                    }
                }
            }
            else if (pattern == 2)
            {
                if ((row - 1) % 2 != 0)
                {
                    if ((ySize - 1 - (cellId - 1) / xSize) % 2 == 0)
                    {
                        return (cellId - 1) % xSize;
                    }
                    else
                    {
                        return xSize - 1 - ((cellId - 1) % xSize);
                    }
                }
                else
                {
                    if ((ySize - 1 - (cellId - 1) / xSize) % 2 != 0)
                    {
                        return (cellId - 1) % xSize;
                    }
                    else
                    {
                        return xSize - 1 - ((cellId - 1) % xSize);
                    }
                }
            }
            else if (pattern == 3)
            {
                if (((cellId - 1) / xSize) % 2 == 0)
                {
                    return (cellId - 1) % xSize;
                }
                else
                {
                    return xSize - 1 - ((cellId - 1) % xSize);
                }
            }
            else if (pattern == 4)
            {
                if (((cellId - 1) / xSize) % 2 != 0)
                {
                    return (cellId - 1) % xSize;
                }
                else
                {
                    return xSize - 1 - ((cellId - 1) % xSize);
                }
            }
            else if (pattern == 5 || pattern == 6)
            {
                return (cellId - 1) / ySize;
            }
            else if (pattern == 7 || pattern == 8)
            {
                return xSize - 1 - (cellId - 1) / ySize;
            }
            else if (pattern == 9 || pattern == 12)
            {
                return (cellId - 1) % xSize;
            }
            else if (pattern == 10 || pattern == 11)
            {
                return xSize - 1 - ((cellId - 1) % xSize);
            }
            else if (pattern == 13 || pattern == 14)
            {
                return (cellId - 1) / ySize;
            }
            else if (pattern == 15 || pattern == 16)
            {
                return xSize - 1 - (cellId - 1) / ySize;
            }
            return 0;
        }
        private void LoadAppsetting()
        {
            this.modelSetting = SystemsManager.Instance.currentModel;
        }
        public void Autogene1(int idPatern, double PitchX, double PitchY, double startPosX, double startPosY, int xColumn, int yRow, bool Use2Matrix)
        {
            /*Auto gener
             * Automatically calculate servo position according to Pitch
             * Parameter : 
             * idPatern : No. ID Patern.
             * PitchX : Distance in the X direction.
             * PitchY : Distance in the Y direction.
             * startPosX : Position start point X.
             * startPosY : Position start point Y.
             * xColumn : Number col in the X direction.
             * yRow : Number row in the Y direction.
            */
            double pitchArowX = 0;
            double pitchArowY = modelSetting.Pattern.offsetY;
            int count = xColumn / 2;
            if (idPatern == 1)
            {
                var autogen = this.modelSetting.Teaching;
                
                autogen.pt1 = new List<Pattern>();
                int id = 0;
                bool isAm = false;
                bool isDuong = false;
                double Xpitch = startPosX;
                double YpitchY = startPosY;
                int idPitchY = 0;
                for (int y = 0; y < yRow; y++)
                {
                    for (int x = 0; x < xColumn; x++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        pattern.pos.ID = id;
                        if ((yRow - 1) % 2 != 0)
                        {
                            if ((yRow - 1 - (id - 1) / xColumn) % 2 != 0)
                            {
                                isAm = false;
                                if (isDuong)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch += PitchX;
                                    }
                                }
                                isDuong = true;
                                if (x == (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                else if (x > (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                double ah = (Xpitch) + pitchArowX;
                                pattern.pos.X = Math.Round(ah, 3);
                                //pattern.pos.X = Math.Round(Xpitch, 3);
                            }
                            else
                            {
                                if (isAm)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch -= PitchX;
                                    }
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch, 3);
                                }
                                else
                                {
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch, 3);
                                }
                                isAm = true;
                                isDuong = false;
                            }
                        }
                        else
                        {
                            if ((yRow - 1 - (id - 1) / xColumn) % 2 == 0)
                            {
                                isAm = false;
                                if (isDuong)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch += PitchX;
                                    }
                                }
                                isDuong = true;
                                if (x == (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                else if (x > (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                double ah = (Xpitch) + pitchArowX;
                                pattern.pos.X = Math.Round(ah, 3);
                                //pattern.pos.X = Math.Round(Xpitch, 3);
                            }
                            else
                            {
                                if (isAm)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch -= PitchX;
                                    }
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch, 3);
                                }
                                else
                                {
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch, 3);
                                }
                                isDuong = false;
                                isAm = true;
                            }
                        }
                        if (id != 1)
                        {
                            pattern.pos.Y = Math.Round(startPosY - (PitchY * idPitchY),3);
                        }
                        autogen.pt1.Add(pattern);
                    }
                    idPitchY++;
                }
            }
            if (idPatern == 2)  
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt2 = new List<Pattern>();
                int id = 0;
                bool isAm = false;
                bool isDuong = false;
                double Xpitch = startPosX;
                double YpitchY = startPosY;
                int idPitchY = 0;
                for (int y = 0; y < yRow; y++)
                {
                    for (int x = 0; x < xColumn; x++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        pattern.pos.ID = id;
                        if ((yRow - 1) % 2 != 0)
                        {
                            if ((yRow - 1 - (id - 1) / xColumn) % 2 == 0)
                            {
                                isAm = false;
                                if (isDuong)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch += PitchX;
                                    }
                                }
                                isDuong = true;
                                if (x == (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                else if (x > (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                double ah = (Xpitch) + pitchArowX;
                                pattern.pos.X = Math.Round(ah, 3);
                                //pattern.pos.X = Math.Round(Xpitch, 3);
                            }
                            else
                            {
                                if (isAm)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch -= PitchX;
                                    }
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch, 3);
                                }
                                else
                                {
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch, 3);
                                }
                                isAm = true;
                                isDuong = false;
                            }
                        }
                        else
                        {
                            if ((yRow - 1 - (id - 1) / xColumn) % 2 != 0)
                            {
                                isAm = false;
                                if (isDuong)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch += PitchX;
                                    }
                                }
                                isDuong = true;
                                if (x == (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                else if (x > (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                double ah = (Xpitch) + pitchArowX;
                                pattern.pos.X = Math.Round(ah, 3);
                                //pattern.pos.X = Math.Round(Xpitch, 3);
                            }
                            else
                            {
                                if (isAm)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch -= PitchX;
                                    }
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch,3);
                                }
                                else
                                {
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch,3);
                                }
                                isDuong = false;
                                isAm = true;
                            }
                        }
                        if (id != 1)
                        {
                            pattern.pos.Y = Math.Round(startPosY - (PitchY * idPitchY), 3);
                        }
                        autogen.pt2.Add(pattern);
                    }
                    idPitchY++;
                }
            }
            if (idPatern == 3)
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt3 = new List<Pattern>();
                int id = 0;
                bool isAm = false;
                bool isDuong = false;
                double Xpitch = startPosX;
                double YpitchY = startPosY;
                int idPitchY = 0;
                for (int y = 0; y < yRow; y++)
                {
                    for (int x = 0; x < xColumn; x++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        pattern.pos.ID = id;
                        if ((yRow - 1) % 2 != 0)
                        {
                            if ((yRow - 1 - (id - 1) / xColumn) % 2 != 0)
                            {
                                isAm = false;
                                if (isDuong)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch += PitchX;
                                    }
                                }
                                isDuong = true;
                                if (x == (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                else if (x > (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                double ah = (Xpitch) + pitchArowX;
                                pattern.pos.X = Math.Round(ah, 3);
                                //pattern.pos.X = Math.Round(Xpitch,3);
                            }
                            else
                            {
                                if (isAm)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch -= PitchX;
                                    }
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch,3);
                                }
                                else
                                {
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch,3);
                                }
                                isAm = true;
                                isDuong = false;
                            }
                        }
                        else
                        {
                            if ((yRow - 1 - (id - 1) / xColumn) % 2 == 0)
                            {
                                isAm = false;
                                if (isDuong)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch += PitchX;
                                    }
                                }
                                isDuong = true;
                                if (x == (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                else if (x > (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                double ah = (Xpitch) + pitchArowX;
                                pattern.pos.X = Math.Round(ah, 3);
                                //pattern.pos.X = Math.Round(Xpitch,3);
                            }
                            else
                            {
                                if (isAm)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch -= PitchX;
                                    }
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch,3);
                                }
                                else
                                {
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch,3);
                                }
                                isDuong = false;
                                isAm = true;
                            }
                        }
                        if (id != 1)
                        {
                            pattern.pos.Y = Math.Round((PitchY * idPitchY) + startPosY,3);
                        }
                        autogen.pt3.Add(pattern);
                    }
                    idPitchY++;
                }
            }
            if (idPatern == 4)
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt4 = new List<Pattern>();
                int id = 0;
                bool isAm = false;
                bool isDuong = false;
                double Xpitch = startPosX;
                double YpitchY = startPosY;
                int idPitchY = 0;
                for (int y = 0; y < yRow; y++)
                {
                    for (int x = 0; x < xColumn; x++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        pattern.pos.ID = id;
                        if ((yRow - 1) % 2 != 0)
                        {
                            if ((yRow - 1 - (id - 1) / xColumn) % 2 == 0)
                            {
                                isAm = false;
                                if (isDuong)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch += PitchX;
                                    }
                                }
                                isDuong = true;
                                if (x == (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                else if (x > (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                double ah = (Xpitch) + pitchArowX;
                                pattern.pos.X = Math.Round(ah, 3);
                                //pattern.pos.X = Math.Round(Xpitch,3);
                            }
                            else
                            {
                                if (isAm)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch -= PitchX;
                                    }
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch,3);
                                }
                                else
                                {
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch,3);
                                }
                                isAm = true;
                                isDuong = false;
                            }
                        }
                        else
                        {
                            if ((yRow - 1 - (id - 1) / xColumn) % 2 != 0)
                            {
                                isAm = false;
                                if (isDuong)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch += PitchX;
                                    }
                                }
                                isDuong = true;
                                if (x == (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                else if (x > (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                double ah = (Xpitch) + pitchArowX;
                                pattern.pos.X = Math.Round(ah, 3);
                                //pattern.pos.X = Math.Round(Xpitch,3);
                            }
                            else
                            {
                                if (isAm)
                                {
                                    if (id == 1)
                                    {
                                        Xpitch = 0;
                                    }
                                    else
                                    {
                                        Xpitch -= PitchX;
                                    }
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch,3);
                                }
                                else
                                {
                                    if (x == (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    else if (x > (count))
                                    {
                                        pitchArowX = modelSetting.Pattern.offsetX;
                                    }
                                    double ah = (Xpitch) + pitchArowX;
                                    pattern.pos.X = Math.Round(ah, 3);
                                    //pattern.pos.X = Math.Round(Xpitch,3);
                                }
                                isDuong = false;
                                isAm = true;
                            }
                        }
                        if (id != 1)
                        {
                            pattern.pos.Y =Math.Round((PitchY * idPitchY) + startPosY,3);
                        }
                        autogen.pt4.Add(pattern);
                    }
                    idPitchY++;
                }
            }
            if (idPatern == 5)
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt5 = new List<Pattern>();
                int id = 0;
                bool isAm = false;
                bool isDuong = false;
                double Xpitch = startPosX;
                double Ypitch = startPosY;
                int idPitchX = 0;
                for (int y = 0; y < xColumn; y++)
                {
                    for (int x = 0; x < yRow; x++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        pattern.pos.ID = id;
                        if (((id - 1) / yRow) % 2 != 0)
                        {
                            isAm = false;
                            if (isDuong)
                            {
                                if (id == 1)
                                {
                                    Ypitch = 0;
                                }
                                else
                                {
                                    Ypitch += PitchY;
                                }
                            }
                            isDuong = true;
                            pattern.pos.Y = Math.Round(Ypitch, 3);
                        }
                        else
                        {
                            if (isAm)
                            {
                                if (id == 1)
                                {
                                    Ypitch = 0;
                                }
                                else
                                {
                                    Ypitch -= PitchY;
                                }
                                pattern.pos.Y = Math.Round(Ypitch, 3);
                            }
                            else
                            {
                                pattern.pos.Y = Math.Round(Ypitch, 3);
                            }
                            isAm = true;
                            isDuong = false;
                        }
                        if (id != 1)
                        {
                            if(y == (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            else if(y > (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            double ah = (startPosX + (PitchX * idPitchX))+ pitchArowX;
                            pattern.pos.X = Math.Round(ah, 3);
                        }
                        autogen.pt5.Add(pattern);
                    }
                    idPitchX++;
                }
            }
            if (idPatern == 6)
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt6 = new List<Pattern>();
                int id = 0;
                bool isAm = false;
                bool isDuong = false;
                double Xpitch = startPosX;
                double Ypitch = startPosY;
                int idPitchX = 0;
                for (int y = 0; y < xColumn; y++)
                {
                    for (int x = 0; x < yRow; x++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        pattern.pos.ID = id;
                        if (((id - 1) / yRow) % 2 == 0)
                        {
                            isAm = false;
                            if (isDuong)
                            {
                                if (id == 1)
                                {
                                    Ypitch = 0;
                                }
                                else
                                {
                                    Ypitch += PitchY;
                                }
                            }
                            isDuong = true;
                            pattern.pos.Y = Math.Round(Ypitch,3);
                        }
                        else
                        {
                            if (isAm)
                            {
                                if (id == 1)
                                {
                                    Ypitch = 0;
                                }
                                else
                                {
                                    Ypitch -= PitchY;
                                }
                                pattern.pos.Y = Math.Round(Ypitch,3);
                            }
                            else
                            {
                                pattern.pos.Y = Math.Round(Ypitch,3);
                            }
                            isAm = true;
                            isDuong = false;
                        }
                        if (id != 1)
                        {
                            if (y == (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            else if (y > (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            double ah = (startPosX + (PitchX * idPitchX)) + pitchArowX;
                            //pattern.pos.X = Math.Round(startPosX + (PitchX * idPitchX),3);
                            pattern.pos.X = Math.Round(ah, 3);
                        }
                        autogen.pt6.Add(pattern);
                    }
                    idPitchX++;
                }
            }
            if (idPatern == 7)
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt7 = new List<Pattern>();
                int id = 0;
                bool isAm = false;
                bool isDuong = false;
                double Xpitch = startPosX;
                double Ypitch = startPosY;
                int idPitchX = 0;
                for (int y = 0; y < xColumn; y++)
                {
                    for (int x = 0; x < yRow; x++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        pattern.pos.ID = id;
                        if ((xColumn - 1) % 2 != 0)
                        {
                            if ((xColumn - 1 - (id - 1) / yRow) % 2 != 0)
                            {
                                isAm = false;
                                if (isDuong)
                                {
                                    if (id == 1)
                                    {
                                        Ypitch = 0;
                                    }
                                    else
                                    {
                                        Ypitch += PitchY;
                                    }
                                }
                                isDuong = true;
                                pattern.pos.Y = Math.Round(Ypitch,3);
                            }
                            else
                            {
                                if (isAm)
                                {
                                    if (id == 1)
                                    {
                                        Ypitch = 0;
                                    }
                                    else
                                    {
                                        Ypitch -= PitchY;
                                    }
                                    pattern.pos.Y = Math.Round(Ypitch,3);
                                }
                                else
                                {
                                    pattern.pos.Y = Math.Round(Ypitch,3);
                                }
                                isAm = true;
                                isDuong = false;
                            }
                        }
                        else
                        {
                            if ((xColumn - 1 - (id - 1) / yRow) % 2 == 0)
                            {
                                isAm = false;
                                if (isDuong)
                                {
                                    if (id == 1)
                                    {
                                        Ypitch = 0;
                                    }
                                    else
                                    {
                                        Ypitch += PitchY;
                                    }
                                }
                                isDuong = true;
                                pattern.pos.Y = Math.Round(Ypitch,3);
                            }
                            else
                            {
                                if (isAm)
                                {
                                    if (id == 1)
                                    {
                                        Ypitch = 0;
                                    }
                                    else
                                    {
                                        Ypitch -= PitchY;
                                    }
                                    pattern.pos.Y = Math.Round(Ypitch,3);
                                }
                                else
                                {
                                    pattern.pos.Y = Math.Round(Ypitch,3);
                                }
                                isAm = true;
                                isDuong = false;
                            }
                        }
                        if (id != 1)
                        {
                            if (y == (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            else if (y > (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            double ah = (startPosX - (PitchX * idPitchX)) + pitchArowX;
                            pattern.pos.X = Math.Round(ah, 3);
                            //pattern.pos.X = Math.Round(startPosX - (PitchX * idPitchX),3);
                        }
                        autogen.pt7.Add(pattern);
                    }
                    idPitchX++;
                }
            }
            if (idPatern == 8)
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt8 = new List<Pattern>();
                int id = 0;
                bool isAm = false;
                bool isDuong = false;
                double Xpitch = startPosX;
                double Ypitch = startPosY;
                int idPitchX = 0;
                for (int y = 0; y < xColumn; y++)
                {
                    for (int x = 0; x < yRow; x++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        pattern.pos.ID = id;
                        if ((xColumn - 1) % 2 != 0)
                        {
                            if ((xColumn - 1 - (id - 1) / yRow) % 2 == 0)
                            {
                                isAm = false;
                                if (isDuong)
                                {
                                    if (id == 1)
                                    {
                                        Ypitch = 0;
                                    }
                                    else
                                    {
                                        Ypitch += PitchY;
                                    }
                                }
                                isDuong = true;
                                pattern.pos.Y = Math.Round(Ypitch,3);
                            }
                            else
                            {
                                if (isAm)
                                {
                                    if (id == 1)
                                    {
                                        Ypitch = 0;
                                    }
                                    else
                                    {
                                        Ypitch -= PitchY;
                                    }
                                    pattern.pos.Y = Math.Round(Ypitch,3);
                                }
                                else
                                {
                                    pattern.pos.Y = Math.Round(Ypitch,3);
                                }
                                isAm = true;
                                isDuong = false;
                            }
                        }
                        else
                        {
                            if ((xColumn - 1 - (id - 1) / yRow) % 2 != 0)
                            {
                                isAm = false;
                                if (isDuong)
                                {
                                    if (id == 1)
                                    {
                                        Ypitch = 0;
                                    }
                                    else
                                    {
                                        Ypitch += PitchY;
                                    }
                                }
                                isDuong = true;
                                pattern.pos.Y = Math.Round(Ypitch,3);
                            }
                            else
                            {
                                if (isAm)
                                {
                                    if (id == 1)
                                    {
                                        Ypitch = 0;
                                    }
                                    else
                                    {
                                        Ypitch -= PitchY;
                                    }
                                    pattern.pos.Y = Math.Round(Ypitch,3);
                                }
                                else
                                {
                                    pattern.pos.Y = Math.Round(Ypitch,3);
                                }
                                isAm = true;
                                isDuong = false;
                            }
                        }
                        if (id != 1)
                        {
                            if (y == (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            else if (y > (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            double ah = (startPosX - (PitchX * idPitchX)) + pitchArowX;
                            pattern.pos.X = Math.Round(ah, 3);
                            //pattern.pos.X = Math.Round(startPosX - (PitchX * idPitchX),3);
                        }
                        autogen.pt8.Add(pattern);
                    }
                    idPitchX++;
                }
            }
            if (idPatern == 9)
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt9 = new List<Pattern>();
                int id = 0;
                int idPitchY = 0;
                bool isRestart = false;
                double Xpitch = startPosX;
                for (int y = 0; y < yRow; y++)
                {
                    for (int x = 0; x < xColumn; x++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.ID = id;
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        if (id != 1)
                        {
                            if (!isRestart)
                            {
                                pattern.pos.X = Math.Round(startPosX,3);
                                Xpitch = startPosX;
                            }
                            else
                            {
                                Xpitch += PitchX;
                                if (x == (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                else if (x > (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                double ah = (Xpitch) + pitchArowX;
                                pattern.pos.X = Math.Round(ah, 3);
                            }
                            pattern.pos.Y = Math.Round(startPosY - (PitchY * idPitchY),3);
                        }
                        autogen.pt9.Add(pattern);
                        isRestart = true;
                    }
                    idPitchY++;
                    isRestart = false;
                }
            }
            if (idPatern == 10)
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt10 = new List<Pattern>();
                int id = 0;
                int idPitchY = 0;
                bool isRestart = false;
                double Xpitch = startPosX;
                for (int y = 0; y < yRow; y++)
                {
                    for (int x = 0; x < xColumn; x++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.ID = id;
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        if (id != 1)
                        {
                            if (!isRestart)
                            {
                                pattern.pos.X = Math.Round(startPosX,3);
                                Xpitch = startPosX;
                            }
                            else
                            {
                                Xpitch -= PitchX;
                                if (x == (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                else if (x > (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                double ah = (Xpitch) + pitchArowX;
                                pattern.pos.X = Math.Round(ah, 3);
                            }
                            pattern.pos.Y = Math.Round(startPosY - (PitchY * idPitchY),3);
                        }
                        autogen.pt10.Add(pattern);
                        isRestart = true;
                    }
                    idPitchY++;
                    isRestart = false;
                }
            }
            if (idPatern == 11)
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt11 = new List<Pattern>();
                int id = 0;
                int idPitchY = 0;
                bool isRestart = false;
                double Xpitch = startPosX;
                for (int y = 0; y < yRow; y++)
                {
                    for (int x = 0; x < xColumn; x++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.ID = id;
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        if (id != 1)
                        {
                            if (!isRestart)
                            {
                                pattern.pos.X = Math.Round(startPosX,3);
                                Xpitch = startPosX;
                            }
                            else
                            {
                                Xpitch -= PitchX;
                                if (x == (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                else if (x > (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                double ah = (Xpitch) + pitchArowX;
                                pattern.pos.X = Math.Round(ah, 3);
                            }
                            pattern.pos.Y = Math.Round(startPosY + (PitchY * idPitchY),3);
                        }
                        autogen.pt11.Add(pattern);
                        isRestart = true;
                    }
                    idPitchY++;
                    isRestart = false;
                }
            }
            if (idPatern == 12)
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt12 = new List<Pattern>();
                int id = 0;
                int idPitchY = 0;
                bool isRestart = false;
                double Xpitch = startPosX;
                for (int y = 0; y < yRow; y++)
                {
                    for (int x = 0; x < xColumn; x++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.ID = id;
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        if (id != 1)
                        {
                            if (!isRestart)
                            {
                                pattern.pos.X = Math.Round(startPosX,3);
                                Xpitch = startPosX;
                            }
                            else
                            {
                                Xpitch += PitchX;
                                if (x == (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                else if (x > (count))
                                {
                                    pitchArowX = modelSetting.Pattern.offsetX;
                                }
                                double ah = (Xpitch) + pitchArowX;
                                pattern.pos.X = Math.Round(ah, 3);
                            }
                            pattern.pos.Y = Math.Round(startPosY + (PitchY * idPitchY),3);
                        }
                        autogen.pt12.Add(pattern);
                        isRestart = true;
                    }
                    idPitchY++;
                    isRestart = false;
                }
            }
            if (idPatern == 13)
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt13 = new List<Pattern>();
                int id = 0;
                int idPitchY = 0;
                bool isRestart = false;
                double Ypitch = startPosY;
                for (int x = 0; x < xColumn; x++)
                {
                    for (int y = 0; y < yRow; y++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.ID = id;
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        if (id != 1)
                        {
                            if (!isRestart)
                            {
                                pattern.pos.Y = Math.Round(startPosY,3);
                                Ypitch = startPosY;
                            }
                            else
                            {
                                Ypitch -= PitchY;
                                pattern.pos.Y = Math.Round(Ypitch,3);
                            }
                            if (x == (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            else if (x > (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            double ah = (startPosX + (PitchX * idPitchY)) + pitchArowX;
                            pattern.pos.X = Math.Round(ah, 3);
                        }
                        autogen.pt13.Add(pattern);
                        isRestart = true;
                    }
                    idPitchY++;
                    isRestart = false;
                }
            }
            if (idPatern == 14)
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt14 = new List<Pattern>();
                int id = 0;
                int idPitchY = 0;
                bool isRestart = false;
                double Ypitch = startPosY;
                for (int x = 0; x < xColumn; x++)
                {
                    for (int y = 0; y < yRow; y++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.ID = id;
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        if (id != 1)
                        {
                            if (!isRestart)
                            {
                                pattern.pos.Y = Math.Round(startPosY,3);
                                Ypitch = startPosY;
                            }
                            else
                            {
                                Ypitch += PitchY;
                                pattern.pos.Y = Math.Round(Ypitch,3);
                            }
                            if (x == (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            else if (x > (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            double ah = (startPosX + (PitchX * idPitchY)) + pitchArowX;
                            pattern.pos.X = Math.Round(ah, 3);
                        }
                        autogen.pt14.Add(pattern);
                        isRestart = true;
                    }
                    idPitchY++;
                    isRestart = false;
                }
            }
            if (idPatern == 15)
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt15 = new List<Pattern>();
                int id = 0;
                int idPitchY = 0;
                bool isRestart = false;
                double Ypitch = startPosY;
                for (int x = 0; x < xColumn; x++)
                {
                    for (int y = 0; y < yRow; y++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.ID = id;
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        if (id != 1)
                        {
                            if (!isRestart)
                            {
                                pattern.pos.Y = Math.Round(startPosY,3);
                                Ypitch = startPosY;
                            }
                            else
                            {
                                Ypitch -= PitchY;
                                pattern.pos.Y = Math.Round(Ypitch,3);
                            }
                            if (x == (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            else if (x > (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            double ah = (startPosX - (PitchX * idPitchY)) + pitchArowX;
                            pattern.pos.X = Math.Round(ah, 3);
                        }
                        autogen.pt15.Add(pattern);
                        isRestart = true;
                    }
                    idPitchY++;
                    isRestart = false;
                }
            }
            if (idPatern == 16)
            {
                var autogen = this.modelSetting.Teaching;
                autogen.pt16 = new List<Pattern>();
                int id = 0;
                int idPitchY = 0;
                bool isRestart = false;
                double Ypitch = startPosY;
                for (int x = 0; x < xColumn; x++)
                {
                    for (int y = 0; y < yRow; y++)
                    {
                        id++;
                        Pattern pattern = new Pattern();
                        pattern.pos.ID = id;
                        pattern.pos.X = startPosX;
                        pattern.pos.Y = startPosY;
                        if (id != 1)
                        {
                            if (!isRestart)
                            {
                                pattern.pos.Y = Math.Round(startPosY,3);
                                Ypitch = startPosY;
                            }
                            else
                            {
                                Ypitch += PitchY;
                                pattern.pos.Y = Ypitch;
                            }
                            if (x == (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            else if (x > (count))
                            {
                                pitchArowX = modelSetting.Pattern.offsetX;
                            }
                            double ah = (startPosX - (PitchX * idPitchY)) + pitchArowX;
                            pattern.pos.X = Math.Round(ah,3);
                        }
                        autogen.pt16.Add(pattern);
                        isRestart = true;
                    }
                    idPitchY++;
                    isRestart = false;
                }
            }
        }
        public void Autogene(int idPatern, double PitchX, double PitchY, double startPosX, double startPosY, int xColumn, int yRow, bool Use2Matrix)
        {
            var autogen = this.modelSetting.Teaching;
            int halfCol = xColumn / 2;
            double offsetX = modelSetting.Pattern.offsetX;

            // --- Hàm tính ID: Ưu tiên nửa trái (Matrix 1) trước nếu Use2Matrix = true ---
            int GetID(int x, int y)
            {

                if (Use2Matrix)
                {
                    if (x < halfCol)
                    {
                        // Matrix 1 (Bên trái): Đánh số từ 1 đến (halfCol * yRow)
                        // Hàng 0: 1, 2... | Hàng 1: 6, 7... | Hàng 2: 11, 12...
                        return (y * halfCol) + x + 1;
                    }
                    else
                    {
                        // Matrix 2 (Bên phải): Bắt đầu từ ID sau khi Matrix 1 kết thúc
                        // Tổng số điểm Matrix 1 là (halfCol * yRow)
                        int pointsInMatrix1 = halfCol * yRow;
                        int localX = x - halfCol; // Cột tương đối trong Matrix 2 (0, 1, 2...)
                        int colsInMatrix2 = xColumn - halfCol;

                        return pointsInMatrix1 + (y * colsInMatrix2) + localX + 1;
                    }
                }

                // Trường hợp Use2Matrix : No (Đánh số liên tục hết chiều ngang rồi mới lên hàng)
                return (y * xColumn) + x + 1;
            }

            // --- Hàm tính X: Luôn có khoảng hở offsetX sau cột halfCol ---
            double GetX(int x)
            {
                double posX = startPosX + (x * PitchX);
                if (x >= halfCol) posX += offsetX;
                return Math.Round(posX, 3);
            }

            // --- GROUP 1: SNAKE HORIZONTAL (1-4) - Đi kiểu chữ S ngang ---
            if (idPatern == 1)
            {
                autogen.pt1 = new List<Pattern>();
                if (Use2Matrix)
                {
                    for (int y = 0; y < yRow; y++)
                    {
                        int yLogic = (yRow - 1) - y; // đi từ dưới lên
                        // ---- Matrix trái ----
                        for (int x = 0; x < halfCol; x++)
                        {
                            int xLogic = (yLogic % 2 == 0) ? x : (halfCol - 1 - x);
                            autogen.pt1.Add(new Pattern { pos = { ID = GetID(xLogic, yLogic), X  = GetX(x), Y = Math.Round(startPosY - yLogic * PitchY, 3) } });
                        }
                        // ---- Matrix phải ----
                        for (int x = halfCol; x < xColumn; x++)
                        {
                            int localX = x - halfCol;
                            int colsM2 = xColumn - halfCol;
                            int xLogic = halfCol + ((yLogic % 2 == 0) ? localX : (colsM2 - 1 - localX));
                            autogen.pt1.Add(new Pattern { pos = { ID = GetID(xLogic, yLogic), X  = GetX(x), Y = Math.Round(startPosY - yLogic * PitchY, 3) } });
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < yRow; y++)
                    {
                        int yLogic = (yRow - 1) - y; // dưới lên
                        for (int x = 0; x < xColumn; x++)
                        {
                            int xLogic = (yLogic % 2 == 0) ? x : (xColumn - 1 - x);
                            autogen.pt1.Add(new Pattern { pos = { ID = GetID(xLogic, yLogic), X  = GetX(x), Y  = Math.Round(startPosY - yLogic * PitchY, 3) } });
                        }
                    }
                }
                autogen.pt1 = autogen.pt1.OrderBy(p => p.pos.ID).ToList();
            }
            if (idPatern == 2)
            {
                autogen.pt2 = new List<Pattern>();
                if (Use2Matrix)
                {
                    for (int y = 0; y < yRow; y++)
                    {
                        int yLogic = (yRow - 1) - y; // đi từ dưới lên
                        // ---- Matrix trái ----
                        for (int x = 0; x < halfCol; x++)
                        {
                            int xLogic = (yLogic % 2 == 0) ? x : (halfCol - 1 - x);
                            autogen.pt2.Add(new Pattern { pos = { ID = GetID(xLogic, yLogic), X = GetX(x), Y = Math.Round(startPosY - yLogic * PitchY, 3) } });
                        }
                        // ---- Matrix phải ----
                        for (int x = halfCol; x < xColumn; x++)
                        {
                            int localX = x - halfCol;
                            int colsM2 = xColumn - halfCol;
                            int xLogic = halfCol + ((yLogic % 2 == 0) ? localX : (colsM2 - 1 - localX));
                            autogen.pt2.Add(new Pattern { pos = { ID = GetID(xLogic, yLogic), X = GetX(x), Y = Math.Round(startPosY - yLogic * PitchY, 3) } });
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < yRow; y++)
                    {
                        int yLogic = (yRow - 1) - y; // dưới lên
                        for (int x = 0; x < xColumn; x++)
                        {
                            int xLogic = (yLogic % 2 == 0) ? x : (xColumn - 1 - x);
                            autogen.pt2.Add(new Pattern { pos = { ID = GetID(xLogic, yLogic), X = GetX(x), Y = Math.Round(startPosY - yLogic * PitchY, 3) } });
                        }
                    }
                }
                autogen.pt2 = autogen.pt2.OrderBy(p => p.pos.ID).ToList();
            }
            if (idPatern == 3)
            {
                autogen.pt3 = new List<Pattern>();
                if (Use2Matrix)
                {
                    for (int y = 0; y < yRow; y++)
                    {
                        int yLogic = (yRow - 1) - y; // đi từ dưới lên
                        // ---- Matrix trái ----
                        for (int x = 0; x < halfCol; x++)
                        {
                            int xLogic = (yLogic % 2 == 0) ? x : (halfCol - 1 - x);
                            autogen.pt3.Add(new Pattern { pos = { ID = GetID(xLogic, yLogic), X = GetX(x), Y = Math.Round(startPosY + yLogic * PitchY, 3) } });
                        }
                        // ---- Matrix phải ----
                        for (int x = halfCol; x < xColumn; x++)
                        {
                            int localX = x - halfCol;
                            int colsM2 = xColumn - halfCol;
                            int xLogic = halfCol + ((yLogic % 2 == 0) ? localX : (colsM2 - 1 - localX));
                            autogen.pt3.Add(new Pattern { pos = { ID = GetID(xLogic, yLogic), X = GetX(x), Y = Math.Round(startPosY + yLogic * PitchY, 3) } });
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < yRow; y++)
                    {
                        int yLogic = (yRow - 1) - y; // dưới lên
                        for (int x = 0; x < xColumn; x++)
                        {
                            int xLogic = (yLogic % 2 == 0) ? x : (xColumn - 1 - x);
                            autogen.pt3.Add(new Pattern { pos = { ID = GetID(xLogic, yLogic), X = GetX(x), Y = Math.Round(startPosY + yLogic * PitchY, 3) } });
                        }
                    }
                }
                autogen.pt3 = autogen.pt3.OrderBy(p => p.pos.ID).ToList();
            }
            if (idPatern == 4)
            {
                autogen.pt4 = new List<Pattern>();
                if (Use2Matrix)
                {
                    for (int y = 0; y < yRow; y++)
                    {
                        int yLogic = (yRow - 1) - y; // đi từ dưới lên
                        // ---- Matrix trái ----
                        for (int x = 0; x < halfCol; x++)
                        {
                            int xLogic = (yLogic % 2 == 0) ? x : (halfCol - 1 - x);
                            autogen.pt4.Add(new Pattern { pos = { ID = GetID(xLogic, yLogic), X = GetX(x), Y = Math.Round(startPosY + yLogic * PitchY, 3) } });
                        }
                        // ---- Matrix phải ----
                        for (int x = halfCol; x < xColumn; x++)
                        {
                            int localX = x - halfCol;
                            int colsM2 = xColumn - halfCol;
                            int xLogic = halfCol + ((yLogic % 2 == 0) ? localX : (colsM2 - 1 - localX));
                            autogen.pt4.Add(new Pattern { pos = { ID = GetID(xLogic, yLogic), X = GetX(x), Y = Math.Round(startPosY + yLogic * PitchY, 3) } });
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < yRow; y++)
                    {
                        int yLogic = (yRow - 1) - y; // dưới lên
                        for (int x = 0; x < xColumn; x++)
                        {
                            int xLogic = (yLogic % 2 == 0) ? x : (xColumn - 1 - x);
                            autogen.pt4.Add(new Pattern { pos = { ID = GetID(xLogic, yLogic), X = GetX(x), Y = Math.Round(startPosY + yLogic * PitchY, 3) } });
                        }
                    }
                }
                autogen.pt4 = autogen.pt4.OrderBy(p => p.pos.ID).ToList();
            }

            // --- GROUP 2: SNAKE VERTICAL (5-8) - Đi kiểu chữ S dọc ---
            if (idPatern == 5)
            {
                autogen.pt5 = new List<Pattern>();
                for (int y = 0; y < yRow; y++)
                {
                    int yPhys = y; // y vật lý (0 = dưới)
                    for (int x = 0; x < xColumn; x++)
                    {
                        int id;
                        if (Use2Matrix)
                        {
                            if (x < halfCol)
                            {
                                // ===== Matrix trái =====
                                int baseId = x * yRow;
                                id = (x % 2 == 0) ? baseId + yPhys + 1 : baseId + (yRow - yPhys);
                            }
                            else
                            {
                                // ===== Matrix phải =====
                                int localX = x - halfCol;
                                int baseOffset = halfCol * yRow;
                                int baseId = baseOffset + localX * yRow;
                                id = (localX % 2 == 0) ? baseId + yPhys + 1 : baseId + (yRow - yPhys);
                            }
                        }
                        else
                        {
                            // ===== 1 matrix =====
                            int baseId = x * yRow;
                            id = (x % 2 == 0) ? baseId + yPhys + 1 : baseId + (yRow - yPhys);
                        }
                        autogen.pt5.Add(new Pattern { pos = {  ID = id, X  = GetX(x), Y  = Math.Round(startPosY - yPhys * PitchY, 3) } });
                    }
                }
                autogen.pt5 = autogen.pt5.OrderBy(p => p.pos.ID).ToList();
            }
            if (idPatern == 6)
            {
                autogen.pt6 = new List<Pattern>();
                for (int y = 0; y < yRow; y++)
                {
                    int yPhys = y; // y vật lý (0 = dưới)
                    for (int x = 0; x < xColumn; x++)
                    {
                        int id;
                        if (Use2Matrix)
                        {
                            if (x < halfCol)
                            {
                                // ===== Matrix trái =====
                                int baseId = x * yRow;
                                id = (x % 2 == 0) ? baseId + yPhys + 1 : baseId + (yRow - yPhys);
                            }
                            else
                            {
                                // ===== Matrix phải =====
                                int localX = x - halfCol;
                                int baseOffset = halfCol * yRow;
                                int baseId = baseOffset + localX * yRow;
                                id = (localX % 2 == 0) ? baseId + yPhys + 1 : baseId + (yRow - yPhys);
                            }
                        }
                        else
                        {
                            // ===== 1 matrix =====
                            int baseId = x * yRow;
                            id = (x % 2 == 0) ? baseId + yPhys + 1 : baseId + (yRow - yPhys);
                        }
                        autogen.pt6.Add(new Pattern { pos = { ID = id, X = GetX(x), Y = Math.Round(startPosY + yPhys * PitchY, 3) } });
                    }
                }
                autogen.pt6 = autogen.pt6.OrderBy(p => p.pos.ID).ToList();
            }
            if (idPatern == 7)
            {
                autogen.pt7 = new List<Pattern>();
                for (int y = 0; y < yRow; y++)
                {
                    int yPhys = y; // y vật lý (0 = dưới)
                    for (int x = 0; x < xColumn; x++)
                    {
                        int id;
                        if (Use2Matrix)
                        {
                            if (x < halfCol)
                            {
                                // ===== Matrix trái =====
                                int baseId = x * yRow;
                                id = (x % 2 == 0) ? baseId + yPhys + 1 : baseId + (yRow - yPhys);
                            }
                            else
                            {
                                // ===== Matrix phải =====
                                int localX = x - halfCol;
                                int baseOffset = halfCol * yRow;
                                int baseId = baseOffset + localX * yRow;
                                id = (localX % 2 == 0) ? baseId + yPhys + 1 : baseId + (yRow - yPhys);
                            }
                        }
                        else
                        {
                            // ===== 1 matrix =====
                            int baseId = x * yRow;
                            id = (x % 2 == 0) ? baseId + yPhys + 1 : baseId + (yRow - yPhys);
                        }
                        autogen.pt7.Add(new Pattern { pos = { ID = id, X = GetX(x), Y = Math.Round(startPosY + yPhys * PitchY, 3) } });
                    }
                }
                autogen.pt7 = autogen.pt7.OrderBy(p => p.pos.ID).ToList();
            }
            if (idPatern == 8)
            {
                autogen.pt8 = new List<Pattern>();
                for (int y = 0; y < yRow; y++)
                {
                    int yPhys = y; // y vật lý (0 = dưới)
                    for (int x = 0; x < xColumn; x++)
                    {
                        int id;
                        if (Use2Matrix)
                        {
                            if (x < halfCol)
                            {
                                // ===== Matrix trái =====
                                int baseId = x * yRow;
                                id = (x % 2 == 0) ? baseId + yPhys + 1 : baseId + (yRow - yPhys);
                            }
                            else
                            {
                                // ===== Matrix phải =====
                                int localX = x - halfCol;
                                int baseOffset = halfCol * yRow;
                                int baseId = baseOffset + localX * yRow;
                                id = (localX % 2 == 0) ? baseId + yPhys + 1 : baseId + (yRow - yPhys);
                            }
                        }
                        else
                        {
                            // ===== 1 matrix =====
                            int baseId = x * yRow;
                            id = (x % 2 == 0) ? baseId + yPhys + 1 : baseId + (yRow - yPhys);
                        }
                        autogen.pt8.Add(new Pattern { pos = { ID = id, X = GetX(x), Y = Math.Round(startPosY - yPhys * PitchY, 3) } });
                    }
                }
                autogen.pt8 = autogen.pt8.OrderBy(p => p.pos.ID).ToList();
            }

            // --- GROUP 3: ZIG-ZAG HORIZONTAL (9-12) - Quét 1 chiều ngang ---
            if (idPatern == 9)
            { // Trái-Trên, Quét ngang 1 chiều
                autogen.pt9 = new List<Pattern>();
                for (int y = 0; y < yRow; y++)
                    for (int x = 0; x < xColumn; x++)
                        autogen.pt9.Add(new Pattern { pos = { ID = GetID(x, y), X = GetX(x), Y = Math.Round(startPosY - (y * PitchY), 3) } });
                autogen.pt9 = autogen.pt9.OrderBy(p => p.pos.ID).ToList();
            }
            if (idPatern == 10)
            { // Phải-Trên, Quét ngang 1 chiều
                autogen.pt10 = new List<Pattern>();
                for (int y = 0; y < yRow; y++)
                    for (int x = 0; x < xColumn; x++)
                    {
                        int xIdx = xColumn - 1 - x;
                        autogen.pt10.Add(new Pattern { pos = { ID = GetID(xIdx, y), X = GetX(xIdx), Y = Math.Round(startPosY - (y * PitchY), 3) } });
                    }
                autogen.pt10 = autogen.pt10.OrderBy(p => p.pos.ID).ToList();
            }
            if (idPatern == 11)
            { // Trái-Dưới, Quét ngang 1 chiều
                autogen.pt11 = new List<Pattern>();
                for (int y = 0; y < yRow; y++)
                    for (int x = 0; x < xColumn; x++)
                    {
                        int yIdx = yRow - 1 - y;
                        autogen.pt11.Add(new Pattern { pos = { ID = GetID(x, yIdx), X = GetX(x), Y = Math.Round(startPosY - (yIdx * PitchY), 3) } });
                    }
                autogen.pt11 = autogen.pt11.OrderBy(p => p.pos.ID).ToList();
            }
            if (idPatern == 12)
            { // Phải-Dưới, Quét ngang 1 chiều
                autogen.pt12 = new List<Pattern>();
                for (int y = 0; y < yRow; y++)
                    for (int x = 0; x < xColumn; x++)
                    {
                        int xIdx = xColumn - 1 - x;
                        int yIdx = yRow - 1 - y;
                        autogen.pt12.Add(new Pattern { pos = { ID = GetID(xIdx, yIdx), X = GetX(xIdx), Y = Math.Round(startPosY + (yIdx * PitchY), 3) } });
                    }
                autogen.pt12 = autogen.pt12.OrderBy(p => p.pos.ID).ToList();
            }

            // --- GROUP 4: ZIG-ZAG VERTICAL (13-16) - Quét 1 chiều dọc ---
            if (idPatern == 13)
            { // Trái-Trên, Quét dọc 1 chiều
                autogen.pt13 = new List<Pattern>();
                for (int x = 0; x < xColumn; x++)
                    for (int y = 0; y < yRow; y++)
                    {
                        int yIdx = y;
                        autogen.pt13.Add(new Pattern { pos = { ID = x, X = GetX(x), Y = Math.Round(startPosY - (yIdx * PitchY), 3) } });
                    }
                autogen.pt13 = autogen.pt13.OrderBy(p => p.pos.ID).ToList();
            }
            if (idPatern == 14)
            { // Trái-Dưới, Quét dọc 1 chiều
                autogen.pt14 = new List<Pattern>();
                for (int x = 0; x < xColumn; x++)
                    for (int y = 0; y < yRow; y++)
                    {
                        int yIdx = y;
                        autogen.pt14.Add(new Pattern { pos = { ID = x, X = GetX(x), Y = Math.Round(startPosY + (yIdx * PitchY), 3) } });
                    }
                autogen.pt14 = autogen.pt14.OrderBy(p => p.pos.ID).ToList();
            }
            if (idPatern == 15)
            { // Phải-Trên, Quét dọc 1 chiều
                autogen.pt15 = new List<Pattern>();
                for (int x = 0; x < xColumn; x++)
                    for (int y = 0; y < yRow; y++)
                    {
                        int xIdx = x;
                        autogen.pt15.Add(new Pattern { pos = { ID = x, X = GetX(xIdx), Y = Math.Round(startPosY - (y * PitchY), 3) } });
                    }
                autogen.pt15 = autogen.pt15.OrderBy(p => p.pos.ID).ToList();
            }
            if (idPatern == 16)
            { // Phải-Dưới, Quét dọc 1 chiều
                autogen.pt16 = new List<Pattern>();
                for (int x = 0; x < xColumn; x++)
                    for (int y = 0; y < yRow; y++)
                    {
                        int xIdx = x;
                        int yIdx = y;
                        autogen.pt16.Add(new Pattern { pos = { ID = x, X = GetX(xIdx), Y = Math.Round(startPosY - (yIdx * PitchY), 3) } });
                    }
                autogen.pt16 = autogen.pt16.OrderBy(p => p.pos.ID).ToList();
            }
        }
        private void EditPositon(int Patern, int id, double valuePosX, double valuePosY)
        {
            var autogen = this.modelSetting.Teaching;
            if (Patern == 1)
            {
                if (autogen.pt1 == null) return;
                autogen.pt1[id - 1].pos.X = valuePosX;
                autogen.pt1[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 2)
            {
                if (autogen.pt2 == null) return;
                autogen.pt2[id - 1].pos.X = valuePosX;
                autogen.pt2[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 3)
            {
                if (autogen.pt3 == null) return;
                autogen.pt3[id - 1].pos.X = valuePosX;
                autogen.pt3[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 4)
            {
                if (autogen.pt4 == null) return;
                autogen.pt4[id - 1].pos.X = valuePosX;
                autogen.pt4[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 5)
            {
                if (autogen.pt5 == null) return;
                autogen.pt5[id - 1].pos.X = valuePosX;
                autogen.pt5[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 6)
            {
                if (autogen.pt6 == null) return;
                autogen.pt6[id - 1].pos.X = valuePosX;
                autogen.pt6[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 7)
            {
                if (autogen.pt7 == null) return;
                autogen.pt7[id - 1].pos.X = valuePosX;
                autogen.pt7[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 8)
            {
                if (autogen.pt8 == null) return;
                autogen.pt8[id - 1].pos.X = valuePosX;
                autogen.pt8[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 9)
            {
                if (autogen.pt9 == null) return;
                autogen.pt9[id - 1].pos.X = valuePosX;
                autogen.pt9[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 10)
            {
                if (autogen.pt10 == null) return;
                autogen.pt10[id - 1].pos.X = valuePosX;
                autogen.pt10[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 11)
            {
                if (autogen.pt11 == null) return;
                autogen.pt11[id - 1].pos.X = valuePosX;
                autogen.pt11[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 12)
            {
                if (autogen.pt12 == null) return;
                autogen.pt12[id - 1].pos.X = valuePosX;
                autogen.pt12[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 13)
            {
                if (autogen.pt13 == null) return;
                autogen.pt13[id - 1].pos.X = valuePosX;
                autogen.pt13[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 14)
            {
                if (autogen.pt14 == null) return;
                autogen.pt14[id - 1].pos.X = valuePosX;
                autogen.pt14[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 15)
            {
                if (autogen.pt15 == null) return;
                autogen.pt15[id - 1].pos.X = valuePosX;
                autogen.pt15[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
            if (Patern == 16)
            {
                if (autogen.pt16 == null) return;
                autogen.pt16[id - 1].pos.X = valuePosX;
                autogen.pt16[id - 1].pos.Y = valuePosY;
                BLLManager.Instance.SaveModel();
            }
        }
        private void PgTeachingMenu_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                this.logger.Create("PgTeachingMenu_Unloaded: " + ex.Message, LogLevel.Error);
            }
        }
        private void PgTeachingMenu_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.LoadAppsetting();
                CheckLock();
                generateCells(modelSetting.Pattern.xRow, modelSetting.Pattern.yColumn, modelSetting.Pattern.CurrentPatern, modelSetting.Pattern.Use2Matrix);
                string filePath = string.Format("/Resource/Image/{0}.PNG", modelSetting.Pattern.CurrentPatern);
                imgPattern.Source = new BitmapImage(new Uri(filePath, UriKind.Relative));
            }
            catch (Exception ex)
            {
                this.logger.Create("PgTeachingMenu_Loaded: " + ex.Message, LogLevel.Error);
            }
        }
        private void CheckLock()
        {
            if (!UserManagers.Instance.CheckAssignLevel(PAGE_ID.PAGE_MENU_TEACHING.ToString()))
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
