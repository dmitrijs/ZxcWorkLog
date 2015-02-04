using System;
using System.Windows.Forms;
using ZxcWorkLog.Data;
using ZxcWorkLog.Util;

namespace ZxcWorkLog
{
    public partial class WorkItemEdit : Form
    {
        private readonly FormMain _formMain;
        private readonly WorkItem _editedItem;

        public WorkItemEdit(FormMain formMain, WorkItem editedItem)
        {
            _formMain = formMain;
            _editedItem = editedItem;
            InitializeComponent();

            textBox1.Text = editedItem.Title;
            checkBox1.Checked = editedItem.WasWorkLogged;
            checkBox2.Checked = editedItem.IsDistributed;
            textBox2.Text = TimeUtil.ToReadableTime(editedItem.PeriodTicks);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            _editedItem.Title = textBox1.Text;
            if (TimeUtil.ToReadableTime(_editedItem.PeriodTicks) != textBox2.Text)
            {
                if (_editedItem.Title.IndexOf("*", StringComparison.Ordinal) != 0)
                {
                    _editedItem.Title = "* " + _editedItem.Title;
                }
            }
            _editedItem.PeriodTicks = TimeUtil.FromReadableTime(textBox2.Text).Ticks;
            _editedItem.WasWorkLogged = checkBox1.Checked;
            _editedItem.IsDistributed = checkBox2.Checked;
            WorkLog.updateItem(_editedItem.Id, _editedItem);

            _formMain.LoadWorkItems();
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WorkItemEdit_Load(object sender, EventArgs e)
        {
            labelGroup.Text = @"Group: " + _editedItem.GroupName;
        }

        private void WorkItemEdit_Shown(object sender, EventArgs e)
        {
            if (_editedItem.WasWorkLogged)
            {
                button1.Enabled = false;
            }
        }
    }
}