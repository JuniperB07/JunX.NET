using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JunX.NET8.MySQL
{
    /// <summary>
    /// Provides methods for an automatic creation of Enum Files consisting of Tables and Columns within a specified database.
    /// The generated Enum Files can then be used via 'Add Existing Item' for enum-driven operations within this library.
    /// </summary>
    public static class EnumGenerator
    {
        private static MySqlConnection conn = new MySqlConnection();

        /// <summary>
        /// Gets or Sets the Database Name to be used for Enum File generation.
        /// </summary>
        public static string DatabaseName { get; set; }

        /// <summary>
        /// Opens the inernal MySQL Connection.
        /// </summary>
        /// <param name="ConnectionString">Connection String to be used. DO NOT INCLUDE DATABASE NAME.</param>
        public static void OpenServerConnection(string ConnectionString)
        {
            conn = new MySqlConnection(ConnectionString);
            conn.Open();
        }

        /// <summary>
        /// Generates Enum files of Tables containing Column Names from the specified database to the specified path.
        /// </summary>
        /// <param name="Path">The directory where the Enum Files will be stored.</param>
        public static void GenerateEnumFiles(string Folder)
        {
            string Enums = "";
            string outputPath = "";

            foreach(string tables in GetTables())
            {
                Enums = GenerateEnumFromList(GetColumns(tables), tables);

                Directory.CreateDirectory(Folder);
                outputPath = Path.Combine(Folder, tables + ".cs");
                File.WriteAllText(outputPath, Enums);
            }
        }

        private static List<string> GetTables()
        {
            List<string> tables = new List<string>();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader reader;

            string command = "SHOW TABLES FROM " + DatabaseName + ";";
            cmd = new MySqlCommand(command, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
                tables.Add(reader[0].ToString());
            reader.Close();
            cmd.Dispose();

            return tables;
        }
        private static List<string> GetColumns(string Table)
        {
            List<string> columns = new List<string>();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader reader;

            string command = "SHOW COLUMNS FROM " + DatabaseName + "." + Table + ";";
            cmd = new MySqlCommand(command, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
                columns.Add(reader[0].ToString());
            reader.Close();
            cmd.Dispose();

            return columns;
        }
        private static string GenerateEnumFromList(List<string> Items, string EnumName)
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
