using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.MySQL
{
    /// <summary>
    /// Provides additional functionality for manipulating MySQL Databases.
    /// </summary>
    public static class MySQLDatabase
    {
        /// <summary>
        /// Checks whether a specified MySQL database exists on the target server using provided credentials.
        /// </summary>
        /// <param name="Server">
        /// The hostname or IP address of the MySQL server.
        /// </param>
        /// <param name="User">
        /// The username used to authenticate with the MySQL server.
        /// </param>
        /// <param name="Database">
        /// The name of the database to verify for existence.
        /// </param>
        /// <param name="Password">
        /// The password for the specified user account. Optional; defaults to an empty string.
        /// </param>
        /// <returns>
        /// <c>true</c> if the database exists; otherwise, <c>false</c>.
        /// </returns>
        public static bool DatabaseExists(string Server, string User, string Database, string Password = "")
        {
            bool e = false;
            string connStr = "server=" + Server + "; uid=" + User + ";";
            MySqlDataReader reader;

            if (Password != "")
                connStr += " password=" + Password + ";";

            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @database";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@database", Database);
            reader = cmd.ExecuteReader();
            e = reader.HasRows;
            reader.Close();
            conn.Close();
            conn.Dispose();
            cmd.Dispose();

            return e;
        }
        /// <summary>
        /// Creates a new MySQL database with the specified name, character set, and collation if it does not already exist.
        /// </summary>
        /// <param name="DBConnection">
        /// An active <c>MySqlConnection</c> object used to execute the database creation command.
        /// </param>
        /// <param name="DBName">
        /// The name of the database to be created.
        /// </param>
        /// <param name="Charset">
        /// The character set to be applied to the new database, defined by the <c>MySQLCharsets</c> enum.
        /// </param>
        /// <param name="Collation">
        /// The collation to be applied to the new database, defined by the <c>MySQLCollation</c> enum.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown when the database creation fails due to a SQL or connection error.
        /// </exception>
        public static void CreateDatabase(MySqlConnection DBConnection, string DBName, MySQLCharsets Charset, MySQLCollation Collation)
        {
            MySqlConnection conn = DBConnection;
            MySqlCommand cmd = new MySqlCommand();
            string query = "";

            query = "CREATE DATABASE IF NOT EXISTS `" +
                DBName + "` DEFAULT CHARACTER SET " +
                Charset.ToString() + " COLLATE " +
                Collation.ToString() + ";";

            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            cmd = new MySqlCommand(query, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Database creation error.", e);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
            }
        }
    }
}
