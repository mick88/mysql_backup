using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseConnection.Databases;
using BackupEngine.Files;
using BackupEngine;
using BackupEngine.Interfaces;
using System.IO;

namespace DatabaseClient.Routines
{
    class Restorer : RoutineBase, IRestoreListener, IDisposable
    {
        private const string ERROR_FILE_PATH = "errors.sql";

        StreamWriter errorFile = null;

        public Restorer(DatabaseManager databaseManager, Database database)
            : base(databaseManager, database)
        {
        }

        public override RoutineBase Run()
        {
            BackupPreferences preferences = new BackupPreferences();
            DirectoryInfo dir = new DirectoryInfo(preferences.backupPath);
            string pattern = ConsoleEx.GetInput("Enter filename pattern: ");
            FileInfo[] files = dir.GetFiles(pattern);
            MultiSqlFileReader sqlFile = new MultiSqlFileReader(preferences.backupPath, files);
            Console.WriteLine("Files will be uploaded in this order: ");
            Console.WriteLine(sqlFile);

            if (ConsoleEx.PromptYn("Continue?"))
            {
                Console.WriteLine("Restoring...");
                Restore restore = new Restore(selectedDatabase)
                    .SetRestoreListener(this)
                    .SetSqlFile(sqlFile)
                    .Run();
                CloseErrorFile();
            }
            return base.Run();
        }

        bool IRestoreListener.OnQueryError(BackupEngine.Restore restore, string query, string errorMessage)
        {
            OpenErrorFile();
            errorFile.WriteLine("-- "+errorMessage);
            errorFile.WriteLine(query);
            errorFile.WriteLine(string.Format("-- Line ~{0} in {1}", restore.SqlFile.LinesRead, restore.SqlFile.CurrentFileName));
            errorFile.WriteLine();

            ConsoleEx.WriteError("Error while executing query "+query+": "+errorMessage);
            return ConsoleEx.PromptYn("Retry?");
        }

        bool IRestoreListener.OnConnectionError(BackupEngine.Restore restore, DatabaseConnection.Connection connection, string errorMessage)
        {
            ConsoleEx.WriteError("Fatal error while connecting: "+errorMessage);
            return ConsoleEx.PromptYn("Retry?");
        }

        void IRestoreListener.OnRestoreComplete(BackupEngine.Restore restore)
        {
            Console.WriteLine("Database succesfully restored!");
        }

        private void OpenErrorFile()
        {
            if (errorFile == null)
            {
                errorFile = new StreamWriter(ERROR_FILE_PATH);
            }
        }

        private void CloseErrorFile()
        {
            if (errorFile != null)
            {
                errorFile.Flush();
                errorFile.Close();
                errorFile = null;
            }
        }

        public void Dispose()
        {
            CloseErrorFile();
        }
    }
}
