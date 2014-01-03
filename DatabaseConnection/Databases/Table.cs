using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseConnection.Databases
{
    public class Table
    {
        public const int 
            INVALID_NUM_ROWS = -1,
            FIELD_TABLE_NAME = 0,
            FIELD_NUM_ROWS = 4;
        private char FIELD_VALUE_SEPARATOR = ',';

        private string INSERT_TEMPLATE;

        public string Name { get; private set; }
        public int NumRows { get; private set; }

        public Table(string name)
        {
            this.NumRows = INVALID_NUM_ROWS;
            this.Name = name;

            INSERT_TEMPLATE = "INSERT INTO "+name+" VALUES ({1});" + Environment.NewLine;
        }

        /// <summary>
        /// Creates new table from result string
        /// </summary>
        /// <param name="resultString">result row</param>
        public Table(string[] resultString) : this(resultString[FIELD_TABLE_NAME])
        {
            this.NumRows = int.Parse(resultString[FIELD_NUM_ROWS]);
        }

        public override string ToString()
        {
            /*StringBuilder builder = new StringBuilder(Name);
            if (NumRows != INVALID_NUM_ROWS) builder.Append('(').Append(NumRows).Append(')');
            return builder.ToString();*/
            return Name;
        }

        /// <summary>
        /// queries database for the CREATE TABLE statement
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public string getCreateTable(Connection connection)
        {
            return new StringBuilder(connection.GetString(string.Format("SHOW CREATE TABLE `{0}`", Name), 1))
                .Append(';')
                .Append(Environment.NewLine)
                .ToString();
        }

        public int getNumRows(Connection connection)
        {
            return connection.QueryInteger(string.Format("SELECT COUNT(*) FROM `{0}`", Name));
        }

        public string [] getInsertQueries(QueryResult queryResult)
        {
            string[] result = new string[queryResult.Count];
            int n=0;
            foreach(string [] row in queryResult)
            {
                StringBuilder values = new StringBuilder();
                for (int i=0; i < row.Length; i++)
                {
                    if (i > 0) values.Append(FIELD_VALUE_SEPARATOR);

                    if (row[i] == null) values.Append("NULL");
                    else values.Append("'").Append(Connection.EscapeString(row[i])).Append("'");
                }
                result[n++] = String.Format(INSERT_TEMPLATE, Name, values.ToString());
            }
            return result;
        }
    }
}
