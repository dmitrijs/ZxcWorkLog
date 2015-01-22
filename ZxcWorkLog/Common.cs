using ZxcWorkLog.Properties;

namespace ZxcWorkLog
{
    internal static class Common
    {
        public static string JiraUrl
        {
            get { return Settings.Default.JiraUrl; }
        }

        public static string JiraUser
        {
            get { return Settings.Default.JiraUser; }
        }

        public static string JiraPass
        {
            get { return Settings.Default.JiraPass; }
        }

        public static string JiraJql
        {
            get { return Settings.Default.JiraJQL; }
        }

        public static string ScreenShotsDir
        {
            get { return Settings.Default.ScreenShotsDir; }
        }

        public static int ScreenShotsTimeout
        {
            get { return Settings.Default.ScreenShotsTimeout; }
        }

        public static bool ScreenShotsEnabled
        {
            get { return Settings.Default.ScreenShotsEnabled; }
        }

        public static int HoursPerDay
        {
            get { return Settings.Default.HoursPerDay; }
        }

        public static string LogPath {
            get { return Settings.Default.LogPath; }
        }

        public static bool EfectivenessWarningEnabled
        {
            get { return Settings.Default.EfectivenessWarningEnabled; }
        }
    }
}
