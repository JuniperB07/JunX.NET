using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.SQLBuilder
{
    /// <summary>
    /// Represents a fluent SQL <c>TRUNCATE TABLE</c> statement builder targeting a table mapped from the enum type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// An enum type whose name corresponds to the target table to be truncated.
    /// </typeparam>
    /// <remarks>
    /// This class provides a metadata-driven approach to constructing SQL <c>TRUNCATE</c> statements, using the enum type <typeparamref name="T"/> to infer the table name.
    /// Designed for scenarios where compile-time enum mapping drives table targeting and schema safety.
    /// </remarks>
    public class TruncateCommand<T> where T: Enum
    {
        StringBuilder cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruncateCommand{T}"/> class for building a SQL <c>TRUNCATE TABLE</c> statement targeting the specified entity type.
        /// </summary>
        /// <remarks>
        /// This constructor begins the command with <c>TRUNCATE TABLE</c> followed by the name of the type <typeparamref name="T"/>.
        /// Intended for metadata-driven truncation logic where <typeparamref name="T"/> maps to a table name.
        /// </remarks>
        public TruncateCommand()
        {
            cmd = new StringBuilder();
            cmd.Append("TRUNCATE TABLE " + typeof(T).Name);
        }
        /// <summary>
        /// Returns the composed SQL <c>TRUNCATE TABLE</c> statement as a string, terminated with a semicolon.
        /// </summary>
        /// <returns>
        /// A complete SQL query string representing the current state of the truncate builder.
        /// </returns>
        /// <remarks>
        /// This override finalizes the command buffer for execution or inspection. It assumes the target table has been correctly inferred from <typeparamref name="T"/>.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ";";
        }
    }

    /// <summary>
    /// Represents a fluent SQL <c>TRUNCATE TABLE</c> statement builder for manually specified table names.
    /// </summary>
    /// <remarks>
    /// This class provides a flexible approach to constructing SQL <c>TRUNCATE</c> statements by allowing the target table name to be set at runtime.
    /// Designed for scenarios where table names are not known at compile time or are determined dynamically.
    /// </remarks>
    public class TruncateCommand
    {
        StringBuilder cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruncateCommand"/> class for building a SQL <c>TRUNCATE TABLE</c> statement.
        /// </summary>
        /// <remarks>
        /// This constructor begins the command with <c>TRUNCATE TABLE</c>, allowing the target table name to be appended later via the <c>Truncate(string Table)</c> method.
        /// Designed for runtime-driven workflows where table names are specified dynamically.
        /// </remarks>
        public TruncateCommand()
        {
            cmd = new StringBuilder();
            cmd.Append("TRUNCATE TABLE ");
        }
        /// <summary>
        /// Returns the composed SQL <c>TRUNCATE TABLE</c> statement as a string, terminated with a semicolon.
        /// </summary>
        /// <returns>
        /// A complete SQL query string representing the current state of the truncate builder.
        /// </returns>
        /// <remarks>
        /// This override finalizes the command buffer for execution or inspection. It assumes the target table name has been appended using the <c>Truncate(string Table)</c> method.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ";";
        }

        /// <summary>
        /// Appends the specified table name to the SQL <c>TRUNCATE TABLE</c> statement.
        /// </summary>
        /// <param name="Table">
        /// The name of the table to truncate. This should be a valid SQL identifier and may include schema qualification if needed.
        /// </param>
        /// <returns>
        /// The current <see cref="TruncateCommand"/> instance, allowing fluent chaining or finalization.
        /// </returns>
        /// <remarks>
        /// This method completes the <c>TRUNCATE TABLE</c> statement by appending the target table name.
        /// Use when table names are determined dynamically at runtime.
        /// </remarks>
        public TruncateCommand Truncate(string Table)
        {
            cmd.Append(Table);
            return this;
        }
    }
}
