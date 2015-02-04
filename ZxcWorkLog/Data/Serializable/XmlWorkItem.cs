using System;
using System.Xml.Serialization;

namespace ZxcWorkLog.Data.Serializable
{
    [Serializable]
    public class XmlWorkItem
    {
        public XmlWorkItem()
        {
        }

        public XmlWorkItem(WorkItem wi)
        {
            Id = wi.Id;
            Title = wi.Title;
            GroupName = wi.GroupName;
            PeriodTicks = wi.PeriodTicks;
            StartTime = wi.StartTime;
            WasWorkLogged = wi.WasWorkLogged;
            IsDistributed = wi.IsDistributed;
        }

        [XmlElement]
        public string Title { set; get; }

        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string GroupName { get; set; }

        [XmlAttribute]
        public long PeriodTicks { get; set; }

        [XmlAttribute]
        public DateTime StartTime { get; set; }

        [XmlAttribute]
        public bool WasWorkLogged { get; set; }

        [XmlAttribute]
        public bool IsDistributed { get; set; }}
}
