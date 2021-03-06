﻿namespace ZxcWorkLog
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbEfectivenessWarning = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxHoursPerDay = new System.Windows.Forms.TextBox();
            this.checkBoxAutostart = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxJiraUrl = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxJiraJQL = new System.Windows.Forms.TextBox();
            this.textBoxJiraPass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxJiraUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.inpScreenShotDir = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.inpScreenShotTimeout = new System.Windows.Forms.TextBox();
            this.inpScreenShotEnabled = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.sessionMinutesUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sessionMinutesUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(91, 393);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(202, 393);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sessionMinutesUpDown);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cbEfectivenessWarning);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBoxHoursPerDay);
            this.groupBox1.Controls.Add(this.checkBoxAutostart);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(9, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(385, 109);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // cbEfectivenessWarning
            // 
            this.cbEfectivenessWarning.AutoSize = true;
            this.cbEfectivenessWarning.Location = new System.Drawing.Point(184, 64);
            this.cbEfectivenessWarning.Name = "cbEfectivenessWarning";
            this.cbEfectivenessWarning.Size = new System.Drawing.Size(167, 17);
            this.cbEfectivenessWarning.TabIndex = 16;
            this.cbEfectivenessWarning.Text = "Warn about low effectiveness";
            this.cbEfectivenessWarning.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Working hours per day:";
            // 
            // textBoxHoursPerDay
            // 
            this.textBoxHoursPerDay.Location = new System.Drawing.Point(129, 62);
            this.textBoxHoursPerDay.Name = "textBoxHoursPerDay";
            this.textBoxHoursPerDay.Size = new System.Drawing.Size(42, 20);
            this.textBoxHoursPerDay.TabIndex = 14;
            this.textBoxHoursPerDay.Text = "8";
            // 
            // checkBoxAutostart
            // 
            this.checkBoxAutostart.AutoSize = true;
            this.checkBoxAutostart.Location = new System.Drawing.Point(13, 86);
            this.checkBoxAutostart.Name = "checkBoxAutostart";
            this.checkBoxAutostart.Size = new System.Drawing.Size(68, 17);
            this.checkBoxAutostart.TabIndex = 13;
            this.checkBoxAutostart.Text = "Autostart";
            this.checkBoxAutostart.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(10, 38);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(367, 20);
            this.textBox1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Path to log file:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxJiraUrl);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxJiraJQL);
            this.groupBox2.Controls.Add(this.textBoxJiraPass);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxJiraUser);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 223);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(385, 164);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "JIRA";
            // 
            // textBoxJiraUrl
            // 
            this.textBoxJiraUrl.Location = new System.Drawing.Point(68, 19);
            this.textBoxJiraUrl.Name = "textBoxJiraUrl";
            this.textBoxJiraUrl.Size = new System.Drawing.Size(306, 20);
            this.textBoxJiraUrl.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Base Url:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "JQL to retrieve issues:";
            // 
            // textBoxJiraJQL
            // 
            this.textBoxJiraJQL.Location = new System.Drawing.Point(9, 89);
            this.textBoxJiraJQL.Multiline = true;
            this.textBoxJiraJQL.Name = "textBoxJiraJQL";
            this.textBoxJiraJQL.Size = new System.Drawing.Size(368, 66);
            this.textBoxJiraJQL.TabIndex = 14;
            this.textBoxJiraJQL.Text = "status in (Open, \"In Progress\", Reopened) AND \r\n((project = \"CF\" and component in" +
    " (\"Php Plugins\", \"CMS2\")) \r\nor project in (\"INT\", \"WEBP\")) and \r\nassignee in (\"n" +
    "otassigned\", \"ds\")";
            // 
            // textBoxJiraPass
            // 
            this.textBoxJiraPass.Location = new System.Drawing.Point(264, 45);
            this.textBoxJiraPass.Name = "textBoxJiraPass";
            this.textBoxJiraPass.PasswordChar = '*';
            this.textBoxJiraPass.Size = new System.Drawing.Size(110, 20);
            this.textBoxJiraPass.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(202, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Password:";
            // 
            // textBoxJiraUser
            // 
            this.textBoxJiraUser.Location = new System.Drawing.Point(68, 45);
            this.textBoxJiraUser.Name = "textBoxJiraUser";
            this.textBoxJiraUser.Size = new System.Drawing.Size(110, 20);
            this.textBoxJiraUser.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Username:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.inpScreenShotDir);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.inpScreenShotTimeout);
            this.groupBox3.Controls.Add(this.inpScreenShotEnabled);
            this.groupBox3.Location = new System.Drawing.Point(13, 127);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(381, 90);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Screen shots";
            // 
            // inpScreenShotDir
            // 
            this.inpScreenShotDir.Location = new System.Drawing.Point(6, 61);
            this.inpScreenShotDir.Name = "inpScreenShotDir";
            this.inpScreenShotDir.Size = new System.Drawing.Size(367, 20);
            this.inpScreenShotDir.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(197, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Directory where screen shots are saved:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(201, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "seconds";
            // 
            // inpScreenShotTimeout
            // 
            this.inpScreenShotTimeout.Location = new System.Drawing.Point(153, 18);
            this.inpScreenShotTimeout.Name = "inpScreenShotTimeout";
            this.inpScreenShotTimeout.Size = new System.Drawing.Size(42, 20);
            this.inpScreenShotTimeout.TabIndex = 1;
            this.inpScreenShotTimeout.Text = "30";
            // 
            // inpScreenShotEnabled
            // 
            this.inpScreenShotEnabled.AutoSize = true;
            this.inpScreenShotEnabled.Location = new System.Drawing.Point(9, 20);
            this.inpScreenShotEnabled.Name = "inpScreenShotEnabled";
            this.inpScreenShotEnabled.Size = new System.Drawing.Size(138, 17);
            this.inpScreenShotEnabled.TabIndex = 0;
            this.inpScreenShotEnabled.Text = "Take screen shot every";
            this.inpScreenShotEnabled.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(154, 87);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Minimum session length:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(331, 87);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "minutes";
            // 
            // sessionMinutesUpDown
            // 
            this.sessionMinutesUpDown.Location = new System.Drawing.Point(281, 83);
            this.sessionMinutesUpDown.Name = "sessionMinutesUpDown";
            this.sessionMinutesUpDown.Size = new System.Drawing.Size(44, 20);
            this.sessionMinutesUpDown.TabIndex = 5;
            // 
            // FormSettings
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(401, 427);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sessionMinutesUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxAutostart;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxJiraJQL;
        private System.Windows.Forms.TextBox textBoxJiraPass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxJiraUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox inpScreenShotDir;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox inpScreenShotTimeout;
        private System.Windows.Forms.CheckBox inpScreenShotEnabled;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxHoursPerDay;
        private System.Windows.Forms.TextBox textBoxJiraUrl;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbEfectivenessWarning;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown sessionMinutesUpDown;
    }
}