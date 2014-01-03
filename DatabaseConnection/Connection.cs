using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using DatabaseConnection.Databases;
using MySql.Data.Types;

namespace DatabaseConnection
{
    /// <summary>
    /// Wrapper for MySQL connection
    /// </summary>
    public class Connection : IDisposable
    {

        public Database database {get; private set;}
        MySqlConnection connection = null;
        public bool IsConnected { get { return connection != null; } }

        /// <summary>
        /// Automatically reconnects if database doesn't respond to ping
        /// </summary>
        public bool AutoReconnect { get; set; }

        public Connection(Database database)
        {
            AutoReconnect = false;

            this.database = database;
        }

        /// <summary>
        /// Connects to the database
        /// </summary>
        /// <returns></returns>
        public Connection Connect()
        {
            if (connection != null) Disconnect();

            this.connection = new MySqlConnection(database.ConnectionString);
            connection.Open();
            return this;
        }

        public void Disconnect()
        {
            if (connection != null)
            {
                try
                {
                    connection.Close();
                    connection = null;
                }
                catch
                {
                }
            }
        }

        public QueryResult Query(string query)
        {
            if (connection == null) throw new Exception("Call Connect() before querying!");
            else if (AutoReconnect == true && connection.Ping() == false)
            {
                Console.WriteLine("Warning: no response to Ping. Reconnecting...");
                Connect();
            }

            QueryResult result;
            using (var mysqlCommand = connection.CreateCommand())
            {
                mysqlCommand.CommandText = query;

                var reader = mysqlCommand.ExecuteReader();

                result = new QueryResult(reader);

                reader.Close();
            }

            return result;
        }

        /// <summary>
        /// Performs multiple queries on a database
        /// </summary>
        public QueryResult Query(IEnumerable<string> queries)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string query in queries)
            {
                if (query.EndsWith(";") == false)
                {
                    builder.Append(query).Append(';');
                }
                else builder.AppendLine(query);
            }

            return Query(builder.ToString());
        }

        /// <summary>
        /// Returns string from the first row
        /// </summary>
        public string GetString(string query, int field)
        {
            if (connection == null) throw new Exception("Call Connect() before querying!");

            var mysqlCommand = connection.CreateCommand();
            mysqlCommand.CommandText = query;

            var reader = mysqlCommand.ExecuteReader();

            reader.Read();
            object result = reader.GetValue(field);

            reader.Close();

            return result.ToString();
        }

        /// <summary>
        /// Returns integer value from the first row
        /// </summary>
        public int GetInteger(string query, int field)
        {
            return int.Parse(GetString(query, field));
        }

        public int QueryInteger(string query)
        {
            return int.Parse(GetString(query, 0));
        }

        /// <summary>
        /// Returns array of tables in the database
        /// </summary>
        public Table[] GetTables()
        {
            QueryResult queryResult = Query("SHOW TABLE STATUS");
            var result = new Table[queryResult.NumRows];

            int i = 0;
            foreach (string[] row in queryResult)
            {
                result[i++] = new Table(row);
            }

            return result;
        }

        public QueryResult Select(string table, string where, int startAt, int limit)
        {
            return Query(String.Format("SELECT * FROM `{0}` WHERE {1} LIMIT {2},{3}", table, where, startAt, limit));
        }

        public QueryResult Select(string table, int limit, int startAt)
        {
            return Select(table, "1", startAt, limit);
        }

        public static string EscapeString(string text)
        {
            if (text == null) return null;

            return System.Text.RegularExpressions.Regex.Replace(text, @"[\r\n\x00\x1a\\'""]", @"\$0");
        }

        void IDisposable.Dispose()
        {
            Disconnect();
        }

        ~Connection()
        {
            Disconnect();
        }
    }
}
