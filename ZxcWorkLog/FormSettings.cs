using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZxcWorkLog
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Common.settingsUpdate(textBox1.Text, textBoxJiraUser.Text, textBoxJiraPass.Text, textBoxJiraJQL.Text, inpScreenShotEnabled.Checked, Int32.Parse(inpScreenShotTimeout.Text), inpScreenShotDir.Text);
            if (checkBoxAutostart.Checked)
            {
                Util.AutoStarter.SetAutoStart();
            }
            else
            {
                Util.AutoStarter.UnSetAutoStart();
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            textBox1.Text = Common.getLogPath();
            textBoxJiraUser.Text = Common.JiraUser;
            textBoxJiraPass.Text = Common.JiraPass;
            textBoxJiraJQL.Text = Common.JiraJQL;
            inpScreenShotEnabled.Checked = Common.ScreenShotsEnabled;
            inpScreenShotTimeout.Text = Common.ScreenShotsTimeout.ToString();
            inpScreenShotDir.Text = Common.ScreenShotsDir;
            checkBoxAutostart.Checked = Util.AutoStarter.IsAutoStartEnabled;
        }
    }
}
