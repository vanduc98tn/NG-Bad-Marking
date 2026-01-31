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
    /// Interaction logic for ButtonJigInformation.xaml
    /// </summary>
    public partial class ButtonMechanical : UserControl
    {
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background", typeof(Brush), typeof(ButtonMechanical), CreateDefaultBackgroundMetadata());
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
    "Content", typeof(object), typeof(ButtonMechanical),new PropertyMetadata("MECHANICAL"));
        public static readonly DependencyProperty RectangleFillProperty = DependencyProperty.Register(
    "RectangleFill", typeof(Brush), typeof(ButtonMechanical),new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty TagProperty = DependencyProperty.Register(
            "Tag", typeof(object), typeof(ButtonMechanical), new PropertyMetadata(null));

        public static new readonly DependencyProperty DeviceButtonProperty = DependencyProperty.Register(
            "DeviceButton", typeof(PAGE_ID), typeof(ButtonMechanical), new PropertyMetadata(PAGE_ID.PAGE_MENU));

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public Brush RectangleFill
        {
            get { return (Brush)GetValue(RectangleFillProperty); }
            set { SetValue(RectangleFillProperty, value); }
        }
        public object Tag
        {
            get { return GetValue(TagProperty); }
            set { SetValue(TagProperty, value); }
        }
        public PAGE_ID DeviceButton
        {
            get { return (PAGE_ID)GetValue(DeviceButtonProperty); }
            set { SetValue(DeviceButtonProperty, value); }
        }
        public static PropertyMetadata CreateDefaultBackgroundMetadata()
        {
            return new PropertyMetadata(
                new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                new GradientStop(Color.FromRgb(51, 153, 255), 0),
                new GradientStop(Color.FromRgb(0, 85, 170), 1)
                    }
                }
            );
        }

        public ButtonMechanical()
        {
            InitializeComponent();
            
        }

        private void btnJigInforMenu_Click(object sender, RoutedEventArgs e)
        {
            LogsManager.Instance.EventLogs.CreateEventLog(this.DeviceButton.ToString()+" Clicked", UserManagers.Instance.CurrentUser);
            UIManager.Instance.SwitchPage(DeviceButton);
        }

        private void btnJigInforMenu_TouchDown(object sender, TouchEventArgs e)
        {
            this.btnJigInforMenu_Click(this, null);
        }
    }
}
