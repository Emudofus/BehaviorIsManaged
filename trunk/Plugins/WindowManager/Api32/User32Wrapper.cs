#region License GNU GPL
// User32Wrapper.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
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