using System;
using System.Windows.Forms;

namespace ZxcWorkLog.Data
{
    public class WorkItem : ListViewItem
    {
        private string _title;

        public string GroupName { get; set; }

        public int Id { get; set; }

        public long PeriodTicks { get; set; }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                Text = value;
            }
        }

        public DateTime StartTime { get; set; }

        public bool InProgress { get; private set; }

        public void StartProgress()
        {
            InProgress = true;
        }

        public void StopProgress()
        {
            InProgress = false;
        }

        public bool WasWorkLogged { get; set; }

        public bool IsDistributed { get; set; }

        public long RealTicks { get; set; }
    }
}
