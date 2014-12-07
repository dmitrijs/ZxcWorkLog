using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ZxcWorkLog.Data;
using ZxcWorkLog.Util;

namespace ZxcWorkLog
{
    class WorkLog
    {
        private static WorkItemCollection wis = null;

        private static string GetLogPath()
        {
            return Common.LogPath;
        }

        public static void saveWorkItems()
        {
            if (wis == null) loadWorkItems();
            if (GetLogPath() == "") return;

            XmlTextWriter textWriter = new XmlTextWriter(GetLogPath(), null);
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
            if (string.IsNullOrEmpty(GetLogPath()))
            {
                Console.WriteLine(@"LogPath is invalid.");
                return;
            }

            if (!File.Exists(GetLogPath()))
            {
                WorkLog.wis = new WorkItemCollection();
                saveWorkItems();
            }

            WorkItemCollection wis = new WorkItemCollection();

            XmlTextReader textReader = new XmlTextReader(GetLogPath());
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

            WorkLog.wis = wis;
        }

        public static WorkItem addNewWorkItem(string title, string periodText, string groupName)
        {
            if (wis == null) loadWorkItems();
            long ticks = TimeUtil.FromReadableTime(periodText).Ticks;
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

        public static void updateItem(int key, WorkItem wi)
        {
            wis[key] = wi;
            saveWorkItems();
        }
    }
}
