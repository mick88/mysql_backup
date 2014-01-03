using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace DatabaseConnection
{
    public class QueryResult : LinkedList<string[]>
    {
        public const char COLUMN_SEPARATOR = '\t';
        public const string FORMAT_DATE_TIME = "yyyy-MM-dd HH:mm:ss";

        public int NumRows { get { return this.Count; } }
        public string[] Fields { get; set; }
        public int NumCols { get { return Fields == null ? 0 : Fields.Length; } }

        public int AffectedRows { get; private set; }

        public QueryResult()
        {
            Fields = new string[0];
        }

        public QueryResult(MySqlDataReader reader) 
            :this()
        {
            AffectedRows = reader.RecordsAffected;

            int numFields = reader.FieldCount;

            Fields = new string[numFields];
            for (int i = 0; i < numFields; i++)
            {
                Fields[i] = reader.GetName(i);
            }

            while (reader.Read())
            {
                string[] row = new string[numFields];
                for (int i = 0; i < numFields; i++)
                {
                    object value = reader.GetValue(i);
                    string datatype = reader.GetDataTypeName(i).ToLower();
                    if (value == null) row[i] = null;
                    else if (value is MySqlDateTime)
                    {
                        var date = (MySqlDateTime)value;
                        if (date.IsValidDateTime) row[i] = date.Value.ToString(FORMAT_DATE_TIME);
                        else row[i] = "0000-00-00 00:00:00";
                    }
                    else if (datatype.Contains("tinyint")) row[i] = reader.GetInt16(i).ToString();
                    else row[i] = value.ToString();
                }

                this.AddLast(row);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string[] row in this)
            {
                foreach (string col in row)
                {
                    builder.Append(col).Append(COLUMN_SEPARATOR);
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}
