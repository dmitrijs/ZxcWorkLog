using System;
using System.Drawing;
using System.Windows.Forms;
using ZxcWorkLog.Data;
using ZxcWorkLog.Jira;
using ZxcWorkLog.Util;

namespace ZxcWorkLog
{
    public partial class WorkItemAdd : Form
    {
        private readonly FormMain _formMain;

        public WorkItemAdd(FormMain formMain)
        {
            _formMain = formMain;
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var wi = WorkLog.addNewWorkItem(comboBox1.Text, textBox1.Text, inpGroup.Text);

            _formMain.LoadWorkItems();
            if (checkBox1.Checked)
            {
                _formMain.StartProgess(wi);
            }
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WorkItemAdd_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (var listViewItem in _formMain.getOrderedItems())
            {
                var item = (WorkItem) listViewItem;
                comboBox1.Items.Add(TextUtil.GetFirstLine(item.Title));
            }

            var workDate = DateTime.Now.Hour < 10 ? DateTime.Now.AddDays(-1) : DateTime.Now;
            inpGroup.Text = workDate.ToString("yyyy.MM.dd");

            foreach (ListViewGroup group in _formMain.getItemGroups())
            {
                inpGroup.Items.Add(group.Name);
            }
            User32.ShowWindowAsync(Handle, 1);
        }

        private void WorkItemAdd_Shown(object sender, EventArgs e)
        {
            BringToFront();
            comboBox1.Focus();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            var originalText = btnUpdate.Text;
            btnUpdate.Text = @"wait...";
            Application.DoEvents();

            comboBox1.Items.Clear();

            var client = new JiraSoapServiceClient();

            var token = client.login(Common.JiraUser, Common.JiraPass);

            var issues = client.getIssuesFromJqlSearch(token, Common.JiraJQL, 100);

            foreach (var issue in issues)
            {
                comboBox1.Items.Add(issue.key + " " + issue.summary);
            }
            btnUpdate.Text = originalText;

            comboBox1.DroppedDown = true;
        }

        private void Button3_Click_1(object sender, EventArgs e)
        {
            inpGroup.Text = DateTime.Now.AddDays(-1).ToString("yyyy.MM.dd");
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            inpGroup.Text = DateTime.Now.ToString("yyyy.MM.dd");
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void InpGroup_TextChanged(object sender, EventArgs e)
        {
            buttonToday.Font = new Font(buttonToday.Font,
                (DateTime.Now.ToString("yyyy.MM.dd") == inpGroup.Text) ? FontStyle.Bold : FontStyle.Regular);
            buttonYesterday.Font = new Font(buttonYesterday.Font,
                (DateTime.Now.AddDays(-1).ToString("yyyy.MM.dd") == inpGroup.Text) ? FontStyle.Bold : FontStyle.Regular);
        }
    }
}