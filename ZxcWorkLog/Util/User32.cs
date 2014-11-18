using System;
using System.Runtime.InteropServices;

namespace ZxcWorkLog.Util
{
    static class User32
    {
        [DllImport("user32.dll")]
        public static extern
        bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
    }
}
