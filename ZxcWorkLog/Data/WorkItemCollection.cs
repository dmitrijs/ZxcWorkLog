using System.Collections.Generic;

namespace ZxcWorkLog
{
    internal class WorkItemCollection : Dictionary<int, WorkItem>
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
            List<WorkItem> list = new List<WorkItem>(Values);
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