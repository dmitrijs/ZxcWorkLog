using System;
using System.Drawing;
using System.Windows.Forms;

namespace ZxcWorkLog
{
    public partial class WorkItemAdd : Form
    {
        private readonly FormMain formMain = null;
        public WorkItemAdd(FormMain formMain)
        {
            this.formMain = formMain;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WorkItem wi = Common.addNewWorkItem(comboBox1.Text, textBox1.Text, inpGroup.Text);

            this.formMain.loadWorkItems();
            if (checkBox1.Checked)
            {
                this.formMain.startProgess(wi);
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WorkItemAdd_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (WorkItem item in formMain.getOrderedItems())
            {
                comboBox1.Items.Add(Common.getFirstLine(item.Title));
            } 
            
            var workDate = DateTime.Now.Hour < 10 ? DateTime.Now.AddDays(-1) : DateTime.Now;
            inpGroup.Text = workDate.ToString("yyyy.MM.dd");

            foreach (ListViewGroup group in formMain.getItemGroups())
            {
                inpGroup.Items.Add(group.Name);
            }
            Common.ShowWindowAsync(Handle, 1);
        }

        private void WorkItemAdd_Shown(object sender, EventArgs e)
        {
            this.BringToFront();
            this.comboBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var originalText = btnUpdate.Text;
            btnUpdate.Text = @"wait...";
            Application.DoEvents();

            comboBox1.Items.Clear();

            var client = new Jira.JiraSoapServiceClient();

            string token = client.login(Common.JiraUser, Common.JiraPass);

            Jira.RemoteIssue[] issues = client.getIssuesFromJqlSearch(token, Common.JiraJQL, 100);

            foreach (Jira.RemoteIssue issue in issues)
            {
                comboBox1.Items.Add(issue.key + " " + issue.summary);
            }
            btnUpdate.Text = originalText;

            comboBox1.DroppedDown = true;
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            inpGroup.Text = DateTime.Now.AddDays(-1).ToString("yyyy.MM.dd");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            inpGroup.Text = DateTime.Now.ToString("yyyy.MM.dd");
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void inpGroup_TextUpdate(object sender, EventArgs e)
        {
        }

        private void inpGroup_TextChanged(object sender, EventArgs e)
        {
            buttonToday.Font = new Font(buttonToday.Font, (DateTime.Now.ToString("yyyy.MM.dd") == inpGroup.Text) ? FontStyle.Bold : FontStyle.Regular);
            buttonYesterday.Font = new Font(buttonYesterday.Font, (DateTime.Now.AddDays(-1).ToString("yyyy.MM.dd") == inpGroup.Text) ? FontStyle.Bold : FontStyle.Regular);
        }
    }
}
