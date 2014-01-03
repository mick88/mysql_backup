using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DatabaseConnection.Databases
{
    /// <summary>
    /// Container for databases
    /// </summary>
    public class DatabaseManager
    {
        public List<Database> databases {get; private set;}
        public string FilePath { get; set; }
        protected const int ENCRYPTION_KEY = 12; // do not change

        public DatabaseManager()
        {
            this.databases = new List<Database>();
            this.FilePath = "Databases.dat";
        }

        /// <summary>
        /// Adds database to manager and saves the list
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public virtual bool AddDatabase(Database database)
        {
            if (databases.Contains(database)) return false;
            databases.Add(database);
            SaveDatabases();
            return true;
        }

        public virtual Database GetDatabase(int id)
        {
            if (id >= 0 && id < databases.Count)
            {
                return databases[id];
            }
            return null;
        }

        public int GetDatabaseID(Database database)
        {
            return databases.IndexOf(database);
        }

        /// <summary>
        /// gets database in 1-based array
        /// </summary>
        /// <param name="id">Database id starting with 1</param>
        public virtual Database GetDatabaseBase1(int id)
        {
            return GetDatabase(id-1);
        }

        /// <summary>
        /// Removes database from the list and saves list
        /// </summary>
        /// <param name="database">Database to be removed</param>
        /// <returns>True if database was found and removed</returns>
        public bool RemoveDatabase(Database database)
        {
            if (this.databases.Remove(database))
            {
                SaveDatabases();
                return true;
            }
            else return false;
        }

        public virtual DatabaseManager LoadDatabases()
        {
            return LoadDatabases(FilePath);
        }

        public virtual DatabaseManager LoadDatabases(string filename)
        {
            if (File.Exists(filename) == false) return this;
            StreamReader reader = new StreamReader(filename);
            while (reader.EndOfStream == false)
            {
                Database database = ReadDatabase(reader.ReadLine());
                if (database != null) databases.Add(database);
            }
            reader.Close();
            return this;
        }

        protected virtual Database ReadDatabase(string line)
        {
            return Database.ImportEncrypted(line, ENCRYPTION_KEY);
        }

        public DatabaseManager SaveDatabases()
        {
            return SaveDatabases(FilePath);
        }

        public DatabaseManager SaveDatabases(string filename)
        {
            StreamWriter writer = new StreamWriter(filename);
            foreach (Database database in databases)
            {
                writer.WriteLine(database.ExportEncrypted(ENCRYPTION_KEY));
            }
            writer.Flush();
            writer.Close();
            return this;
        }
    }
}
