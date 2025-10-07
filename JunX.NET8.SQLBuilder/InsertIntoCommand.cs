using JunX.NET8.MySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.SQLBuilder
{
    /// <summary>
    /// Represents a fluent SQL <c>INSERT INTO</c> statement builder driven by enum-based metadata.
    /// </summary>
    /// <typeparam name="T">
    /// An <see cref="Enum"/> type whose members represent column names for the target table.
    /// </typeparam>
    /// <remarks>
    /// This class enables structured and type-safe insertion logic by mapping enum members to SQL columns.
    /// It maintains internal state for column and value tracking, and initializes the command with the table name inferred from <typeparamref name="T"/>.
    /// </remarks>
    public class InsertIntoCommand<T> where T: Enum
    {
        StringBuilder cmd;
        bool _hasColumns;
        bool _hasValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertIntoCommand{T}"/> class and begins an SQL <c>INSERT INTO</c> statement.
        /// </summary>
        /// <remarks>
        /// This constructor sets up the internal command buffer with the target table name inferred from the enum type <typeparamref name="T"/>.
        /// It also resets internal flags for column and value tracking, preparing the builder for structured insertion logic.
        /// </remarks>
        public InsertIntoCommand()
        {
            cmd = new StringBuilder();
            cmd.Append("INSERT INTO " + typeof(T).Name);
            _hasColumns = false;
            _hasValues = false;
        }
        /// <summary>
        /// Returns the composed SQL <c>INSERT INTO</c> statement as a string, terminated with a closing parenthesis and semicolon.
        /// </summary>
        /// <returns>
        /// A complete SQL query string representing the current state of the insertion builder.
        /// </returns>
        /// <remarks>
        /// This override finalizes the query for execution or inspection. It assumes that column and value sections have been properly appended.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ");";
        }

        #region COLUMN REGION
        /// <summary>
        /// Appends a column to the SQL <c>INSERT INTO</c> clause using an enum member representing the column name.
        /// </summary>
        /// <param name="Column">
        /// The enum member of type <typeparamref name="T"/> that identifies the column to insert into.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the column list with an opening parenthesis if it's the first column, and appends commas between subsequent columns.
        /// Use in combination with <c>Values(...)</c> to complete the insertion statement.
        /// </remarks>
        public InsertIntoCommand<T> Column(T Column)
        {
            if (_hasColumns)
                cmd.Append(", ");
            else
            {
                cmd.Append(" (");
                _hasColumns = true;
            }
            cmd.Append(Column.ToString());
            return this;
        }
        /// <summary>
        /// Appends multiple columns to the SQL <c>INSERT INTO</c> clause using enum members representing column names.
        /// </summary>
        /// <param name="Columns">
        /// A sequence of enum members of type <typeparamref name="T"/> that identify the columns to insert into.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the column list with an opening parenthesis if it's the first column, and appends commas between subsequent columns.
        /// Use in combination with <c>Values(...)</c> to complete the insertion statement.
        /// </remarks>
        public InsertIntoCommand<T> Column(IEnumerable<T> Columns)
        {
            foreach(T C in Columns)
            {
                if(_hasColumns)
                    cmd.Append(", ");
                else
                {
                    cmd.Append(" (");
                    _hasColumns = true;
                }
                cmd.Append(C.ToString());
            }
            return this;
        }
        #endregion

        #region VALUES REGION
        /// <summary>
        /// Appends a single SQL-safe value to the <c>VALUES</c> clause of the <c>INSERT INTO</c> statement.
        /// </summary>
        /// <param name="Value">
        /// The raw string value to insert.
        /// </param>
        /// <param name="DataType">
        /// The <see cref="MySQLDataType"/> representing the SQL type of the value (e.g., <c>VARCHAR</c>, <c>INT</c>, <c>DATE</c>).
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the <c>VALUES</c> clause with a closing parenthesis from the column list, followed by <c>VALUES (</c>.
        /// Subsequent calls append comma-separated values. Each value is formatted safely using <c>Construct.SQLSafeValue</c>.
        /// </remarks>
        public InsertIntoCommand<T> Values(string Value, MySQLDataType DataType)
        {
            if (_hasValues)
                cmd.Append(", ");
            else
            {
                cmd.Append(") VALUES (");
                _hasValues = true;
            }
            cmd.Append(Construct.SQLSafeValue(DataType, Value));
            return this;
        }
        /// <summary>
        /// Appends multiple SQL-safe values to the <c>VALUES</c> clause of the <c>INSERT INTO</c> statement using typed metadata.
        /// </summary>
        /// <param name="Values">
        /// A sequence of <see cref="ValuesMetadata"/> instances, each representing a value and its associated SQL data type.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the <c>VALUES</c> clause with a closing parenthesis from the column list, followed by <c>VALUES (</c>.
        /// Subsequent values are appended with commas and formatted safely using each metadata's <c>Value</c> property.
        /// </remarks>
        public InsertIntoCommand<T> Values(IEnumerable<ValuesMetadata> Values)
        {
            foreach(ValuesMetadata V in Values)
            {
                if (_hasValues)
                    cmd.Append(", ");
                else
                {
                    cmd.Append(") VALUES (");
                    _hasValues = true;
                }
                cmd.Append(V.Value);
            }
            return this;
        }
        #endregion
    }

    /// <summary>
    /// Represents a fluent SQL <c>INSERT INTO</c> statement builder without enum-based metadata.
    /// </summary>
    /// <remarks>
    /// This class provides a flexible, non-generic alternative to <c>InsertIntoCommand&lt;T&gt;</c>, allowing manual specification of table name, columns, and values.
    /// It maintains internal state for column and value tracking, and initializes the command buffer for SQL composition.
    /// </remarks>
    public class InsertIntoCommand
    {
        StringBuilder cmd;
        bool _hasColumns;
        bool _hasValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertIntoCommand"/> class for building an SQL <c>INSERT INTO</c> statement.
        /// </summary>
        /// <remarks>
        /// This constructor sets up the internal command buffer and resets column/value tracking flags.
        /// The target table name must be specified separately before appending columns and values.
        /// </remarks>
        public InsertIntoCommand()
        {
            cmd = new StringBuilder();
            cmd.Append("INSERT INTO ");
            _hasColumns = false;
            _hasValues = false;
        }
        /// <summary>
        /// Returns the composed SQL <c>INSERT INTO</c> statement as a string, terminated with a closing parenthesis and semicolon.
        /// </summary>
        /// <returns>
        /// A complete SQL query string representing the current state of the insertion builder.
        /// </returns>
        /// <remarks>
        /// This override finalizes the query for execution or inspection. It assumes that column and value sections have been properly appended.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ");";
        }

        #region INSERT INTO REGION
        /// <summary>
        /// Specifies the target table for the SQL <c>INSERT INTO</c> statement.
        /// </summary>
        /// <param name="Table">
        /// The name of the table into which data will be inserted.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method appends the table name directly to the command buffer. It should be called before defining columns and values.
        /// </remarks>
        public InsertIntoCommand InsertInto(string Table)
        {
            cmd.Append(Table);
            return this;
        }
        #endregion

        #region COLUMN REGION
        /// <summary>
        /// Appends a column name to the SQL <c>INSERT INTO</c> clause.
        /// </summary>
        /// <param name="Column">
        /// The name of the column to insert into.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the column list with an opening parenthesis if it's the first column, and appends commas between subsequent columns.
        /// Use in combination with <c>Values(...)</c> to complete the insertion statement.
        /// </remarks>
        public InsertIntoCommand Column(string Column)
        {
            if (_hasColumns)
                cmd.Append(", ");
            else
            {
                cmd.Append(" (");
                _hasColumns = true;
            }
            cmd.Append(Column);
            return this;
        }
        /// <summary>
        /// Appends multiple column names to the SQL <c>INSERT INTO</c> clause.
        /// </summary>
        /// <param name="Columns">
        /// A sequence of column names to insert into.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the column list with an opening parenthesis if it's the first column, and appends commas between subsequent columns.
        /// Use in combination with <c>Values(...)</c> to complete the insertion statement.
        /// </remarks>
        public InsertIntoCommand Column(IEnumerable<string> Columns)
        {
            foreach(string C in Columns)
            {
                if(_hasColumns)
                    cmd.Append(", ");
                else
                {
                    cmd.Append(" (");
                    _hasColumns = true;
                }
                cmd.Append(C);
            }
            return this;
        }
        #endregion

        #region VALUES REGION
        /// <summary>
        /// Appends a single SQL-safe value to the <c>VALUES</c> clause of the <c>INSERT INTO</c> statement.
        /// </summary>
        /// <param name="Value">
        /// The raw string value to insert.
        /// </param>
        /// <param name="DataType">
        /// The <see cref="MySQLDataType"/> representing the SQL type of the value (e.g., <c>VARCHAR</c>, <c>INT</c>, <c>DATE</c>).
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the <c>VALUES</c> clause with a closing parenthesis from the column list, followed by <c>VALUES (</c>.
        /// Subsequent calls append comma-separated values. Each value is formatted safely using <c>Construct.SQLSafeValue</c>.
        /// </remarks>
        public InsertIntoCommand Values(string Value, MySQLDataType DataType)
        {
            if(_hasValues)
                cmd.Append(", ");
            else
            {
                cmd.Append(") VALUES (");
                _hasValues = true;
            }
            cmd.Append(Construct.SQLSafeValue(DataType, Value));
            return this;
        }
        /// <summary>
        /// Appends multiple SQL-safe values to the <c>VALUES</c> clause of the <c>INSERT INTO</c> statement using typed metadata.
        /// </summary>
        /// <param name="Values">
        /// A sequence of <see cref="ValuesMetadata"/> instances, each containing a raw value and its associated SQL data type.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoCommand"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method begins the <c>VALUES</c> clause with a closing parenthesis from the column list, followed by <c>VALUES (</c>.
        /// Each value is appended with comma separation and formatted safely using the metadata's <c>Value</c> property.
        /// </remarks>
        public InsertIntoCommand Values(IEnumerable<ValuesMetadata> Values)
        {
            foreach(ValuesMetadata V in Values)
            {
                if (_hasValues)
                    cmd.Append(", ");
                else
                {
                    cmd.Append(") VALUES (");
                    _hasValues = true;
                }
                cmd.Append(V.Value);
            }
            return this;
        }
        #endregion
    }
}
