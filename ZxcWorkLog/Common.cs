using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace ZxcWorkLog
{
    class Common
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
            get
            {
                return jiraUser;
            }
        }
        public static string JiraPass
        {
            get
            {
                return jiraPass;
            }
        }
        public static string JiraJQL
        {
            get
            {
                return jiraJQL;
            }
        }

        public static string ScreenShotsDir
        {
            get
            {
                return ssDir;
            }
        }
        public static int ScreenShotsTimeout
        {
            get
            {
                return ssTimeout;
            }
        }
        public static bool ScreenShotsEnabled
        {
            get
            {
                return ssEnabled;
            }
        }
        public static void saveWorkItems()
        {
            if (Common.wis == null) Common.loadWorkItems();
            if (Common.logpath == "") return;

            XmlTextWriter textWriter = new XmlTextWriter(Common.logpath, null);
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("WorkItems");
            foreach (WorkItem wi in wis.Values)
            {
                textWriter.WriteStartElement("WorkItem");

                textWriter.WriteStartAttribute("ID");
                textWriter.WriteString(wi.ID.ToString());
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
            if (Common.wis == null) Common.loadWorkItems();
            return Common.wis;
        }

        private static void loadWorkItems()
        {
            if (string.IsNullOrEmpty(Common.logpath))
            {
                Console.WriteLine(@"LogPath is invalid.");
                return;
            }

            if (!File.Exists(Common.logpath))
            {
                Common.wis = new WorkItemCollection();
                saveWorkItems();
            }

            WorkItemCollection wis = new WorkItemCollection();

            XmlTextReader textReader = new XmlTextReader(Common.logpath);
            textReader.Read();
            while (textReader.Read())
            {
                if (textReader.Name == "WorkItem")
                {
                    WorkItem wi = new WorkItem();
                    wi.ID = Int32.Parse(textReader.GetAttribute("ID"));
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
                    wi.ID = wis.nextFreeKey();
                    wis.Add(wi.ID, wi);
                }
            }
            textReader.Close();

            Common.wis = wis;
        }

        public static string getLogPath() 
        {
            return Common.logpath;
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
                                Common.logpath = textReader.GetAttribute("value");
                                break;
                            case "JiraUser":
                                Common.jiraUser = textReader.GetAttribute("value");
                                break;
                            case "JiraPass":
                                Common.jiraPass = textReader.GetAttribute("value");
                                break;
                            case "JiraJQL":
                                Common.jiraJQL = textReader.GetAttribute("value");
                                break;
                            case "ScreenShotsEnabled":
                                Common.ssEnabled = Boolean.Parse(textReader.GetAttribute("value"));
                                break;
                            case "ScreenShotsDir":
                                Common.ssDir = textReader.GetAttribute("value");
                                break;
                            case "ScreenShotsTimeout":
                                Common.ssTimeout = Int32.Parse(textReader.GetAttribute("value"));
                                break;
                        }
                    }
                }
                textReader.Close();
            }
            else
            {
                Common.logpath = @".\worklog.xml";
                Console.WriteLine("settings.xml not found");
            }
        }

        public static WorkItem addNewWorkItem(string title, string periodText, string groupName)
        {
            if (Common.wis == null) Common.loadWorkItems();
            long ticks = fromReadableTime(periodText).Ticks;
            var wi = new WorkItem
                              {
                                  ID = wis.nextFreeKey(),
                                  Title = (ticks > 0 ? "* " : "") + title,
                                  StartTime = DateTime.Now,
                                  PeriodTicks = ticks,
                                  GroupName = groupName
                              };
            wis.Add(wi.ID, wi);
            Common.saveWorkItems();
            return wi;
        }

        public static void settingsUpdate(string _logpath, string _jiraUser, string _jiraPass, string _jiraJQL, bool _ssEnabled, int _ssTimeout, string _ssDir)
        {
            Common.logpath = _logpath;
            Common.jiraUser = _jiraUser;
            Common.jiraPass = _jiraPass;
            Common.jiraJQL = _jiraJQL;
            Common.ssEnabled = _ssEnabled;
            Common.ssTimeout = _ssTimeout;
            Common.ssDir = _ssDir;
            Common.settingsSave();
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
                    textWriter.WriteString(Common.logpath);
                    textWriter.WriteEndAttribute();
                textWriter.WriteEndElement();
                textWriter.WriteStartElement("setting");
                    textWriter.WriteStartAttribute("name");
                    textWriter.WriteString("JiraUser");
                    textWriter.WriteEndAttribute();
                    textWriter.WriteStartAttribute("value");
                    textWriter.WriteString(Common.jiraUser);
                    textWriter.WriteEndAttribute();
                textWriter.WriteEndElement();
                textWriter.WriteStartElement("setting");
                    textWriter.WriteStartAttribute("name");
                    textWriter.WriteString("JiraPass");
                    textWriter.WriteEndAttribute();
                    textWriter.WriteStartAttribute("value");
                    textWriter.WriteString(Common.jiraPass);
                    textWriter.WriteEndAttribute();
                textWriter.WriteEndElement();
                textWriter.WriteStartElement("setting");
                    textWriter.WriteStartAttribute("name");
                    textWriter.WriteString("JiraJQL");
                    textWriter.WriteEndAttribute();
                    textWriter.WriteStartAttribute("value");
                    textWriter.WriteString(Common.jiraJQL);
                    textWriter.WriteEndAttribute();
                textWriter.WriteEndElement();
                textWriter.WriteStartElement("setting");
                    textWriter.WriteStartAttribute("name");
                    textWriter.WriteString("ScreenShotsEnabled");
                    textWriter.WriteEndAttribute();
                    textWriter.WriteStartAttribute("value");
                    textWriter.WriteString(Common.ssEnabled.ToString());
                    textWriter.WriteEndAttribute();
                textWriter.WriteEndElement();
                textWriter.WriteStartElement("setting");
                    textWriter.WriteStartAttribute("name");
                    textWriter.WriteString("ScreenShotsTimeout");
                    textWriter.WriteEndAttribute();
                    textWriter.WriteStartAttribute("value");
                    textWriter.WriteString(Common.ssTimeout.ToString());
                    textWriter.WriteEndAttribute();
                textWriter.WriteEndElement();
                textWriter.WriteStartElement("setting");
                    textWriter.WriteStartAttribute("name");
                    textWriter.WriteString("ScreenShotsDir");
                    textWriter.WriteEndAttribute();
                    textWriter.WriteStartAttribute("value");
                    textWriter.WriteString(Common.ssDir);
                    textWriter.WriteEndAttribute();
                textWriter.WriteEndElement();
            textWriter.WriteEndElement();
            textWriter.WriteEndDocument();
            textWriter.Close();
        }

        public static void updateItem(int key, WorkItem wi)
        {
            Common.wis[key] = wi;
            Common.saveWorkItems();
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

            int days=0, hours=0, minutes=0, seconds=0;

            Match m = r.Match(text);
            while (m.Success)
            {
                try
                {
                    int num = Int32.Parse(m.Groups[1].Captures[0].Value);

                    switch (m.Groups[2].Captures[0].Value)
                    {
                        case "d": days = num; break;
                        case "h": hours = num; break;
                        case "m": minutes = num; break;
                        case "s": seconds = num; break;
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
