using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Mozilla;
using System.ComponentModel;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace JunX.NET8.MySQL
{
    /// <summary>
    /// Provides method that will construct various MySQL Command strings.
    /// </summary>
    public static class Construct
    {
        /// <summary>
        /// Generates a SQL <c>SELECT</c> statement that retrieves all columns from the specified table.
        /// </summary>
        /// <param name="From">
        /// The name of the table to query.
        /// </param>
        /// <param name="Where">
        /// An optional SQL <c>WHERE</c> clause to filter the results.
        /// </param>
        /// <returns>
        /// A complete SQL <c>SELECT</c> command string.
        /// </returns>
        public static string SelectAllCommand(string From, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT * FROM " + From);

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(";");

            return cmd.ToString();
        }
        /// <summary>
        /// Generates a SQL <c>SELECT</c> statement with the specified column and optional filtering.
        /// </summary>
        /// <param name="Select">
        /// The column to include in the <c>SELECT</c> clause.
        /// </param>
        /// <param name="From">
        /// The name of the table to query.
        /// </param>
        /// <param name="Where">
        /// An optional SQL <c>WHERE</c> clause to filter the results.
        /// </param>
        /// <returns>
        /// A complete SQL <c>SELECT</c> command string.
        /// </returns>
        public static string SelectCommand(string Select, string From, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT " + Select + " FROM " + From);

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(";");

            return cmd.ToString();
        }
        /// <summary>
        /// Generates a SQL <c>SELECT</c> statement with multiple specified columns and optional filtering.
        /// </summary>
        /// <param name="Select">
        /// An array of column names or expressions to include in the <c>SELECT</c> clause.
        /// </param>
        /// <param name="From">
        /// The name of the table to query.
        /// </param>
        /// <param name="Where">
        /// An optional SQL <c>WHERE</c> clause to filter the results.
        /// </param>
        /// <returns>
        /// A complete SQL <c>SELECT</c> command string.
        /// </returns>
        public static string SelectCommand(string[] Select, string From, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT ");

            for(int a=0; a<Select.Length; a++)
            {
                cmd.Append(Select[a]);

                if (a < Select.Length - 1)
                    cmd.Append(", ");
            }
            cmd.Append(" FROM " + From);

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(";");
            return cmd.ToString();
        }
        /// <summary>
        /// Constructs a SQL SELECT statement that assigns an alias to a single column.
        /// </summary>
        /// <param name="AliasMetadata">
        /// Contains the column name to select and the alias to assign.
        /// </param>
        /// <param name="From">
        /// The name of the table to select from.
        /// </param>
        /// <param name="Where">
        /// Optional WHERE clause to filter the result set. If omitted, no filtering is applied.
        /// </param>
        /// <returns>
        /// A complete SQL SELECT statement string with aliasing and optional filtering.
        /// </returns>
        public static string SelectAliasCommand(SelectAsMetadata AliasMetadata, string From, string Where= "")
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT " + AliasMetadata.Column + " AS '" + AliasMetadata.Alias + "'");
            cmd.Append(" FROM " + From);

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(";");
            return cmd.ToString();
        }
        /// <summary>
        /// Constructs a SQL SELECT statement that assigns aliases to multiple columns.
        /// </summary>
        /// <param name="AliasMetadata">
        /// An array of column-to-alias mappings to include in the SELECT clause.
        /// </param>
        /// <param name="From">
        /// The name of the table to select from.
        /// </param>
        /// <param name="Where">
        /// Optional WHERE clause to filter the result set. If omitted, no filtering is applied.
        /// </param>
        /// <returns>
        /// A complete SQL SELECT statement string with aliasing and optional filtering.
        /// </returns>
        public static string SelectAliasCommand(SelectAsMetadata[] AliasMetadata, string From, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT ");

            for(int a=0; a<AliasMetadata.Length; a++)
            {
                cmd.Append(AliasMetadata[a].Column + " AS '" + AliasMetadata[a].Alias + "'");

                if(a < AliasMetadata.Length - 1)
                    cmd.Append(", ");
            }

            cmd.Append(" FROM " + From);

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(";");
            return cmd.ToString();
        }
        /// <summary>
        /// Constructs a SQL SELECT statement that assigns an alias to a single column and applies ordering.
        /// </summary>
        /// <param name="AliasMetadata">
        /// Contains the column name to select and the alias to assign.
        /// </param>
        /// <param name="From">
        /// The name of the table to select from.
        /// </param>
        /// <param name="OrderBy">
        /// The column name used to order the result set.
        /// </param>
        /// <param name="OrderMode">
        /// The ordering direction, typically <c>ASC</c> or <c>DESC</c>.
        /// </param>
        /// <param name="Where">
        /// Optional WHERE clause to filter the result set. If omitted, no filtering is applied.
        /// </param>
        /// <returns>
        /// A complete SQL SELECT statement string with aliasing, optional filtering, and ordering.
        /// </returns>
        /// <remarks>
        /// This method builds a SQL query that selects a single column with an alias using the <c>AS</c> keyword,  
        /// includes a FROM clause, conditionally appends a WHERE clause, and applies an ORDER BY clause.  
        /// Useful for generating readable and sorted result sets in dynamic SQL construction.
        /// </remarks>
        public static string SelectAliasOrderByCommand(SelectAsMetadata AliasMetadata, string From, string OrderBy, MySQLOrderBy OrderMode, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT " + AliasMetadata.Column + " AS '" + AliasMetadata.Alias + "'");
            cmd.Append(" FROM " + From);

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(" ORDER BY " + OrderBy + " " + OrderMode.ToString());
            cmd.Append(";");
            return cmd.ToString();
        }
        /// <summary>
        /// Constructs a SQL SELECT statement that assigns aliases to multiple columns and applies ordering.
        /// </summary>
        /// <param name="AliasMetadata">
        /// An array of column-to-alias mappings to include in the SELECT clause.
        /// </param>
        /// <param name="From">
        /// The name of the table to select from.
        /// </param>
        /// <param name="OrderBy">
        /// The column name used to order the result set.
        /// </param>
        /// <param name="OrderMode">
        /// The ordering direction, typically <c>ASC</c> or <c>DESC</c>.
        /// </param>
        /// <param name="Where">
        /// Optional WHERE clause to filter the result set. If omitted, no filtering is applied.
        /// </param>
        /// <returns>
        /// A complete SQL SELECT statement string with aliasing, optional filtering, and ordering.
        /// </returns>
        /// <remarks>
        /// This method builds a SQL query that selects multiple columns with aliases using the <c>AS</c> keyword,  
        /// includes a FROM clause, conditionally appends a WHERE clause, and applies an ORDER BY clause.  
        /// Useful for generating readable and sorted result sets in dynamic SQL construction.
        /// </remarks>
        public static string SelectAliasOrderByCommand(SelectAsMetadata[] AliasMetadata, string From, string OrderBy, MySQLOrderBy OrderMode, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT ");

            for(int a=0; a<AliasMetadata.Length; a++)
            {
                cmd.Append(AliasMetadata[a].Column + " AS '" + AliasMetadata[a].Alias  + "'");

                if(a< AliasMetadata.Length - 1)
                    cmd.Append(", ");
            }
            cmd.Append(" FROM " + From);

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(" ORDER BY " + OrderBy + " " + OrderMode.ToString());
            cmd.Append(";");
            return cmd.ToString();
        }
        /// <summary>
        /// Generates a SQL <c>SELECT DISTINCT</c> statement for the specified column or expression.
        /// </summary>
        /// <param name="SelectDistinct">
        /// The column or expression to include in the <c>SELECT DISTINCT</c> clause.
        /// </param>
        /// <param name="From">
        /// The name of the table to query.
        /// </param>
        /// <returns>
        /// A complete SQL <c>SELECT DISTINCT</c> command string.
        /// </returns>
        public static string SelectDistinctCommand(string SelectDistinct, string From)
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT DISTINCT " + SelectDistinct + " FROM " + From + ";");

            return cmd.ToString();
        }
        /// <summary>
        /// Generates a SQL <c>SELECT DISTINCT</c> statement with multiple specified columns.
        /// </summary>
        /// <param name="SelectDistinct">
        /// An array of column names or expressions to include in the <c>SELECT DISTINCT</c> clause.
        /// </param>
        /// <param name="From">
        /// The name of the table to query.
        /// </param>
        /// <returns>
        /// A complete SQL <c>SELECT DISTINCT</c> command string.
        /// </returns>
        public static string SelectDistinctCommand(string[] SelectDistinct, string From)
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT DISTINCT ");

            for(int a=0; a<SelectDistinct.Length; a++)
            {
                cmd.Append(SelectDistinct[a]);

                if(a < SelectDistinct.Length - 1) 
                    cmd.Append(", ");
            }

            cmd.Append(';'); ;
            return cmd.ToString();
        }
        /// <summary>
        /// Generates a SQL <c>SELECT</c> statement with optional filtering and ordering.
        /// </summary>
        /// <param name="Select">
        /// The columns or expressions to include in the <c>SELECT</c> clause.
        /// </param>
        /// <param name="From">
        /// The name of the table to query.
        /// </param>
        /// <param name="OrderBy">
        /// The column or expression to use for ordering the results.
        /// </param>
        /// <param name="OrderMode">
        /// The sort direction, typically <c>ASC</c> or <c>DESC</c>.
        /// </param>
        /// <param name="Where">
        /// An optional SQL <c>WHERE</c> clause to filter the results.
        /// </param>
        /// <returns>
        /// A complete SQL <c>SELECT</c> command string.
        /// </returns>
        public static string SelectOrderByCommand(string Select, string From, string OrderBy, MySQLOrderBy OrderMode, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT " + Select + " FROM " + From);

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(" ORDER BY " + OrderBy + " " + OrderMode.ToString());

            cmd.Append(";");
            return cmd.ToString();
        }
        /// <summary>
        /// Generates a SQL <c>SELECT</c> statement with multiple specified columns and ordering.
        /// </summary>
        /// <param name="Select">
        /// An array of column names or expressions to include in the <c>SELECT</c> clause.
        /// </param>
        /// <param name="From">
        /// The name of the table to query.
        /// </param>
        /// <param name="OrderBy">
        /// The column or expression to use for ordering the results.
        /// </param>
        /// <param name="OrderMode">
        /// The sort direction, typically <c>ASC</c> or <c>DESC</c>.
        /// </param>
        /// <param name="Where">
        /// An optional SQL <c>WHERE</c> clause to filter the results.
        /// </param>
        /// <returns>
        /// A complete SQL <c>SELECT</c> command string.
        /// </returns>
        public static string SelectOrderByCommand(string[] Select, string From, string OrderBy, MySQLOrderBy OrderMode, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT ");

            for(int a=0; a< Select.Length; a++)
            {
                cmd.Append(Select[a]);

                if(a<Select.Length - 1)
                    cmd.Append(", ");
            }
            cmd.Append(" FROM " + From);
            cmd.Append(" ORDER BY " + OrderBy + " " + OrderMode.ToString());
            cmd.Append(";");

            return cmd.ToString();
        }
        /// <summary>
        /// Generates a SQL <c>INSERT INTO</c> statement for a single column and value.
        /// </summary>
        /// <param name="InsertInto">
        /// The name of the table to insert into.
        /// </param>
        /// <param name="InsertMetadata">
        /// The metadata containing the column name, data type, and value to insert.
        /// </param>
        /// <returns>
        /// A complete SQL <c>INSERT INTO</c> command string.
        /// </returns>
        public static string InsertIntoCommand(string InsertInto, InsertColumnMetadata InsertMetadata)
        {
            var cmd = new StringBuilder();

            cmd.Append("INSERT INTO " + InsertInto + " (");
            cmd.Append(InsertMetadata.Column + ") VALUES (");
            cmd.Append(SQLSafeValue(InsertMetadata.DataType, InsertMetadata.Value) + ");");

            return cmd.ToString();
        }
        /// <summary>
        /// Generates a SQL <c>INSERT INTO</c> statement for multiple columns and values.
        /// </summary>
        /// <param name="InsertInto">
        /// The name of the table to insert into.
        /// </param>
        /// <param name="InsertMetadata">
        /// An array of metadata containing column names, data types, and values to insert.
        /// </param>
        /// <returns>
        /// A complete SQL <c>INSERT INTO</c> command string.
        /// </returns>
        public static string InsertIntoCommand(string InsertInto, InsertColumnMetadata[] InsertMetadata)
        {
            var cmd = new StringBuilder();

            cmd.Append("INSERT INTO " + InsertInto + " (");

            for(int a=0; a<InsertMetadata.Length; a++)
            {
                cmd.Append(InsertMetadata[a].Column);

                if(a<InsertMetadata.Length - 1)
                    cmd.Append(", ");
            }
            cmd.Append(") VALUES (");

            for(int a=0; a<InsertMetadata.Length; a++)
            {
                cmd.Append(SQLSafeValue(InsertMetadata[a].DataType, InsertMetadata[a].Value));

                if(a<InsertMetadata.Length - 1)
                    cmd.Append(',');
            }
            cmd.Append(");");

            return cmd.ToString();
        }
        /// <summary>
        /// Constructs a parameter-safe SQL UPDATE command string using the specified table, column metadata, and optional WHERE clause.
        /// </summary>
        /// <param name="Update">The name of the table to update.</param>
        /// <param name="UpdateMetadata">Metadata describing the column and value to update.</param>
        /// <param name="Where">Optional WHERE clause to filter affected rows.</param>
        /// <returns>
        /// A <c>string</c> containing the complete SQL UPDATE command.
        /// </returns>
        public static string UpdateCommand(string Update, UpdateColumnMetadata UpdateMetadata, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("UPDATE " + Update + " SET ");

            cmd.Append(UpdateMetadata.Column + "="
                    + SQLSafeValue(UpdateMetadata.DataType, UpdateMetadata.Value));
           
            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(";");

            return cmd.ToString();
        }

        /// <summary>
        /// Generates a SQL <c>UPDATE</c> statement for one or more columns.
        /// </summary>
        /// <param name="Update">
        /// The name of the table to update.
        /// </param>
        /// <param name="UpdateMetadata">
        /// An array of metadata containing column names, data types, and new values to assign.
        /// </param>
        /// <param name="Where">
        /// An optional SQL <c>WHERE</c> clause to filter which rows are updated.
        /// <b>Warning:</b> Omitting this parameter will update <i>all</i> rows in the table.
        /// </param>
        /// <returns>
        /// A complete SQL <c>UPDATE</c> command string.
        /// </returns>
        public static string UpdateCommand(string Update, UpdateColumnMetadata[] UpdateMetadata, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("UPDATE " + Update + " SET ");

            for(int a=0; a<UpdateMetadata.Length; a++)
            {
                cmd.Append(UpdateMetadata[a].Column + "=" 
                    + SQLSafeValue(UpdateMetadata[a].DataType, UpdateMetadata[a].Value));

                if(a <  UpdateMetadata.Length - 1)
                    cmd.Append(", ");
            }

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(";");

            return cmd.ToString();
        }
        /// <summary>
        /// Generates a SQL <c>DELETE</c> statement for the specified table.
        /// </summary>
        /// <param name="From">
        /// The name of the table from which to delete rows.
        /// </param>
        /// <param name="Where">
        /// An optional SQL <c>WHERE</c> clause to filter which rows are deleted.
        /// </param>
        /// <returns>
        /// A complete SQL <c>DELETE</c> command string.
        /// </returns>
        /// <remarks>
        /// <b>Warning:</b> Omitting the <paramref name="Where"/> clause will delete <i>all</i> rows from the specified table.
        /// </remarks>
        public static string DeleteCommand(string From, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("DELETE FROM " + From);

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(";");
            return cmd.ToString();
        }
        /// <summary>
        /// Generates a SQL <c>SELECT</c> statement with an <c>INNER JOIN</c> clause.
        /// </summary>
        /// <param name="Select">
        /// The table and column to include in the <c>SELECT</c> clause.
        /// </param>
        /// <param name="From">
        /// The name of the primary table to query.
        /// </param>
        /// <param name="InnerJoin">
        /// The name of the table to join.
        /// </param>
        /// <param name="OnLeft">
        /// The table and column on the left side of the join condition.
        /// </param>
        /// <param name="OnRight">
        /// The table and column on the right side of the join condition.
        /// </param>
        /// <param name="Where">
        /// An optional SQL <c>WHERE</c> clause to filter the results.
        /// </param>
        /// <returns>
        /// A complete SQL <c>SELECT</c> command string with an <c>INNER JOIN</c>.
        /// </returns>
        /// <remarks>
        /// <b>Warning:</b> Omitting the <paramref name="Where"/> clause will return all matching rows from the joined tables.
        /// </remarks>
        public static string InnerJoinCommand(JoinMetadata Select, string From, string InnerJoin, JoinMetadata OnLeft, JoinMetadata OnRight, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT " + Select.Table + "." + Select.Column);
            cmd.Append(" FROM " + From);
            cmd.Append("INNER JOIN " + InnerJoin);
            cmd.Append(" ON " + OnLeft.Table + "." + OnLeft.Column + "=" + OnRight.Table + "." + OnRight.Column);

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(";");
            return cmd.ToString();
        }
        /// <summary>
        /// Generates a SQL <c>SELECT</c> statement with multiple columns and an <c>INNER JOIN</c> clause.
        /// </summary>
        /// <param name="Select">
        /// An array of table and column metadata to include in the <c>SELECT</c> clause.
        /// </param>
        /// <param name="From">
        /// The name of the primary table to query.
        /// </param>
        /// <param name="InnerJoin">
        /// The name of the table to join.
        /// </param>
        /// <param name="OnLeft">
        /// The table and column on the left side of the join condition.
        /// </param>
        /// <param name="OnRight">
        /// The table and column on the right side of the join condition.
        /// </param>
        /// <param name="Where">
        /// An optional SQL <c>WHERE</c> clause to filter the results.
        /// </param>
        /// <returns>
        /// A complete SQL <c>SELECT</c> command string with an <c>INNER JOIN</c>.
        /// </returns>
        /// <remarks>
        /// <b>Warning:</b> Omitting the <paramref name="Where"/> clause will return all matching rows from the joined tables.
        /// </remarks>
        public static string InnerJoinCommand(JoinMetadata[] Select, string From, string InnerJoin, JoinMetadata OnLeft, JoinMetadata OnRight, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT ");

            for(int a=0; a<Select.Length;  a++)
            {
                cmd.Append(Select[a].Table + "." + Select[a].Column);

                if (a < Select.Length - 1)
                    cmd.Append(", ");
            }
            cmd.Append(" FROM " + From);
            cmd.Append(" INNER JOIN " + InnerJoin);
            cmd.Append(" ON " + OnLeft.Table + "." + OnLeft.Column + "=" + OnRight.Table + "." + OnRight.Column);

            if (Where != "")
                cmd.Append(" WHERE " + Where);
            cmd.Append(";");
            return cmd.ToString();
        }
        /// <summary>
        /// Constructs a SQL inner join command with an aliased select column.
        /// </summary>
        /// <param name="SelectWithAlias">The column to select with aliasing metadata.</param>
        /// <param name="From">The base table to select from.</param>
        /// <param name="InnerJoin">The table to join with.</param>
        /// <param name="OnLeft">The left-side join condition metadata.</param>
        /// <param name="OnRight">The right-side join condition metadata.</param>
        /// <param name="Where">An optional WHERE clause to filter results.</param>
        /// <returns>A complete SQL inner join command string.</returns>
        /// <remarks>
        /// This method builds a SQL statement using metadata-driven inputs for modular query construction.
        /// Ensure that aliasing and join conditions are properly scoped to avoid ambiguous references.
        /// </remarks>
        public static string InnerJoinAliasCommand(JoinMetadata SelectWithAlias, string From, string InnerJoin, JoinMetadata OnLeft, JoinMetadata OnRight, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT " + SelectWithAlias.Table + "." + SelectWithAlias.Column);
            cmd.Append(" AS '" + SelectWithAlias.Alias + "'");
            cmd.Append(" FROM " + From);
            cmd.Append("INNER JOIN " + InnerJoin);
            cmd.Append(" ON " + OnLeft.Table + "." + OnLeft.Column + "=" + OnRight.Table + "." + OnRight.Column);

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(";");
            return cmd.ToString();
        }
        /// <summary>
        /// Constructs a SQL inner join command with multiple aliased select columns.
        /// </summary>
        /// <param name="SelectWithAlias">An array of metadata describing the columns to select and alias.</param>
        /// <param name="From">The base table to select from.</param>
        /// <param name="InnerJoin">The table to join with.</param>
        /// <param name="OnLeft">The left-side join condition metadata.</param>
        /// <param name="OnRight">The right-side join condition metadata.</param>
        /// <param name="Where">An optional WHERE clause to filter results.</param>
        /// <returns>A complete SQL inner join command string.</returns>
        /// <remarks>
        /// This method builds a SQL statement using metadata-driven inputs for modular query construction.
        /// It supports multiple aliased columns and ensures proper comma separation in the SELECT clause.
        /// </remarks>
        public static string InnerJoinAliasCommand(JoinMetadata[] SelectWithAlias, string From, string InnerJoin, JoinMetadata OnLeft, JoinMetadata OnRight, string Where = "")
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT ");

            for (int a = 0; a < SelectWithAlias.Length; a++)
            {
                cmd.Append(SelectWithAlias[a].Table + "." + SelectWithAlias[a].Column);
                cmd.Append(" AS '" + SelectWithAlias[a].Alias + "'");

                if (a < SelectWithAlias.Length - 1)
                    cmd.Append(", ");
            }
            cmd.Append(" FROM " + From);
            cmd.Append(" INNER JOIN " + InnerJoin);
            cmd.Append(" ON " + OnLeft.Table + "." + OnLeft.Column + "=" + OnRight.Table + "." + OnRight.Column);

            if (Where != "")
                cmd.Append(" WHERE " + Where);
            cmd.Append(";");
            return cmd.ToString();
        }
        /// <summary>
        /// Generates a SQL command string to truncate all rows from the specified table.
        /// </summary>
        /// <param name="Table">
        /// The name of the table to truncate. This should be a valid SQL table identifier.
        /// </param>
        /// <returns>
        /// A <c>string</c> containing the SQL <c>TRUNCATE TABLE</c> command for the given table.
        /// </returns>
        /// <warning>
        /// This command will irreversibly delete all data from the table without triggering <c>DELETE</c> triggers or logging individual row deletions.
        /// Use with extreme caution, especially in production environments.
        /// </warning>
        public static string TruncateCommand(string Table)
        {
            return "TRUNCATE TABLE " + Table + ";";
        }
        /// <summary>
        /// Appends an ORDER BY clause to an existing SQL command string.
        /// </summary>
        /// <param name="CommandString">The base SQL command string to modify.</param>
        /// <param name="OrderBy">The column name to order by.</param>
        /// <param name="OrderMode">The ordering mode (e.g., ASC or DESC).</param>
        /// <returns>A modified SQL command string with an appended ORDER BY clause.</returns>
        /// <remarks>
        /// If the original command ends with a semicolon, it is removed before appending the ORDER BY clause.
        /// This ensures syntactic correctness and avoids malformed SQL statements.
        /// </remarks>
        public static string AppendOrderBy(string CommandString, string OrderBy, MySQLOrderBy OrderMode)
        {
            var cmd = new StringBuilder();
            string lastChar = CommandString.Substring(CommandString.Length - 1);

            if (lastChar == ";")
                cmd.Append(CommandString.Substring(0, CommandString.Length - 1));

            cmd.Append(" ORDER BY " + OrderBy + " " + OrderMode.ToString());
            cmd.Append(";");
            return cmd.ToString();
        }

        #region PRIVATE
        /// <summary>
        /// Formats a value for safe inclusion in a SQL statement based on its MySQL data type,
        /// escaping single quotes when necessary.
        /// </summary>
        /// <param name="dType">
        /// The MySQL data type of the value.
        /// </param>
        /// <param name="value">
        /// The value to format.
        /// </param>
        /// <returns>
        /// The formatted value as a string.
        /// </returns>
        internal static string SQLSafeValue(MySQLDataType dType, string value)
        {
            if (value != null)
                value = value.Replace("'", "''");

            switch (dType)
            {
                case MySQLDataType.Char:
                case MySQLDataType.VarChar:
                case MySQLDataType.TinyText:
                case MySQLDataType.Text:
                case MySQLDataType.LongText:
                case MySQLDataType.MediumText:
                case MySQLDataType.Date:
                case MySQLDataType.DateTime:
                case MySQLDataType.Timestamp:
                case MySQLDataType.Time:
                case MySQLDataType.Year:
                    return "'" + value + "'";
                default:
                    return value;
            }
        }
        #endregion
    }
}
