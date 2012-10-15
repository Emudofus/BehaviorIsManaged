using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowManager.Api32
{
    public class User32Wrapper
    {
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public static void SendKey(IntPtr hWnd, Keys key)
        {
            SendMessage(hWnd, WM_KEYDOWN, Convert.ToInt32(key), 0);
            SendMessage(hWnd, WM_KEYUP, Convert.ToInt32(key), 0);
        }

        public static void SendSysKey(IntPtr hWnd, Keys key)
        {
            SendMessage(hWnd, WM_SYSKEYDOWN, Convert.ToInt32(key), 0);
            SendMessage(hWnd, WM_SYSKEYUP, Convert.ToInt32(key), 0);
        }

        public static void SendChar(IntPtr hWnd, char c)
        {
            SendMessage(hWnd, WM_CHAR, c, 0);
        }

        #region Constants

        // Messages
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_CHAR = 0x105;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;

        #endregion
    }
}