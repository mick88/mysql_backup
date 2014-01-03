using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseConnection.Databases;
using DatabaseConnection;

namespace DatabaseClient.Routines
{
    class Query:RoutineBase
    {

        public Query(DatabaseManager databaseManager, Database database)
            : base(databaseManager, database)
        {
        }

        private void SendQuery(Connection connection, string query)
        {
            try
            {
                QueryResult result = connection.Query(query);
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                ConsoleEx.WriteError(e.Message);
            }
        }

        public override RoutineBase Run()
        {
            Console.Clear();

            if (selectedDatabase == null)
            {
                ConsoleEx.WriteError("No database selected");
            }
            else
            {

                using (Connection connection = new Connection(selectedDatabase))
                {
                    try
                    {
                        connection.Connect();
                    }
                    catch (Exception e)
                    {
                        ConsoleEx.WriteError(e.Message);
                        return base.Run();
                    }

                    connection.AutoReconnect = true;
                    while (true)
                    {
                        ConsoleEx.WriteColor(selectedDatabase + ": ", ConsoleColor.DarkYellow);
                        string input = Console.ReadLine();

                        if (String.IsNullOrEmpty(input)) break;
                        SendQuery(connection, input);
                    }
                }
            }
            return base.Run();
        }
    }
}
