using System;
using System.Windows.Forms;

namespace ZxcWorkLog
{
    internal static class Program
    {
        private static FormMain _main;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
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