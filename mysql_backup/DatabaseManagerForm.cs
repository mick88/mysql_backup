using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DatabaseConnection.Databases;

namespace mysql_backup
{
    public partial class DatabaseManagerForm : Form
    {
        DatabaseManager databaseManager;
        public DatabaseManagerForm(DatabaseManager databaseManager)
        {
            this.databaseManager = databaseManager;

            InitializeComponent();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(labelTxt.Text))
            {
                errorTxt.Text = "Missing server name";
                labelTxt.Focus();
                return;
            }

            if (string.IsNullOrEmpty(addressTxt.Text))
            {
                errorTxt.Text = "Missing server address";
                addressTxt.Focus();
                return;
            }

            if (string.IsNullOrEmpty(usernameTxt.Text))
            {
                errorTxt.Text = "Missing username";
                usernameTxt.Focus();
                return;
            }

            Database database = new Database(addressTxt.Text, databaseTxt.Text, usernameTxt.Text, passwordTxt.Text);
            databaseManager.AddDatabase(database);

            populateList();
            updateButtons();
        }

        void populateList()
        {
            dbList.Items.Clear();
            foreach (Database database in databaseManager.databases)
            {
                dbList.Items.Add(database.ToString());
            }
        }

        private void addServerFrm_Load(object sender, EventArgs e)
        {
            errorTxt.Text = "";
            dbList.Items.Clear();
            populateList();

            autoBackupHr.Items.Clear();
            for (int i = 0; i < 24; i++)
            {
                autoBackupHr.Items.Add(i.ToString()+":00");
            }
            autoBackupHr.SelectedIndex = 7;
            autoBackupHr.Enabled = false;
        }

        private void usernameTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            databaseManager.RemoveDatabase(SelectedDatabase());

            populateList();
            updateButtons();
        }

        void populateFields(Database database)
        {
            if (database == null) return;
            labelTxt.Text = database.Label;
            addressTxt.Text = database.Address;
            databaseTxt.Text = database.DatabaseName;
            usernameTxt.Text = database.Username;
            passwordTxt.Text = database.Password;
            autoBackupChk.Checked = false;
            //autoBackupHr.SelectedIndex = server.backupHour;
        }

        private void dbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SavedServers.ServerDetails server = getServerByName(dbList.Text);
            Database selectedDb = databaseManager.GetDatabase(dbList.SelectedIndex);
            populateFields(selectedDb);
        }

        Database SelectedDatabase()
        {
            return databaseManager.GetDatabase(dbList.SelectedIndex);
        }

        void updateButtons()
        {
            //updateBtn.Enabled = SavedServers.isInServerList(labelTxt.Text);
            //okBtn.Enabled = !SavedServers.isInServerList(labelTxt.Text);
        }
        private void labelTxt_TextChanged(object sender, EventArgs e)
        {
            updateButtons();
        }

        private void autoBackupChk_CheckedChanged(object sender, EventArgs e)
        {
            autoBackupHr.Enabled = autoBackupChk.Checked;
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            /*int id = SavedServers.getServerIdByName(labelTxt.Text);
            Database database;

            try
            {
                if (id == -1)
                {
                    MessageBox.Show("ERROR: Database does not exist in the list!");
                    return;
                }
               // database = SavedServers.serverList[id];
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return;
            }

            if (string.IsNullOrEmpty(labelTxt.Text))
            {
                errorTxt.Text = "Missing server name";
                labelTxt.Focus();
                return;
            }

            if (string.IsNullOrEmpty(addressTxt.Text))
            {
                errorTxt.Text = "Missing server address";
                addressTxt.Focus();
                return;
            }
            if (string.IsNullOrEmpty(usernameTxt.Text))
            {
                errorTxt.Text = "Missing username";
                usernameTxt.Focus();
                return;
            }
            
            database.serverAddress = addressTxt.Text;
            database.serverDatabase = databaseTxt.Text;
            database.serverPassword = passwordTxt.Text;
            database.serverUsername = usernameTxt.Text;
            database.autoBackup = autoBackupChk.Checked;
            database.backupHour = autoBackupHr.SelectedIndex;
            database.updateNextBackup();

            //SavedServers.serverList[id] = database;
            SavedServers.saveServers();
            */
        }
    }
}
