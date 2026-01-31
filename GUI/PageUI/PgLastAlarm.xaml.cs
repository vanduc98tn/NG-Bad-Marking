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
using BLL;
using DTO;

namespace GUI
{
    /// <summary>
    /// Interaction logic for PgLastAlarm.xaml
    /// </summary>
    public partial class PgLastAlarm : Page
    {
        private LoggerDebug logger = new LoggerDebug("PgLastAlarm");
        private const int ALARM_READ_LIMIT = 10;
        private List<AlarmLog> alarms;
        private static Brush BT_ACTIVE_BACKGROUND = Brushes.SkyBlue;
        private List<Rectangle> recCodeButton;

        private int selectedIndex = 0;

        public PgLastAlarm()
        {
            InitializeComponent();
            this.Loaded += PgLastAlarm_Loaded;
        }

        private void PgLastAlarm_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadAlarmListAsync();
        }
        private async void LoadAlarmListAsync()
        {
            try
            {
                this.ugridJamList.Children.Clear();
                this.alarms = new List<AlarmLog>();
                var x = await LogsManager.Instance.AlarmLogs.GetLatestAlarmLog(ALARM_READ_LIMIT);
                this.alarms = x.ToList();
                if (this.alarms == null) return;
                this.CreateButtonCode();
            }
            catch(Exception ex)
            {
                this.logger.Create("LoadAlarmList: " + ex.Message,LogLevel.Error);
            }
        }
        private void CreateButtonCode()
        {
            this.recCodeButton = new List<Rectangle>();
            for (int i = 0; i < this.alarms.Count; i++)
            {
                var alarm = this.alarms[i];
                Button myButton = new Button();

                // Tạo một Rectangle và thiết lập các thuộc tính
                Rectangle myRectangle = new Rectangle();
                myRectangle.Width = 15;
                myRectangle.Height = 48;
                myRectangle.Fill = new SolidColorBrush(Colors.LightGray);
                myRectangle.Margin = new Thickness(-100, 0, 0, 0);
                myRectangle.Tag = i;
                this.recCodeButton.Add(myRectangle);
                // Tạo một StackPanel và thiết lập Orientation là Horizontal
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;

                // Thêm Rectangle và TextBlock vào StackPanel
                stackPanel.Children.Add(myRectangle);
                stackPanel.Children.Add(CreateTextBlock(alarm.AlarmCode));

                // Đặt StackPanel làm nội dung cho Button
                myButton.Content = stackPanel;
                myButton.Margin = new Thickness(5);
                myButton.Tag = i;
                myButton.Click += Bt_Click;

                ugridJamList.Children.Add(myButton);
            }
            selectedIndex = 0;
            if (this.alarms.Count > 0)
            {
                updateAlarm(this.alarms[0]);
            }
        }
        private void Bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var bt = (Button)sender;
                selectedIndex = (int)bt.Tag;
                var alarm = this.alarms[selectedIndex];

                this.updateAlarm(alarm);
            }
            catch (Exception ex)
            {
                logger.Create("Bt_Click:" + ex.Message,LogLevel.Error);
            }
        }
        private void updateAlarm(AlarmLog alarm)
        {
            lblTime.Content = alarm.CreatedTime.ToString();
            lblCode.Content = alarm.AlarmCode.ToString();
            lblSeqId.Content = alarm.Id.ToString();
            lblMode.Content = alarm.Mode.ToString();


            lblMessage.Text = alarm.Message.ToString();
            lblSolution.Text = alarm.Solution.ToString();

            foreach (var obj in ugridJamList.Children)
            {
                var bt = obj as Button;
                if (bt != null)
                {
                    if ((int)bt.Tag == selectedIndex)
                    {
                        bt.Background = BT_ACTIVE_BACKGROUND;
                        var rec = this.recCodeButton.FirstOrDefault(x => (int)x.Tag == selectedIndex);
                        rec.Fill = new SolidColorBrush(Colors.DarkRed);
                    }
                    else
                    {
                        var rec = this.recCodeButton[(int)bt.Tag];
                        rec.Fill = new SolidColorBrush(Colors.LightGray);
                        bt.ClearValue(Button.BackgroundProperty);
                    }
                }
            }
        }
        private TextBlock CreateTextBlock(string text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Padding = new Thickness(10, 0, 0, 0);
            textBlock.Text = text;
            return textBlock;
        }

        private UIElement CreateButtonContent(string buttonText)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            Rectangle rectangle = new Rectangle();
            rectangle.Width = 15;
            rectangle.Height = 48;
            rectangle.Fill = new SolidColorBrush(Colors.DarkRed);

            stackPanel.Children.Add(rectangle);
            stackPanel.Children.Add(CreateTextBlock(buttonText));

            return stackPanel;
        }
    }
}
