using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DWorkLog
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
            set { this.title = value; this.Text = value; }
        }

        public DateTime StartTime { get; set; }

        public bool InProgress
        {
            get { return inProgress; }
        }

        public void startProgress()
        {
            this.inProgress = true;
        }
        public void stopProgress()
        {
            this.inProgress = false;
        }

        public bool WasWorkLogged { get; set; }
    }
}
