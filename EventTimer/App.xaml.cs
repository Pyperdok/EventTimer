using EventTimer.Source.Model;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace EventTimer
{
    enum HookType
    {
        Keyboard = 13,
        Mouse = 14
    }
    public partial class App : System.Windows.Application
    {
        delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        private static HookProc hook = (code, wParam, lParam) =>
        {
            if (code < 0)
            {
                //you need to call CallNextHookEx without further processing
                //and return the value returned by CallNextHookEx
                return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
            }

            int msgType = wParam.ToInt32();
            Keys key = Keys.None;
            if (MouseKeys.IsMouseKey(msgType))
            {
                int xButtonFlag = Marshal.ReadInt32(lParam, 8) >> 16;
                int wmKey = wParam.ToInt32();
                key = MouseKeys.GetKey(wmKey, xButtonFlag);
            }
            else if (msgType == 0x0100) //keydown
            {
                key = (Keys)Marshal.ReadInt32(lParam);
            }

            if (key != Keys.None)
            {
                KeyIsPressed(key);
            }

            //return the value returned by CallNextHookEx
            return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        };

        public static OverlayEventLog _overlayEventLog = new OverlayEventLog();
        public static List<ModelEventTimer> _binds = new List<ModelEventTimer>();
        private static void KeyIsPressed(Keys key)
        {
            foreach (var bind in _binds)
            {
                if (bind.Key == key)
                {
                    if (!bind.IsEnabled)
                    {                     
                        _overlayEventLog.ActivateTimer(bind);
                    }
                    else
                    {
                        bind.Reset();
                    }
                }
                if(key == Keys.End)
                {
                    bind.Reset();
                }

                if (key == Keys.Home)
                {
                    
                    if (_overlayEventLog.Background == Brushes.Transparent)
                    {
                        _overlayEventLog.GameMode = false;
                        _overlayEventLog.ResizeMode = ResizeMode.CanResizeWithGrip;
                        _overlayEventLog.Background = Brushes.Gray;
                    }
                    else //хуйня
                    {
                        _overlayEventLog.GameMode = true;
                        _overlayEventLog.ResizeMode = ResizeMode.NoResize;
                        _overlayEventLog.Background = Brushes.Transparent;
                    }
                }
            }
        }

        public static void AddBind(ModelEventTimer timer)
        {
            _binds.Add(timer);
        }

        public static void RemoveBind(ModelEventTimer timer)
        {
            _binds.Remove(timer);
        }

        public App()
        {
            Console.WriteLine("Application is setting hooks");
            SetWindowsHookEx(HookType.Keyboard, hook, IntPtr.Zero, 0);
            SetWindowsHookEx(HookType.Mouse, hook, IntPtr.Zero, 0);
            Console.WriteLine("Hooks are setted");
            Console.WriteLine($"My MainWindow: {Current.MainWindow?.IsVisible}");
            _overlayEventLog.Show();
        }
    }
}
