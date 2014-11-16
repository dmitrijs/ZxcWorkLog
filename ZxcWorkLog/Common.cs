using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;
using ZxcWorkLog.Data;

namespace ZxcWorkLog
{
    internal class Common
    {
        private static string logpath;
        private static string jiraUser;
        private static string jiraPass;
        private static string jiraJQL;
        private static WorkItemCollection wis = null;
        private static string ssDir;
        private static int ssTimeout;
        private static bool ssEnabled;


        public static string JiraUser
        {
            get { return jiraUser; }
        }

        public static string JiraPass
        {
            get { return jiraPass; }
        }

        public static string JiraJQL
        {
            get { return jiraJQL; }
        }

        public static string ScreenShotsDir
        {
            get { return ssDir; }
        }

        public static int ScreenShotsTimeout
        {
            get { return ssTimeout; }
        }

        public static bool ScreenShotsEnabled
        {
            get { return ssEnabled; }
        }

        public static void saveWorkItems()
        {
            if (wis == null) loadWorkItems();
            if (logpath == "") return;

            XmlTextWriter textWriter = new XmlTextWriter(logpath, null);
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("WorkItems");
            foreach (WorkItem wi in wis.Values)
            {
                textWriter.WriteStartElement("WorkItem");

                textWriter.WriteStartAttribute("ID");
                textWriter.WriteString(wi.Id.ToString());
                textWriter.WriteEndAttribute();

                textWriter.WriteStartAttribute("Title");
                textWriter.WriteString(wi.Title);
                textWriter.WriteEndAttribute();

                textWriter.WriteStartAttribute("PeriodTicks");
                textWriter.WriteString(wi.PeriodTicks.ToString());
                textWriter.WriteEndAttribute();

                textWriter.WriteStartAttribute("StartTime");
                textWriter.WriteString(wi.StartTime.ToString());
                textWriter.WriteEndAttribute();

                textWriter.WriteStartAttribute("WasWorkLogged");
                textWriter.WriteString(wi.WasWorkLogged.ToString());
                textWriter.WriteEndAttribute();

                textWriter.WriteStartAttribute("GroupName");
                textWriter.WriteString(wi.GroupName);
                textWriter.WriteEndAttribute();

                textWriter.WriteEndElement();
            }
            textWriter.WriteEndElement();

            textWriter.WriteEndDocument();
            textWriter.Close();
        }

        public static WorkItemCollection getWorkItems()
        {
            if (wis == null) loadWorkItems();
            return wis;
        }

        private static void loadWorkItems()
        {
            if (string.IsNullOrEmpty(logpath))
            {
                Console.WriteLine(@"LogPath is invalid.");
                return;
            }

            if (!File.Exists(logpath))
            {
                Common.wis = new WorkItemCollection();
                saveWorkItems();
            }

            WorkItemCollection wis = new WorkItemCollection();

            XmlTextReader textReader = new XmlTextReader(logpath);
            textReader.Read();
            while (textReader.Read())
            {
                if (textReader.Name == "WorkItem")
                {
                    WorkItem wi = new WorkItem();
                    wi.Id = Int32.Parse(textReader.GetAttribute("ID"));
                    wi.PeriodTicks = long.Parse(textReader.GetAttribute("PeriodTicks"));
                    wi.StartTime = DateTime.Parse(textReader.GetAttribute("StartTime"));
                    wi.Title = textReader.GetAttribute("Title");
                    if (textReader.GetAttribute("WasWorkLogged") != null)
                    {
                        wi.WasWorkLogged = Boolean.Parse(textReader.GetAttribute("WasWorkLogged"));
                    }
                    if (textReader.GetAttribute("GroupName") != null)
                    {
                        wi.GroupName = textReader.GetAttribute("GroupName");
                    }
                    else
                    {
                        wi.GroupName = "Default";
                    }
                    wi.Id = wis.NextFreeKey();
                    wis.Add(wi.Id, wi);
                }
            }
            textReader.Close();

            Common.wis = wis;
        }

        public static string getLogPath()
        {
            return logpath;
        }

        public static void settingsLoad()
        {
            if (File.Exists("settings.xml"))
            {
                XmlTextReader textReader = new XmlTextReader("settings.xml");
                textReader.Read();
                while (textReader.Read())
                {
                    if (textReader.Name == "setting")
                    {
                        switch (textReader.GetAttribute("name"))
                        {
                            case "LogPath":
                                logpath = textReader.GetAttribute("value");
                                break;
                            case "JiraUser":
                                jiraUser = textReader.GetAttribute("value");
                                break;
                            case "JiraPass":
                                jiraPass = textReader.GetAttribute("value");
                                break;
                            case "JiraJQL":
                                jiraJQL = textReader.GetAttribute("value");
                                break;
                            case "ScreenShotsEnabled":
                                ssEnabled = Boolean.Parse(textReader.GetAttribute("value"));
                                break;
                            case "ScreenShotsDir":
                                ssDir = textReader.GetAttribute("value");
                                break;
                            case "ScreenShotsTimeout":
                                ssTimeout = Int32.Parse(textReader.GetAttribute("value"));
                                break;
                        }
                    }
                }
                textReader.Close();
            }
            else
            {
                logpath = @".\worklog.xml";
                Console.WriteLine("settings.xml not found");
            }
        }

        public static WorkItem addNewWorkItem(string title, string periodText, string groupName)
        {
            if (wis == null) loadWorkItems();
            long ticks = fromReadableTime(periodText).Ticks;
            var wi = new WorkItem
                     {
                         Id = wis.NextFreeKey(),
                         Title = (ticks > 0 ? "* " : "") + title,
                         StartTime = DateTime.Now,
                         PeriodTicks = ticks,
                         GroupName = groupName
                     };
            wis.Add(wi.Id, wi);
            saveWorkItems();
            return wi;
        }

        public static void settingsUpdate(string _logpath, string _jiraUser, string _jiraPass, string _jiraJQL,
            bool _ssEnabled, int _ssTimeout, string _ssDir)
        {
            logpath = _logpath;
            jiraUser = _jiraUser;
            jiraPass = _jiraPass;
            jiraJQL = _jiraJQL;
            ssEnabled = _ssEnabled;
            ssTimeout = _ssTimeout;
            ssDir = _ssDir;
            settingsSave();
        }

        private static void settingsSave()
        {
            XmlTextWriter textWriter = new XmlTextWriter("settings.xml", null);
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("settings");
            textWriter.WriteStartElement("setting");
            textWriter.WriteStartAttribute("name");
            textWriter.WriteString("LogPath");
            textWriter.WriteEndAttribute();
            textWriter.WriteStartAttribute("value");
            textWriter.WriteString(logpath);
            textWriter.WriteEndAttribute();
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("setting");
            textWriter.WriteStartAttribute("name");
            textWriter.WriteString("JiraUser");
            textWriter.WriteEndAttribute();
            textWriter.WriteStartAttribute("value");
            textWriter.WriteString(jiraUser);
            textWriter.WriteEndAttribute();
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("setting");
            textWriter.WriteStartAttribute("name");
            textWriter.WriteString("JiraPass");
            textWriter.WriteEndAttribute();
            textWriter.WriteStartAttribute("value");
            textWriter.WriteString(jiraPass);
            textWriter.WriteEndAttribute();
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("setting");
            textWriter.WriteStartAttribute("name");
            textWriter.WriteString("JiraJQL");
            textWriter.WriteEndAttribute();
            textWriter.WriteStartAttribute("value");
            textWriter.WriteString(jiraJQL);
            textWriter.WriteEndAttribute();
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("setting");
            textWriter.WriteStartAttribute("name");
            textWriter.WriteString("ScreenShotsEnabled");
            textWriter.WriteEndAttribute();
            textWriter.WriteStartAttribute("value");
            textWriter.WriteString(ssEnabled.ToString());
            textWriter.WriteEndAttribute();
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("setting");
            textWriter.WriteStartAttribute("name");
            textWriter.WriteString("ScreenShotsTimeout");
            textWriter.WriteEndAttribute();
            textWriter.WriteStartAttribute("value");
            textWriter.WriteString(ssTimeout.ToString());
            textWriter.WriteEndAttribute();
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("setting");
            textWriter.WriteStartAttribute("name");
            textWriter.WriteString("ScreenShotsDir");
            textWriter.WriteEndAttribute();
            textWriter.WriteStartAttribute("value");
            textWriter.WriteString(ssDir);
            textWriter.WriteEndAttribute();
            textWriter.WriteEndElement();
            textWriter.WriteEndElement();
            textWriter.WriteEndDocument();
            textWriter.Close();
        }

        public static void updateItem(int key, WorkItem wi)
        {
            wis[key] = wi;
            saveWorkItems();
        }

        public static string toReadableTime(long ticks, bool showSeconds = true, bool showDays = true)
        {
            TimeSpan span = new TimeSpan(ticks);
            long days = span.Days;
            long hours = span.Hours;
            long minutes = span.Minutes;
            long seconds = span.Seconds;

            if (!showDays)
            {
                hours += days*24;
                days = 0;
            }

            string ret = "";
            if (days > 0) ret += days + "d ";
            if (hours > 0) ret += hours + "h ";
            if (minutes > 0) ret += minutes + "m ";
            if (seconds > 0 && showSeconds) ret += seconds + "s";

            if (ret != "") return ret;
            return showSeconds ? "0s" : "0m";
        }

        public static TimeSpan fromReadableTime(string text)
        {
            Console.WriteLine(text);
            string pat = @"(\d+)(\w+)";

            Regex r = new Regex(pat);

            int days = 0, hours = 0, minutes = 0, seconds = 0;

            Match m = r.Match(text);
            while (m.Success)
            {
                try
                {
                    int num = Int32.Parse(m.Groups[1].Captures[0].Value);

                    switch (m.Groups[2].Captures[0].Value)
                    {
                        case "d":
                            days = num;
                            break;
                        case "h":
                            hours = num;
                            break;
                        case "m":
                            minutes = num;
                            break;
                        case "s":
                            seconds = num;
                            break;
                    }
                }
                catch (Exception)
                {
                }
                m = m.NextMatch();
            }
            return new TimeSpan(days, hours, minutes, seconds);
        }

        public static String getFirstLine(string text)
        {
            var result = Regex.Split(text, "\r\n|\r|\n");
            return result[0];
        }

        [DllImport("user32.dll")]
        public static extern
        bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
    }
}