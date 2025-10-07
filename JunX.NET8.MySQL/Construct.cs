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
        /// Constructs a SQL <c>SELECT *</c> command using the enum type name as the table name.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing the target table.</typeparam>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter results. If omitted or blank, no filter is applied.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>SELECT *</c> command string targeting the table represented by <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// Uses <c>typeof(T).Name</c> to resolve the table name from the enum type.
        /// </remarks>
        public static string SelectAllCommand<T>(string Where="") where T : Enum
        {
            if (string.IsNullOrWhiteSpace(Where))
                return "SELECT * FROM " + typeof(T).Name + ";";
            else
                return "SELECT * FROM " + typeof(T).Name + " WHERE " + Where + ";";
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
        /// Constructs a SQL <c>SELECT</c> command using an enum value for the column name and the enum type name as the table name.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL metadata tokens.</typeparam>
        /// <param name="Select">The enum value representing the column to select.</param>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter results. If omitted or blank, no filter is applied.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>SELECT</c> command string targeting the table represented by <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// Uses <c>Select.ToString()</c> for the column name and <c>typeof(<typeparamref name="T"/>).Name</c> for the table name.
        /// </remarks>
        public static string SelectCommand<T>(T Select, string Where = "") where T : Enum
        {
            if (string.IsNullOrWhiteSpace(Where))
                return "SELECT " + Select.ToString() + " FROM " + typeof(T).Name + ";";
            else
                return "SELECT " + Select.ToString() + " FROM " + typeof(T).Name + " WHERE " + Where + ";";
        }
        /// <summary>
        /// Constructs a SQL <c>SELECT</c> command using a sequence of column names and a specified table name.
        /// </summary>
        /// <param name="Select">A sequence of column names to include in the <c>SELECT</c> clause.</param>
        /// <param name="From">The name of the table to query.</param>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter results. If omitted or blank, no filter is applied.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>SELECT</c> command string targeting the specified table.
        /// </returns>
        /// <remarks>
        /// Column names are joined with commas. The <c>WHERE</c> clause is appended only if non-blank.
        /// </remarks>
        public static string SelectCommand(IEnumerable<string> Select, string From, string Where = "")
        {
            var cmd = new StringBuilder();
            int a = 0;

            cmd.Append("SELECT ");
            foreach(string s in Select)
            {
                if (a > 0)
                    cmd.Append(", ");
                cmd.Append(s);
                a++;
            }
            cmd.Append(" FROM " + From);

            if (!string.IsNullOrWhiteSpace(Where))
                cmd.Append(" WHERE " + Where);
            return cmd.ToString() + ";";
        }
        /// <summary>
        /// Constructs a SQL <c>SELECT</c> command using enum values for column names and the enum type name as the table name.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL metadata tokens.</typeparam>
        /// <param name="Select">A sequence of enum values representing the columns to include in the <c>SELECT</c> clause.</param>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter results. If omitted or blank, no filter is applied.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>SELECT</c> command string targeting the table represented by <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// Column names are extracted using <c>ToString()</c> on each enum value. Table name is resolved via <c>typeof(T).Name</c>.
        /// </remarks>
        public static string SelectCommand<T>(IEnumerable<T> Select, string Where = "") where T: Enum
        {
            var cmd = new StringBuilder();
            int x = 0;

            cmd.Append("SELECT ");
            foreach(T t in Select)
            {
                if (x > 0)
                    cmd.Append(", ");

                cmd.Append(t.ToString());
                x++;
            }
            cmd.Append(" FROM " + typeof(T).Name);

            if (!string.IsNullOrWhiteSpace(Where))
                cmd.Append(" WHERE " + Where);

            return cmd.ToString() + ";";
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
        /// Constructs a SQL <c>SELECT</c> command using a single enum-based column and an alias, targeting the enum type name as the table.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL column metadata.</typeparam>
        /// <param name="AliasMetadata">A metadata token pairing the column name with its alias.</param>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter results. If omitted or blank, no filter is applied.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>SELECT</c> command string with aliasing, targeting the table represented by <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// The column name is resolved via <c>ToString()</c> on the enum value. The alias is enclosed in single quotes for SQL compatibility.
        /// </remarks>
        public static string SelectAliasCommand<T>(GenericSelectAsMetadata<T> AliasMetadata, string Where="") where T : Enum
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT " + AliasMetadata.Column.ToString() + " AS '" + AliasMetadata.Alias + "'");
            cmd.Append(" FROM " + typeof(T).Name);

            if (!string.IsNullOrWhiteSpace(Where))
                cmd.Append(" WHERE " + Where);

            return cmd.ToString() + ";";
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
        public static string SelectAliasCommand(IEnumerable<SelectAsMetadata> AliasMetadata, string From, string Where = "")
        {
            var cmd = new StringBuilder();
            int a = 0;

            cmd.Append("SELECT ");

            foreach(SelectAsMetadata AM in AliasMetadata)
            {
                if (a > 0)
                    cmd.Append(", ");
                cmd.Append(AM.Column + " AS '" + AM.Alias + "'");
                a++;
            }

            cmd.Append(" FROM " + From);

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(";");
            return cmd.ToString();
        }
        /// <summary>
        /// Constructs a SQL <c>SELECT</c> command using a sequence of enum-based column and alias pairs, targeting the enum type name as the table.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL column metadata.</typeparam>
        /// <param name="AliasMetadata">A sequence of metadata tokens pairing each column with its alias.</param>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter results. If omitted or blank, no filter is applied.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>SELECT</c> command string with aliasing, targeting the table represented by <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// Each column name is resolved via <c>ToString()</c> on the enum value and aliased using SQL <c>AS</c> syntax. The table name is derived from <c>typeof(T).Name</c>.
        /// </remarks>
        public static string SelectAliasCommand<T>(IEnumerable<GenericSelectAsMetadata<T>> AliasMetadata, string Where="") where T : Enum
        {
            var cmd = new StringBuilder();
            int a = 0;

            cmd.Append("SELECT ");
            foreach(GenericSelectAsMetadata<T> AM in AliasMetadata)
            {
                if (a > 0)
                    cmd.Append(", ");
                cmd.Append(AM.Column.ToString() + " AS '" + AM.Alias + "'");
                a++;
            }
            cmd.Append(" FROM " + typeof(T).Name);

            if (!string.IsNullOrWhiteSpace(Where))
                cmd.Append(" WHERE " + Where);
            return cmd.ToString() + ";";
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
        /// Constructs a SQL <c>SELECT</c> command using a single enum-based column with aliasing, an optional <c>WHERE</c> clause, and an <c>ORDER BY</c> directive.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL column metadata.</typeparam>
        /// <param name="AliasMetadata">A metadata token pairing the selected column with its alias.</param>
        /// <param name="OrderBy">The enum value representing the column to sort by.</param>
        /// <param name="OrderMode">The sort direction, typically <c>ASC</c> or <c>DESC</c>, defined by <see cref="MySQLOrderBy"/>.</param>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter results. If omitted or blank, no filter is applied.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>SELECT</c> command string with aliasing and ordering, targeting the table represented by <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// The column and table names are resolved via <c>ToString()</c> and <c>typeof(T).Name</c>. Aliases are enclosed in single quotes for SQL compatibility.
        /// </remarks>
        public static string SelectAliasOrderByCommand<T>(GenericSelectAsMetadata<T> AliasMetadata, T OrderBy, MySQLOrderBy OrderMode, string Where="") where T : Enum
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT " + AliasMetadata.Column.ToString() + " AS '" + AliasMetadata.Alias + "'");
            cmd.Append(" FROM " + typeof(T).Name);

            if (!string.IsNullOrWhiteSpace(Where))
                cmd.Append(" WHERE " + Where);

            cmd.Append(" ORDER BY " + OrderBy.ToString() + " " + OrderMode.ToString());
            return cmd.ToString() + ";";

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
        public static string SelectAliasOrderByCommand(IEnumerable<SelectAsMetadata> AliasMetadata, string From, string OrderBy, MySQLOrderBy OrderMode, string Where = "")
        {
            var cmd = new StringBuilder();
            int a = 0;

            cmd.Append("SELECT ");

            foreach(SelectAsMetadata AM in AliasMetadata)
            {
                if (a > 0)
                    cmd.Append(", ");
                cmd.Append(AM.Column + " AS '" +  AM.Alias + "'");
                a++;
            }

            cmd.Append(" FROM " + From);

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(" ORDER BY " + OrderBy + " " + OrderMode.ToString());
            cmd.Append(";");
            return cmd.ToString();
        }
        /// <summary>
        /// Constructs a SQL <c>SELECT</c> command using a sequence of enum-based column and alias pairs, with optional filtering and ordering.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL column metadata.</typeparam>
        /// <param name="AliasMetadata">A sequence of metadata tokens pairing each column with its alias.</param>
        /// <param name="OrderBy">The enum value representing the column to sort by.</param>
        /// <param name="OrderMode">The sort direction, defined by <see cref="MySQLOrderBy"/> (e.g., <c>ASC</c> or <c>DESC</c>).</param>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter results. If omitted or blank, no filter is applied.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>SELECT</c> command string with aliasing, filtering, and ordering, targeting the table represented by <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// Column names are resolved via <c>ToString()</c> on each enum value. Aliases are applied using SQL <c>AS</c> syntax. The table name is derived from <c>typeof(T).Name</c>.
        /// </remarks>
        public static string SelectAliasOrderByCommand<T>(IEnumerable<GenericSelectAsMetadata<T>> AliasMetadata, T OrderBy, MySQLOrderBy OrderMode, string Where="") where T: Enum
        {
            var cmd = new StringBuilder();
            int a = 0;

            cmd.Append("SELECT ");

            foreach(GenericSelectAsMetadata<T> AM in AliasMetadata)
            {
                if (a > 0)
                    cmd.Append(", ");
                cmd.Append(AM.Column.ToString() + " AS '" + AM.Alias + "'");
                a++;
            }

            cmd.Append(" FROM " + typeof(T).Name);

            if (!string.IsNullOrWhiteSpace(Where))
                cmd.Append(" WHERE " + Where);

            cmd.Append(" ORDER BY " + OrderBy.ToString() + " " + OrderMode.ToString());
            return cmd.ToString() + ";";
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
        public static string SelectDistinctCommand(IEnumerable<string> SelectDistinct, string From)
        {
            var cmd = new StringBuilder();
            int a = 0;

            cmd.Append("SELECT DISTINCT ");

            foreach(string SD in SelectDistinct)
            {
                if (a > 0)
                    cmd.Append(", ");
                cmd.Append(SD);
                a++;
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
        /// Constructs a SQL <c>SELECT</c> command using a single enum-based column, with optional filtering and ordering.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL column metadata.</typeparam>
        /// <param name="Select">The enum value representing the column to include in the <c>SELECT</c> clause.</param>
        /// <param name="OrderBy">The enum value representing the column to sort by.</param>
        /// <param name="OrderMode">The sort direction, defined by <see cref="MySQLOrderBy"/> (e.g., <c>ASC</c> or <c>DESC</c>).</param>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter results. If omitted or blank, no filter is applied.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>SELECT</c> command string with filtering and ordering, targeting the table represented by <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// The column and table names are resolved via <c>ToString()</c> and <c>typeof(T).Name</c>. The <c>ORDER BY</c> clause is appended using SQL syntax.
        /// </remarks>
        public static string SelectOrderByCommand<T>(T Select, T OrderBy, MySQLOrderBy OrderMode, string Where="") where T : Enum
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT " + Select.ToString() + " FROM " + typeof(T).Name);

            if (!string.IsNullOrWhiteSpace(Where))
                cmd.Append(" WHERE " + Where);

            cmd.Append(" ORDER BY " + OrderBy.ToString() + " " + OrderMode.ToString());
            return cmd.ToString() + ";";
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
        public static string SelectOrderByCommand(IEnumerable<string> Select, string From, string OrderBy, MySQLOrderBy OrderMode, string Where = "")
        {
            var cmd = new StringBuilder();
            int a = 0;

            cmd.Append("SELECT ");

            foreach(string s in Select)
            {
                if (a > 0)
                    cmd.Append(", ");
                cmd.Append(s);
                a++;
            }
            
            cmd.Append(" FROM " + From);
            cmd.Append(" ORDER BY " + OrderBy + " " + OrderMode.ToString());
            cmd.Append(";");

            return cmd.ToString();
        }
        /// <summary>
        /// Constructs a SQL <c>SELECT</c> command using a sequence of enum-based column names, with optional filtering and ordering.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL column metadata.</typeparam>
        /// <param name="Select">A sequence of enum values representing the columns to include in the <c>SELECT</c> clause.</param>
        /// <param name="OrderBy">The enum value representing the column to sort by.</param>
        /// <param name="OrderMode">The sort direction, defined by <see cref="MySQLOrderBy"/> (e.g., <c>ASC</c> or <c>DESC</c>).</param>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter results. If omitted or blank, no filter is applied.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>SELECT</c> command string with filtering and ordering, targeting the table represented by <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// Column names are resolved via <c>ToString()</c> on each enum value. The table name is derived from <c>typeof(T).Name</c>. The <c>ORDER BY</c> clause is appended using SQL syntax.
        /// </remarks>
        public static string SelectOrderByCommand<T>(IEnumerable<T> Select, T OrderBy, MySQLOrderBy OrderMode, string Where="")where T : Enum
        {
            var cmd = new StringBuilder();
            int a = 0;

            cmd.Append("SELECT ");

            foreach(T t in Select)
            {
                if (a > 0)
                    cmd.Append(", ");
                cmd.Append(t.ToString());
                a++;
            }

            cmd.Append(" FROM " + typeof(T).Name);

            if (!string.IsNullOrWhiteSpace(Where))
                cmd.Append(" WHERE " + Where);

            cmd.Append(" ORDER BY " + OrderBy.ToString() + " " + OrderMode.ToString());
            return cmd.ToString() + ";";
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
        /// Constructs a SQL <c>INSERT INTO</c> command using a single enum-based column, its data type, and value.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL column metadata.</typeparam>
        /// <param name="InsertMetadata">A metadata token containing the column, data type, and value to insert.</param>
        /// <returns>
        /// A formatted SQL <c>INSERT INTO</c> command string targeting the table represented by <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// The column name is resolved via <c>ToString()</c> on the enum value. The value is formatted using <c>SQLSafeValue</c> to ensure type-safe insertion.
        /// </remarks>
        public static string InsertIntoCommand<T>(GenericInsertColumnMetadata<T> InsertMetadata) where T : Enum
        {
            var cmd = new StringBuilder();

            cmd.Append("INSERT INTO " + typeof(T).Name + " (");
            cmd.Append(InsertMetadata.Column.ToString() + ") VALUES (");
            cmd.Append(SQLSafeValue(InsertMetadata.DataType, InsertMetadata.Value) + ")");

            return cmd.ToString() + ";";
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
        public static string InsertIntoCommand(string InsertInto, IEnumerable<InsertColumnMetadata> InsertMetadata)
        {
            var cmd = new StringBuilder();
            int a = 0;

            cmd.Append("INSERT INTO " + InsertInto + " (");

            foreach(InsertColumnMetadata IM in  InsertMetadata)
            {
                if (a > 0)
                    cmd.Append(", ");
                cmd.Append(IM.Column);
                a++;
            }

            cmd.Append(") VALUES (");

            a = 0;
            foreach(InsertColumnMetadata IM in InsertMetadata)
            {
                if (a > 0)
                    cmd.Append(", ");
                cmd.Append(SQLSafeValue(IM.DataType, IM.Value));
                a++;
            }
            cmd.Append(");");

            return cmd.ToString();
        }
        /// <summary>
        /// Constructs a SQL <c>INSERT INTO</c> command using a sequence of enum-based column metadata tokens.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL column metadata.</typeparam>
        /// <param name="InsertMetadata">A sequence of metadata tokens, each containing a column, data type, and value to insert.</param>
        /// <returns>
        /// A formatted SQL <c>INSERT INTO</c> command string targeting the table represented by <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// Column names are resolved via <c>ToString()</c> on each enum value. Values are formatted using <c>SQLSafeValue</c> to ensure type-safe insertion. The table name is derived from <c>typeof(T).Name</c>.
        /// </remarks>
        public static string InsertIntoCommand<T>(IEnumerable<GenericInsertColumnMetadata<T>> InsertMetadata) where T : Enum
        {
            var cmd = new StringBuilder();
            int a = 0;

            cmd.Append("INSERT INTO " + typeof(T).Name + " (");

            foreach(GenericInsertColumnMetadata<T> IM in InsertMetadata)
            {
                if (a > 0)
                    cmd.Append(", ");
                cmd.Append(IM.Column.ToString());
                a++;
            }

            cmd.Append(") VALUES (");
            a = 0;
            foreach(GenericInsertColumnMetadata<T> IM in InsertMetadata)
            {
                if(a>0)
                    cmd.Append(", ");
                cmd.Append(SQLSafeValue(IM.DataType, IM.Value));
                a++;
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
        /// Constructs a SQL <c>UPDATE</c> command using a single enum-based column, its data type, and updated value.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL column metadata.</typeparam>
        /// <param name="UpdateMetadata">A metadata token containing the column, data type, and new value to apply.</param>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter which rows are updated. If omitted or blank, all rows are affected.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>UPDATE</c> command string targeting the table represented by <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// The column name is resolved via <c>ToString()</c> on the enum value. The value is formatted using <c>SQLSafeValue</c> to ensure type-safe updates. The table name is derived from <c>typeof(T).Name</c>.
        /// </remarks>
        public static string UpdateCommand<T>(GenericUpdateColumnMetadata<T> UpdateMetadata, string Where="") where T : Enum
        {
            var cmd = new StringBuilder();

            cmd.Append("UPDATE " + typeof(T).Name + " SET ");
            cmd.Append(UpdateMetadata.Column + "=");
            cmd.Append(SQLSafeValue(UpdateMetadata.DataType, UpdateMetadata.Value));

            if (!string.IsNullOrWhiteSpace(Where))
                cmd.Append(" WHERE " + Where);
            return cmd.ToString() + ";";
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
        public static string UpdateCommand(string Update, IEnumerable<UpdateColumnMetadata> UpdateMetadata, string Where = "")
        {
            var cmd = new StringBuilder();
            int a = 0;

            cmd.Append("UPDATE " + Update + " SET ");

            foreach(UpdateColumnMetadata UM in  UpdateMetadata)
            {
                if (a > 0)
                    cmd.Append(", ");
                cmd.Append(UM.Column + "=");
                cmd.Append(SQLSafeValue(UM.DataType, UM.Value));
                a++;
            }

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(";");

            return cmd.ToString();
        }
        /// <summary>
        /// Constructs a SQL <c>UPDATE</c> command using a sequence of enum-based column metadata tokens, with optional filtering.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL column metadata.</typeparam>
        /// <param name="UpdateMetadata">A sequence of metadata tokens, each containing a column, data type, and updated value.</param>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter which rows are updated. If omitted or blank, all rows are affected.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>UPDATE</c> command string targeting the table represented by <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// Column names are resolved via <c>ToString()</c> on each enum value. Values are formatted using <c>SQLSafeValue</c> to ensure type-safe updates. The table name is derived from <c>typeof(T).Name</c>.
        /// </remarks>
        public static string UpdateCommand<T>(IEnumerable<GenericUpdateColumnMetadata<T>> UpdateMetadata, string Where="") where T : Enum
        {
            var cmd = new StringBuilder();
            int a = 0;

            cmd.Append("UPDATE " + typeof(T).Name + " SET ");

            foreach(GenericUpdateColumnMetadata<T> UM in UpdateMetadata)
            {
                if(a > 0)
                    cmd.Append(", ");
                cmd.Append(UM.Column + "=");
                cmd.Append(SQLSafeValue(UM.DataType, UM.Value));
                a++;
            }

            if (!string.IsNullOrWhiteSpace(Where))
                cmd.Append(" WHERE " + Where);
            return cmd.ToString() + ";";
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
        /// Constructs a SQL <c>DELETE FROM</c> command targeting the table represented by the enum type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL table metadata.</typeparam>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter which rows are deleted. If omitted or blank, all rows will be deleted.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>DELETE FROM</c> command string.
        /// </returns>
        /// <remarks>
        /// The table name is derived from <c>typeof(T).Name</c>.
        /// </remarks>
        /// <warning>
        /// ⚠️ If <paramref name="Where"/> is omitted or empty, the command will delete all rows from the table. Use with extreme caution.
        /// </warning>
        public static string DeleteCommand<T>(string Where = "")
        {
            if (!string.IsNullOrWhiteSpace(Where))
                return "DELETE FROM " + typeof(T).Name + " WHERE " + Where + ";";
            else
                return "DELETE FROM " + typeof(T).Name + ";";
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
            cmd.Append(" INNER JOIN " + InnerJoin);
            cmd.Append(" ON " + OnLeft.Table + "." + OnLeft.Column + "=" + OnRight.Table + "." + OnRight.Column);

            if (Where != "")
                cmd.Append(" WHERE " + Where);

            cmd.Append(";");
            return cmd.ToString();
        }
        /// <summary>
        /// Constructs a SQL <c>INNER JOIN</c> command using strongly typed metadata for the source and joined tables.
        /// </summary>
        /// <typeparam name="From">An <see cref="Enum"/> type representing the source table's column metadata.</typeparam>
        /// <typeparam name="Join">An <see cref="Enum"/> type representing the joined table's column metadata.</typeparam>
        /// <param name="Select">
        /// A metadata token representing the column to select, including its inferred table name and optional alias.
        /// </param>
        /// <param name="OnLeft">
        /// A metadata token representing the join key from the source table.
        /// </param>
        /// <param name="OnRight">
        /// A metadata token representing the join key from the joined table.
        /// </param>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter the joined result. If omitted or blank, no filtering is applied.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>INNER JOIN</c> command string that selects a single column and joins two tables on matching keys.
        /// </returns>
        /// <remarks>
        /// Table names are inferred from <c>typeof(T).Name</c> for each metadata token. Column names are resolved via <c>ToString()</c> on the enum values. Aliases are supported via the <c>Alias</c> property of <paramref name="Select"/>, though not yet applied in this implementation.
        /// </remarks>
        public static string InnerJoinCommand<From, Join>(GenericJoinMetadata<Enum> Select, GenericJoinMetadata<From> OnLeft, GenericJoinMetadata<Join> OnRight, string Where="") where From : Enum where Join : Enum
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT " + Select.Table + "." + Select.Column);
            cmd.Append(" FROM " + typeof(From).Name);
            cmd.Append(" INNER JOIN " + typeof(Join).Name);
            cmd.Append(" ON " + OnLeft.Table + "." + OnLeft.Column.ToString() + "=" + OnRight.Table + "." + OnRight.Column.ToString());

            if (!string.IsNullOrWhiteSpace(Where))
                cmd.Append(" WHERE " + Where);
            return cmd.ToString() + ";";
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
        public static string InnerJoinCommand(IEnumerable<JoinMetadata> Select, string From, string InnerJoin, JoinMetadata OnLeft, JoinMetadata OnRight, string Where = "")
        {
            var cmd = new StringBuilder();
            int a = 0;

            cmd.Append("SELECT ");

            foreach(JoinMetadata S in  Select)
            {
                if (a > 0)
                    cmd.Append(", ");
                cmd.Append(S.Table + "." + S.Column.ToString());
                a++;
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
        /// Constructs a SQL <c>INNER JOIN</c> command that selects a single column with an alias and joins two tables on matching keys.
        /// </summary>
        /// <typeparam name="From">An <see cref="Enum"/> type representing the source table's column metadata.</typeparam>
        /// <typeparam name="Join">An <see cref="Enum"/> type representing the joined table's column metadata.</typeparam>
        /// <param name="SelectWithAlias">
        /// A metadata token representing the column to select, including its inferred table name and alias to apply in the result.
        /// </param>
        /// <param name="OnLeft">
        /// A metadata token representing the join key from the source table.
        /// </param>
        /// <param name="OnRight">
        /// A metadata token representing the join key from the joined table.
        /// </param>
        /// <param name="Where">
        /// An optional <c>WHERE</c> clause to filter the joined result. If omitted or blank, no filtering is applied.
        /// </param>
        /// <returns>
        /// A formatted SQL <c>INNER JOIN</c> command string that selects one aliased column and joins two tables on matching keys.
        /// </returns>
        /// <remarks>
        /// Table names are inferred from <c>typeof(T).Name</c>. Column names are resolved via <c>ToString()</c> on the enum values. The alias is applied using the <c>AS</c> keyword. This method assumes that each metadata token exposes a <c>Condition</c> property representing a fully qualified column reference for join predicates.
        /// </remarks>
        public static string InnerJoinAliasCommand<From, Join>(GenericJoinMetadata<Enum> SelectWithAlias, GenericJoinMetadata<From> OnLeft, GenericJoinMetadata<Join> OnRight, string Where="") where From:Enum where Join : Enum
        {
            var cmd = new StringBuilder();

            cmd.Append("SELECT " + SelectWithAlias.Table + "." + SelectWithAlias.Column.ToString() + " AS '" + SelectWithAlias.Alias + "'");
            cmd.Append(" FROM " + typeof(From).Name);
            cmd.Append(" INNER JOIN " + typeof(Join).Name);
            cmd.Append(" ON " + OnLeft.Condition + "=" + OnRight.Condition);

            if (!string.IsNullOrWhiteSpace(Where))
                cmd.Append(" WHERE " + Where);
            return cmd.ToString() + ";";
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
        public static string InnerJoinAliasCommand(IEnumerable<JoinMetadata> SelectWithAlias, string From, string InnerJoin, JoinMetadata OnLeft, JoinMetadata OnRight, string Where = "")
        {
            var cmd = new StringBuilder();
            int a = 0;

            cmd.Append("SELECT ");

            foreach(JoinMetadata SWA in SelectWithAlias)
            {
                if (a > 0)
                    cmd.Append(", ");
                cmd.Append(SWA.Table + "." + SWA.Column + " AS '" + SWA.Alias + "'");
                a++;
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
        /// Constructs a SQL <c>TRUNCATE TABLE</c> command targeting the table represented by the enum type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL table metadata.</typeparam>
        /// <returns>
        /// A formatted SQL <c>TRUNCATE TABLE</c> command string.
        /// </returns>
        /// <remarks>
        /// The table name is inferred from <c>typeof(T).Name</c>.
        /// ⚠️ <b>Warning:</b> <c>TRUNCATE TABLE</c> removes all rows from the table without logging individual row deletions and cannot be rolled back in most database systems. Use with extreme caution.
        /// </remarks>
        public static string TruncateCommand<T>() where T: Enum
        {
            return "TRUNCATE TABLE " + typeof(T).Name + ";";
        }
        /// <summary>
        /// Constructs a SQL <c>TRUNCATE TABLE</c> command targeting the table represented by the enum type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">An <see cref="Enum"/> type representing SQL table metadata.</typeparam>
        /// <returns>
        /// A formatted SQL <c>TRUNCATE TABLE</c> command string.
        /// </returns>
        /// <remarks>
        /// The table name is inferred from <c>typeof(T).Name</c>.
        /// ⚠️ <b>Warning:</b> <c>TRUNCATE TABLE</c> removes all rows from the table without logging individual row deletions and cannot be rolled back in most database systems. Use with extreme caution.
        /// </remarks>
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
