using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.MySQL.SQLBuilders
{
    /// <summary>
    /// Constructs a SQL <c>SELECT</c> command using an enum type to represent a table and its columns.
    /// </summary>
    /// <typeparam name="T">
    /// An <see cref="Enum"/> type where the enum name is treated as the SQL table name, and each enum member represents a column in that table.
    /// </typeparam>
    /// <remarks>
    /// The builder uses a fluent API to compose a <c>SELECT</c> clause. Column names are inferred from <c>ToString()</c> on the enum values.
    /// The table name is inferred from <c>typeof(<typeparamref name="T"/>).Name</c>. This class does not yet include <c>FROM</c> or <c>WHERE</c> clauses — it focuses solely on column selection.
    /// </remarks>
    public class SelectBuilder<T> where T: Enum
    {
        private StringBuilder cmd = new StringBuilder();
        private bool _hasSelectColumns = false;
        private bool _hasWhere = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectBuilder{T}"/> class and begins constructing a SQL <c>SELECT</c> command.
        /// </summary>
        /// <remarks>
        /// The enum type <typeparamref name="T"/> is treated as the SQL table name, and its members represent the columns to be selected.
        /// This constructor starts the command with the <c>SELECT</c> keyword, preparing the builder for column additions.
        /// </remarks>
        public SelectBuilder()
        {
            cmd.Append("SELECT ");
            _hasSelectColumns = false;
            _hasWhere = false;
        }
        /// <summary>
        /// Returns the fully constructed SQL <c>SELECT</c> command as a string, terminated with a semicolon.
        /// </summary>
        /// <returns>
        /// A string representation of the SQL query composed by the current <see cref="SelectBuilder{T}"/> instance.
        /// </returns>
        /// <remarks>
        /// This method finalizes the command for execution or inspection, including all selected columns, joins, and conditions.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ";";
        }


        /// <summary>
        /// Adds a column to the SQL <c>SELECT</c> clause, optionally qualifying it with the table name.
        /// </summary>
        /// <param name="Column">
        /// The enum member representing a column in the table inferred from <typeparamref name="T"/>.
        /// </param>
        /// <param name="IsFullyQualified">
        /// If <c>true</c>, the column is prefixed with the table name (e.g., <c>Table.Column</c>); otherwise, only the column name is used.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method supports both qualified and unqualified column references, enabling compatibility with joins or disambiguation in multi-table queries.
        /// </remarks>
        public SelectBuilder<T> Select(T Column, bool IsFullyQualified=false)
        {
            if (_hasSelectColumns)
                cmd.Append(", ");
            else
                _hasSelectColumns = true;

            if (IsFullyQualified)
                cmd.Append(typeof(T).Name + "." + Column.ToString());
            else
                cmd.Append(Column.ToString());

            return this;
        }
        /// <summary>
        /// Adds multiple columns to the SQL <c>SELECT</c> clause, optionally qualifying each with the table name.
        /// </summary>
        /// <param name="Columns">
        /// A sequence of enum members representing columns in the table inferred from <typeparamref name="T"/>.
        /// </param>
        /// <param name="IsFullyQualified">
        /// If <c>true</c>, each column is prefixed with the table name (e.g., <c>Table.Column</c>); otherwise, only the column name is used.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method supports batch inclusion of metadata-defined fields and enables compatibility with multi-table queries by qualifying column names when needed.
        /// </remarks>
        public SelectBuilder<T> Select(IEnumerable<T> Columns, bool IsFullyQualified=false)
        {
            foreach(T cols in Columns)
            {
                if (_hasSelectColumns)
                    cmd.Append(", ");
                else
                    _hasSelectColumns = true;

                if (IsFullyQualified)
                    cmd.Append(typeof(T).Name + "." +  cols.ToString());
                else
                    cmd.Append(cols.ToString());
            }
            return this;
        }


        /// <summary>
        /// Adds a fully qualified column to the SQL <c>SELECT</c> clause from a secondary enum-based table.
        /// </summary>
        /// <typeparam name="Join">
        /// An <see cref="Enum"/> type representing a secondary table whose name is inferred from <c>typeof(Join).Name</c>.
        /// </typeparam>
        /// <param name="Column">
        /// The enum member representing a column in the secondary table.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method supports multi-table queries by allowing selection of columns from a joined table.
        /// The column is qualified using the table name derived from the enum type <typeparamref name="Join"/>.
        /// </remarks>
        public SelectBuilder<T> Select<Join>(Join Column) where Join : Enum
        {
            if (_hasSelectColumns)
                cmd.Append(", ");
            else
                _hasSelectColumns = true;

            cmd.Append(typeof(Join).Name + "." + Column.ToString());
            return this;
        }
        /// <summary>
        /// Adds multiple fully qualified columns to the SQL <c>SELECT</c> clause from a secondary enum-based table.
        /// </summary>
        /// <typeparam name="Join">
        /// An <see cref="Enum"/> type representing a secondary table whose name is inferred from <c>typeof(Join).Name</c>.
        /// </typeparam>
        /// <param name="Columns">
        /// A sequence of enum members representing columns in the secondary table.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// Each column is prefixed with the table name derived from the enum type <typeparamref name="Join"/>.
        /// This method supports multi-table queries and batch inclusion of metadata-defined fields from joined tables.
        /// </remarks>
        public SelectBuilder<T> Select<Join>(IEnumerable<Join> Columns)
        {
            foreach(Join cols in Columns)
            {
                if (_hasSelectColumns)
                    cmd.Append(", ");
                else
                    _hasSelectColumns = true;

                cmd.Append(typeof(Join).Name + "." + cols.ToString());
            }
            return this;
        }


        /// <summary>
        /// Appends an SQL <c>AS</c> clause to alias the current table or expression with the specified name.
        /// </summary>
        /// <param name="Alias">
        /// The alias to assign, typically used for table or subquery references in SQL statements.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method is useful for disambiguating table names in joins or simplifying references in complex queries.
        /// The alias is enclosed in single quotes to preserve formatting consistency.
        /// </remarks>
        public SelectBuilder<T> As(string Alias)
        {
            cmd.Append(" AS '" + Alias + "'");
            return this;
        }
        /// <summary>
        /// Adds multiple aliased columns to the SQL <c>SELECT</c> clause using metadata-defined mappings, with optional full qualification.
        /// </summary>
        /// <param name="SelectAs">
        /// A sequence of <see cref="AliasMetadata{T}"/> instances, each representing a column and its SQL alias.
        /// </param>
        /// <param name="IsFullyQualified">
        /// If <c>true</c>, each column is prefixed with the table name (e.g., <c>Table.Column AS 'Alias'</c>); otherwise, only the column name is used.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method supports batch aliasing with optional table qualification, enabling readable and disambiguated SQL output.
        /// Useful for multi-table queries, reporting modules, and metadata-driven projections.
        /// </remarks>
        public SelectBuilder<T> SelectAs(IEnumerable<AliasMetadata<T>> SelectAs, bool IsFullyQualified = false)
        {
            foreach(AliasMetadata<T> SA in SelectAs)
            {
                if (_hasSelectColumns)
                    cmd.Append(", ");
                else
                    _hasSelectColumns = true;

                if (IsFullyQualified)
                    cmd.Append(SA.FullyQualified + " AS '" + SA.Alias + "'");
                else
                    cmd.Append(SA.Column.ToString() + " AS '" + SA.Alias + "'");
            }
            return this;
        }
        /// <summary>
        /// Adds multiple aliased columns to the SQL <c>SELECT</c> clause from a secondary enum-based table.
        /// </summary>
        /// <typeparam name="Join">
        /// An <see cref="Enum"/> type representing the joined table whose name is inferred from <c>typeof(Join).Name</c>.
        /// </typeparam>
        /// <param name="SelectAs">
        /// A sequence of <see cref="AliasMetadata{Join}"/> instances, each representing a column and its SQL alias from the joined table.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// Each column is rendered in fully qualified form (e.g., <c>JoinTable.Column AS 'Alias'</c>).
        /// This method supports metadata-driven aliasing for joined tables, improving clarity and disambiguation in multi-table queries.
        /// </remarks>
        public SelectBuilder<T> SelectAs<Join>(IEnumerable<AliasMetadata<Join>> SelectAs) where Join : Enum
        {
            foreach(AliasMetadata<Join> SA in SelectAs)
            {
                if (_hasSelectColumns)
                    cmd.Append(", ");
                else
                    _hasSelectColumns = true;

                cmd.Append(SA.FullyQualified + " AS '" + SA.Alias + "'");
            }
            return this;
        }


        /// <summary>
        /// Appends the SQL <c>FROM</c> clause using the name of the enum type as the table name.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// The table name is inferred from <c>typeof(<typeparamref name="T"/>).Name</c>, treating the enum type itself as the SQL table.
        /// This method finalizes the selection context before applying filters or executing the query.
        /// </remarks>
        public SelectBuilder<T> From
        {
            get
            {
                cmd.Append(" FROM " + typeof(T).Name);
                return this;
            }
        }


        /// <summary>
        /// Appends a SQL join clause using the specified join mode and column relationship between two enum-based tables.
        /// </summary>
        /// <typeparam name="TJoin">
        /// An <see cref="Enum"/> type representing the secondary table to join, whose name is inferred from <c>typeof(TJoin).Name</c>.
        /// </typeparam>
        /// <param name="JoinMode">
        /// The join strategy to apply, such as <c>INNER_JOIN</c>, <c>LEFT_JOIN</c>, or <c>FULL_OUTER_JOIN</c>, defined by <see cref="JoinModes"/>.
        /// </param>
        /// <param name="OnLeft">
        /// The enum member from the primary table <typeparamref name="T"/> representing the join key.
        /// </param>
        /// <param name="OnRight">
        /// The enum member from the secondary table <typeparamref name="TJoin"/> representing the join key.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// The join clause is constructed by qualifying both columns with their respective table names.
        /// The join mode is rendered in SQL syntax by replacing underscores with spaces (e.g., <c>FULL_OUTER_JOIN</c> becomes <c>FULL OUTER JOIN</c>).
        /// </remarks>
        public SelectBuilder<T> Join<TJoin>(JoinModes JoinMode, T OnLeft, TJoin OnRight) where TJoin : Enum
        {
            cmd.Append(" " + JoinMode.ToString().Replace("_", " ") + " " + typeof(TJoin).Name);
            cmd.Append(" ON " + typeof(T).Name + "." + OnLeft.ToString() + "=");
            cmd.Append(typeof(TJoin).Name + "." + OnRight.ToString());
            return this;
        }


        /// <summary>
        /// Appends a condition to the SQL <c>WHERE</c> clause, using the specified logical operator to combine expressions.
        /// </summary>
        /// <param name="Where">
        /// A string representing the SQL condition to apply (e.g., <c>"Age &gt; 30"</c>).
        /// </param>
        /// <param name="Operator">
        /// The logical operator used to join this condition with any existing ones, such as <c>AND</c> or <c>OR</c>.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// The first condition initializes the <c>WHERE</c> clause. Subsequent calls append conditions using the specified logical operator.
        /// </remarks>
        public SelectBuilder<T> Where(string Where, WhereOperators Operator = WhereOperators.NONE)
        {
            if (!_hasWhere)
            {
                cmd.Append(" WHERE ");
                _hasWhere = true;
            }
            else
                cmd.Append(" " + Operator.ToString() + " ");
            cmd.Append(Where);
            return this;
        }
        /// <summary>
        /// Appends a conditional expression to the SQL <c>WHERE</c> clause using the specified column, operator, and value.
        /// </summary>
        /// <param name="Column">
        /// The enum member representing the column to filter.
        /// </param>
        /// <param name="Operator">
        /// The comparison operator to apply, defined by <see cref="SQLOperator"/> and rendered as a SQL symbol.
        /// </param>
        /// <param name="Value">
        /// The value to compare against, rendered directly into the SQL expression.
        /// </param>
        /// <param name="WhereOperator">
        /// The logical connector (<c>AND</c>, <c>OR</c>, etc.) to prepend if chaining multiple conditions. Defaults to <c>NONE</c> for the first condition.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method supports symbolic SQL rendering via <see cref="SqlOperatorExtensions.ToSymbol(SQLOperator)"/> and enables dynamic condition chaining.
        /// </remarks>
        public SelectBuilder<T> Where(T Column, SQLOperator Operator, string Value, WhereOperators WhereOperator = WhereOperators.NONE)
        {
            if (!_hasWhere)
            {
                cmd.Append(" WHERE ");
                _hasWhere = true;
            }
            else
                cmd.Append(" " + WhereOperator.ToString() + " ");
            cmd.Append(Column.ToString() + Operator.ToSymbol() + Value);
            return this;
        }
        /// <summary>
        /// Appends a conditional expression to the SQL <c>WHERE</c> clause using a column from a joined enum-based table.
        /// </summary>
        /// <typeparam name="Join">
        /// An <see cref="Enum"/> type representing the joined table whose name is inferred from <c>typeof(Join).Name</c>.
        /// </typeparam>
        /// <param name="Column">
        /// The enum member representing the column to filter from the joined table.
        /// </param>
        /// <param name="Operator">
        /// The comparison operator to apply, defined by <see cref="SQLOperator"/> and rendered as a SQL symbol.
        /// </param>
        /// <param name="Value">
        /// The value to compare against, rendered directly into the SQL expression.
        /// </param>
        /// <param name="WhereOperator">
        /// The logical connector (<c>AND</c>, <c>OR</c>, etc.) to prepend if chaining multiple conditions. Defaults to <c>NONE</c> for the first condition.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method enables condition chaining across joined tables using fully qualified column references and symbolic SQL operators.
        /// </remarks>
        public SelectBuilder<T> Where<Join>(Join Column, SQLOperator Operator, string Value, WhereOperators WhereOperator = WhereOperators.NONE) where Join : Enum
        {
            if (!_hasWhere)
            {
                cmd.Append(" WHERE ");
                _hasWhere = true;
            }
            else
                cmd.Append(" " + WhereOperator.ToString() + " ");

            cmd.Append(typeof(Join).Name + "." + Column.ToString() + Operator.ToSymbol() + Value);
            return this;
        }
        /// <summary>
        /// Begins a grouped condition block within the SQL <c>WHERE</c> clause using an opening parenthesis.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of grouped conditions.
        /// </returns>
        /// <remarks>
        /// If no <c>WHERE</c> clause has been started, this property initializes it with <c>WHERE (</c>. Otherwise, it appends <c>(</c> to continue grouping.
        /// Use this in combination with <c>Where</c> and <c>EndGroupedWhere</c> to construct nested logical expressions.
        /// </remarks>
        public SelectBuilder<T> StartGroupedWhere
        {
            get
            {
                if (!_hasWhere)
                {
                    cmd.Append(" WHERE (");
                    _hasWhere = true;
                }
                else
                    cmd.Append(" (");
                return this;
            }
        }
        /// <summary>
        /// Ends a grouped condition block within the SQL <c>WHERE</c> clause using a closing parenthesis.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This property finalizes a logical grouping started with <c>StartGroupedWhere</c>, enabling nested or compound filter expressions.
        /// Use it to close parenthesized conditions in complex <c>WHERE</c> clauses.
        /// </remarks>
        public SelectBuilder<T> EndGroupedWhere
        {
            get
            {
                cmd.Append(")");
                return this;
            }
        }


        /// <summary>
        /// Appends an SQL <c>ORDER BY</c> clause using the specified column and sort direction.
        /// </summary>
        /// <param name="OrderBy">
        /// The enum member representing the column to sort by.
        /// </param>
        /// <param name="OrderMode">
        /// The sort direction, either <c>ASC</c> or <c>DESC</c>, defined by <see cref="OrderModes"/>.
        /// </param>
        /// <param name="IsFullyQualified">
        /// If <c>true</c>, the column is prefixed with the table name (e.g., <c>Table.Column</c>); otherwise, only the column name is used.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method enables precise control over result ordering in SQL queries, supporting both qualified and unqualified column references.
        /// </remarks>
        public SelectBuilder<T> OrderBy(T OrderBy, MySQLOrderBy OrderMode, bool IsFullyQualified = false)
        {
            if (IsFullyQualified)
                cmd.Append(" ORDER BY " + typeof(T).Name + "." + OrderBy.ToString() + " " + OrderMode.ToString());
            else
                cmd.Append(" ORDER BY " + OrderBy.ToString() + " " + OrderMode.ToString());
            return this;
        }
        /// <summary>
        /// Appends an SQL <c>ORDER BY</c> clause using a column from a joined enum-based table and the specified sort direction.
        /// </summary>
        /// <typeparam name="Join">
        /// An <see cref="Enum"/> type representing the joined table whose name is inferred from <c>typeof(Join).Name</c>.
        /// </typeparam>
        /// <param name="OrderBy">
        /// The enum member representing the column to sort by from the joined table.
        /// </param>
        /// <param name="OrderMode">
        /// The sort direction, either <c>ASC</c> or <c>DESC</c>, defined by <see cref="OrderModes"/>.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method enables result ordering based on columns from joined tables, using fully qualified references for clarity and disambiguation.
        /// </remarks>
        public SelectBuilder<T> OrderBy<Join>(Join OrderBy, MySQLOrderBy OrderMode) where Join: Enum
        {
            cmd.Append(" ORDER BY " + typeof(Join).Name + "." + OrderBy.ToString() + " " + OrderMode.ToString());
            return this;
        }
    }
}
