﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;
using ZxcWorkLog.Data;

namespace ZxcWorkLog
{
    internal class Common
    {
        private static string logPath;
        private static string jiraUser;
        private static string jiraPass;
        private static string jiraJQL;
        private static string ssDir;
        private static int ssTimeout;
        private static bool ssEnabled;
        private static int hoursPerDay;
        
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

        public static int HoursPerDay
        {
            get { return hoursPerDay; }
        }

        public static string LogPath {
            get { return logPath; }
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
                                logPath = textReader.GetAttribute("value");
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
                            case "WorkingHoursPerDay":
                                hoursPerDay = Int32.Parse(textReader.GetAttribute("value"));
                                break;
                        }
                    }
                }
                textReader.Close();
            }
            else
            {
                logPath = @".\worklog.xml";
                Console.WriteLine("settings.xml not found");
            }
            // default value in case of migration
            if (hoursPerDay == 0)
            {
                hoursPerDay = 8;
            }
        }

        public static void settingsUpdate(string _logpath, string _jiraUser, string _jiraPass, string _jiraJQL, bool _ssEnabled, int _ssTimeout, string _ssDir, int _hoursPerDay)
        {
            logPath = _logpath;
            jiraUser = _jiraUser;
            jiraPass = _jiraPass;
            jiraJQL = _jiraJQL;
            ssEnabled = _ssEnabled;
            ssTimeout = _ssTimeout;
            ssDir = _ssDir;
            hoursPerDay = _hoursPerDay;
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
            textWriter.WriteString(logPath);
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
            textWriter.WriteStartElement("setting");
            textWriter.WriteStartAttribute("name");
            textWriter.WriteString("WorkingHoursPerDay");
            textWriter.WriteEndAttribute();
            textWriter.WriteStartAttribute("value");
            textWriter.WriteString(hoursPerDay.ToString());
            textWriter.WriteEndAttribute();
            textWriter.WriteEndElement();
            textWriter.WriteEndElement();
            textWriter.WriteEndDocument();
            textWriter.Close();
        }
    }
}
