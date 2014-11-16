using System.Collections.Generic;

namespace ZxcWorkLog.Data
{
    internal class WorkItemCollection : Dictionary<int, WorkItem>
    {
        public int NextFreeKey()
        {
            var max = -1;
            foreach (var key in Keys)
            {
                if (key > max) max = key;
            }
            return max + 1;
        }

        public IEnumerable<WorkItem> GetSortedList()
        {
            var list = new List<WorkItem>(Values);
            list.Sort(CompareWorkItems);
            return list;
        }

        private static int CompareWorkItems(WorkItem wi1, WorkItem wi2)
        {
            if (wi1.StartTime > wi2.StartTime)
            {
                return -1;
            }
            return 1;
        }
    }
}