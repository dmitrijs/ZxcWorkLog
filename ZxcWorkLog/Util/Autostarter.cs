using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Reflection;

namespace ZxcWorkLog.Util
{
    public static class AutoStarter
    {
        private const string RUN_LOCATION = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string PROGRAM_NAME = "ZxcWorkLog";

        public static void SetAutoStart()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
            key.SetValue(PROGRAM_NAME, Assembly.GetExecutingAssembly().Location);
        }

        public static bool IsAutoStartEnabled
        {
            get
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_LOCATION);
                if (key == null)
                    return false;

                string value = (string)key.GetValue(PROGRAM_NAME);
                if (value == null)
                    return false;
                return (value == Assembly.GetExecutingAssembly().Location);
            }
        }

        public static void UnSetAutoStart()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
                key.DeleteValue(PROGRAM_NAME);
            }
            catch (Exception)
            { }
        }
    }
}
