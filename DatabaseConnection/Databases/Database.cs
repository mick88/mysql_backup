using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseConnection.Databases
{
    public class Database : ICloneable
    {
        public const string LOCAL_HOST = "localhost",
            ROOT_USERNAME = "root";
        protected const char EXPORT_SEPARATOR = ';';

        /// <summary>
        /// Unique label for the database
        /// </summary>
        public string Label { get; set; }

        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets connection string
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return new StringBuilder("SERVER=").Append(this.Address)
                    .Append(";DATABASE=").Append(this.DatabaseName)
                    .Append(";UID=").Append(this.Username)
                    .Append(";PASSWORD=").Append(this.Password)
                    .Append(";Allow Zero Datetime=true;")
                    .ToString();
            }
        }

        public Database(string address, string databaseName, string username, string password)
        {
            this.Address = address;
            this.Username = username;
            this.Password = password;
            this.DatabaseName = databaseName;

            this.Label = String.Format("{0} on {1}", databaseName, address);
        }

        public Database(string label, string address, string databaseName, string username, string password)
            : this(address, databaseName, username, password)
        {
            this.Label = label;
        }

        public Database(Database database) 
            :this(database.Address, database.DatabaseName, database.Username, database.Password)
        {
        }

        /// <summary>
        /// Imports database from plain text
        /// </summary>
        /// <param name="exportedString"></param>
        public Database(string exportedString) 
            :this(string.Empty, string.Empty, string.Empty, string.Empty)
        {
            if (import(exportedString) == false) throw new Exception("Exported database string is invalid!");
        }

        protected virtual bool import(string exportedString)
        {
            string[] values = exportedString.Split(EXPORT_SEPARATOR);
            if (values.Length < 5) return false;

            this.Label = values[0];
            this.Address = values[1];
            this.DatabaseName = values[2];
            this.Username = values[3];
            this.Password = values[4];
            return true;
        }

        object ICloneable.Clone()
        {
            return new Database(this);
        }

        public override bool Equals(object obj)
        {
            if (obj is Database)
            {
                Database other = (Database) obj;
                return other.Address.Equals(this.Address) && other.DatabaseName.Equals(this.DatabaseName);
            }
            else return base.Equals(obj);
        }

        public override string ToString()
        {
            return Label;
        }

        public virtual StringBuilder ExportPlain()
        {
            return new StringBuilder()
                .Append(Label)
                .Append(EXPORT_SEPARATOR)
                .Append(Address)
                .Append(EXPORT_SEPARATOR)
                .Append(DatabaseName)
                .Append(EXPORT_SEPARATOR)
                .Append(Username)
                .Append(EXPORT_SEPARATOR)
                .Append(Password);
        }

        public StringBuilder ExportEncrypted(int key)
        {
            StringBuilder plain = ExportPlain();
            char[] chars = new char[plain.Length];
            plain.CopyTo(0, chars, 0, chars.Length);
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] += (char)key;
            }
            return new StringBuilder(new String(chars));
        }

        public static Database ImportEncrypted(string exportedString, int key)
        {
            char [] chars = exportedString.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] -= (char)(key);
            }

            try
            {
                return new Database(new string(chars));
            }
            catch
            {
                return null;
            }
        }

        public override int GetHashCode()
        {
            int hash = 0;
            for (int i = 0; i < Label.Length; i++)
            {
                hash += Label[i] * (i+1);
            }
            return hash;
        }
    }
}
