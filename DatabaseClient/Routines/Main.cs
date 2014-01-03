using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseConnection.Databases;
using BackupEngine.Files;

namespace DatabaseClient.Routines
{
    class Main:RoutineBase
    {
        public Main(DatabaseManager databaseManager)
            : base(databaseManager)
        {

        }

        public override RoutineBase Run()
        {

            bool finished = false;
            while (finished == false)
            {
                Console.Clear();

                Console.WriteLine("Select database:");
                for (int i = 0; i < databaseManager.databases.Count; i++)
                {
                    string line = String.Format("{0}) {1}", i+1, databaseManager.databases[i]);
                    if (databaseManager.databases[i] == selectedDatabase)
                    {
                        ConsoleEx.WriteLineColor(line, ConsoleColor.DarkYellow);
                    }
                    else Console.WriteLine(line);
                }
                Console.WriteLine();

                if (selectedDatabase != null)
                {
                    Console.WriteLine("(B)ackup: backup "+selectedDatabase);
                    Console.WriteLine("(Q)uery: query "+selectedDatabase);
                    Console.WriteLine("(R)estore: restores database from backup " + selectedDatabase);
                }
                Console.WriteLine("(M)anage: manage databases");
                Console.WriteLine("E(x)it: Finish program");
                Console.WriteLine();

                string action = ConsoleEx.GetInput().ToLower();
                switch (action)
                {
                    case "b":
                    case "backup": new BackupRoutine(databaseManager, selectedDatabase).Run();
                        break;

                    case "q":
                    case "query":
                        new Query(databaseManager, selectedDatabase).Run();
                        break;

                    case "m":
                    case "manage":
                        new Manage(databaseManager, selectedDatabase).Run();
                        break;

                    case "x":
                    case "exit": finished = true;
                        break;

                    case "r":
                    case "restore":
                        new Restorer(databaseManager, selectedDatabase).Run();
                        break;

                    default:
                        int id;
                        if (int.TryParse(action, out id))
                        {
                            if (id > 0 && id <= databaseManager.databases.Count)
                            {
                                selectedDatabase = databaseManager.GetDatabaseBase1(id);
                            }
                            else
                            {
                                ConsoleEx.WriteError(id + " is not a correct database ID");
                                Console.ReadKey();
                            }
                        }                        
                        break;
                }
            }
            return this;
        }
    }
}
