using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.MySQL
{
    /// <summary>
    /// Provides additional functionality for manipulating MySQL Database Tables.
    /// </summary>
    public static class MySQLTables
    {
        /// <summary>
        /// Generates a MySQL <c>CREATE TABLE</c> command string using the specified table metadata.
        /// </summary>
        /// <param name="TableName">
        /// The name of the table to be created.
        /// </param>
        /// <param name="Columns">
        /// An array of <c>ColumnInformation</c> structs defining the table's columns, types, nullability, and default values.
        /// </param>
        /// <param name="AIColumn">
        /// An <c>AutoIncrement</c> struct specifying whether a column should be marked as <c>AUTO_INCREMENT</c>.
        /// </param>
        /// <param name="PKColumn">
        /// A <c>PrimaryKey</c> struct indicating whether a column should be designated as the table's primary key.
        /// </param>
        /// <param name="Engine">
        /// The MySQL storage engine to be used for the table (e.g., <c>InnoDB</c>, <c>MyISAM</c>).
        /// </param>
        /// <param name="Charset">
        /// The character set to be applied to the table (e.g., <c>utf8mb4</c>).
        /// </param>
        /// <param name="Collation">
        /// The collation to be applied to the table (e.g., <c>utf8mb4_0900_ai_ci</c>).
        /// </param>
        /// <returns>
        /// A fully constructed MySQL <c>CREATE TABLE</c> command string.
        /// </returns>
        public static string CreateTableCommand(string TableName, ColumnInformation[] Columns, AutoIncrement AIColumn, PrimaryKey PKColumn, MySQLEngine Engine, MySQLCharsets Charset, MySQLCollation Collation)
        {
            var cmd = new StringBuilder();

            cmd.Append("CREATE TABLE IF NOT EXISTS `" + TableName + "` (");

            for (int a = 0; a < Columns.Length; a++)
            {
                cmd.Append("`" + Columns[a].ColumnName + "` " + Columns[a].Type.ToString().ToLower());

                if (Columns[a].Nullable == false)
                    cmd.Append(" NOT NULL");
                else
                    cmd.Append(" NULL");

                if (AIColumn.Enabled == true)
                    if (AIColumn.Column == Columns[a].ColumnName)
                        cmd.Append(" AUTO_INCREMENT");

                if (Columns[a].Default == MySQLDefaultMode.AsDefined)
                    cmd.Append(" DEFAULT '" + Columns[a].DefaultValue + "'");
                else if (Columns[a].Default == MySQLDefaultMode.Null)
                    cmd.Append(" DEFAULT NULL");

                if (a < Columns.Length - 1)
                    cmd.Append(", ");
            }

            if (PKColumn.Enabled == true)
                cmd.Append(", PRIMARY KEY (`" + PKColumn.Column + "`)");

            cmd.Append(")");
            cmd.Append(" ENGINE " + Engine.ToString());
            cmd.Append(" DEFAULT CHARSET=" + Charset.ToString());
            cmd.Append(" COLLATE=" + Collation.ToString());
            cmd.Append(";");

            return cmd.ToString();
        }
        /// <summary>
        /// Retrieves the list of column names from a specified MySQL table.
        /// </summary>
        /// <param name="DBConnection">
        /// An active <c>MySqlConnection</c> used to query the <c>INFORMATION_SCHEMA</c>.
        /// </param>
        /// <param name="DBName">
        /// The name of the database containing the target table.
        /// </param>
        /// <param name="TableName">
        /// The name of the table whose columns are to be retrieved.
        /// </param>
        /// <returns>
        /// A <c>List&lt;string&gt;</c> containing the names of all columns in the specified table.
        /// </returns>
        public static List<string> GetColumns(MySqlConnection DBConnection, string DBName, string TableName)
        {
            List<string> cols = new List<string>();
            MySqlConnection conn = DBConnection;
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader reader;

            string command = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA=@Database AND TABLE_NAME=@Table;";

            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();
            cmd = new MySqlCommand(command, conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Database", DBName);
            cmd.Parameters.AddWithValue("@Table", TableName);
            reader = cmd.ExecuteReader();
            while (reader.Read())
                cols.Add(reader[0].ToString());
            reader.Close();
            conn.Close();
            cmd.Dispose();

            return cols;
        }
        /// <summary>
        /// Generates a C# <c>enum</c> definition from a list of string values.
        /// </summary>
        /// <param name="Items">
        /// A list of strings representing the desired enum members. Each item will be sanitized
        /// by replacing spaces and hyphens with underscores to ensure valid C# identifiers.
        /// </param>
        /// <param name="EnumName">
        /// The name of the enum to be generated. This will be used as the type name in the output code.
        /// </param>
        /// <returns>
        /// A formatted C# <c>enum</c> declaration as a string, suitable for writing to a <c>.cs</c> file.
        /// </returns>
        public static string GenerateEnumFromList(List<string> Items, string EnumName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("public enum " + EnumName);
            sb.AppendLine("{");

            sb.AppendLine("    " + EnumName + ",");
            foreach (var item in Items)
            {
                string safeName = item.Replace(" ", "_").Replace("-", "_");
                sb.AppendLine("    " + safeName + ",");
            }

            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
