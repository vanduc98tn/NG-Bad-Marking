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
using System.Windows.Media.Effects;
using System.Threading;

namespace GUI
{
    /// <summary>
    /// Interaction logic for PgIO.xaml
    /// </summary>
    public partial class PgIO : Page, IObserverChangeBits
    {
        private const string colorStatusNormal = "#FFFFF6F6";
        private const string colorStatusNotUse = "#FFB9B5B5";
        private const string colorStatusAction = "#FF64FF29";
        BrushConverter convertBrush = new BrushConverter();
        private CancellationTokenSource receiveCancellation;
        private LoggerDebug logger = new LoggerDebug("PgIO");

        private List<Button> btnIOButtons = new List<Button>();
        private List<Ellipse> elIOMaps = new List<Ellipse>();

        private NotifyPLCBits notifyPLCBits;
        public PgIO()
        {
            InitializeComponent();
            this.Loaded += PgIO_Loaded;
            this.Unloaded += PgIO_Unloaded;
        }

        private void PgIO_Unloaded(object sender, RoutedEventArgs e)
        {
            this.UnregisterNotifyBits();
            this.UnregisterPLCMap();
            this.receiveCancellation?.Cancel();
        }

        private async void PgIO_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.RegisterNotifyBits();
                receiveCancellation = new CancellationTokenSource();
                this.RegisterPLCMap();
                await this.UpdateIOMAPToUI(0);
                await UpdatePLCTOUI();
            }
            catch(Exception ex)
            {
                logger.Create("PgIO_Loaded: " + ex.Message, LogLevel.Error);
            }
        }
        private void RegisterNotifyBits()
        {
            this.notifyPLCBits = SystemsManager.Instance.NotifyPLCBits;
            this.notifyPLCBits.Attach(this);
        }
        private void UnregisterNotifyBits()
        {
            this.notifyPLCBits.Detach(this);
        }
        private async Task UpdateIOMAPToUI(int startIdex)
        {
            try
            {
                var tsk = Task.Run(() => {
                    this.Dispatcher.Invoke(() => { 
                        this.UpdateUIStatus(startIdex); 
                        this.AddButtonIOMap();
                        this.ClearBrushButton("0");
                    });
                });
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                logger.Create("UpdateIOMAPToUI: " + ex.Message, LogLevel.Error);
            }
        }
        private void AddButtonIOMap()
        {
            var IOMAPs = BLLManager.Instance.IoMonitorService.GetAll();
            List<FileIOMonitor> valuesList = IOMAPs.Values.ToList();
            int Length = IOMAPs.Count;
            gridButtonIO.Children.Clear();
            gridButtonIO.ColumnDefinitions.Clear();

            var countButton = Length / 32;
            if(Length%32!=0)
            {
                countButton++;
            }    
            int idex = 0;
            for (int i =0;i<10;i++)
            {
                var columnDef = new ColumnDefinition();
                columnDef.Width = new GridLength(1, GridUnitType.Star);
                this.gridButtonIO.ColumnDefinitions.Add(columnDef);
            }
            for(int i = 0; i<countButton; i++)
            {
                string nameDevice = "";
                if((Length-idex)>32)
                {
                    nameDevice = valuesList[idex].Device + "~" + valuesList[idex + 31].Device;
                }    
                else
                {
                    nameDevice = valuesList[idex].Device + "~" + valuesList[Length-1].Device;
                }
                var button = createButtonMAP(nameDevice, idex);
                this.gridButtonIO.Children.Add(button);
                Grid.SetColumn(button, i);
                idex += 32;

            }
        }
        private void UpdateUIStatus(int startIdex)
        {
            
            this.ClearUI();
            this.elIOMaps = new List<Ellipse>();
            for (int i = 0;i<16;i++)
            {
                this.AddRow();  
            }
            for (int i = 0; i < 16; i++)
            {
                this.AddUIIOMAP1(i+startIdex, startIdex);
            }
            for (int i = 0; i < 16; i++)
            {
                this.AddUIIOMAP2(i + startIdex, startIdex);
            }
            
        }
        private void AddUIIOMAP1(int idex,int length)
        {
            Label cellStatus;
            Label cellDevice;
            Label cellDescription;

            var IOMAPs = BLLManager.Instance.IoMonitorService.GetAll();
            List<FileIOMonitor> valuesList = IOMAPs.Values.ToList();
            int Length = IOMAPs.Count;
            if (idex < Length)
            {
                cellStatus = createCellStatus(idex, colorStatusNormal, valuesList[idex].DeviceCode.ToString());
                cellDevice = createCellDevice(idex, colorStatusNormal, valuesList[idex].Device);
                cellDescription = createCellDescription(idex, colorStatusNormal, valuesList[idex].Description);
            }
            else
            {
                cellStatus = createCellStatus(idex, colorStatusNotUse,"NO");
                cellDevice = createCellDevice(idex, colorStatusNotUse, "Spare");
                cellDescription = createCellDescription(idex, colorStatusNotUse, "Spare");
            }
            this.gridStatus1.Children.Add(cellStatus);
            Grid.SetRow(cellStatus, idex- length);
            this.gridDevice1.Children.Add(cellDevice);
            Grid.SetRow(cellDevice, idex- length);
            this.gridDescription1.Children.Add(cellDescription);
            Grid.SetRow(cellDescription, idex- length);
        }
        private void AddUIIOMAP2(int idex, int length)
        {
            Label cellStatus2;
            Label cellDevice2;
            Label cellDescription2;
            var IOMAPs = BLLManager.Instance.IoMonitorService.GetAll();
            List<FileIOMonitor> valuesList = IOMAPs.Values.ToList();
            int Length = IOMAPs.Count;
            if ((idex + 16) < Length)
            {
                cellStatus2 = createCellStatus2(idex, colorStatusNormal, valuesList[idex + 16].DeviceCode.ToString());
                cellDevice2 = createCellDevice2(idex, colorStatusNormal, valuesList[idex+16].Device);
                cellDescription2 = createCellDescription2(idex, colorStatusNormal, valuesList[idex + 16].Description);
            }
            else
            {
                cellStatus2 = createCellStatus2(idex, colorStatusNotUse,"NO");
                cellDevice2 = createCellDevice2(idex, colorStatusNotUse, "Spare");
                cellDescription2 = createCellDescription2(idex, colorStatusNotUse, "Spare");
            }
            this.gridStatus2.Children.Add(cellStatus2);
            Grid.SetRow(cellStatus2, idex- length);
            this.gridDevice2.Children.Add(cellDevice2);
            Grid.SetRow(cellDevice2, idex- length);
            this.gridDescription2.Children.Add(cellDescription2);
            Grid.SetRow(cellDescription2, idex- length);
        }
        private void ClearUI()
        {
            this.gridStatus1.Children.Clear();
            this.gridStatus2.Children.Clear();
            this.gridDevice1.Children.Clear();
            this.gridDevice2.Children.Clear();
            this.gridDescription1.Children.Clear();
            this.gridDescription2.Children.Clear();

            this.gridStatus1.RowDefinitions.Clear();
            this.gridStatus2.RowDefinitions.Clear();
            this.gridDevice1.RowDefinitions.Clear();
            this.gridDevice2.RowDefinitions.Clear();
            this.gridDescription1.RowDefinitions.Clear();
            this.gridDescription2.RowDefinitions.Clear();
        }
        private void AddRow()
        {
            var rowDef = new RowDefinition();
            rowDef.Height = new GridLength(1, GridUnitType.Star);
            this.gridStatus1.RowDefinitions.Add(rowDef);

            var rowDef1 = new RowDefinition();
            rowDef1.Height = new GridLength(1, GridUnitType.Star);
            this.gridStatus2.RowDefinitions.Add(rowDef1);

            var rowDef2 = new RowDefinition();
            rowDef2.Height = new GridLength(1, GridUnitType.Star);
            this.gridDevice1.RowDefinitions.Add(rowDef2);

            var rowDef3 = new RowDefinition();
            rowDef3.Height = new GridLength(1, GridUnitType.Star);
            this.gridDevice2.RowDefinitions.Add(rowDef3);

            var rowDef4 = new RowDefinition();
            rowDef4.Height = new GridLength(1, GridUnitType.Star);
            this.gridDescription1.RowDefinitions.Add(rowDef4);

            var rowDef5 = new RowDefinition();
            rowDef5.Height = new GridLength(1, GridUnitType.Star);
            this.gridDescription2.RowDefinitions.Add(rowDef5);
        }
        private Label createCellStatus(int number,string brush,string device)
        {
            var cell = new Label();
            // Tạo một đối tượng Ellipse để đại diện cho hình tròn
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 31; // Điều chỉnh kích thước hình tròn tại đây
            ellipse.Height = 31;
            ellipse.Fill = (Brush)convertBrush.ConvertFrom(brush); // Đặt màu nền cho hình tròn
            ellipse.Stroke = (Brush)convertBrush.ConvertFrom("#FF57B5D1");
            ellipse.Tag = device;
            // Đặt độ dày của viền
            ellipse.StrokeThickness = 2; // Điều chỉnh độ dày tại đây
            elIOMaps.Add(ellipse);
            // Gán hình tròn vào Label
            cell.Content = ellipse;

            cell.Name = String.Format("lblCellStatus{0:00}", number);
            cell.HorizontalContentAlignment = HorizontalAlignment.Center;
            cell.VerticalContentAlignment = VerticalAlignment.Center;
            cell.Margin = new Thickness(1, 1, 1, 1);
            cell.BorderBrush = Brushes.White;
            cell.BorderThickness = new Thickness(1, 1, 1, 1);
            cell.Background = (Brush)convertBrush.ConvertFrom("#FF8B8B75");
            cell.Tag = device;
            var dropShadow = new DropShadowEffect
            {
                Color = Colors.Gray,
                Direction = 320,
                ShadowDepth = 2,
                Opacity = 0.5,
            };
            cell.Effect = dropShadow;
            
            return cell;
        }
        private Label createCellDevice(int number, string brush,string content)
        {
            var cell = new Label();
            cell.Name = String.Format("lblCellDevice{0:00}", number);
            cell.HorizontalContentAlignment = HorizontalAlignment.Center;
            cell.Content = this.createCellContent(content, "Showcard Gothic");
            cell.VerticalContentAlignment = VerticalAlignment.Center;
            cell.Margin = new Thickness(1, 1, 1, 1);
            cell.BorderBrush = Brushes.White;
            cell.BorderThickness = new Thickness(1, 1, 1, 1);
            cell.Background = (Brush)convertBrush.ConvertFrom(brush);
            var dropShadow = new DropShadowEffect
            {
                Color = Colors.Gray,
                Direction = 320,
                ShadowDepth = 2,
                Opacity = 0.5,
            };
            cell.Effect = dropShadow;

            return cell;
        }
        private Label createCellDescription(int number, string brush, string content)
        {
            var cell = new Label();
            cell.Name = String.Format("lblCellDescription{0:00}", number);
            cell.HorizontalContentAlignment = HorizontalAlignment.Center;
            cell.Content = this.createCellContent(content, "Segoe UI Black");
            cell.VerticalContentAlignment = VerticalAlignment.Center;
            cell.Margin = new Thickness(1, 1, 1, 1);
            cell.BorderBrush = Brushes.White;
            cell.BorderThickness = new Thickness(1, 1, 1, 1);
            cell.Background = (Brush)convertBrush.ConvertFrom(brush);
            var dropShadow = new DropShadowEffect
            {
                Color = Colors.Gray,
                Direction = 320,
                ShadowDepth = 2,
                Opacity = 0.5,
            };
            cell.Effect = dropShadow;

            return cell;
        }
        private Label createCellStatus2(int number, string brush,string device)
        {
            var cell = new Label();

            // Tạo một đối tượng Ellipse để đại diện cho hình tròn
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 31; // Điều chỉnh kích thước hình tròn tại đây
            ellipse.Height = 31;
            ellipse.Fill = (Brush)convertBrush.ConvertFrom(brush); // Đặt màu nền cho hình tròn
            ellipse.Stroke = (Brush)convertBrush.ConvertFrom("#FF57B5D1");
            ellipse.Tag = device;
            // Đặt độ dày của viền
            ellipse.StrokeThickness = 2; // Điều chỉnh độ dày tại đây
            // Gán hình tròn vào Label
            cell.Content = ellipse;
            elIOMaps.Add(ellipse);
            cell.Name = String.Format("lblCellStatus2{0:00}", number);
            cell.HorizontalContentAlignment = HorizontalAlignment.Center;
            cell.VerticalContentAlignment = VerticalAlignment.Center;
            cell.Margin = new Thickness(1, 1, 1, 1);
            cell.BorderBrush = Brushes.White;
            cell.BorderThickness = new Thickness(1, 1, 1, 1);
            cell.Background = (Brush)convertBrush.ConvertFrom("#FF8B8B75");
            cell.Tag = device;
            var dropShadow = new DropShadowEffect
            {
                Color = Colors.Gray,
                Direction = 320,
                ShadowDepth = 2,
                Opacity = 0.5,
            };
            cell.Effect = dropShadow;

            return cell;
        }
        private Label createCellDevice2(int number, string brush, string content)
        {
            var cell = new Label();
            cell.Name = String.Format("lblCellDevice2{0:00}", number);
            cell.HorizontalContentAlignment = HorizontalAlignment.Center;
            cell.Content = this.createCellContent(content, "Showcard Gothic");
            cell.VerticalContentAlignment = VerticalAlignment.Center;
            cell.Margin = new Thickness(1, 1, 1, 1);
            cell.BorderBrush = Brushes.White;
            cell.BorderThickness = new Thickness(1, 1, 1, 1);
            cell.Background = (Brush)convertBrush.ConvertFrom(brush);
            var dropShadow = new DropShadowEffect
            {
                Color = Colors.Gray,
                Direction = 320,
                ShadowDepth = 2,
                Opacity = 0.5,
            };
            cell.Effect = dropShadow;

            return cell;
        }
        private Label createCellDescription2(int number, string brush, string content)
        {
            var cell = new Label();
            cell.Name = String.Format("lblCellDescription2{0:00}", number);
            cell.HorizontalContentAlignment = HorizontalAlignment.Center;
            cell.Content = this.createCellContent(content, "Segoe UI Black");
            cell.VerticalContentAlignment = VerticalAlignment.Center;
            cell.Margin = new Thickness(1, 1, 1, 1);
            cell.BorderBrush = Brushes.White;
            cell.BorderThickness = new Thickness(1, 1, 1, 1);
            cell.Background = (Brush)convertBrush.ConvertFrom(brush);
            var dropShadow = new DropShadowEffect
            {
                Color = Colors.Gray,
                Direction = 320,
                ShadowDepth = 2,
                Opacity = 0.5,
            };
            cell.Effect = dropShadow;

            return cell;
        }
        private object createCellContent(String content,string fontFamily)
        {
            var cellText = new TextBlock();
            cellText.TextWrapping = TextWrapping.Wrap;
            cellText.Text = String.Format("{0}", content);
            cellText.FontSize = 20;
            cellText.FontWeight = FontWeights.Bold;
            cellText.FontFamily = new FontFamily(fontFamily);
            return cellText;
        }
        private Button createButtonMAP(string content,int id)
        {
            Button button = new Button();
            button.Content = content; // Nội dung của nút
            button.Name = "btnIO"+id;
            button.Tag = id;
            // Thiết lập màu nền và viền
            button.Margin = new Thickness(2, 2, 2, 2);
            button.Background = this.CreateGradient();
            button.BorderBrush = new SolidColorBrush(Colors.White);
            button.BorderThickness = new Thickness(2);
            button.Foreground = Brushes.White;
            // Thiết lập font chữ và kích thước
            button.FontFamily = new FontFamily("Rockwell Extra Bold");
            button.FontSize = 23;
            button.Click += Button_Click;
            this.btnIOButtons.Add(button);

            return button;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                var tsk = Task.Run(() => {

                    this.Dispatcher.Invoke(() => {
                        this.UpdateUIStatus(Convert.ToInt32(btn.Tag));
                        this.ClearBrushButton(btn.Tag.ToString());
                    });
                });
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                logger.Create("Button_Click: " + ex.Message, LogLevel.Error);
            }
        }
        private LinearGradientBrush CreateGradient()
        {
            LinearGradientBrush gradientBrush = new LinearGradientBrush();

            // Thiết lập StartPoint và EndPoint
            gradientBrush.StartPoint = new Point(0.5, 0);
            gradientBrush.EndPoint = new Point(0.5, 1);

            // Thêm các GradientStop để định nghĩa gradient
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Black, 0));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x4F, 0xC9, 0xF7), 1));
            return gradientBrush;
        }
        private void ClearBrushButton(string tag)
        {
            for(int i = 0; i<this.btnIOButtons.Count;i++)
            {
                if (btnIOButtons[i].Tag.ToString() == tag.ToString())
                {
                    btnIOButtons[i].Background = (Brush)convertBrush.ConvertFrom("#FFF1EB74");
                    continue;
                }
                btnIOButtons[i].Background = this.CreateGradient();
            }
        }
        private async Task UpdatePLCTOUI()
        {
            while (!this.receiveCancellation.IsCancellationRequested)
            {
                for (int i = 0; i < elIOMaps.Count; i++)
                {
                    //bool bit = await UIManager.Instance.PLC.ReadBitsData(elIOMaps[i].Tag.ToString());
                    bool bit;
                    BLLManager.Instance.PLC.monitorDeviceBits_M.TryGetValue("M"+elIOMaps[i].Tag.ToString(), out bit);
                    elIOMaps[i].Fill = bit ? Brushes.Red : Brushes.White;
                }
                await Task.Delay(1);
            }
        }
        private void RegisterPLCMap()
        {
            var IOMAPs = BLLManager.Instance.IoMonitorService.GetAll().ToList();
            for(int i = 0; i< IOMAPs.Count;i++)
            {
                BLLManager.Instance.PLC.AddBitAddress("M",(ushort)IOMAPs[i].Value.DeviceCode);
            }
        }
        private void UnregisterPLCMap()
        {
            var IOMAPs = BLLManager.Instance.IoMonitorService.GetAll().ToList();
            for (int i = 0; i < IOMAPs.Count; i++)
            {
                BLLManager.Instance.PLC.RemoveBitAddress("M",(ushort)IOMAPs[i].Value.DeviceCode);
            }
        }

        public void NotifyChangeBits(string key, bool status)
        {
            //for (int i = 0; i < elIOMaps.Count; i++)
            //{
            //    if (elIOMaps[i].Tag.ToString() != key) continue;
            //    elIOMaps[i].Fill = status ? Brushes.Red : Brushes.White;
            //}
        }
    }
}
