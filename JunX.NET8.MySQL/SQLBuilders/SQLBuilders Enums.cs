using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.MySQL.SQLBuilders
{
    /// <summary>
    /// Represents logical operators used to combine SQL <c>WHERE</c> clause conditions.
    /// </summary>
    /// <remarks>
    /// Commonly used to chain multiple filter expressions in SQL queries, such as <c>WHERE A = 1 AND B = 2</c>.
    /// </remarks>
    public enum WhereOperators
    {
        NONE,
        AND,
        OR
    }
    /// <summary>
    /// Specifies SQL join strategies for combining rows from multiple tables based on related columns.
    /// </summary>
    /// <remarks>
    /// These modes define how records from the primary and secondary tables are matched and included in the result set.
    /// Commonly used in query builders to control join semantics.
    /// </remarks>
    public enum JoinModes
    {
        INNER_JOIN,
        RIGHT_JOIN,
        LEFT_JOIN,
        FULL_OUTER_JOIN
    }


    /// <summary>
    /// Defines symbolic SQL comparison operators for use in <c>WHERE</c> clauses and conditional expressions.
    /// </summary>
    /// <remarks>
    /// These operators represent common relational comparisons such as equality, inequality, and pattern matching.
    /// </remarks>
    public enum SQLOperator
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        Like
    }
    /// <summary>
    /// Provides extension methods for the <see cref="SqlOperator"/> enum, enabling symbolic SQL rendering.
    /// </summary>
    /// <remarks>
    /// These methods convert enum members into their corresponding SQL operator symbols for use in query generation.
    /// </remarks>
    public static class SqlOperatorExtensions
    {
        /// <summary>
        /// Converts a <see cref="SqlOperator"/> enum value into its corresponding SQL symbol.
        /// </summary>
        /// <param name="op">
        /// The <see cref="SqlOperator"/> value to convert.
        /// </param>
        /// <returns>
        /// A string representing the SQL symbol, such as <c>=</c>, <c>!=</c>, <c>&gt;</c>, <c>&lt;</c>, or <c>LIKE</c>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the provided <paramref name="op"/> value is not defined in <see cref="SqlOperator"/>.
        /// </exception>
        /// <remarks>
        /// This method enables symbolic rendering of SQL operators for use in query builders and expression generators.
        /// </remarks>
        public static string ToSymbol(this SQLOperator op) => op switch
        {
            SQLOperator.Equal => "=",
            SQLOperator.NotEqual => "!=",
            SQLOperator.GreaterThan => ">",
            SQLOperator.LessThan => "<",
            SQLOperator.Like => "LIKE",
            _ => throw new ArgumentOutOfRangeException(nameof(op))
        };
    }
}
