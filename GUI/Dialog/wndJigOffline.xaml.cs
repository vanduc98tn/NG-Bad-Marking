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
using System.Windows.Shapes;
using DTO;
using BLL;

namespace GUI
{
    /// <summary>
    /// Interaction logic for wndJigOffline.xaml
    /// </summary>
    public partial class wndJigOffline : Window
    {
        private Brush EM_COLOR = Brushes.Red;
        private JIGAoiOffline jIGAoiOffline;
        public wndJigOffline()
        {
            InitializeComponent();
            this.Loaded += WndJigOffline_Loaded;
            this.btClose.Click += BtClose_Click;
            this.btClose.TouchDown+= BtClose_Click;

            this.btCancel.Click += BtCancel_Click;
            this.btCancel.TouchDown+= BtCancel_Click;
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            this.GetSelectPositon();
            this.Close();
        }

        private void WndJigOffline_Loaded(object sender, RoutedEventArgs e)
        {
            this.generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, SystemsManager.Instance.currentModel.Pattern.CurrentPatern, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
            LoadPosition();
        }

        public JIGAoiOffline DoSettings(Window owner, JIGAoiOffline oldSettings)
        {
            this.Owner = owner;
            this.jIGAoiOffline = oldSettings;
            this.ShowDialog();
            return this.jIGAoiOffline;
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
        private void generateCells1(int rowCnt, int colCnt, int pattern, bool Use2Matrix)
        {
            this.gridSelections.Children.Clear();
            this.gridSelections.RowDefinitions.Clear();
            for (int r = 0; r < rowCnt; r++)
            {
                var rowDef = new RowDefinition();
                rowDef.Height = new GridLength(1, GridUnitType.Star);
                this.gridSelections.RowDefinitions.Add(rowDef);
            }
            this.gridSelections.ColumnDefinitions.Clear();
            for (int c = 0; c < colCnt; c++)
            {
                var colDef = new ColumnDefinition();
                colDef.Width = new GridLength(1, GridUnitType.Star);
                this.gridSelections.ColumnDefinitions.Add(colDef);
            }
            for (int id = 1; id <= rowCnt * colCnt; id++)
            {
                var r = GetCellRow(id, pattern, rowCnt, colCnt, Use2Matrix);
                var c = GetCellColumn(id, pattern, rowCnt, colCnt, Use2Matrix);
                var cell = createCell(id);
                gridSelections.Children.Add(cell);
                Grid.SetRow(cell, r);
                Grid.SetColumn(cell, c);
            }
        }
        private void generateCells(int rowCnt, int colCnt, int pattern, bool Use2Matrix)
        {
            //lstButtonPos = new List<Button>();
            gridSelections.Children.Clear();
            gridSelections.RowDefinitions.Clear();
            gridSelections.ColumnDefinitions.Clear();

            // Rows
            for (int r = 0; r < rowCnt; r++)
            {
                gridSelections.RowDefinitions.Add(
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
                    gridSelections.ColumnDefinitions.Add(
                        new ColumnDefinition { Width = new GridLength(5) } // px
                    );
                }
                else
                {
                    gridSelections.ColumnDefinitions.Add(
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
                gridSelections.Children.Add(cell);
                Grid.SetRow(cell, r);
                Grid.SetColumn(cell, c);
            }
        }
        private void GetSelectPositon()
        {
            var lstPos = new List<int>();
            foreach (var cell in gridSelections.Children)
            {
                var btCell = cell as Label;
                
                if (btCell != null && btCell.Background == EM_COLOR)
                {
                    var textcell = btCell.Content as TextBlock;
                    lstPos.Add(Convert.ToInt32(textcell.Text.ToString()));
                }
            }
            lstPos.Sort();
            SystemsManager.Instance.AppSettings.JigAoiOffline.positionNGs = new List<int>(lstPos);
            SystemsManager.Instance.AppSettings.JigAoiOffline.pattern = SystemsManager.Instance.currentModel.Pattern.CurrentPatern;
            SystemsManager.Instance.SaveAppSettings();
        }
        private void LoadPosition()
        {
            var pos = SystemsManager.Instance.AppSettings.JigAoiOffline.positionNGs;
            for(int i = 0; i< pos.Count;i++)
            {
                FindPos(pos[i]);
            }
            
        }
        private void FindPos(int id)
        {
            foreach (var cell in gridSelections.Children)
            {
                var btCell = cell as Label;
                if(btCell!=null)
                {
                    var textcell = btCell.Content as TextBlock;
                    if (textcell.Text == id.ToString())
                    {
                        btCell.Background = EM_COLOR;
                    }
                }
                
            }
        }
        private Label createCell(int number)
        {
            var cell = new Label();
            cell.Tag = false; // Unselected
            cell.Content = createCellContent(String.Format("{0}", number));
            cell.Name = String.Format("lblCell{0:00}", number);
            cell.HorizontalContentAlignment = HorizontalAlignment.Center;
            cell.VerticalContentAlignment = VerticalAlignment.Center;
            cell.BorderThickness = new Thickness(1);
            cell.BorderBrush = Brushes.Gray;

            cell.MouseDown += this.Cell_MouseDown;
            return cell;
        }
        private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var cell = sender as Label;
            if ((bool)cell.Tag)
            {
                cell.Tag = false;
                cell.ClearValue(Label.BackgroundProperty);
            }
            else
            {
                cell.Tag = true;
                cell.Background = EM_COLOR;
            }
            updateCounter();
        }
        private void updateCounter()
        {
            var cellItems = this.gridSelections.Children;
            int remain = 0;
            foreach (var cell in cellItems)
            {
                var lbl = cell as Label;
                if (!(bool)lbl.Tag)
                {
                    remain++;
                }
            }
            this.lblRemain.Content = remain.ToString();
        }
        private object createCellContent(String qr)
        {
            var cellText = new TextBlock();
            cellText.TextWrapping = TextWrapping.Wrap;
            cellText.FontSize = 10;
            cellText.Text = String.Format("{0}", qr);
            return cellText;
        }
    }
}
