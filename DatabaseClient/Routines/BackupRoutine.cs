using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseConnection.Databases;
using BackupEngine;
using BackupEngine.Interfaces;

namespace DatabaseClient.Routines
{
    class BackupRoutine : RoutineBase, IStatusListener
    {
        const string BACKUP_PREF_FILE = "preferences.dat";
        const int PROGRESS_SIZE = 50;
        Backup backup = null;
        BackupPreferences preferences;

        public BackupRoutine(DatabaseManager databaseManager, Database database)
            : base(databaseManager, database)
        {
            preferences = new BackupPreferences(BACKUP_PREF_FILE);
            preferences.MaxFileSizeMB = 45;
            preferences.Save(BACKUP_PREF_FILE);
        }

        public override RoutineBase Run()
        {
            try
            {
                backup = new Backup(selectedDatabase, preferences)
                    .setStatusListener(this);
                backup.Run();
            }
            catch (Exception e)
            {
                ConsoleEx.WriteError("Backup failed: " + e.Message);
            }
            return base.Run();
        }

        protected override void OnTerminate(object sender, ConsoleCancelEventArgs args)
        {
            if (backup != null)
            {
                backup.Cancel();
            }
            base.OnTerminate(sender, args);
        }

        void IStatusListener.OnBackupComplete(Backup backup)
        {
            Console.WriteLine();
            Console.WriteLine("Backup complete!");
        }

        void IStatusListener.OnBackupError(Backup backup, string message)
        {
            ConsoleEx.WriteError(message);
        }

        void IStatusListener.OnBackupStarted(Backup backup)
        {
            Console.WriteLine("Backing up "+backup.Database);
            Console.WriteLine("Preparing tables");
        }

        void IStatusListener.OnBackupWarning(Backup backup, string message)
        {
            ConsoleEx.WriteWarning(message);
        }

        void IStatusListener.OnNewTable(Backup backup, Table table)
        {
            if ((backup.Status & Backup.WorkStatus.GettingData) == Backup.WorkStatus.GettingData)
            {
                Console.WriteLine();
                Console.WriteLine(table);
            }
            else if ((backup.Status & Backup.WorkStatus.GettingTables) == Backup.WorkStatus.GettingTables)
            {
                ConsoleEx.DrawProgressBar((float)(backup.currentTable + 1) / backup.totalTables, PROGRESS_SIZE);
            }
        }

        void IStatusListener.OnTableProgressUpdate(Backup backup, int currentRow, int numRows)
        {
            ConsoleEx.DrawProgressBar((float)currentRow / numRows, PROGRESS_SIZE);
        }

        void IStatusListener.OnTotalProgressUpdate(Backup backup, int currentTable, int numTables)
        {
        }

        void IStatusListener.onStatusChanged(Backup backup, Backup.WorkStatus status)
        {
            if (backup.IsStatus(Backup.WorkStatus.Packing))
            {
                Console.WriteLine();
                Console.WriteLine("Zipping "+backup.OutputFile.FileNames.Count+" files into"+backup.OutputFile.BaseFileName);
            }
        }


        void IStatusListener.OnTableFinished(Backup backup, Table table)
        {
            
        }
    }
}
