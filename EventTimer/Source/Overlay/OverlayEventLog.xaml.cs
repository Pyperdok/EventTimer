using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace EventTimer
{
    /// <summary>
    /// Логика взаимодействия для EventOverlay.xaml
    /// </summary>
    ///   [DllImport("user32.dll")]
    
    public partial class OverlayEventLog : Window
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public bool GameMode { get; set; }

        public OverlayEventLog()
        {
            InitializeComponent();

            IntPtr handle = new WindowInteropHelper(this).EnsureHandle();
            SetWindowPos(handle, (IntPtr)(-1), 0, 0, 0, 0, 0x4000);
        }



        public void ActivateTimer(ModelEventTimer timer)
        {
            var overlayTimer = new EventOverlayTimer();
            ST_EventOverlay.Children.Add(overlayTimer);
            overlayTimer.Init(timer);
            overlayTimer.Start();
        }

        public void ResetTimer(ModelEventTimer timer)
        {
            timer.Reset();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Console.WriteLine("Deactivated");
        }

        private void Window_Initialized(object sender, EventArgs e)
        {         

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Overlay_EventLog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !GameMode)
            {
                DragMove();
            }
        }

        private void Overlay_EventLog_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MinHeight = ST_EventOverlay.ActualHeight;
            MaxHeight = ST_EventOverlay.ActualHeight;
        }
    }
}
