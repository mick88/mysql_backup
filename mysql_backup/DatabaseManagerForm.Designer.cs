namespace mysql_backup
{
    partial class DatabaseManagerForm
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
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.addressTxt = new System.Windows.Forms.TextBox();
            this.usernameTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.passwordTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.databaseTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.errorTxt = new System.Windows.Forms.Label();
            this.labelTxt = new System.Windows.Forms.TextBox();
            this.dbList = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.autoBackupChk = new System.Windows.Forms.CheckBox();
            this.autoBackupHr = new System.Windows.Forms.ComboBox();
            this.updateBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // okBtn
            // 
            this.okBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.okBtn.Location = new System.Drawing.Point(24, 192);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 5;
            this.okBtn.Text = "Add new";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(319, 192);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 6;
            this.cancelBtn.Text = "Close";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Address:";
            // 
            // addressTxt
            // 
            this.addressTxt.Location = new System.Drawing.Point(77, 38);
            this.addressTxt.MaxLength = 100;
            this.addressTxt.Name = "addressTxt";
            this.addressTxt.Size = new System.Drawing.Size(128, 20);
            this.addressTxt.TabIndex = 1;
            // 
            // usernameTxt
            // 
            this.usernameTxt.Location = new System.Drawing.Point(77, 64);
            this.usernameTxt.MaxLength = 100;
            this.usernameTxt.Name = "usernameTxt";
            this.usernameTxt.Size = new System.Drawing.Size(128, 20);
            this.usernameTxt.TabIndex = 2;
            this.usernameTxt.TextChanged += new System.EventHandler(this.usernameTxt_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Username:";
            // 
            // passwordTxt
            // 
            this.passwordTxt.Location = new System.Drawing.Point(77, 90);
            this.passwordTxt.MaxLength = 100;
            this.passwordTxt.Name = "passwordTxt";
            this.passwordTxt.Size = new System.Drawing.Size(128, 20);
            this.passwordTxt.TabIndex = 3;
            this.passwordTxt.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Password:";
            // 
            // databaseTxt
            // 
            this.databaseTxt.Location = new System.Drawing.Point(77, 116);
            this.databaseTxt.MaxLength = 100;
            this.databaseTxt.Name = "databaseTxt";
            this.databaseTxt.Size = new System.Drawing.Size(128, 20);
            this.databaseTxt.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Database:";
            // 
            // errorTxt
            // 
            this.errorTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.errorTxt.Location = new System.Drawing.Point(12, 172);
            this.errorTxt.Name = "errorTxt";
            this.errorTxt.Size = new System.Drawing.Size(195, 13);
            this.errorTxt.TabIndex = 10;
            this.errorTxt.Text = "ERROR";
            // 
            // labelTxt
            // 
            this.labelTxt.Location = new System.Drawing.Point(77, 12);
            this.labelTxt.MaxLength = 16;
            this.labelTxt.Name = "labelTxt";
            this.labelTxt.Size = new System.Drawing.Size(128, 20);
            this.labelTxt.TabIndex = 0;
            this.labelTxt.TextChanged += new System.EventHandler(this.labelTxt_TextChanged);
            // 
            // dbList
            // 
            this.dbList.FormattingEnabled = true;
            this.dbList.Location = new System.Drawing.Point(211, 12);
            this.dbList.Name = "dbList";
            this.dbList.Size = new System.Drawing.Size(183, 173);
            this.dbList.TabIndex = 11;
            this.dbList.SelectedIndexChanged += new System.EventHandler(this.dbList_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(211, 191);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Delete selected";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Label:";
            // 
            // autoBackupChk
            // 
            this.autoBackupChk.AutoSize = true;
            this.autoBackupChk.Enabled = false;
            this.autoBackupChk.Location = new System.Drawing.Point(15, 144);
            this.autoBackupChk.Name = "autoBackupChk";
            this.autoBackupChk.Size = new System.Drawing.Size(101, 17);
            this.autoBackupChk.TabIndex = 14;
            this.autoBackupChk.Text = "auto backup at:";
            this.autoBackupChk.UseVisualStyleBackColor = true;
            this.autoBackupChk.CheckedChanged += new System.EventHandler(this.autoBackupChk_CheckedChanged);
            // 
            // autoBackupHr
            // 
            this.autoBackupHr.AllowDrop = true;
            this.autoBackupHr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoBackupHr.FormattingEnabled = true;
            this.autoBackupHr.Location = new System.Drawing.Point(120, 142);
            this.autoBackupHr.Name = "autoBackupHr";
            this.autoBackupHr.Size = new System.Drawing.Size(85, 21);
            this.autoBackupHr.TabIndex = 15;
            // 
            // updateBtn
            // 
            this.updateBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.updateBtn.Enabled = false;
            this.updateBtn.Location = new System.Drawing.Point(120, 192);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(75, 23);
            this.updateBtn.TabIndex = 16;
            this.updateBtn.Text = "Update";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.updateBtn_Click);
            // 
            // DatabaseManagerForm
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(406, 227);
            this.Controls.Add(this.updateBtn);
            this.Controls.Add(this.autoBackupHr);
            this.Controls.Add(this.autoBackupChk);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dbList);
            this.Controls.Add(this.labelTxt);
            this.Controls.Add(this.errorTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.databaseTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.passwordTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.usernameTxt);
            this.Controls.Add(this.addressTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseManagerForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage databases";
            this.Load += new System.EventHandler(this.addServerFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox addressTxt;
        private System.Windows.Forms.TextBox usernameTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox passwordTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox databaseTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label errorTxt;
        private System.Windows.Forms.TextBox labelTxt;
        private System.Windows.Forms.ListBox dbList;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox autoBackupChk;
        private System.Windows.Forms.ComboBox autoBackupHr;
        private System.Windows.Forms.Button updateBtn;
    }
}