using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseConnection.Databases;
using BackupEngine;
using DatabaseConnection;
using BackupEngine.Interfaces;

namespace DatabaseClient.Routines
{
    /// <summary>
    /// Handles commandline arguments
    /// </summary>
    class ArgProcess: RoutineBase, IStatusListener
    {
        string[] args;

        public ArgProcess(DatabaseManager databaseManager, string [] args)
            : base(databaseManager)
        {
            this.args = args;
        }

        private void SelectDatabase(Database database)
        {
            selectedDatabase = database;
            Console.WriteLine("Selected database: " + selectedDatabase);
        }

        private void ProcessArgument(string[] args, ref int i)
        {
            switch (args[i].ToLower())
            {
                case "?":
                case "-h":
                case "-help":
                    Console.WriteLine("DatabaseClient commands:");
                    Console.WriteLine("-help: This help message");
                    Console.WriteLine("-dbid [id]: Select database by id from manager (base 1)");
                    Console.WriteLine("-database [host] [database] [username] [password]: Select database manually");
                    Console.WriteLine("-dbadd [host] [database] [username] [password]: adds database to the manager and selects it");
                    Console.WriteLine("-query [query string] query database and print result");
                    Console.WriteLine("-backup: perform backup on selected database");
                    Console.WriteLine();
                    break;

                case "-db":
                case "-database":
                    SelectDatabase(new Database(args[++i], args[++i], args[++i], args[++i]));
                    break;

                case "-dbid":
                    SelectDatabase(databaseManager.GetDatabaseBase1(int.Parse(args[++i])));
                    break;

                case "-dbadd":
                    Database database = new Database(args[++i], args[++i], args[++i], args[++i]);
                    databaseManager.AddDatabase(database);
                    int id = databaseManager.GetDatabaseID(database);
                    Console.WriteLine("Database " + database + " added at position " + id);
                    SelectDatabase(database);
                    break;

                case "-backup":
                    Backup backup = new Backup(selectedDatabase).setStatusListener(this);
                    backup.Run();
                    break;

                case "-q":
                case "-query":
                    if (selectedDatabase == null)
                    {
                        ConsoleEx.WriteError("No database selected. Please refer to -help");
                    }
                    else
                    {
                        try
                        {
                            string query = args[++i];
                            ConsoleEx.WriteLineColor("Query:", ConsoleColor.DarkYellow);
                            Console.WriteLine(query);
                            Connection connection = new Connection(selectedDatabase)
                                .Connect();
                            QueryResult result = connection.Query(query);
                            connection.Disconnect();

                            ConsoleEx.WriteLineColor("Result:", ConsoleColor.DarkYellow);
                            Console.WriteLine(result);
                        }
                        catch (Exception e)
                        {
                            ConsoleEx.WriteError(e.Message);
                        }
                    }
                    break;

                default:
                    ConsoleEx.WriteWarning("Unknown argument \"" + args[i] + "\"");
                    break;
            }
        }

        public override RoutineBase Run()
        {
            for (int i = 0; i < args.Length; i++)
            {
                try
                {
                    ProcessArgument(args, ref i);
                }
                catch (Exception e)
                {
                    ConsoleEx.WriteError(e.Message);
                }
            }
            return this;
        }

        void IStatusListener.OnBackupStarted(Backup backup)
        {
            Console.WriteLine("Backing up "+backup.Database+" stared");
        }

        void IStatusListener.OnNewTable(Backup backup, Table table)
        {
            Console.WriteLine(table);
        }

        void IStatusListener.OnTableProgressUpdate(Backup backup, int currentRow, int numRows)
        {
        }

        void IStatusListener.OnTotalProgressUpdate(Backup backup, int currentTable, int numTables)
        {
        }

        void IStatusListener.OnBackupComplete(Backup backup)
        {
            Console.WriteLine("Database backed up successfully: "+backup.Database);
        }

        void IStatusListener.OnBackupError(Backup backup, string message)
        {
            ConsoleEx.WriteError(message);
        }

        void IStatusListener.OnBackupWarning(Backup backup, string message)
        {
            ConsoleEx.WriteWarning(message);
        }

        void IStatusListener.onStatusChanged(Backup backup, Backup.WorkStatus status)
        {
            ConsoleColor statusColor = ConsoleColor.DarkGray;
            if (backup.IsStatus(Backup.WorkStatus.Connecting)) ConsoleEx.WriteLineColor("Connecting...", statusColor);
            //else if (backup.IsStatus(Backup.WorkStatus.Finished)) WriteLineColor("Finished!", statusColor);
            else if (backup.IsStatus(Backup.WorkStatus.GettingTables)) ConsoleEx.WriteLineColor("Reading tables:", statusColor);
            else if (backup.IsStatus(Backup.WorkStatus.GettingData)) ConsoleEx.WriteLineColor("Downloading data:", statusColor);
            else if (backup.IsStatus(Backup.WorkStatus.Packing)) ConsoleEx.WriteLineColor("Zipping files...", statusColor);
        }


        void IStatusListener.OnTableFinished(Backup backup, Table table)
        {
            
        }
    }
}
