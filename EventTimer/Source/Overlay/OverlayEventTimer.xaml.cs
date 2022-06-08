using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace EventTimer
{
    /// <summary>
    /// Логика взаимодействия для EventOverlayTimer.xaml
    /// </summary>
    public partial class EventOverlayTimer : UserControl
    {
        private ModelEventTimer _timer;
        public EventOverlayTimer()
        {
            InitializeComponent();
        }

        public void Init(ModelEventTimer timer)
        {
            _timer = timer;
            _timer.Updated += TimerUpdated;
            _timer.Ended += TimerEnded;
            TB_TimerDesc.Text = _timer.Desc;
            TB_TimerDesc.Foreground = _timer.Color;

            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            if (_timer.Icon != null && _timer.Icon != "")
            {
                logo.UriSource = new Uri(_timer.Icon);
                logo.EndInit();
                IMG_Icon.Source = logo;
            }
            else
            {
                logo.UriSource = new Uri(Properties.Resources.XMissing);
                logo.EndInit();
                IMG_Icon.Source = logo;
            }
        }

        public void Start()
        {
            _timer.Start();
        }

        private void TimerEnded()
        {
            Dispatcher.Invoke(() =>
            {
                
                TB_Timer.Text = $"0:00";
                Console.WriteLine("Removed");
                (Parent as StackPanel).Children.Remove(this);
            });
        }

        private void TimerUpdated(TimeSpan remainTime)
        {
            Dispatcher.Invoke(() =>
            {
                double left = _timer.RemainTime / _timer.TotalTime;
                byte yellow = (byte)(left * 255);
                byte orange = (byte)(0xA5 + ((1 - left) * (0xFF - 0xA5)));

                Color color = Color.FromArgb(0xFF, 0xFF, orange, yellow);
                TB_Timer.Foreground = new SolidColorBrush(color);          
                TB_Timer.Text = $"{(int)remainTime.TotalSeconds}:{remainTime.Milliseconds/10:D2}";
            });
        }
    }
}
