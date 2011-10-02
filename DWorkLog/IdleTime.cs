﻿using System;
using System.Runtime.InteropServices;

namespace DWorkLog
{
    internal struct LASTINPUTINFO
    {
        public uint cbSize;
        public uint dwTime;
    }

    public sealed class IdleTime
    {
        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        public static uint GetUserIdleTime()
        {
            var lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint) Marshal.SizeOf(lastInputInfo);
            GetLastInputInfo(ref lastInputInfo);
   			return (uint)Environment.TickCount - lastInputInfo.dwTime;
        }

        public static long GetUserIdleSeconds()
        {
            var lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            GetLastInputInfo(ref lastInputInfo);
            return (Environment.TickCount - lastInputInfo.dwTime) / 1000;
        }
    }
}
