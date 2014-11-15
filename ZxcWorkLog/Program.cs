using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using ZxcWorkLog.Jira;

namespace ZxcWorkLog
{
    static class Program
    {
        private static FormMain _main;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            int processes = 0;
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName.Replace(".vshost", "").Equals(Process.GetCurrentProcess().ProcessName.Replace(".vshost", "")))
                {
                    processes++;
                }
            }
            if (processes > 1)
            {
                MessageBox.Show("Program is already running!");
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                _main = new FormMain();
                Application.Run(_main);
            }
        }

        public static FormMain FormMain
        {
            get { return _main; }
        }
    }
}
