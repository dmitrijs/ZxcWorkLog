using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DWorkLog
{
    public partial class WorkItemEdit : Form
    {
        private FormMain formMain = null;
        private WorkItem editedItem = null;

        public WorkItemEdit(FormMain formMain, WorkItem editedItem)
        {
            this.formMain = formMain;
            this.editedItem = editedItem;
            InitializeComponent();

            this.textBox1.Text = editedItem.Title;
            this.checkBox1.Checked = editedItem.WasWorkLogged;
            this.textBox2.Text = Common.toReadableTime(editedItem.PeriodTicks);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.editedItem.Title = textBox1.Text;
            if (Common.toReadableTime(editedItem.PeriodTicks) != textBox2.Text)
            {
                if (this.editedItem.Title.IndexOf("*") != 0)
                {
                    this.editedItem.Title = "* " + this.editedItem.Title;
                }
            }
            this.editedItem.PeriodTicks = Common.fromReadableTime(textBox2.Text).Ticks;
            this.editedItem.WasWorkLogged = checkBox1.Checked;
            Common.updateItem(this.editedItem.ID, this.editedItem);
            
            this.formMain.loadWorkItems();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void WorkItemEdit_Load(object sender, EventArgs e)
        {
            labelGroup.Text = @"Group: " + editedItem.GroupName;
        }

        private void WorkItemEdit_Shown(object sender, EventArgs e)
        {
            if (this.editedItem.WasWorkLogged)
            {
                this.button1.Enabled = false;
            }
        }
    }
}
