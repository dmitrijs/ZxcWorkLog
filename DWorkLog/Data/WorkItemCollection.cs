using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DWorkLog
{
    class WorkItemCollection : Dictionary<int, WorkItem>
    {
        public int nextFreeKey()
        {
            int max = -1;
            foreach (int key in Keys)
            {
                if (key > max) max = key;
            }
            return max + 1;
        }

        public List<WorkItem> getSortedList()
        {
            List<WorkItem> list = new List<WorkItem>(this.Values);
            list.Sort(compareWorkItems);
            return list;
        }

        private int compareWorkItems(WorkItem wi1, WorkItem wi2)
        {
            if (wi1.StartTime > wi2.StartTime)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}
