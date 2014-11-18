using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ZxcWorkLog.Data.Serializable
{
    [Serializable]
    public class XmlWorkLog
    {
        public XmlWorkLog()
        {
            Items = new List<XmlWorkItem>();
        }

        public XmlWorkLog(WorkItemCollection wic)
        {
            Items = new List<XmlWorkItem>();
            foreach (var workItem in wic.GetSortedList())
            {
                Items.Add(new XmlWorkItem(workItem));
            }
        }

        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<XmlWorkItem> Items { get; set; }
    }
}
