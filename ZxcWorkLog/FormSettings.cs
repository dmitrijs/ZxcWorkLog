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
            Settings.Default.JiraUrl = textBoxJiraUrl.Text;
            Settings.Default.JiraPass = textBoxJiraPass.Text;
            Settings.Default.JiraJQL = textBoxJiraJQL.Text;
            Settings.Default.ScreenShotsEnabled = inpScreenShotEnabled.Checked;
            Settings.Default.ScreenShotsTimeout = Int32.Parse(inpScreenShotTimeout.Text);
            Settings.Default.ScreenShotsDir = inpScreenShotDir.Text;
            Settings.Default.EfectivenessWarningEnabled = cbEfectivenessWarning.Checked;
            Settings.Default.Save();
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            textBox1.Text = Settings.Default.LogPath;
            textBoxHoursPerDay.Text = string.Format("{0}", Settings.Default.HoursPerDay);
            textBoxJiraUrl.Text = Settings.Default.JiraUrl;
            textBoxJiraUser.Text = Settings.Default.JiraUser;
            textBoxJiraPass.Text = Settings.Default.JiraPass;
            textBoxJiraJQL.Text = Settings.Default.JiraJQL;
            inpScreenShotEnabled.Checked = Settings.Default.ScreenShotsEnabled;
            inpScreenShotTimeout.Text = string.Format("{0}", Settings.Default.ScreenShotsTimeout);
            inpScreenShotDir.Text = Settings.Default.ScreenShotsDir;
            checkBoxAutostart.Checked = AutoStarter.IsAutoStartEnabled;
            cbEfectivenessWarning.Checked = Settings.Default.EfectivenessWarningEnabled;
        }
    }
}