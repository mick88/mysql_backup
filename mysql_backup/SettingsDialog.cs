using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace mysql_backup
{
    public partial class SettingsDialog : Form
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {

            if (!Directory.Exists(backupPathTxt.Text))
            {
                try
                {
                    Directory.CreateDirectory(backupPathTxt.Text);
                }
                catch
                {
                    MessageBox.Show("Selected directory does not exist and could not be created!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            int size;
            try
            {
                size = int.Parse(maxSizeTxt.Text);
            }
            catch
            {
                MessageBox.Show("Invalid max filesize!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (size < 1)
            {
                MessageBox.Show("Max filesize cannot be less than 1 MB!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ProgramSettings.MaxFileSize = size * 1024 * 1024;
            ProgramSettings.backupPath = backupPathTxt.Text;

            RegistryAccess.setStartup(startWithSystemChk.CheckState);

            ProgramSettings.saveSettings();
            Close();
            //cancelBtn_Click(sender, e);
        }

        private void backupPathTxt_TextChanged(object sender, EventArgs e)
        {
            if (!Directory.Exists(backupPathTxt.Text)) backupPathTxt.ForeColor = Color.Red;
            else backupPathTxt.ForeColor = SystemColors.WindowText;
        }

        private void backupPathTxt_Click(object sender, EventArgs e)
        {
            
        }

        private void SettingsDialog_Load(object sender, EventArgs e)
        {
            backupPathTxt.Text = ProgramSettings.backupPath;
            maxSizeTxt.Text = (ProgramSettings.MaxFileSize / 1024 / 1024).ToString();

            CheckState state;
            RegistryAccess.checkStartup(out state);
            startWithSystemChk.CheckState = state;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                backupPathTxt.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void startWithSystemChk_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
