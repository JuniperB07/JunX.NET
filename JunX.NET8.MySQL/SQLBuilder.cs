using JunX.NET8.MySQL;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.MySQL
{
    /// <summary>
    /// Provides a fluent interface for constructing SQL command strings.
    /// </summary>
    /// <remarks>
    /// Designed for modular query composition using chainable methods like <c>AddWhere</c> and <c>AddOrderBy</c>.
    /// Use <c>ToString()</c> to retrieve the final SQL command.
    /// </remarks>
    public static class SQLBuilder
    {
        /// <summary>
        /// Provides a fluent interface for constructing SQL SELECT command strings.
        /// </summary>
        public class SQLSelect
        {
            private StringBuilder cmd = new StringBuilder();
            private bool _hasSelectColumns = false;
            private bool _hasWhere = false;

            /// <summary>
            /// Appends a column name to the SELECT clause of the SQL query.
            /// </summary>
            /// <param name="ColumnName">The name of the column to include in the result set.</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method builds the SELECT clause by appending column names one at a time.
            /// It automatically inserts the <c>SELECT</c> keyword on the first call and separates subsequent columns with commas.
            /// Call this method repeatedly to include multiple columns in the query.
            /// </remarks>
            public SQLSelect Column(string ColumnName)
            {
                if (!_hasSelectColumns)
                {
                    cmd.Append("SELECT ");
                    _hasSelectColumns = true;
                }
                else
                    cmd.Append(", ");
                cmd.Append(ColumnName);
                return this;
            }
            /// <summary>
            /// Appends multiple column names to the SELECT clause of the SQL statement.
            /// </summary>
            /// <param name="ColumnNames">An array of column names to include in the result set.</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method initializes the SELECT clause with <c>SELECT</c> on first use, and appends each column name separated by commas.
            /// It ensures correct syntax by avoiding redundant commas and supports dynamic column selection.
            /// Use this for unqualified column names.
            /// </remarks>
            public SQLSelect Columns(string[] ColumnNames)
            {
                if (!_hasSelectColumns)
                {
                    cmd.Append("SELECT ");
                    _hasSelectColumns = true;
                }

                for(int i=0; i<ColumnNames.Length; i++)
                {
                    if (i > 0)
                        cmd.Append(", ");

                    cmd.Append(ColumnNames[i]);
                }

                return this;
            }
            /// <summary>
            /// Appends a fully qualified column name to the SELECT clause using the specified table and column.
            /// </summary>
            /// <param name="Table">The name of the table that owns the column.</param>
            /// <param name="ColumnName">The name of the column to include in the result set.</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method builds the SELECT clause by appending <c>[Table].[ColumnName]</c>.
            /// It automatically inserts the <c>SELECT</c> keyword on the first call and separates subsequent columns with commas.
            /// Use this for disambiguating columns in multi-table queries or joins.
            /// </remarks>
            public SQLSelect Column(string Table, string ColumnName)
            {
                if (!_hasSelectColumns)
                {
                    cmd.Append("SELECT ");
                    _hasSelectColumns = true;
                }
                else
                    cmd.Append(", ");
                cmd.Append(Table + "." + ColumnName);
                return this;
            }
            /// <summary>
            /// Appends multiple fully qualified column names to the SELECT clause using metadata.
            /// </summary>
            /// <param name="QualifiedColumns">An array of <see cref="SelectMetadata"/> objects representing table-qualified columns.</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method initializes the SELECT clause with <c>SELECT</c> on first use, and appends each <c>[Table].[Column]</c> pair separated by commas.
            /// It ensures correct syntax and supports metadata-driven column selection for multi-table queries or joins.
            /// Use this when building queries dynamically with structured column metadata.
            /// </remarks>
            public SQLSelect Columns(SelectMetadata[] QualifiedColumns)
            {
                if (!_hasSelectColumns)
                {
                    cmd.Append("SELECT ");
                    _hasSelectColumns = true;
                }

                for (int i = 0; i < QualifiedColumns.Length; i++)
                {
                    if (i > 0)
                        cmd.Append(", ");

                    cmd.Append(QualifiedColumns[i].Table + "." + QualifiedColumns[i].Column);
                }

                return this;
            }
            /// <summary>
            /// Appends a FROM clause to the SQL SELECT statement, specifying the source table.
            /// </summary>
            /// <param name="Table">The name of the table to select data from.</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method adds <c>FROM [Table]</c> to the SQL command. 
            /// It should follow one or more <see cref="Column"/> calls to complete the SELECT clause.
            /// </remarks>
            public SQLSelect From(string Table)
            {
                cmd.Append(" FROM " + Table);
                return this;
            }
            /// <summary>
            /// Appends a condition to the WHERE clause of the SQL SELECT statement, optionally prefixed by a logical operator.
            /// </summary>
            /// <param name="Condition">The condition to apply for filtering results.</param>
            /// <param name="LogicalOperator">
            /// The logical operator to prepend before the condition (e.g., <c>AND</c>, <c>OR</c>). 
            /// Defaults to <see cref="MySQLLogicalOperators.None"/> for the first condition.
            /// </param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method builds the WHERE clause incrementally. 
            /// It automatically inserts the <c>WHERE</c> keyword on the first call and prepends logical operators on subsequent calls.
            /// Use this to construct complex filter logic with multiple conditions.
            /// </remarks>
            public SQLSelect Where(string Condition, MySQLLogicalOperators LogicalOperator = MySQLLogicalOperators.None)
            {
                if (!_hasWhere)
                {
                    cmd.Append(" WHERE ");
                    _hasWhere = true;
                }
                else
                    cmd.Append(" " + LogicalOperator.ToString() + " ");
                cmd.Append(Condition);
                return this;
            }
            /// <summary>
            /// Begins a grouped condition block within the WHERE clause by appending an opening parenthesis.
            /// </summary>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// Use this method to start a logical grouping of conditions, such as nested filters or compound expressions.
            /// Pair it with <see cref="EndGroupedWhere"/> to close the group.
            /// Example: <c>.Where("A = 1").StartGroupedWhere().Where("B = 2", AND).Where("C = 3", OR).EndGroupedWhere()</c>
            /// </remarks>
            public SQLSelect StartGroupedWhere()
            {
                cmd.Append("(");
                return this;
            }
            /// <summary>
            /// Ends a grouped condition block within the WHERE clause by appending a closing parenthesis.
            /// </summary>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// Use this method to close a logical grouping of conditions started with <see cref="StartGroupedWhere"/>.
            /// Grouped conditions allow for nested expressions and compound filters in SQL queries.
            /// Ensure that each opening parenthesis has a matching closing call to maintain valid syntax.
            /// </remarks>
            public SQLSelect EndGroupedWhere()
            {
                cmd.Append(") ");
                return this;
            }
            /// <summary>
            /// Begins a SELECT DISTINCT statement for the specified column, eliminating duplicate values.
            /// </summary>
            /// <param name="Column">The name of the column to apply DISTINCT on.</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method initializes the SELECT clause with <c>SELECT DISTINCT [Column]</c>, ensuring only unique values are returned.
            /// It flags the SELECT clause as initialized, preventing redundant keyword insertion in subsequent <see cref="Column"/> calls.
            /// Use this when selecting a single column with uniqueness enforced.
            /// </remarks>
            public SQLSelect Distinct(string Column)
            {
                cmd.Append("SELECT DISTINCT " + Column);
                _hasSelectColumns = true;
                return this;
            }
            /// <summary>
            /// Appends an alias to the most recently added column or expression in the SELECT clause.
            /// </summary>
            /// <param name="Alias">The alias name to assign, enclosed in single quotes.</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method appends <c>AS 'Alias'</c> to the SQL command, allowing you to rename columns or expressions in the result set.
            /// It should be called immediately after a <see cref="Column"/> or expression to ensure proper placement.
            /// </remarks>
            public SQLSelect As(string Alias)
            {
                cmd.Append(" AS '" + Alias + "'");
                return this;
            }
            /// <summary>
            /// Appends an ORDER BY clause to the SQL SELECT statement using the specified column and sort direction.
            /// </summary>
            /// <param name="Column">The name of the column to sort by.</param>
            /// <param name="OrderMode">
            /// The sort direction, specified as a <see cref="MySQLOrderBy"/> value (e.g., <c>ASC</c> or <c>DESC</c>).
            /// </param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method adds <c>ORDER BY [Column] [OrderMode]</c> to the SQL command.
            /// Use it to control the result set's sort order based on one or more columns.
            /// </remarks>
            public SQLSelect OrderBy(string Column, MySQLOrderBy OrderMode)
            {
                cmd.Append(" ORDER BY " + Column + " " + OrderMode.ToString());
                return this;
            }
            /// <summary>
            /// Begins a SELECT statement that returns the minimum value of the specified column.
            /// </summary>
            /// <param name="Column">The name of the column to apply the MIN aggregate function to.</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method initializes the SELECT clause with <c>SELECT MIN([Column])</c>, 
            /// allowing you to retrieve the smallest value from the specified column.
            /// It flags the SELECT clause as initialized, preventing redundant keyword insertion in subsequent column calls.
            /// </remarks>
            public SQLSelect Min(string Column)
            {
                cmd.Append("SELECT MIN(" + Column + ")");
                _hasSelectColumns = true;
                return this;
            }
            /// <summary>
            /// Begins a SELECT statement that returns the maximum value of the specified column.
            /// </summary>
            /// <param name="Column">The name of the column to apply the MAX aggregate function to.</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method initializes the SELECT clause with <c>SELECT MAX([Column])</c>, 
            /// allowing you to retrieve the largest value from the specified column.
            /// It flags the SELECT clause as initialized, preventing redundant keyword insertion in subsequent column calls.
            /// </remarks>
            public SQLSelect Max(string Column)
            {
                cmd.Append("SELECT MAX(" + Column + ")");
                _hasSelectColumns = true;
                return this;
            }
            /// <summary>
            /// Begins a SELECT statement that returns the count of non-null values in the specified column.
            /// </summary>
            /// <param name="Column">The name of the column to apply the COUNT aggregate function to.</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method initializes the SELECT clause with <c>SELECT COUNT([Column])</c>, 
            /// allowing you to retrieve the number of non-null entries in the specified column.
            /// It flags the SELECT clause as initialized, preventing redundant keyword insertion in subsequent column calls.
            /// </remarks>
            public SQLSelect Count(string Column)
            {
                cmd.Append("SELECT COUNT(" + Column + ")");
                _hasSelectColumns = true;
                return this;
            }
            /// <summary>
            /// Begins a SELECT statement that returns the sum of values in the specified column.
            /// </summary>
            /// <param name="Column">The name of the column to apply the SUM aggregate function to.</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method initializes the SELECT clause with <c>SELECT SUM([Column])</c>, 
            /// allowing you to compute the total of numeric values in the specified column.
            /// It flags the SELECT clause as initialized, preventing redundant keyword insertion in subsequent column calls.
            /// </remarks>
            public SQLSelect Sum(string Column)
            {
                cmd.Append("SELECT SUM(" + Column + ")");
                _hasSelectColumns = true;
                return this;
            }
            /// <summary>
            /// Begins a SELECT statement that returns the average value of the specified column.
            /// </summary>
            /// <param name="Column">The name of the column to apply the AVG aggregate function to.</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method initializes the SELECT clause with <c>SELECT AVG([Column])</c>, 
            /// allowing you to compute the mean of numeric values in the specified column.
            /// It flags the SELECT clause as initialized, preventing redundant keyword insertion in subsequent column calls.
            /// </remarks>
            public SQLSelect Avg(string Column)
            {
                cmd.Append("SELECT AVG(" + Column + ")");
                _hasSelectColumns = true;
                return this;
            }
            /// <summary>
            /// Appends an INNER JOIN clause to the SQL SELECT statement using metadata for join conditions.
            /// </summary>
            /// <param name="Table">The name of the table to join.</param>
            /// <param name="Left">Metadata describing the left side of the join condition (table and column).</param>
            /// <param name="Right">Metadata describing the right side of the join condition (table and column).</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method appends <c>INNER JOIN [Table] ON [Left.Table].[Left.Column] = [Right.Table].[Right.Column]</c>
            /// to the SQL command. It enables metadata-driven join construction for clean and modular query building.
            /// </remarks>
            public SQLSelect InnerJoin(string Table, JoinMetadata Left, JoinMetadata Right)
            {
                cmd.Append(" INNER JOIN " + Table);
                cmd.Append(" ON " + Left.Table + "." + Left.Column);
                cmd.Append("=" +  Right.Table + "." + Right.Column);
                return this;
            }
            /// <summary>
            /// Appends a LEFT JOIN clause to the SQL SELECT statement using metadata for join conditions.
            /// </summary>
            /// <param name="Table">The name of the table to join.</param>
            /// <param name="Left">Metadata describing the left side of the join condition (table and column).</param>
            /// <param name="Right">Metadata describing the right side of the join condition (table and column).</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method appends <c>LEFT JOIN [Table] ON [Left.Table].[Left.Column] = [Right.Table].[Right.Column]</c>
            /// to the SQL command. It enables metadata-driven join construction for clean and modular query building.
            /// Use this when you want to include all rows from the left table and matched rows from the right table.
            /// </remarks>
            public SQLSelect LeftJoin(string Table, JoinMetadata Left, JoinMetadata Right)
            {
                cmd.Append(" LEFT JOIN " + Table);
                cmd.Append(" ON " + Left.Table + "." + Left.Column);
                cmd.Append("=" + Right.Table + "." + Right.Column);
                return this;
            }
            /// <summary>
            /// Appends a RIGHT JOIN clause to the SQL SELECT statement using metadata for join conditions.
            /// </summary>
            /// <param name="Table">The name of the table to join.</param>
            /// <param name="Left">Metadata describing the left side of the join condition (table and column).</param>
            /// <param name="Right">Metadata describing the right side of the join condition (table and column).</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method appends <c>RIGHT JOIN [Table] ON [Left.Table].[Left.Column] = [Right.Table].[Right.Column]</c>
            /// to the SQL command. It enables metadata-driven join construction for clean and modular query building.
            /// Use this when you want to include all rows from the right table and matched rows from the left table.
            /// </remarks>
            public SQLSelect RightJoin(string Table, JoinMetadata Left, JoinMetadata Right)
            {
                cmd.Append(" RIGHT JOIN " + Table);
                cmd.Append(" ON " + Left.Table + "." + Left.Column);
                cmd.Append("=" + Right.Table + "." + Right.Column);
                return this;
            }
            /// <summary>
            /// Appends a FULL OUTER JOIN clause to the SQL SELECT statement using metadata for join conditions.
            /// </summary>
            /// <param name="Table">The name of the table to join.</param>
            /// <param name="Left">Metadata describing the left side of the join condition (table and column).</param>
            /// <param name="Right">Metadata describing the right side of the join condition (table and column).</param>
            /// <returns>The current <see cref="SQLSelect"/> instance for fluent chaining.</returns>
            /// <remarks>
            /// This method appends <c>FULL OUTER JOIN [Table] ON [Left.Table].[Left.Column] = [Right.Table].[Right.Column]</c>
            /// to the SQL command. It enables metadata-driven join construction for comprehensive result sets that include all rows from both tables.
            /// Use this when you need to preserve unmatched rows from both sides of the join.
            /// </remarks>
            public SQLSelect FullOuterJoin(string Table, JoinMetadata Left, JoinMetadata Right)
            {
                cmd.Append(" FULL OUTER JOIN " + Table);
                cmd.Append(" ON " + Left.Table + "." + Left.Column);
                cmd.Append("=" + Right.Table + "." + Right.Column);
                return this;
            }


            /// <summary>
            /// Returns the complete SQL command as a string, terminated with a semicolon.
            /// </summary>
            /// <returns>A string representation of the constructed SQL statement.</returns>
            /// <remarks>
            /// This method appends a semicolon to the current SQL command for execution readiness.
            /// Useful for logging, debugging, or passing the final query to a database engine.
            /// </remarks>
            public override string ToString() => cmd.ToString() + ";";
        }
    }
}
