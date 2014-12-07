using System;
using System.Windows.Forms;
using ZxcWorkLog.Properties;
using ZxcWorkLog.Util;

namespace ZxcWorkLog
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (checkBoxAutostart.Checked)
            {
                AutoStarter.SetAutoStart();
            }
            else
            {
                AutoStarter.UnSetAutoStart();
            }
            Settings.Default.LogPath = textBox1.Text;
            Settings.Default.HoursPerDay = Int32.Parse(textBoxHoursPerDay.Text);
            Settings.Default.JiraUser = textBoxJiraUser.Text;
            Settings.Default.JiraPass = textBoxJiraPass.Text;
            Settings.Default.JiraJQL = textBoxJiraJQL.Text;
            Settings.Default.ScreenShotsEnabled = inpScreenShotEnabled.Checked;
            Settings.Default.ScreenShotsTimeout = Int32.Parse(inpScreenShotTimeout.Text);
            Settings.Default.ScreenShotsDir = inpScreenShotDir.Text;
            Settings.Default.Save();
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            textBox1.Text = Common.LogPath;
            textBoxHoursPerDay.Text = string.Format("{0}", Common.HoursPerDay);
            textBoxJiraUser.Text = Common.JiraUser;
            textBoxJiraPass.Text = Common.JiraPass;
            textBoxJiraJQL.Text = Common.JiraJql;
            inpScreenShotEnabled.Checked = Common.ScreenShotsEnabled;
            inpScreenShotTimeout.Text = string.Format("{0}", Common.ScreenShotsTimeout);
            inpScreenShotDir.Text = Common.ScreenShotsDir;
            checkBoxAutostart.Checked = AutoStarter.IsAutoStartEnabled;
        }
    }
}