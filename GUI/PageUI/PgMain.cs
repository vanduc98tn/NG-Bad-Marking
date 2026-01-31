using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using DTO;
using BLL;
using System.Windows.Media.Imaging;

namespace GUI
{
    public partial class PgMain: Page
    {

        private bool isShowMESOK = false;
        private List<int> lstDataAoiResult;
        private List<int> lstDataAoiResultNormal;
        private List<int> lstDataAoiResultReverse;
        private int PaternAoiResult;
        private string cmdSendAOI = "<NG_COMP_LIST_REQ>";
        private int CountNGResult;
        private bool isReceive = false;
        private string ResultAoi = "";

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
            this.gridResults.Children.Clear();
            this.gridResults.RowDefinitions.Clear();
            for (int r = 0; r < rowCnt; r++)
            {
                var rowDef = new RowDefinition();
                rowDef.Height = new GridLength(1, GridUnitType.Star);
                this.gridResults.RowDefinitions.Add(rowDef);
            }
            this.gridResults.ColumnDefinitions.Clear();
            for (int c = 0; c < colCnt; c++)
            {
                var colDef = new ColumnDefinition();
                colDef.Width = new GridLength(1, GridUnitType.Star);
                this.gridResults.ColumnDefinitions.Add(colDef);
            }
            for (int id = 1; id <= rowCnt * colCnt; id++)
            {
                var r = GetCellRow(id, pattern, rowCnt, colCnt, Use2Matrix);
                var c = GetCellColumn(id, pattern, rowCnt, colCnt, Use2Matrix);
                var cell = createCell(id);
                gridResults.Children.Add(cell);
                Grid.SetRow(cell, r);
                Grid.SetColumn(cell, c);
            }
        }
        private void generateCells(int rowCnt, int colCnt, int pattern, bool Use2Matrix)
        {
            //lstButtonPos = new List<Button>();
            gridResults.Children.Clear();
            gridResults.RowDefinitions.Clear();
            gridResults.ColumnDefinitions.Clear();

            // Rows
            for (int r = 0; r < rowCnt; r++)
            {
                gridResults.RowDefinitions.Add(
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
                    gridResults.ColumnDefinitions.Add(
                        new ColumnDefinition { Width = new GridLength(5) } // px
                    );
                }
                else
                {
                    gridResults.ColumnDefinitions.Add(
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
                gridResults.Children.Add(cell);
                Grid.SetRow(cell, r);
                Grid.SetColumn(cell, c);
            }
        }
        private Label createCell(int number)
        {
            var cell = new Label();
            cell.Content = createCellContent(String.Format("{0}", number));
            cell.Name = String.Format("lblCell{0:00}", number);
            cell.HorizontalContentAlignment = HorizontalAlignment.Center;
            cell.VerticalContentAlignment = VerticalAlignment.Center;
            cell.Margin = new Thickness(1, 1, 1, 1);
            cell.FontWeight = FontWeights.Bold;
            cell.Background = Brushes.LightBlue;
            return cell;
        }
        private object createCellContent(String qr)
        {
            var cellText = new TextBlock();
            cellText.TextWrapping = TextWrapping.Wrap;
            cellText.Text = String.Format("{0}", qr);
            cellText.FontSize = 10;
            return cellText;
        }
        private Int32 findPos(int id, int pattern, string dir)
        {
            Int32 ret = 0;
            if (pattern == 1)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt1[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt1[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 2)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt2[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt2[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 3)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt3[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt3[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 4)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt4[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt4[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 5)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt5[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt5[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 6)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt6[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt6[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 7)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt7[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt7[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 8)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt8[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt8[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 9)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt9[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt9[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 10)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt10[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt10[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 11)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt11[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt11[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 12)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt12[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt12[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 13)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt13[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt13[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 14)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt14[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt14[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 15)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt15[id - 1].pos.X;
                    return ret = (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt15[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }
            else if (pattern == 16)
            {
                if (dir == "X")
                {
                    var x = SystemsManager.Instance.currentModel.Teaching.pt16[id - 1].pos.X;
                    return ret =  (int)(x * 1000);
                }
                var y = SystemsManager.Instance.currentModel.Teaching.pt16[id - 1].pos.Y;
                return ret = (int)(y * 1000);
            }

            return ret;
        }
        private int ShowUiReverse()
        {
            int PatternReverse = 0;
            try
            {
                if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 1)
                {
                    PatternReverse = 3;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 2)
                {
                    PatternReverse = 4;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 3)
                {
                    PatternReverse = 1;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 4)
                {
                    PatternReverse = 2;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 5)
                {
                    PatternReverse = 6;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 6)
                {
                    PatternReverse = 5;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 7)
                {
                    PatternReverse = 8;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 8)
                {
                    PatternReverse = 7;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 9)
                {
                    PatternReverse = 12;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 10)
                {
                    PatternReverse = 11;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 11)
                {
                    PatternReverse = 10;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 12)
                {
                    PatternReverse = 9;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 13)
                {
                    PatternReverse = 14;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 14)
                {
                    PatternReverse = 13;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 15)
                {
                    PatternReverse = 16;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
                else if (SystemsManager.Instance.currentModel.Pattern.CurrentPatern == 16)
                {
                    PatternReverse = 15;
                    if (this.isShowReverse) return PatternReverse;
                    Dispatcher.Invoke(() => {
                        generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, PatternReverse, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                        showPosNGtoUI(lstDataAoiResult);
                    });
                }
            }
            catch(Exception ex)
            {
                logger.Create("ShowUiReverse: " + ex.Message, LogLevel.Error);
            }
            return PatternReverse;
        }
        private void showPosNGtoUI(List<int> lst)
        {
            int total = SystemsManager.Instance.currentModel.Pattern.xRow * SystemsManager.Instance.currentModel.Pattern.yColumn;
            for (int i = 0; i < lst.Count; i++)
            {
                Label x = findCellControl(gridResults, lst[i]);
                x.Background = Brushes.Red;
            }
            for (int i = 0; i < total; i++)
            {
                Label x = findCellControl(gridResults, i + 1);
                if (x.Background != Brushes.Red)
                {
                    x.Background = System.Windows.Media.Brushes.Lime;
                }
            }
        }
        private Label findCellControl(Grid grid, int pkgId)
        {
            var cellName = String.Format("lblCell{0:00}", pkgId);
            foreach (var cell in grid.Children)
            {
                var lblCell = cell as Label;
                if (lblCell != null && lblCell.Name.Equals(cellName))
                {
                    return lblCell;
                }
            }
            return null;
        }
        private void editResultAoi(string AOI,string barcode)
        {
            lstDataAoiResult = new List<int>();
            var dataAOI = AOI.Split(',');
            if (dataAOI.Length == 0)
            {
                //Show Alarm
                lstDataAoiResult = null;
                return;
            }
            var cmd = dataAOI[0];
            if (cmd != "NG_COMP_LIST_REP")
            {
                //show alarm
                lstDataAoiResult = null;
                return;
            }
            var barcodeAOI = dataAOI[1];
            if (barcodeAOI != barcode)
            {
                //show alarm
                lstDataAoiResult = null;
                return;
            }
            var data = dataAOI[2];
            if (data != "GUID")
            {
                //show alarm
                lstDataAoiResult = null;
                return;
            }
            this.CountNGResult = Convert.ToInt32(dataAOI[3]);
            lblAoiCountNG.Content = this.CountNGResult;
            if (this.CountNGResult == 0)
            {
                isReceive = true;
                return;
            }
            else
            {
                this.PaternAoiResult = Convert.ToInt32(dataAOI[4]);

                lblArrayDirection.Content = this.PaternAoiResult;
                string filePath = string.Format("/Resource/Image/{0}.PNG", this.PaternAoiResult);
                imgPattern.Source = new BitmapImage(new Uri(filePath, UriKind.Relative));
                
                for (int i = 5; i < dataAOI.Length; i++)
                {
                    lstDataAoiResult.Add(Convert.ToInt32(dataAOI[i]));
                }
            }
            isReceive = true;
        }
        private bool AOIOfflineData(string barcode)
        {
            MESCheck data = new MESCheck();
            data.LotNo = lotInData.LotId;
            data.MES_Result = "OFF";
            data.PCB_Code = barcode;
            this.LogDataMES(data);
            if (SystemsManager.Instance.AppSettings.JigAoiOffline.positionNGs == null) return false;
            if (SystemsManager.Instance.AppSettings.JigAoiOffline.positionNGs.Count <= 0) return false;
            this.PaternAoiResult = SystemsManager.Instance.AppSettings.JigAoiOffline.pattern;
            this.lstDataAoiResult = new List<int>(SystemsManager.Instance.AppSettings.JigAoiOffline.positionNGs);
            return true;
        }
        private void ShowLog(byte[] txBuf)
        {
            this.Dispatcher.Invoke(() => {
                var str = DateTime.Now.ToString("HH:mm:ss.ff") + ":ITM->AOI:" + ASCIIEncoding.ASCII.GetString(txBuf);
                this.txtLogMes.Text += String.Format("\r\n{0}", str);
                this.txtLogMes.ScrollToEnd();
            });
        }
        private void FillDataAOI(byte[] buf,string barcode)
        {
            try
            {
                isReceive = false;
                this.Dispatcher.Invoke(() => {
                    var str = DateTime.Now.ToString("HH:mm:ss.ff") + ":AOI->ITM:" + ASCIIEncoding.ASCII.GetString(buf);
                    ResultAoi = ASCIIEncoding.ASCII.GetString(buf);
                    ResultAoi = ResultAoi.Remove(0, 1);
                    ResultAoi = ResultAoi.Remove(ResultAoi.Length - 1, 1);
                    editResultAoi(ResultAoi, barcode);
                    this.txtLogMes.Text += String.Format("\r\n{0}", str);
                    this.txtLogMes.ScrollToEnd();
                });
            }
            catch (Exception ex)
            {
                logger.Create("FillDataAOI: " + ex.Message,LogLevel.Error);
            }
        }
        private void CloseAllDialog()
        {
            try
            {
                if (!this.isShowMESOK) return;
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != Application.Current.MainWindow && window.IsLoaded)
                    {
                        window.Close();
                    }
                }
                this.isShowMESOK = false;
            }
            catch (Exception ex)
            {
                this.logger.Create("CloseAllDialog: " + ex.Message, LogLevel.Error);
            }
        }
        private async void ShowAlarm(string message, string solution, string code)
        {
            await Task.Delay(100);
            var alarmService = new SqlLiteAlarmLog();
            var alarm = new AlarmLog(code, message, solution, "AUTO");
            var alarmLog = new AlarmLogService(alarmService.AlarmLogRepository);
            alarmLog.CreateAlarm(alarm);
            this.isShowMESOK = true;
            var alarmMessage = new WndAlarmShow(message, solution, code);
            Task.Run(() => {
                this.Dispatcher.Invoke(() => { alarmMessage.ShowDialog(); });
            });
        }
        //public async Task<int> SendToAoi(string barcode)
        //{
        //    // 0 = Error AOI.
        //    // 1 = Pass.
        //    // 2 = NG.
        //    //int ret = 0;
        //    //if (SystemsManager.Instance.AppSettings.RunSetting.AOIOnline)
        //    //{
        //    //    isReceive = false;
        //    //    var txBuf = ASCIIEncoding.ASCII.GetBytes(cmdSendAOI+","+barcode+",GUID");
        //    //    ShowLog(txBuf);
        //    //    var txReceiver = await BLLManager.Instance.serviceAOI.ReadAOI(cmdSendAOI + "," + barcode + ",GUID");
        //    //    if (string.IsNullOrEmpty(txReceiver))
        //    //    {
        //    //        //AOI Time out;
        //    //        ShowAlarm("Read AOI Time Out!!!", "Check Machine AOI , Check Cable", "3000");
        //    //        return ret = 0;
        //    //    }
        //    //    byte[] data = ASCIIEncoding.ASCII.GetBytes(txReceiver);
        //    //    this.FillDataAOI(data, barcode);
        //    //    this.UpdateLogs(txReceiver);

        //    //    if (txReceiver!=null)
        //    //    {
        //    //        this.UpdateLogs("AOI Receivered!");
        //    //        if (lstDataAoiResult != null)
        //    //        {
        //    //            if (CountNGResult == 0)
        //    //            {
        //    //                return ret = 1;
        //    //                // pass
        //    //            }
        //    //            else
        //    //            {
        //    //                Dispatcher.Invoke(() => {
        //    //                    generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, this.PaternAoiResult);
        //    //                    showPosNGtoUI(lstDataAoiResult);
        //    //                });
        //    //                return ret = 2;
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            ShowAlarm("Data Response Format Incorrect!!!", "Check Machine AOI", "3002");
        //    //            return ret = 0;
        //    //        }
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    if (!AOIOfflineData())
        //    //    {
        //    //        UpdateLogs("Data Offline PASS!");
        //    //        return ret = 1;
        //    //    }
        //    //    UpdateLogs("Data Offline NG!");
        //    //    return 2;
        //    //}  
        //    //return ret = 1;
        //}
        private MESCheck GetDataCheckMES(String barcode)
        {
            MESCheck mesdata = new MESCheck();
            mesdata.EquipmentId = SystemsManager.Instance.AppSettings.MESSetting.EquimentID;
            //var TOTAL = SystemsManager.Instance.currentModel.Pattern.xRow * SystemsManager.Instance.currentModel.Pattern.yColumn;

            //if(TOTAL==44)
            //{
            //    mesdata.Status = "E011";
            //}
            //else
            //{
            //    mesdata.Status = "E021";
            //}

            var UseE011 = SystemsManager.Instance.AppSettings.RunSetting.UseE011;
            if (UseE011)
            {
                mesdata.Status = "E011";
            }
            else
            {
                mesdata.Status = "E021";
            }

            mesdata.LotNo = this.lotInData.LotId;
            mesdata.PCB_Code = barcode;
            mesdata.CheckSum = this.lotInData.LotId;
            return mesdata;
        }
        private void LogDataMES(MESCheck mesData)
        {
            var logs = new MESResultDataLogService(this.lotInData.DeviceId, this.lotInData.LotId);
            MESResultData mesResult = new MESResultData();
            mesResult.LotId = this.lotInData.LotId;
            mesResult.QrCode = mesData.PCB_Code;
            mesResult.MESResult = mesData.MES_Result;
            logs.CreateData(mesResult);
        }
        private async Task MESCHECK(string barcode)
        {
            var dataSendMes = this.GetDataCheckMES(barcode);
            this.UpdateLogs($@"START CHECK MES READY...");
            var mesReady = await BLLManager.Instance.MES.SendReady(dataSendMes);
            if (!mesReady)
            {
                //await BLLManager.Instance.PLC.Device.WriteBit(this.plcNgMES, true, "M");
                this.UpdateLogs("MES CHECKING READY IS NG!!!");
                await BLLManager.Instance.PLC.Device.WriteBit(this.PC_AOI_ERROR_W, true, "M");
                this.ShowAlarm("MES CHECK READY NG", "Check Cable Ethernet and Check MES Accept!!!", "30001");
                this.UpdateLogs("PROCESS FINISHED...");
                return;
            }
            var mes = await BLLManager.Instance.MES.SendPCB(dataSendMes);
            mes.PCB_Code = barcode;
            if (mes == null)
            {
                this.UpdateLogs("MES CHECKING PCB TIME OUT OR DATA FORMAT INCORRECT!!!");
                await BLLManager.Instance.PLC.Device.WriteBit(this.PC_AOI_ERROR_W, true, "M");
                this.ShowAlarm($@"MES CHECING PCB TIME OUT OR DATA FORMAT INCORRECT!!!", "Check Cable Ethernet and Check MES Accept , check data MES response!!!", "30003");
                this.UpdateLogs("PROCESS FINISHED...");

                return;
            }
            this.LogDataMES(mes);
            if (!mes.MES_Result.Contains("OK"))
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.CounterLOT(0, 1);
                });
                // write bit error for PLC.
                UpdateLogs("PC Write Bits CHECK AOI ERROR!");
                await BLLManager.Instance.PLC.Device.WriteBit(this.PC_AOI_ERROR_W, true, "M");
                this.UpdateLogs("MES CHECKING IS NG!!!");
                this.ShowAlarm($@"MES CHECK PCB NG {Environment.NewLine}{barcode}{Environment.NewLine}{mes.MES_Result}", "Check Connection MES OR Check Data AOI!!!", "30002");
                this.UpdateLogs("PROCESS FINISHED...");
                return;
            }
            this.UpdateLogs("MES CHECKING OK");
            int countSetting = SystemsManager.Instance.currentModel.Pattern.yColumn * SystemsManager.Instance.currentModel.Pattern.xRow;
            if (mes.TotalCount != countSetting)
            {
                await BLLManager.Instance.PLC.Device.WriteBit(this.PC_AOI_ERROR_W, true, "M");
                this.UpdateLogs($@"Map Count MES [{mes.TotalCount}] Is Different Current Model Setting [{countSetting}]!!!");
                this.ShowAlarm($@"Map Count MES[{mes.TotalCount}] Is Different Current Model Setting[{countSetting}]!!!", "Check Data AOI OR Check Model Setting!!!", "30004");
                this.UpdateLogs("PROCESS FINISHED...");
                return;
            }
            this.Dispatcher.Invoke(() =>
            {
                this.CounterLOT(1, 0);
            });
            Dispatcher.Invoke(() => {
                generateCells(SystemsManager.Instance.currentModel.Pattern.xRow, SystemsManager.Instance.currentModel.Pattern.yColumn, SystemsManager.Instance.currentModel.Pattern.CurrentPatern, SystemsManager.Instance.currentModel.Pattern.Use2Matrix);
                this.showPosNGtoUI(mes.MapNG);
            });
            if (mes.MapNG.Count == 0 || mes.MapNG == null) // case PASS.
            {
                // write bit pass for PLC.
                this.UpdateLogs("AOI MAP NOT NG!");
                UpdateLogs("PC Write Bits CHECK OK!");
                await BLLManager.Instance.PLC.Device.WriteBit(this.PC_AOI_OK_PASS_W, true, "M");
                return;
            }
            else
            {
                UpdateLogs("PC Write Bits CHECK AOI NG!");
                await BLLManager.Instance.PLC.Device.WriteBit(this.PC_AOI_NG_W, true, "M");
                UpdateLogs("PC Send Bits AOI NG!");
                this.lstDataAoiResult = new List<int>(mes.MapNG);
                lstDataAoiResultNormal = new List<int>(mes.MapNG);
                lstDataAoiResultReverse = new List<int>(mes.MapNG);
            }
        }
        private async Task MESCHECKOFFLINE(string barcode)
        {
            if (!AOIOfflineData(barcode))
            {
                UpdateLogs("Data Offline PASS!");
                await BLLManager.Instance.PLC.Device.WriteBit(this.PC_AOI_OK_PASS_W, true, "M");
                return;
            }
            Dispatcher.Invoke(() => {
                this.showPosNGtoUI(this.lstDataAoiResult);
            });
            UpdateLogs("Data Offline NG!");
            lstDataAoiResultNormal = new List<int>(this.lstDataAoiResult);
            lstDataAoiResultReverse = new List<int>(this.lstDataAoiResult);
            await BLLManager.Instance.PLC.Device.WriteBit(this.PC_AOI_NG_W, true, "M");
        }
        private void CounterLOT(int okCount = 0, int ngCount = 0)
        {
            this.lblOkCount.Content = (this.lotInData.OKCount += okCount).ToString();
            this.lblNgCount.Content = (this.lotInData.NGCount += ngCount).ToString();
            this.lotInData.TotalCount = this.lotInData.OKCount + this.lotInData.NGCount;
            this.lblTotalCount.Content = this.lotInData.TotalCount;
            SystemsManager.Instance.SaveAppSettings();
        }
    }
}
