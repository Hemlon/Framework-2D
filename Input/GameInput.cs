using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Framework2D
{
    static class GameInput
    {
        static Dictionary<Keys, bool> KeyDown = new Dictionary<Keys, bool>();
        static Dictionary<char, bool> KeyChar = new Dictionary<char, bool>();
        static Dictionary<MouseButtons, bool> MouseButton = new Dictionary<MouseButtons, bool>();
        static PointF mouseLocation = new PointF(0, 0);
        public static void SetMouseLocation(PointF loc)
        {
            mouseLocation = loc;
        }
        public static PointF GetMouseLocation()
        {
            return mouseLocation;
        }
        public static bool GetMouseButton(MouseButtons button)
        {
            return button.GetByKey(MouseButton);
        }
        public static void SetMouseButton(MouseButtons button)
        {
            button.SetByKeyTo<MouseButtons, bool>(MouseButton, true);
        }
        public static void ResetMouseButton(MouseButtons button)
        {
            button.SetByKeyTo<MouseButtons, bool>(MouseButton, false);
        }
        public static void SetKeyDown(Keys key)
        {
            key.SetByKeyTo<Keys, bool>(KeyDown, true);
        }
        public static void ResetKeyDown(Keys key)
        {
            key.SetByKeyTo<Keys, bool>(KeyDown, false);
        }
        public static void SetKeyChar(char key)
        {
            key.SetByKeyTo<Char, bool>(KeyChar, true);
        }
        public static void ResetKeyChar(char key)
        {
            key.SetByKeyTo<Char, bool>(KeyChar, false);
        }
        public static bool isKeyDown(Keys key)
        {
           return key.GetByKeyFrom(KeyDown);
        }
        public static bool isKeyChar(char key)
        {
            return key.GetByKeyFrom(KeyChar);
        }

    }
}
