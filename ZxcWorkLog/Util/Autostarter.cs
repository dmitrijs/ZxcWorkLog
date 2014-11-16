using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;

namespace ZxcWorkLog.Util
{
    public static class AutoStarter
    {
        private const string RUN_LOCATION = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string PROGRAM_NAME = "ZxcWorkLog";

        public static void SetAutoStart()
        {
            var key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
            Debug.Assert(key != null);
            key.SetValue(PROGRAM_NAME, Assembly.GetExecutingAssembly().Location);
        }

        public static bool IsAutoStartEnabled
        {
            get
            {
                var key = Registry.CurrentUser.OpenSubKey(RUN_LOCATION);
                if (key == null)
                    return false;

                var value = (string) key.GetValue(PROGRAM_NAME);
                if (value == null)
                    return false;
                return (value == Assembly.GetExecutingAssembly().Location);
            }
        }

        public static void UnSetAutoStart()
        {
            try
            {
                var key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
                Debug.Assert(key != null);
                key.DeleteValue(PROGRAM_NAME);
            }
            catch (Exception)
            {
            }
        }
    }
}
