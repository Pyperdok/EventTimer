using EventTimer.Source.Model;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EventTimer.Source.View
{
    using Keys = System.Windows.Forms.Keys;
    public partial class ViewEventTimer : UserControl
    {
        private ModelEventTimer _timer = new ModelEventTimer();
        private OpenFileDialog _iconDialog;

        public ViewEventTimer()
        {
            InitializeComponent();
            _timer.Key = Keys.None;
            _iconDialog = new OpenFileDialog()
            {
                InitialDirectory = Environment.CurrentDirectory,
                DefaultExt = ".png",
                Filter = "Timer configs (.png)|*.png"
            };

            App.AddBind(_timer);
        }

        public ModelEventTimer GetTimer() => _timer;

        private void BT_Click_SetIcon(object sender, RoutedEventArgs e)
        {
            bool? result = _iconDialog.ShowDialog();

            if (result == true)
            {
                string filename = _iconDialog.FileName;
                Image image = new Image();
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(filename);
                logo.EndInit();
                image.Source = logo;
                BT_Icon.Content = image;
                _timer.Icon = filename;
            }
        }

        private void BT_RemoveTimer_Click(object sender, RoutedEventArgs e)
        {
            StackPanel panel = Parent as StackPanel;
            panel.Children.Remove(this);
            App.RemoveBind(_timer);
        }

        private void BindHotKey(object sender, InputEventArgs e)
        {

            ViewBindKey viewBindKey = sender as ViewBindKey;
            Keys bind = Keys.None;
            if (e is KeyEventArgs keyboard)
            {
                bind = (Keys)KeyInterop.VirtualKeyFromKey(keyboard.Key);
            }

            if (e is MouseButtonEventArgs mouse)
            {
                bind = MouseKeys.GetKey(mouse.ChangedButton);
            }


            viewBindKey.Close();
            if (bind == Keys.Delete)
            {
                _timer.Key = Keys.None;
                TB_Key.Text = Keys.None.ToString();
            }
            else
            {
                _timer.Key = bind;
                TB_Key.Text = bind.ToString();
            }
        }

        private void TB_Key_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var viewBindKey = new ViewBindKey();

            viewBindKey.PreviewKeyDown += BindHotKey;
            viewBindKey.PreviewMouseDown += BindHotKey;
            viewBindKey.Owner = Application.Current.MainWindow;
            viewBindKey.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            viewBindKey.ShowDialog();
        }

        private void TB_SecondsChanged(object sender, TextChangedEventArgs e)
        {
            if (TB_Timer.Text == "")
            {
                _timer.TotalTime = 0;
                return;
            }

            int seconds = int.Parse(TB_Timer.Text);
            Console.WriteLine(seconds);
            _timer.TotalTime = seconds;
        }

        private void TB_Timer_KeyDown(object sender, KeyEventArgs e)
        {
            var isNumber = e.Key >= Key.D0 && e.Key <= Key.D9;
            if (!isNumber)
            {
                e.Handled = true;
            }
        }

        private void TB_Desc_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(TB_Desc.Text);
            _timer.Desc = TB_Desc.Text;
        }

        private void TB_DescColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var solid = (SolidColorBrush)new BrushConverter().ConvertFrom("#" + TB_DescColor.Text);
                TB_Desc.Foreground = solid;
                _timer.Color = solid;
            }
            catch
            {

            }
        }
    }
}
