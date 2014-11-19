using System;
using System.Windows.Forms;

namespace ZxcWorkLog
{
    internal static class Program
    {
        public static FormMain FormMain { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FormMain = new FormMain();
            if (FormMain.Setup())
            {
                Application.Run(FormMain);
            }
        }
    }
}
