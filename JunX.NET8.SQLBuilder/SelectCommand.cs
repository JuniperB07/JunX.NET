using JunX.NET8.MySQL;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.SQLBuilder
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
    public class SelectCommand<T> where T: Enum
    {
        public StringBuilder cmd = new StringBuilder();
        private bool _hasColumns = false;
        private bool _hasWhere = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommand{T}"/> class and begins composing an SQL <c>SELECT</c> statement.
        /// </summary>
        /// <remarks>
        /// Resets internal flags to track column and condition state during query construction.
        /// </remarks>
        public SelectCommand()
        {
            cmd = new StringBuilder();
            cmd.Append("SELECT ");
            _hasColumns = false;
            _hasWhere = false;
        }
        /// <summary>
        /// Returns the composed SQL <c>SELECT</c> statement as a complete string.
        /// </summary>
        /// <returns>
        /// A string representing the finalized SQL query, terminated with a semicolon.
        /// </returns>
        /// <remarks>
        /// This method completes the builder output for execution or inspection.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ";";
        }

        #region PROPERTIES
        /// <summary>
        /// Appends the <c>FROM</c> clause to the SQL <c>SELECT</c> statement using the enum type name as the table name.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This property infers the table name from the enum type <typeparamref name="T"/> and appends it to the query.
        /// </remarks>
        public SelectCommand<T> From
        {
            get
            {
                cmd.Append(" FROM " + typeof(T).Name);
                return this;
            }
        }
        /// <summary>
        /// Appends a <c>WHERE</c> clause to the SQL <c>SELECT</c> statement, initiating conditional filtering.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This property begins the <c>WHERE</c> clause, enabling subsequent condition composition.
        /// </remarks>
        public SelectCommand<T> Where
        {
            get
            {
                cmd.Append(" WHERE ");
                return this;
            }
        }
        /// <summary>
        /// Begins a grouped <c>WHERE</c> clause in the SQL <c>SELECT</c> statement, opening a parenthesis for nested conditions.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// If a <c>WHERE</c> clause has already started, this appends an opening parenthesis for grouping.
        /// Otherwise, it starts the <c>WHERE</c> clause and opens the group.
        /// </remarks>
        public SelectCommand<T> StartGroupedWhere
        {
            get
            {
                if (_hasWhere)
                    cmd.Append(" (");
                else
                    cmd.Append(" WHERE (");
                return this;
            }
        }
        /// <summary>
        /// Ends a grouped <c>WHERE</c> clause in the SQL <c>SELECT</c> statement by appending a closing parenthesis.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This property closes a logical grouping started by <see cref="StartGroupedWhere"/>, enabling nested condition structures.
        /// </remarks>
        public SelectCommand<T> EndGroupedWhere
        {
            get
            {
                cmd.Append(")");
                return this;
            }
        }
        #endregion

        #region COLUMN REGION
        /// <summary>
        /// Appends a column to the SQL <c>SELECT</c> clause using an enum-defined column, with optional full qualification.
        /// </summary>
        /// <param name="Column">
        /// The enum member representing the column to select.
        /// </param>
        /// <param name="IsFullyQualified">
        /// If <c>true</c>, prefixes the column with the enum type name to fully qualify it (e.g., <c>Table.Column</c>); otherwise, uses the column name alone.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method builds the column list for the SQL select statement, inserting commas as needed.
        /// </remarks>
        public SelectCommand<T> Select(T Column, bool IsFullyQualified = false)
        {
            if (_hasColumns)
                cmd.Append(", ");
            else
                _hasColumns = true;

            if (IsFullyQualified)
                cmd.Append(typeof(T).Name + "." + Column.ToString());
            else
                cmd.Append(Column.ToString());

            return this;
        }
        /// <summary>
        /// Appends multiple columns to the SQL <c>SELECT</c> clause using an enumerable of enum-defined columns, with optional full qualification.
        /// </summary>
        /// <param name="Columns">
        /// A sequence of enum members representing the columns to select.
        /// </param>
        /// <param name="IsFullyQualified">
        /// If <c>true</c>, prefixes each column with the enum type name to fully qualify it (e.g., <c>Table.Column</c>); otherwise, uses the column name alone.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method builds the column list for the SQL select statement, inserting commas as needed.
        /// </remarks>
        public SelectCommand<T> Select(IEnumerable<T> Columns, bool IsFullyQualified = false)
        {
            foreach(T C in Columns)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                    _hasColumns = true;

                if (IsFullyQualified)
                    cmd.Append(typeof(T).Name + "." + C.ToString());
                else
                    cmd.Append(C.ToString());
            }
            return this;
        }
        /// <summary>
        /// Appends a fully qualified column to the SQL <c>SELECT</c> clause using an enum from a joined table.
        /// </summary>
        /// <typeparam name="Join">
        /// An <see cref="Enum"/> type representing the joined table whose members define selectable columns.
        /// </typeparam>
        /// <param name="Column">
        /// The enum member representing the column to select from the joined table.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method enables cross-table column selection by prefixing the column with its enum type name (e.g., <c>JoinTable.Column</c>).
        /// </remarks>
        public SelectCommand<T> SelectJoin<Join>(Join Column) where Join: Enum
        {
            if (_hasColumns)
                cmd.Append(", ");
            else
                _hasColumns = true;

            cmd.Append(typeof(Join).Name + "." + Column.ToString());
            return this;
        }
        /// <summary>
        /// Appends multiple fully qualified columns to the SQL <c>SELECT</c> clause using an enumerable of enum members from a joined table.
        /// </summary>
        /// <typeparam name="Join">
        /// An <see cref="Enum"/> type representing the joined table whose members define selectable columns.
        /// </typeparam>
        /// <param name="Columns">
        /// A sequence of enum members representing the columns to select from the joined table.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// Each column is prefixed with the base enum type name <typeparamref name="T"/> rather than <typeparamref name="Join"/>, which may be intentional for aliasing or schema mapping.
        /// </remarks>
        public SelectCommand<T> SelectJoin<Join>(IEnumerable<Join> Columns)
        {
            foreach(Join C in Columns)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                    _hasColumns = true;

                cmd.Append(typeof(T).Name + "." + C.ToString());
            }
            return this;
        }
        #endregion

        #region ALIAS REGION
        /// <summary>
        /// Appends an <c>AS</c> alias clause to the SQL statement, renaming the current expression or column.
        /// </summary>
        /// <param name="Alias">
        /// The alias to assign, enclosed in single quotes (e.g., <c>'TotalAmount'</c>).
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method is typically used after a column or expression to assign a readable or contextual alias.
        /// </remarks>
        public SelectCommand<T> As(string Alias)
        {
            cmd.Append(" AS '" + Alias + "'");
            return this;
        }
        /// <summary>
        /// Appends multiple aliased columns to the SQL <c>SELECT</c> clause using metadata that maps enum members to aliases.
        /// </summary>
        /// <param name="SelectAs">
        /// A sequence of <see cref="AliasMetadata{T}"/> objects, each containing a column and its corresponding alias.
        /// </param>
        /// <param name="IsFullyQualified">
        /// If <c>true</c>, uses the fully qualified column name (e.g., <c>Table.Column</c>); otherwise, uses the column name alone.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method enables expressive column aliasing for improved readability or client-side mapping.
        /// </remarks>
        public SelectCommand<T> SelectAs(IEnumerable<AliasMetadata<T>> SelectAs, bool IsFullyQualified = false)
        {
            foreach(AliasMetadata<T> SA in SelectAs)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                    _hasColumns = true;

                if (IsFullyQualified)
                    cmd.Append(SA.FullyQualified + " AS '" + SA.Alias + "'");
                else
                    cmd.Append(SA.Column.ToString() + " AS '" + SA.Alias + "'");
            }
            return this;
        }
        /// <summary>
        /// Appends a fully qualified column from a joined table to the <c>SELECT</c> clause, using an alias for clarity.
        /// </summary>
        /// <typeparam name="Join">
        /// An enum representing the joined table, where each value corresponds to a column name.
        /// </typeparam>
        /// <param name="SelectAs">
        /// An <see cref="AliasMetadata{Join}"/> instance containing the column to select and the alias to apply.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional column selections.
        /// </returns>
        /// <remarks>
        /// This method appends <c>JoinTable.Column AS 'Alias'</c> to the <c>SELECT</c> clause.
        /// If other columns have already been selected, a comma is inserted before appending.
        /// Use this method to include joined table columns with readable aliases in the result set.
        /// </remarks>
        public SelectCommand<T> SelectAsJoin<Join>(AliasMetadata<Join> SelectAs) where Join: Enum
        {
            if (_hasColumns)
                cmd.Append(", ");
            else
                _hasColumns = true;
            cmd.Append(typeof(Join).Name + "." + SelectAs.Column.ToString());
            cmd.Append(" AS '" + SelectAs.Alias + "'");
            return this;
        }
        /// <summary>
        /// Appends multiple aliased columns from a joined table to the SQL <c>SELECT</c> clause using metadata that maps enum members to aliases.
        /// </summary>
        /// <typeparam name="Join">
        /// An <see cref="Enum"/> type representing the joined table whose members define selectable columns.
        /// </typeparam>
        /// <param name="SelectAs">
        /// A sequence of <see cref="AliasMetadata{Join}"/> objects, each containing a fully qualified column and its corresponding alias.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method enables expressive column aliasing for joined tables, using fully qualified names (e.g., <c>Orders.Total AS 'OrderTotal'</c>).
        /// </remarks>
        public SelectCommand<T> SelectAsJoin<Join>(IEnumerable<AliasMetadata<Join>> SelectAs) where Join: Enum
        {
            foreach(AliasMetadata<Join> SA in SelectAs)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                    _hasColumns = true;

                cmd.Append(SA.FullyQualified + " AS '" + SA.Alias + "'");
            }
            return this;
        }
        #endregion

        #region WHERE REGION
        /// <summary>
        /// Appends a conditional expression to the SQL <c>WHERE</c> clause, optionally prefixed with a logical connector.
        /// </summary>
        /// <param name="Where">
        /// The raw SQL condition to append (e.g., <c>Age &gt; 30</c>, <c>Name = 'Juniper'</c>).
        /// </param>
        /// <param name="Connector">
        /// The logical connector to use before the condition (e.g., <see cref="WhereConnectors.AND"/> or <see cref="WhereConnectors.OR"/>). Defaults to <see cref="WhereConnectors.NONE"/>.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// If this is the first condition, no connector is added. Subsequent conditions are prefixed with the specified connector.
        /// </remarks>
        public SelectCommand<T> Condition(string Where, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;

            cmd.Append(Where);
            return this;
        }
        /// <summary>
        /// Appends a typed conditional expression to the <c>WHERE</c> clause of the SQL <c>SELECT</c> statement.
        /// </summary>
        /// <param name="Column">
        /// The column to compare, represented by the enum value <typeparamref name="T"/>.
        /// </param>
        /// <param name="Operator">
        /// The <see cref="SQLOperator"/> used to compare the column and value (e.g., <c>=</c>, <c>&gt;</c>, <c>LIKE</c>).
        /// </param>
        /// <param name="Value">
        /// The right-hand operand, typically a literal value or expression. It should be SQL-safe or preformatted.
        /// </param>
        /// <param name="Connector">
        /// A logical connector from <see cref="WhereConnectors"/> (e.g., <c>AND</c>, <c>OR</c>) used to join this condition with previous ones. Defaults to <c>NONE</c>.
        /// </param>
        /// <param name="IsFullyQualified">
        /// Indicates whether the column should be fully qualified with the table name (e.g., <c>Table.Column</c>).
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// If a <c>WHERE</c> clause has already been started, the specified connector is inserted before the condition.
        /// Otherwise, this marks the beginning of the <c>WHERE</c> clause and sets the internal flag accordingly.
        /// Use this method for structured, type-safe condition composition with optional table qualification.
        /// </remarks>
        public SelectCommand<T> Condition(T Column, SQLOperator Operator, string Value, WhereConnectors Connector = WhereConnectors.NONE, bool IsFullyQualified = false)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector + " ");
            else
                _hasWhere = true;

            if (IsFullyQualified)
                cmd.Append(typeof(T).Name + "." + Column.ToString() + Operator.ToSymbol() + Value);
            else
                cmd.Append(Column.ToString() + Operator.ToSymbol() + Value);

            return this;
        }
        /// <summary>
        /// Appends a conditional expression to the SQL <c>WHERE</c> clause using a column from a joined table, a SQL operator, and a value, optionally prefixed with a logical connector.
        /// </summary>
        /// <typeparam name="Join">
        /// An <see cref="Enum"/> type representing the joined table whose members define filterable columns.
        /// </typeparam>
        /// <param name="Column">
        /// The enum member representing the column to filter from the joined table.
        /// </param>
        /// <param name="Operator">
        /// The SQL comparison operator, defined by <see cref="SQLOperator"/>, used to compare the column and value.
        /// </param>
        /// <param name="Value">
        /// The raw value to compare against, represented as a string.
        /// </param>
        /// <param name="Connector">
        /// The logical connector to use before the condition (e.g., <see cref="WhereConnectors.AND"/> or <see cref="WhereConnectors.OR"/>). Defaults to <see cref="WhereConnectors.NONE"/>.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method enables cross-table filtering by prefixing the column with its enum type name (e.g., <c>JoinTable.Column</c>).
        /// </remarks>
        public SelectCommand<T> JoinCondition<Join>(Join Column, SQLOperator Operator, string Value, WhereConnectors Connector = WhereConnectors.NONE) where Join: Enum
        {
            if (_hasWhere)
                cmd.Append(" " + Connector + " ");
            else
                _hasWhere = true;

            cmd.Append(typeof(Join).Name + "." + Column.ToString());
            cmd.Append(Operator.ToSymbol() + Value);
            return this;
        }
        #endregion

        #region JOIN REGION
        /// <summary>
        /// Appends a SQL <c>JOIN</c> clause to the <c>SELECT</c> statement using the specified join mode and join condition.
        /// </summary>
        /// <typeparam name="TJoin">
        /// An <see cref="Enum"/> type representing the joined table whose members define joinable columns.
        /// </typeparam>
        /// <param name="JoinMode">
        /// The type of SQL join to perform (e.g., <c>INNER_JOIN</c>, <c>LEFT_JOIN</c>), defined by <see cref="JoinModes"/>.
        /// </param>
        /// <param name="OnLeft">
        /// The column from the base table (type <typeparamref name="T"/>) used in the join condition.
        /// </param>
        /// <param name="OnRight">
        /// The column from the joined table (type <typeparamref name="TJoin"/>) used in the join condition.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method constructs a join clause such as <c>LEFT JOIN Orders ON Customers.Id = Orders.CustomerId</c>,
        /// using enum type names as table identifiers and enum members as column names.
        /// </remarks>
        public SelectCommand<T> Join<TJoin>(JoinModes JoinMode, T OnLeft, TJoin OnRight) where TJoin : Enum
        {
            cmd.Append(" " + JoinMode.ToString().Replace("_", " ") + " ");
            cmd.Append(typeof(TJoin).Name);
            cmd.Append(" ON " + typeof(T).Name + "." + OnLeft.ToString());
            cmd.Append("=" + typeof(TJoin).Name + "." + OnRight.ToString());
            return this;
        }
        #endregion

        #region ORDER BY REGION
        /// <summary>
        /// Appends an <c>ORDER BY</c> clause to the SQL <c>SELECT</c> statement using a typed column, sort direction, and optional full qualification.
        /// </summary>
        /// <param name="OrderBy">
        /// The enum member representing the column to sort by.
        /// </param>
        /// <param name="OrderMode">
        /// The sort direction, defined by <see cref="MySQLOrderBy"/> (e.g., <c>ASC</c> or <c>DESC</c>).
        /// </param>
        /// <param name="IsFullyQualitied">
        /// If <c>true</c>, prefixes the column with the enum type name to fully qualify it (e.g., <c>Table.Column</c>); otherwise, uses the column name alone.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method enables precise sorting with optional table qualification for disambiguation in multi-table queries.
        /// </remarks>
        public SelectCommand<T> OrderBy(T OrderBy, MySQLOrderBy OrderMode, bool IsFullyQualitied = false)
        {
            cmd.Append(" ORDER BY ");

            if (IsFullyQualitied)
                cmd.Append(typeof(T).Name + "." + OrderBy.ToString());
            else
                cmd.Append(OrderBy.ToString());

            cmd.Append(" " + OrderMode.ToString());
            return this;
        }
        /// <summary>
        /// Appends an <c>ORDER BY</c> clause to the SQL <c>SELECT</c> statement using a column from a joined table and a sort direction.
        /// </summary>
        /// <typeparam name="Join">
        /// An <see cref="Enum"/> type representing the joined table whose members define sortable columns.
        /// </typeparam>
        /// <param name="OrderBy">
        /// The enum member representing the column to sort by from the joined table.
        /// </param>
        /// <param name="OrderMode">
        /// The sort direction, defined by <see cref="MySQLOrderBy"/> (e.g., <c>ASC</c> or <c>DESC</c>).
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method enables sorting by columns from joined tables using fully qualified names (e.g., <c>Orders.Date DESC</c>).
        /// </remarks>
        public SelectCommand<T> OrderByJoin<Join>(Join OrderBy, MySQLOrderBy OrderMode) where Join: Enum
        {
            cmd.Append(" ORDER BY " + typeof(Join).Name + "." + OrderBy.ToString());
            cmd.Append(" " + OrderMode.ToString());
            return this;
        }
        #endregion
    }

    /// <summary>
    /// Represents a non-generic SQL <c>SELECT</c> statement builder for dynamic or loosely typed query composition.
    /// </summary>
    /// <remarks>
    /// This class provides a foundational structure for building SQL queries without relying on enum-based type safety.
    /// It maintains internal state for column tracking and <c>WHERE</c> clause management.
    /// Use this when generic constraints or metadata-driven composition are not required.
    /// </remarks>
    public class SelectCommand
    {
        private StringBuilder cmd = new StringBuilder();
        private bool _hasColumns = false;
        private bool _hasWhere = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommand"/> class and begins a SQL <c>SELECT</c> statement.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the internal command buffer with <c>SELECT</c> and resets tracking flags for column and <c>WHERE</c> clause composition.
        /// Use this as the starting point for building dynamic SQL queries.
        /// </remarks>
        public SelectCommand()
        {
            cmd = new StringBuilder();
            cmd.Append("SELECT ");
            _hasColumns = false;
            _hasWhere = false;
        }
        /// <summary>
        /// Returns the composed SQL <c>SELECT</c> statement as a string, terminated with a semicolon.
        /// </summary>
        /// <returns>
        /// A complete SQL query string representing the current state of the builder.
        /// </returns>
        /// <remarks>
        /// This override finalizes the query for execution or inspection by appending a terminating semicolon.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ";";
        }

        #region PROPERTIES
        /// <summary>
        /// Appends a <c>WHERE</c> clause to the SQL <c>SELECT</c> statement and returns the current builder for condition chaining.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of conditional expressions.
        /// </returns>
        /// <remarks>
        /// This property inserts the <c>WHERE</c> keyword into the query. It assumes that subsequent calls will append valid conditions.
        /// </remarks>
        public SelectCommand Where
        {
            get
            {
                cmd.Append(" WHERE ");
                return this;
            }
        }
        /// <summary>
        /// Begins a grouped <c>WHERE</c> clause by appending <c>WHERE (</c> or <c>(</c> depending on clause state.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of grouped conditional expressions.
        /// </returns>
        /// <remarks>
        /// This property inserts an opening parenthesis for grouped conditions. If no <c>WHERE</c> clause has been started, it prepends <c>WHERE</c>.
        /// Use in combination with <c>EndGroupedWhere</c> to wrap complex logical expressions.
        /// </remarks>
        public SelectCommand StartGroupedWhere
        {
            get
            {
                if (_hasWhere)
                    cmd.Append(" (");
                else
                    cmd.Append(" WHERE (");
                return this;
            }
        }
        /// <summary>
        /// Ends a grouped <c>WHERE</c> clause by appending a closing parenthesis to the SQL statement.
        /// </summary>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This property completes a logical grouping started with <see cref="StartGroupedWhere"/>, enabling nested or compound conditions.
        /// </remarks>
        public SelectCommand EndGroupedWhere
        {
            get
            {
                cmd.Append(")");
                return this;
            }
        }
        #endregion

        #region SELECT REGION
        /// <summary>
        /// Appends a column to the SQL <c>SELECT</c> clause using a raw string identifier.
        /// </summary>
        /// <param name="Column">
        /// The name of the column to include in the <c>SELECT</c> clause.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method supports dynamic or loosely typed column selection. If one or more columns have already been added, a comma is prepended.
        /// </remarks>
        public SelectCommand Select(string Column)
        {
            if (_hasColumns)
                cmd.Append(", ");
            else
                _hasColumns = true;
            cmd.Append(Column);
            return this;
        }
        /// <summary>
        /// Appends multiple columns to the SQL <c>SELECT</c> clause using raw string identifiers.
        /// </summary>
        /// <param name="Columns">
        /// A sequence of column names to include in the <c>SELECT</c> clause.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method supports dynamic column selection. Commas are automatically inserted between columns as needed.
        /// </remarks>
        public SelectCommand Select(IEnumerable<string> Columns)
        {
            foreach(string C in Columns)
            {
                if (_hasColumns)
                    cmd.Append(", ");
                else
                    _hasColumns = true;
                cmd.Append(C);
            }
            return this;
        }
        #endregion

        #region ALIAS REGION
        /// <summary>
        /// Appends an <c>AS</c> alias clause to the SQL statement, renaming the most recently added column or expression.
        /// </summary>
        /// <param name="Alias">
        /// The alias to assign, enclosed in single quotes (e.g., <c>'TotalAmount'</c>).
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method is typically used after a column or expression to assign a readable or contextual alias.
        /// It assumes that a valid column or expression was previously appended to the command buffer.
        /// </remarks>
        public SelectCommand As(string Alias)
        {
            cmd.Append(" AS '" + Alias + "'");
            return this;
        }
        #endregion

        #region FROM REGION
        /// <summary>
        /// Appends a <c>FROM</c> clause to the SQL <c>SELECT</c> statement using a raw table name.
        /// </summary>
        /// <param name="Table">
        /// The name of the table to query from.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method sets the source table for the query. It assumes that the <c>SELECT</c> clause has already been initialized.
        /// </remarks>
        public SelectCommand From(string Table)
        {
            cmd.Append(" FROM " + Table);
            return this;
        }
        #endregion

        #region WHERE REGION
        /// <summary>
        /// Appends a conditional expression to the SQL <c>WHERE</c> clause, optionally prefixed by a logical connector.
        /// </summary>
        /// <param name="Condition">
        /// The raw SQL condition to append (e.g., <c>Amount &gt; 100</c>).
        /// </param>
        /// <param name="Connector">
        /// The logical connector to prepend before the condition (e.g., <c>AND</c>, <c>OR</c>). Defaults to <c>NONE</c>, which omits the connector.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method tracks whether a <c>WHERE</c> clause has been started and inserts the appropriate connector if needed.
        /// Use in combination with <see cref="StartGroupedWhere"/> and <see cref="EndGroupedWhere"/> for complex logic.
        /// </remarks>
        public SelectCommand Condition(string Condition, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(Condition);
            return this;
        }
        /// <summary>
        /// Appends a conditional expression to the SQL <c>WHERE</c> clause using structured operands and a logical connector.
        /// </summary>
        /// <param name="Left">
        /// The left-hand side of the condition, typically a column name or expression.
        /// </param>
        /// <param name="Operator">
        /// The SQL comparison operator to apply (e.g., <c>=</c>, <c>&gt;</c>, <c>LIKE</c>), represented by the <see cref="SQLOperator"/> enum.
        /// </param>
        /// <param name="Right">
        /// The right-hand side of the condition, typically a literal value or parameter.
        /// </param>
        /// <param name="Connector">
        /// The logical connector to prepend before the condition (e.g., <c>AND</c>, <c>OR</c>). Defaults to <c>NONE</c>, which omits the connector.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method constructs a condition like <c>Amount &gt; 100</c> or <c>Name LIKE 'J%'</c>, and inserts the appropriate connector if needed.
        /// Use in combination with <see cref="StartGroupedWhere"/> and <see cref="EndGroupedWhere"/> for complex logical expressions.
        /// </remarks>
        public SelectCommand Condition(string Left, SQLOperator Operator, string Right, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(Left + Operator.ToString() + Right);
            return this;
        }
        #endregion

        #region JOIN REGION
        /// <summary>
        /// Appends a SQL <c>JOIN</c> clause to the <c>SELECT</c> statement using the specified join mode and condition.
        /// </summary>
        /// <param name="JoinMode">
        /// The type of join to apply (e.g., <c>INNER_JOIN</c>, <c>LEFT_JOIN</c>), represented by the <see cref="JoinModes"/> enum.
        /// Underscores in enum names are automatically replaced with spaces (e.g., <c>LEFT_JOIN</c> → <c>LEFT JOIN</c>).
        /// </param>
        /// <param name="JoinTable">
        /// The name of the table to join.
        /// </param>
        /// <param name="OnLeft">
        /// The left-hand side of the join condition, typically a column from the base table.
        /// </param>
        /// <param name="OnRight">
        /// The right-hand side of the join condition, typically a column from the joined table.
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method constructs a join clause like <c>LEFT JOIN Orders ON Customers.Id = Orders.CustomerId</c>.
        /// It assumes that the base table has already been specified via <see cref="From(string)"/>.
        /// </remarks>
        public SelectCommand Join(JoinModes JoinMode, string JoinTable, string OnLeft, string OnRight)
        {
            cmd.Append(" " + JoinMode.ToString().Replace("_", " ") + " ");
            cmd.Append(JoinTable);
            cmd.Append(" ON " + OnLeft + "=" + OnRight);
            return this;
        }
        #endregion

        #region ORDER BY REGION
        /// <summary>
        /// Appends an <c>ORDER BY</c> clause to the SQL <c>SELECT</c> statement using the specified column and sort direction.
        /// </summary>
        /// <param name="OrderBy">
        /// The name of the column to sort by.
        /// </param>
        /// <param name="OrderMode">
        /// The sort direction, represented by the <see cref="MySQLOrderBy"/> enum (e.g., <c>ASC</c>, <c>DESC</c>).
        /// </param>
        /// <returns>
        /// The current <see cref="SelectCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method constructs a clause like <c>ORDER BY CreatedDate DESC</c>. It assumes that the <c>SELECT</c> and <c>FROM</c> clauses have already been composed.
        /// </remarks>
        public SelectCommand OrderBy(string OrderBy, MySQLOrderBy OrderMode)
        {
            cmd.Append(" ORDER BY " + OrderBy + " " + OrderMode.ToString());
            return this;
        }
        #endregion
    }
}
