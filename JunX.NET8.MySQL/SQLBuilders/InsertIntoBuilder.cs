using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.MySQL.SQLBuilders
{
    /// <summary>
    /// Provides a fluent API for composing SQL <c>INSERT INTO</c> statements using enum-based column definitions.
    /// </summary>
    /// <typeparam name="T">
    /// An <see cref="Enum"/> type representing the target table whose members define insertable columns.
    /// </typeparam>
    /// <remarks>
    /// This builder supports metadata-driven insert logic, enabling structured and type-safe SQL generation.
    /// </remarks>
    public class InsertIntoBuilder<T> where T : Enum
    {
        private StringBuilder cmd;
        private bool _hasColumn;
        private bool _hasValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertIntoBuilder{T}"/> class and begins composing an SQL <c>INSERT INTO</c> statement.
        /// </summary>
        /// <remarks>
        /// The target table is inferred from the enum type <typeparamref name="T"/>.
        /// Internal flags are reset to track column and value state during insert composition.
        /// </remarks>
        public InsertIntoBuilder()
        {
            cmd =  new StringBuilder();
            cmd.Append("INSERT INTO " + typeof(T).Name);
            _hasColumn = false;
            _hasValue = false;
        }
        /// <summary>
        /// Returns the composed SQL <c>INSERT INTO</c> statement as a complete string, including closing syntax.
        /// </summary>
        /// <returns>
        /// A string representing the finalized SQL insert command, terminated with <c>);</c>.
        /// </returns>
        /// <remarks>
        /// This method completes the builder output for execution or inspection, ensuring proper closure of column and value groups.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ");";
        }


        /// <summary>
        /// Appends a column name to the SQL <c>INSERT INTO</c> clause using an enum-defined column.
        /// </summary>
        /// <param name="Column">
        /// The enum member representing the column to insert into.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method builds the column list for the SQL insert statement, inserting commas as needed and opening the column group with a parenthesis.
        /// </remarks>
        public InsertIntoBuilder<T> Column(T Column)
        {
            if (_hasColumn)
                cmd.Append(", ");
            else
            {
                cmd.Append(" (");
                _hasColumn = true;
            }
            cmd.Append(Column.ToString());
            return this;
        }
        /// <summary>
        /// Appends multiple column names to the SQL <c>INSERT INTO</c> clause using an enumerable of enum-defined columns.
        /// </summary>
        /// <param name="Columns">
        /// A sequence of enum members representing the columns to insert into.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method builds the column list for the SQL insert statement, inserting commas as needed and opening the column group with a parenthesis.
        /// </remarks>
        public InsertIntoBuilder<T> Column(IEnumerable<T> Columns)
        {
            foreach(T cols in Columns)
            {
                if (_hasColumn)
                    cmd.Append(", ");
                else
                {
                    cmd.Append(" (");
                    _hasColumn = true;
                }

                cmd.Append(cols.ToString());
            }
            return this;
        }


        /// <summary>
        /// Appends a value to the SQL <c>VALUES</c> clause using the specified data type for safe formatting.
        /// </summary>
        /// <param name="Value">
        /// The raw value to be inserted, represented as a string.
        /// </param>
        /// <param name="DataType">
        /// The SQL data type of the value, defined by <see cref="MySQLDataType"/>, used to format the value safely.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method builds the value list for the SQL insert statement, inserting commas as needed and opening the value group with a parenthesis.
        /// Values are formatted using <c>Construct.SQLSafeValue</c> to ensure type-safe SQL rendering.
        /// </remarks>
        public InsertIntoBuilder<T> Values(string Value, MySQLDataType DataType)
        {
            if (_hasValue)
                cmd.Append(", ");
            else
            {
                cmd.Append(") VALUES (");
                _hasValue = true;
            }
            cmd.Append(Construct.SQLSafeValue(DataType, Value));
            return this;
        }
        /// <summary>
        /// Appends multiple values to the SQL <c>VALUES</c> clause using a sequence of typed metadata entries.
        /// </summary>
        /// <param name="Values">
        /// A collection of <see cref="ValuesMetadata"/> instances, each containing a raw value and its associated SQL data type.
        /// </param>
        /// <returns>
        /// The current <see cref="InsertIntoBuilder{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// This method builds the value list for the SQL insert statement, inserting commas as needed and opening the value group with a parenthesis.
        /// Each value is rendered safely using its <see cref="MySQLDataType"/> via <c>ValuesMetadata.Value</c>.
        /// </remarks>
        public InsertIntoBuilder<T> Values(IEnumerable<ValuesMetadata> Values)
        {
            foreach(ValuesMetadata V in Values)
            {
                if (_hasValue)
                    cmd.Append(", ");
                else
                {
                    cmd.Append(") VALUES (");
                    _hasValue = true;
                }

                cmd.Append(V.Value);
            }
            return this;
        }
    }
}
