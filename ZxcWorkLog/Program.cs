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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _main = new FormMain();
            if (_main.Setup())
            {
                Application.Run(_main);
            }
        }

        public static FormMain FormMain
        {
            get { return _main; }
        }
    }
}
