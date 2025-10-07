using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.SQLBuilder
{
    /// <summary>
    /// Represents a fluent SQL <c>DELETE</c> statement builder targeting a table mapped from the enum type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// An enum type whose name corresponds to the target table for deletion.
    /// </typeparam>
    /// <remarks>
    /// This class provides a metadata-driven approach to constructing SQL <c>DELETE</c> statements, using the enum type <typeparamref name="T"/> to infer the table name.
    /// It supports fluent composition of conditional logic via <c>WHERE</c> clauses and grouped expressions.
    /// Designed for scenarios where compile-time enum mapping drives table targeting and clause safety.
    /// </remarks>
    public class DeleteCommand<T> where T: Enum
    {
        StringBuilder cmd;
        bool _hasWhere;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommand{T}"/> class for building a SQL <c>DELETE</c> statement targeting the specified entity type.
        /// </summary>
        /// <remarks>
        /// This constructor begins the command with <c>DELETE FROM</c> followed by the name of the type <typeparamref name="T"/>.
        /// It also resets the internal <c>WHERE</c> clause flag to ensure clean composition.
        /// Intended for metadata-driven deletion logic where <typeparamref name="T"/> maps to a table name.
        /// </remarks>
        public DeleteCommand()
        {
            cmd = new StringBuilder();
            cmd.Append("DELETE FROM " + typeof(T).Name);
            _hasWhere = false;
        }
        /// <summary>
        /// Returns the composed SQL <c>DELETE</c> statement as a string, terminated with a semicolon.
        /// </summary>
        /// <returns>
        /// A complete SQL query string representing the current state of the delete builder.
        /// </returns>
        /// <remarks>
        /// This override finalizes the command buffer for execution or inspection. It assumes that any necessary <c>WHERE</c> clauses have been appended.
        /// </remarks>
        public override string ToString()
        {
            return cmd.ToString() + ";";
        }

        #region PROPERTIES
        /// <summary>
        /// Begins the <c>WHERE</c> clause of the SQL <c>DELETE</c> statement.
        /// </summary>
        /// <value>
        /// The current <see cref="DeleteCommand{T}"/> instance, allowing fluent chaining of conditional expressions.
        /// </value>
        /// <remarks>
        /// This property appends the <c>WHERE</c> keyword to the command buffer, enabling the addition of conditional logic for targeted deletions.
        /// It should be followed by column comparisons or logical expressions.
        /// If <c>StartGroupedWhere</c> is used first, there is no need to call this property separately.
        /// </remarks>
        public DeleteCommand<T> Where
        {
            get
            {
                cmd.Append(" WHERE ");
                return this;
            }
        }
        /// <summary>
        /// Begins a grouped <c>WHERE</c> clause in the SQL <c>DELETE</c> statement, allowing compound conditions to be enclosed in parentheses.
        /// </summary>
        /// <value>
        /// The current <see cref="DeleteCommand{T}"/> instance, allowing fluent chaining of conditional expressions.
        /// </value>
        /// <remarks>
        /// If a <c>WHERE</c> clause has already been started, this appends an opening parenthesis for grouping.
        /// Otherwise, it begins the clause with <c>WHERE (</c>, enabling nested or compound logic.
        /// If this method is used first, there is no need to call the <c>Where</c> property separately.
        /// Use in combination with <c>EndGroupedWhere</c> to close the group.
        /// </remarks>
        public DeleteCommand<T> StartGroupedWhere
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
        /// Closes a grouped <c>WHERE</c> clause in the SQL <c>DELETE</c> statement.
        /// </summary>
        /// <value>
        /// The current <see cref="DeleteCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </value>
        /// <remarks>
        /// This property appends a closing parenthesis to the command buffer, completing a grouped conditional expression started by <c>StartGroupedWhere</c>.
        /// It should only be used after a corresponding <c>StartGroupedWhere</c> call to ensure proper SQL syntax.
        /// </remarks>
        public DeleteCommand<T> EndGroupedWhere
        {
            get
            {
                cmd.Append(")");
                return this;
            }
        }
        #endregion

        #region WHERE REGION
        /// <summary>
        /// Appends a raw conditional expression to the <c>WHERE</c> clause of the SQL <c>DELETE</c> statement.
        /// </summary>
        /// <param name="Condition">
        /// A string representing the SQL condition (e.g., <c>"Status = 'Inactive'"</c>) to be added to the <c>WHERE</c> clause.
        /// </param>
        /// <param name="Connector">
        /// A logical connector from <see cref="WhereConnectors"/> (e.g., <c>AND</c>, <c>OR</c>) used to join this condition with previous ones. Defaults to <c>NONE</c>.
        /// </param>
        /// <returns>
        /// The current <see cref="DeleteCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// If a <c>WHERE</c> clause has already been started, the specified connector is inserted before the condition.
        /// Otherwise, this marks the beginning of the <c>WHERE</c> clause and sets the internal flag accordingly.
        /// If <c>StartGroupedWhere</c> is used first, there is no need to call the <c>Where</c> property separately.
        /// Use this method for dynamic or loosely typed condition injection.
        /// </remarks>
        public DeleteCommand<T> Condition(string Condition, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(Condition);
            return this;
        }
        /// <summary>
        /// Appends a typed conditional expression to the <c>WHERE</c> clause of the SQL <c>DELETE</c> statement.
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
        /// <returns>
        /// The current <see cref="DeleteCommand{T}"/> instance, allowing fluent chaining of additional builder methods.
        /// </returns>
        /// <remarks>
        /// If a <c>WHERE</c> clause has already been started, the specified connector is inserted before the condition.
        /// Otherwise, this marks the beginning of the <c>WHERE</c> clause and sets the internal flag accordingly.
        /// If <c>StartGroupedWhere</c> is used first, there is no need to call the <c>Where</c> property separately.
        /// Use this method for structured, type-safe condition composition.
        /// </remarks>
        public DeleteCommand<T> Condition(T Column, SQLOperator Operator, string Value, WhereConnectors Connector = WhereConnectors.NONE)
        {
            if (_hasWhere)
                cmd.Append(" " + Connector.ToString() + " ");
            else
                _hasWhere = true;
            cmd.Append(Column.ToString() + Operator.ToSymbol() + Value);
            return this;
        }
        #endregion
    }
}
