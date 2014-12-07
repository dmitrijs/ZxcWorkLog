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
            Common.settingsUpdate(textBox1.Text, textBoxJiraUser.Text, textBoxJiraPass.Text, textBoxJiraJQL.Text,
                inpScreenShotEnabled.Checked, Int32.Parse(inpScreenShotTimeout.Text), inpScreenShotDir.Text, Int32.Parse(textBoxHoursPerDay.Text));
            if (checkBoxAutostart.Checked)
            {
                AutoStarter.SetAutoStart();
            }
            else
            {
                AutoStarter.UnSetAutoStart();
            }
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
            textBoxJiraJQL.Text = Common.JiraJQL;
            inpScreenShotEnabled.Checked = Common.ScreenShotsEnabled;
            inpScreenShotTimeout.Text = string.Format("{0}", Common.ScreenShotsTimeout);
            inpScreenShotDir.Text = Common.ScreenShotsDir;
            checkBoxAutostart.Checked = AutoStarter.IsAutoStartEnabled;
        }
    }
}