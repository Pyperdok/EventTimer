using EventTimer.Source.View;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EventTimer
{
    public partial class MainWindow : Window
    {
        private SaveFileDialog _saveFileDialog; //ХУЙНЯ
        private OpenFileDialog _loadFileDialog; //ХУЙНЯ
        public MainWindow()
        {
            InitializeComponent();
            _saveFileDialog = new SaveFileDialog() //ХУЙНЯ
            {
                InitialDirectory = Environment.CurrentDirectory,
                DefaultExt = ".cfg",
                Filter = "Timer configs (.cfg)|*.cfg"
            };

            _loadFileDialog = new OpenFileDialog() //ХУЙНЯ
            {
                InitialDirectory = Environment.CurrentDirectory,
                DefaultExt = ".cfg",
                Filter = "Timer configs (.cfg)|*.cfg"
            };
        }

        private void BT_AddTimer_Click(object sender, RoutedEventArgs e)
        {
            var eventTimer = new ViewEventTimer();
            ST_Timers.Children.Add(eventTimer);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double height = SV_Panel.ActualHeight + RD_Header.ActualHeight + RD_Footer.MinHeight + BT_AddTimer.ActualHeight;
            (MinHeight, MaxHeight) = (height, height);
        }

        private void MI_Save_Click(object sender, RoutedEventArgs e)
        {
            var x = App._overlayEventLog.Left;
            var y = App._overlayEventLog.Top;
            var w = App._overlayEventLog.Width;
            Console.WriteLine($"X:{x} Y:{y} W: {w}");

            string json = $"{{\"x\": {x}, \"y\": {y}, \"w\": {w}, \"binds\": ["; //ХУЙНЯ
            foreach (var bind in App._binds) {
                json += bind + ",\n";
            }
            json = json.TrimEnd(',', '\n');
            json += "]}"; //ХУЙНЯ

            if (_saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(_saveFileDialog.FileName, json);
                Console.WriteLine(json);
                System.Windows.MessageBox.Show("Saved", "Event Timer", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Saving config has been interrupted", "Event Timer", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        
        private void MI_About_Click(object sender, RoutedEventArgs e)
        {
            string message = "" + //ХУЙНЯ
                "Created by Pyperdok\n" + 
                "Discord: Pyperdok#7625\n" +
                "Steam: https://steamcommunity.com/profiles/76561198426585696";
            System.Windows.MessageBox.Show(message, "Event Timer", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        private void MI_Load_Click(object sender, RoutedEventArgs e)
        {
            var result = _loadFileDialog.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                string raw = File.ReadAllText(_loadFileDialog.FileName);
                raw = raw.Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("\\", "\\\\");
                Console.WriteLine(raw);
                JsonDocument json = JsonDocument.Parse(raw);
                App._binds.Clear();
                ST_Timers.Children.Clear();
                var x = json.RootElement.GetProperty("x").GetDouble();
                var y = json.RootElement.GetProperty("y").GetDouble();
                var w = json.RootElement.GetProperty("w").GetDouble();
                App._overlayEventLog.Left = x;
                App._overlayEventLog.Top = y;
                App._overlayEventLog.Width = w;

                foreach (var bind in json.RootElement.GetProperty("binds").EnumerateArray()) //ХУЙНЯ
                {
                    var viewBind = new ViewEventTimer();
                    viewBind.TB_Timer.Text = bind.GetProperty("seconds").GetInt32().ToString(); //ХУЙНЯ
                    viewBind.TB_Key.Text = bind.GetProperty("key").GetString(); //ХУЙНЯ
                    viewBind.TB_Desc.Text = bind.GetProperty("description").GetString(); //ХУЙНЯ
                    viewBind.TB_DescColor.Text = bind.GetProperty("color").GetString().Replace("#", ""); //ХУЙНЯ

                    string icon = bind.GetProperty("icon").GetString(); //ХУЙНЯ
                    if (icon != "") //ХУЙНЯ
                    {
                        BitmapImage logo = new BitmapImage();
                        logo.BeginInit();
                        logo.UriSource = new Uri(icon);
                        logo.EndInit();
                        viewBind.BT_Icon.Content = new Image() { Source = logo };
                    }

                    var timer = viewBind.GetTimer(); //ХУЙНЯ
                    timer.Icon = icon; //ХУЙНЯ
                    timer.TotalTime = int.Parse(viewBind.TB_Timer.Text);
                    timer.Key = (Keys)Enum.Parse(typeof(Keys), viewBind.TB_Key.Text);
                    timer.Desc = viewBind.TB_Desc.Text;
                    timer.Color = (SolidColorBrush)new BrushConverter().ConvertFrom("#"+viewBind.TB_DescColor.Text);

                    ST_Timers.Children.Add(viewBind);
                }
            }
        }
    }
}
