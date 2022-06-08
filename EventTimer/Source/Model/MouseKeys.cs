using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace EventTimer.Source.Model
{
    class MouseKeys
    {
        public const int Left = 0x201;
        public const int Right = 0x204;
        public const int Middle = 0x207;
        public const int XButton = 0x20B;

        private readonly static Dictionary<int, Keys> _buttons = new Dictionary<int, Keys> 
        {   { Left, Keys.LButton },
            { Right, Keys.RButton },
            { Middle, Keys.MButton },
            { XButton, (Keys)0x04 } // + flag
        };

        private readonly static Dictionary<MouseButton, Keys> _mbuttons = new Dictionary<MouseButton, Keys>
        {   { MouseButton.Left, Keys.LButton },
            { MouseButton.Right, Keys.RButton },
            { MouseButton.Middle, Keys.MButton },
            { MouseButton.XButton1, Keys.XButton1 },
            { MouseButton.XButton2, Keys.XButton2 }
        };

        public static bool IsMouseKey(int vmkey)
        {
            return _buttons.ContainsKey(vmkey);
        }

        public static Keys GetKey(int vmkey, int xButtonFlag)
        {
            Keys key = _buttons[vmkey];
            if (key == _buttons[XButton])
            {
                key += xButtonFlag;
            }

            return key;
        }

        public static Keys GetKey(MouseButton button)
        {
            return _mbuttons[button];
        }
    }
}
