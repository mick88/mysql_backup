using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using BackupEngine;
using DatabaseConnection.Databases;
using mysql_backup.Backups;
using BackupEngine.Interfaces;

namespace mysql_backup
{
    public partial class MainForm : Form, IManagerListener
    {
        private bool isFormVisible;

        /// <summary>
        ///  Current running backup
        /// </summary>
        Backup backup=null;

        BackupManager backupManager;
        DatabaseManager databaseManager;       

        public MainForm()
        {
            InitializeComponent();
            backupManager = new BackupManager()
                .SetManagerListener(this);
            databaseManager = new DatabaseManager()
                .LoadDatabases();
        }

        public delegate void updateStatusDelegate(string text);
        public delegate void updateProgressDelegate(int val = -1, int itemVal = -1);
        delegate void setBackupRunningDelegate(bool isRunning);
        delegate void setStopButtonDelegate(bool enabled);

        public void updateStatus(string text)
        {
            if (statusLbl.InvokeRequired == true)
            {
                updateStatusDelegate d = new updateStatusDelegate(updateStatus);
                Invoke(d, new object[] { text });
                return;
            }
            statusLbl.Text = text;
        }
        
        /// <summary>
        /// Sets progressbars
        /// </summary>
        /// <param name="totalProgress">Total progress</param>
        /// <param name="detailProgress">Progress of current table</param>
        public void updateProgress(int totalProgress=-1, int detailProgress=-1)
        {
            if (progress.InvokeRequired == true)
            {
                updateProgressDelegate d = new updateProgressDelegate(updateProgress);
                Invoke(d, new object[] { totalProgress, detailProgress }); 
                return;
            }
            if (totalProgress >= 0 && totalProgress <= progress.Maximum) progress.Value = totalProgress;
            if (detailProgress >= 0 && detailProgress <= itemProgress.Maximum) itemProgress.Value = detailProgress;
        }
        
        public void setCaption(string text="")
        {
            string caption;
            if (string.IsNullOrEmpty(text)) caption = ProgramSettings.programTitle;
            else caption = string.Format("{0}: {1}", ProgramSettings.programTitle, text);

            this.Text = caption;
            notifyIcon1.Text = caption;
        }
        
        public void setBackupRunning(bool isRunning)
        {
            if (startButton.InvokeRequired == true)
            {
                setBackupRunningDelegate d = new setBackupRunningDelegate(setBackupRunning);
                Invoke(d, new object[] { isRunning });
                return;
            }

            startButton.Enabled = !isRunning;
            databaseDropDown.Enabled = !isRunning;
            AddServerBtn.Enabled = !isRunning;
            stopBtn.Enabled = isRunning;

            if (isRunning == false)
            {
                itemProgress.Value = 0;
                progress.Value = 0;
                setCaption();
            }
            else
            {
                setCaption("backup in progress");                
            }
        }

        public void setStopButton(bool enabled)
        {
            if (stopBtn.InvokeRequired == true)
            {
                setStopButtonDelegate d = new setStopButtonDelegate(setStopButton);
                Invoke(d, new object[] { enabled });
            }
            else
            {
                stopBtn.Enabled = enabled;
            }
        }

        delegate void setMaxProgressDelegate(int maxValue);
        public void setMaxProgress(int maxValue)
        {
            if (progress.InvokeRequired == true)
            {
                setMaxProgressDelegate d = new setMaxProgressDelegate(setMaxProgress);
                Invoke(d, new object[] { maxValue });
            }
            else
            {
                progress.Maximum = maxValue;
            }
        }

        delegate void setMaxItemProgressDelegate(int maxValue);
        public void setMaxItemProgress(int maxValue)
        {
            if (progress.InvokeRequired == true)
            {
                setMaxItemProgressDelegate d = new setMaxItemProgressDelegate(setMaxItemProgress);
                Invoke(d, new object[] { maxValue });
            }
            else
            {
                itemProgress.Maximum = maxValue;
            }
        }

        private void OnStartClicked(object sender, EventArgs e)
        {
            if (databaseDropDown.SelectedIndex == -1)
            {
                MessageBox.Show("Select a server first.");
                return;
            }
            Database database = databaseManager.GetDatabase(databaseDropDown.SelectedIndex);

            if (database == null)
            {
                MessageBox.Show("Selected server is not valid", "ERROR!");
                return;
            }

            backup = backupManager.StartBackup(database);
        }

        private void OnStopClicked(object sender, EventArgs e)
        {
            backup.Cancel();
            stopBtn.Enabled = false;
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            new DatabaseManagerForm(databaseManager).ShowDialog();
            //newAddSrv.ShowDialog();
            ReloadDatabaseDropdown();
        }

        private void selectServer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void ReloadDatabaseDropdown()
        {
            var list = databaseManager.databases;
            databaseDropDown.Items.Clear();
            int selected = databaseDropDown.SelectedIndex;
            foreach (Database database in list)
            {
                int id = databaseDropDown.Items.Add(database.ToString());
            }
            if (databaseDropDown.Items.Count > selected && selected > -1) databaseDropDown.SelectedIndex = selected;
            else if (databaseDropDown.Items.Count > 0) databaseDropDown.SelectedIndex = 0;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            setCaption();
            autoBackupTimer.Interval = ProgramSettings.autoBackupInterval;
            ReloadDatabaseDropdown();
            if (databaseManager.databases.Count > 0) databaseDropDown.SelectedIndex = 0;
        }

        bool IsBackupRunning(bool includeBackground)
        {
            if (includeBackground) return backupManager.IsBackupRunning;
            else return (backup != null || backup.IsStatus(Backup.WorkStatus.Finished));
        }

        DialogResult CloseConfirm()
        {
            if (IsBackupRunning(true))
            {
                MessageBox.Show("Cannot close application while backup is being made\nCancel the process before exiting.", "Backup in progress", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return System.Windows.Forms.DialogResult.No;
            }
            return MessageBox.Show("You may have scheduled backups. Backups will not work if application is not running!", "Close?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult dr = CloseConfirm();
                if (dr != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void trayIconMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            btnManage_Click(sender, e);
        }

        private void mainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                toggleFormVisible(false);                
            }
        }

        void toggleFormVisible(bool visible)
        {
            if (visible == false)
            {
                this.Hide();
                this.WindowState = FormWindowState.Minimized;                
            }
            else
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            this.Visible = visible;
            this.ShowInTaskbar = visible;
            notifyIcon1.Visible = !visible;
            isFormVisible = visible;
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                toggleFormVisible(!isFormVisible);
                notifyIcon1.Visible = false;
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleFormVisible(!isFormVisible);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = CloseConfirm();
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
            
        }

        private void autoBackupTimer_Tick(object sender, EventArgs e)
        {
            /*foreach (SavedServers.ServerDetails server in SavedServers.serverList)
            {
                if (server.autoBackup == true && server.inUse == false)
                {
                    if (server.nextBackupTime <= DateTime.Now)
                    {
                        try
                        {
                            EventLog.LogEvent("Automatic backup of database " + server.serverName+":");
                            notifyIcon1.ShowBalloonTip(4, "Automatic backup", "Automatic backup commenced for database " + server.serverName, ToolTipIcon.Info);

                            LegacyBackup autoBackup = new LegacyBackup(server);
                            backgroundBackups.Add(autoBackup);
                            autoBackup.start();
                        }
                        catch
                        {
                        }
                    }
                }
            }*/
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsDialog sd = new SettingsDialog();
            sd.ShowDialog();
        }

        private void settingsBtn_Click(object sender, EventArgs e)
        {
            SettingsDialog sd = new SettingsDialog();
            DialogResult result = sd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ProgramSettings.saveSettings();
            }
        }

        private void openBackupFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = ProgramSettings.backupPath;
            
            if (Path.IsPathRooted(path) == false)
            {
                path = Path.Combine(Environment.CurrentDirectory, path);
            }
             
            try
            {
                Process.Start(path);
            }
            catch
            {
                EventLog.LogEvent("Path " + path + " could not be open.", true);
            }
        }

        private void mainForm_Shown(object sender, EventArgs e)
        {
            if (ProgramSettings.startMinimized)
            {
                ProgramSettings.startMinimized = false;
                toggleFormVisible(false);
            }
            else toggleFormVisible(true);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var box = new AboutBox();
            box.ShowDialog(this);
        }

        private void statusLbl_Click(object sender, EventArgs e)
        {

        }

        void IManagerListener.OnAllBackupsComplete(BackupManager backupManager)
        {
            
        }

        void IManagerListener.OnBackupAdded(BackupManager backupManager, Backup backup)
        {
            
        }

        void IStatusListener.OnBackupStarted(Backup backup)
        {
            if (backup == this.backup) setBackupRunning(true);
        }

        void IStatusListener.OnNewTable(Backup backup, Table table)
        {
            if (this.backup == backup)
            {
                if (backup.IsStatus(Backup.WorkStatus.GettingTables)) updateStatus("Indexing table "+table);
                else updateStatus("Downloading table " + table);
            }
        }

        void IStatusListener.OnTableProgressUpdate(Backup backup, int currentRow, int numRows)
        {
            if (backup == this.backup)
            {
                setMaxItemProgress(numRows);
                updateProgress(detailProgress: currentRow);
            }
        }

        void IStatusListener.OnTotalProgressUpdate(Backup backup, int currentTable, int numTables)
        {
            if (backup == this.backup)
            {
                setMaxProgress(numTables);
                updateProgress(totalProgress: currentTable);
            }
        }

        void IStatusListener.OnBackupComplete(Backup backup)
        {
            if (backup == this.backup) setBackupRunning(false);
        }

        void IStatusListener.OnBackupError(Backup backup, string message)
        {
            if (backup == this.backup) setBackupRunning(false);
        }

        void IStatusListener.OnBackupWarning(Backup backup, string message)
        {
            
        }

        void IStatusListener.onStatusChanged(Backup backup, Backup.WorkStatus status)
        {
            if (this.backup == backup)
            {
                if (backup.IsStatus(Backup.WorkStatus.Connecting)) updateStatus("Connecting...");
                else if (backup.IsStatus(Backup.WorkStatus.Failed)) updateStatus("Failed!");
                else if (backup.IsStatus(Backup.WorkStatus.Finished)) updateStatus("Backup of "+backup.Database+" complete!");
                else if (backup.IsStatus(Backup.WorkStatus.Idle)) updateStatus("Ready");
                else if (backup.IsStatus(Backup.WorkStatus.Packing)) updateStatus("Zipping files...");
                else if (backup.IsStatus(Backup.WorkStatus.Working)) updateStatus("Working...");
            }
        }


        void IStatusListener.OnTableFinished(Backup backup, Table table)
        {
            
        }
    }
}
