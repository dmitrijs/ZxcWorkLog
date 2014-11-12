using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace ZxcWorkLog
{
    static class Program
    {
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
                Application.Run(new FormMain());
            }
        }
    }
}
