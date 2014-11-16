using System;
using System.Windows.Forms;

namespace ZxcWorkLog
{
    public class WorkItem : ListViewItem
    {
        private string title;
        private bool inProgress;

        public string GroupName { get; set; }

        public int ID { get; set; }

        public TimeSpan Period
        {
            get
            {
                var spanTicks = new TimeSpan(PeriodTicks);
                return spanTicks;
            }
        }

        public long PeriodTicks { get; set; }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                Text = value;
            }
        }

        public DateTime StartTime { get; set; }

        public bool InProgress
        {
            get { return inProgress; }
        }

        public void startProgress()
        {
            inProgress = true;
        }

        public void stopProgress()
        {
            inProgress = false;
        }

        public bool WasWorkLogged { get; set; }
    }
}