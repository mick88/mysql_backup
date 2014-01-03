using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseConnection.Databases;

namespace DatabaseClient.Routines
{
    class Manage : RoutineBase
    {

        public Manage(DatabaseManager manager, Database selectedDatabase) : base(manager)
        {
        }

        public override RoutineBase Run()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Databases:");
                int n = 1;
                foreach (Database database in databaseManager.databases)
                {
                    Console.WriteLine(n+") "+database);
                    n++;
                }

                Console.WriteLine();
                Console.WriteLine("Commands: ");
                Console.WriteLine("(V)iew: [id] - view database details");
                Console.WriteLine("(A)dd: [host] [database] [username] (password)");
                Console.WriteLine("(C)opy: [id] [new_database_name] - dupliate host an credentials");
                Console.WriteLine("(D)elete: [id]");
                Console.WriteLine("(B)ack: return to main menu");
                string input = ConsoleEx.GetInput();
                string [] args = input.Split(' ');
                if (args.Length == 0) return this;
                switch (args[0].ToLower())
                {
                    case "b":
                    case "back":
                        return this;
                    
                    case "a":
                    case "add":
                        if (args.Length > 3)
                        {
                            string password = args.Length == 4 ? "" : args[4];
                            Database database = new Database(args[1], args[2], args[3], password);
                            databaseManager.AddDatabase(database);
                        }
                        else
                        {
                            ConsoleEx.WriteError("Usage: Add [host] [database] [username] (password)");
                            ConsoleEx.PressAnyKeyPrompt();
                        }
                        break;
                    case "d":
                    case "delete":
                        int id;
                        if (int.TryParse(args[1], out id))
                        {
                            Database database = databaseManager.GetDatabaseBase1(id);
                            databaseManager.RemoveDatabase(database);
                        }
                        else
                        {
                            ConsoleEx.WriteError("Invalid id");
                            ConsoleEx.PressAnyKeyPrompt();
                        }
                        break;

                    case "c":
                    case "copy":
                        int db_id;
                        if (args.Length != 3)
                        {
                            ConsoleEx.WriteError("Usage: Copy [database_id] [new_database_name]");
                            ConsoleEx.PressAnyKeyPrompt();
                        }
                        else if (int.TryParse(args[1], out db_id))
                        {
                            Database database = new Database(databaseManager.GetDatabaseBase1(db_id));
                            database.DatabaseName = args[2];
                            database.Label = String.Format("{0} on {1}", database.DatabaseName, database.Address);
                            databaseManager.AddDatabase(database);
                        }
                        else
                        {
                            ConsoleEx.WriteError("Invalid id");
                            ConsoleEx.PressAnyKeyPrompt();
                        }
                        break;

                    case "v":
                    case "view":
                        int view_id;
                        if (int.TryParse(args[1], out view_id))
                        {
                            Database database = databaseManager.GetDatabaseBase1(view_id);
                            
                            Console.WriteLine();
                            ConsoleEx.WriteLineColor(database.ToString(), ConsoleColor.DarkYellow);
                            ConsoleEx.WriteColor("Host: ", ConsoleColor.DarkYellow);
                            Console.WriteLine(database.Address);
                            ConsoleEx.WriteColor("Database: ", ConsoleColor.DarkYellow);
                            Console.WriteLine(database.DatabaseName);
                            ConsoleEx.WriteColor("Username: ", ConsoleColor.DarkYellow);
                            Console.WriteLine(database.Username);
                            ConsoleEx.WriteColor("Password: ", ConsoleColor.DarkYellow);
                            Console.WriteLine(database.Password);

                            ConsoleEx.PressAnyKeyPrompt();
                        }
                        else
                        {
                            ConsoleEx.WriteError(args[1] + " is an invalid database id");
                            ConsoleEx.PressAnyKeyPrompt();
                        }
                        break;
                }
            }
        }
    }
}
