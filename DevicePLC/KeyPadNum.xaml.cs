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

namespace DevicePLC
{
    /// <summary>
    /// Interaction logic for WndKeyboard.xaml
    /// </summary>
    public partial class KeyPadNum : Window
    {
        private string data;
        private string dataSource;
        private bool isFirtEnter;
        private int NoOfDecimalDigits;
        private int NoOfDisplay;
        private bool isEnt;
        public KeyPadNum()
        {
            InitializeComponent();
            this.data = "";
            this.btn0.Click += BtnNumber_Click;
            //this.btn0.TouchDown+= BtnNumber_Click;
            this.btn1.Click += BtnNumber_Click;
            //this.btn1.TouchDown+= BtnNumber_Click;
            this.btn2.Click += BtnNumber_Click;
            //this.btn2.TouchDown += BtnNumber_Click;
            this.btn3.Click += BtnNumber_Click;
            //this.btn3.TouchDown += BtnNumber_Click;
            this.btn4.Click += BtnNumber_Click;
            //this.btn4.TouchDown += BtnNumber_Click;
            this.btn5.Click += BtnNumber_Click;
            //this.btn5.TouchDown += BtnNumber_Click;
            this.btn6.Click += BtnNumber_Click;
            //this.btn6.TouchDown += BtnNumber_Click;
            this.btn7.Click += BtnNumber_Click;
            //this.btn7.TouchDown += BtnNumber_Click;
            this.btn8.Click += BtnNumber_Click;
            //this.btn8.TouchDown += BtnNumber_Click;
            this.btn9.Click += BtnNumber_Click;
            //this.btn9.TouchDown += BtnNumber_Click;
            this.btnDot.Click += BtnNumber_Click;
            //this.btnDot.TouchDown += BtnNumber_Click;
            this.btnNegative.Click += BtnNumber_Click;
            //this.btnNegative.TouchDown += BtnNumber_Click;

            this.btnESC.Click += BtnESC_Click;
            //this.btnESC.TouchDown+= BtnESC_Click;
            this.btnENT.Click += BtnENT_Click;
            //this.btnENT.TouchDown+= BtnENT_Click;
            this.btnCLR.Click += BtnCLR_Click;
            //this.btnCLR.Click+= BtnCLR_Click;

            this.Closing += WndKeyboard_Closed;
        }

        private void WndKeyboard_Closed(object sender, EventArgs e)
        {
            if (!isEnt)
            {
                this.data = this.dataSource;
            }
        }

        private void BtnCLR_Click(object sender, RoutedEventArgs e)
        {
            this.data = string.Empty;
            this.lblNumber.Content = this.data;
        }

        private void BtnENT_Click(object sender, RoutedEventArgs e)
        {
            this.isEnt = true;
            this.Close();
        }

        private void BtnESC_Click(object sender, RoutedEventArgs e)
        {
            this.isEnt = false;
            this.data = this.dataSource;
            this.Close();
        }

        private void BtnNumber_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (!this.isFirtEnter)
            {
                this.BtnCLR_Click(this, null);
                this.isFirtEnter = true;
            }
            if (btn.Content.ToString() == "-")
            {
                if (!string.IsNullOrEmpty(this.data)) return;
            }
            if (btn.Content.ToString() == ".")
            {
                if (string.IsNullOrEmpty(this.data)) return;
            }
            this.data += btn.Content.ToString();
            int dotCount = this.data.Count(c => c == '.');
            if (dotCount > 1)
            {
                var x = this.data.Remove(this.data.Length - 1);
                this.data = x;
            }
            if (!this.data.Contains('.'))
            {
                int noOf = this.NoOfDisplay;
                if (this.data.Contains('-'))
                {
                    noOf += 1;
                }
                if (this.data.Length > noOf)
                {
                    var x = this.data.Remove(this.data.Length - 1);
                    this.data = x;
                }
                noOf = this.NoOfDisplay;
            }

            lblNumber.Content = this.data;
        }
        public string ShowKeypad(string data, int NoOfDecimalDigits, int NoOfDisplay, Window ownen = null)
        {
            this.data = data;
            this.dataSource = data;
            this.NoOfDecimalDigits = NoOfDecimalDigits;
            this.NoOfDisplay = NoOfDisplay;
            this.isFirtEnter = false;
            lblNumber.Content = this.data;
            this.ShowInPositionMouse();
            this.ShowDialog();

            if (this.data.Length > 0 && this.data.Length - 1 == '.')
            {
                this.data.Remove(this.data.Length - 1);
            }
            if (string.IsNullOrEmpty(this.data))
            {
                this.data = data;
                return this.data;
            }
            if (!this.data.Contains('.') && NoOfDecimalDigits>0)
            {
                var a = "0";
                a = a.PadRight(NoOfDecimalDigits, '0');
                this.data = this.data + "." + a;
                string befor = "";
                var x = this.data.Split('.');
                if (x[0].Length < NoOfDisplay)
                {
                    befor = x[0].PadLeft(NoOfDisplay, '0');
                }
                else
                {
                    befor = x[0];
                }
                this.data = befor + "." + x[1];
            }
            else if(this.data.Contains('.'))
            {
                var x = this.data.Split('.');
                string after = "";
                string befor = "";
                var x1 = x[0];
                var x2 = x[1];
                if (x2.Length > NoOfDecimalDigits)
                {
                    after = x2.Substring(0, NoOfDecimalDigits);
                }
                else
                {
                    after = x2.PadRight(NoOfDecimalDigits, '0');
                }
                if (x1.Length < NoOfDisplay)
                {
                    befor = x1.PadLeft(NoOfDisplay, '0');
                }
                else
                {
                    befor = x1;
                }
                this.data = befor + "." + after;
            }
            else if(NoOfDisplay>0)
            {
                string befor = this.data.PadLeft(NoOfDisplay,'0');
                this.data = befor;
            }
            return this.data;
        }
        private void ShowInPositionMouse()
        {
            //Point mousePosition = Mouse.GetPosition(null);
            //Left = mousePosition.X;
            //Top = mousePosition.Y;
            Point mousePosition = Mouse.GetPosition(null);

            // Kích thước cửa sổ
            double windowWidth = 350;
            double windowHeight = 450;

            // Kích thước màn hình
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            // Khoảng cách từ chuột đến biên của màn hình
            double distanceFromEdge = 20; // Điều chỉnh khoảng cách

            double left = mousePosition.X;
            double top = mousePosition.Y;

            // Kiểm tra xem cửa sổ có vượt ra ngoài biên màn hình không
            if (left + windowWidth + distanceFromEdge > screenWidth)
            {
                left = screenWidth - windowWidth - distanceFromEdge;
            }
            else if (left < distanceFromEdge)
            {
                left = distanceFromEdge;
            }

            if (top + windowHeight + distanceFromEdge > screenHeight)
            {
                top = screenHeight - windowHeight - distanceFromEdge;
            }
            else if (top < distanceFromEdge)
            {
                top = distanceFromEdge;
            }

            Left = left;
            Top = top;
        }
    }
}
