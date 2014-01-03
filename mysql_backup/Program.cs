using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using DatabaseConnection.Databases;
using BackupEngine;

namespace mysql_backup
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            bool isNewInstance = true;
            
            Mutex m = new Mutex(true, "MySQL Backup Michal Dabski", out isNewInstance);

            if (isNewInstance)
            {
                ProgramSettings.startMinimized = false;
                foreach (string val in args)
                {
                    switch (val)
                    {
                        case "-startup":
                            ProgramSettings.startMinimized = true;
                            //MessageBox.Show("Application will be minimized");
                            break;
                    }
                }
                EventLog.LogEvent("Application started", true);
                ProgramSettings.loadSettings();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                MainForm MainForm = new MainForm();
                Application.Run(MainForm);

                EventLog.LogEvent("Application stopped.", true);
            }
            else
            {
                EventLog.LogEvent("Application started, but another instance detected.", true);
            }
        }
    }
}
