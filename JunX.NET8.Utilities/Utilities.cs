using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.Utilities
{
    /// <summary>
    /// Provides utility methods for working with enum types.
    /// </summary>
    /// <typeparam name="T">The enum type to operate on.</typeparam>
    public static class ThisEnum<T> where T : Enum
    {
        /// <summary>
        /// Returns all member names of the enum type <typeparamref name="T"/> as a list of strings.
        /// </summary>
        /// <returns>A list of enum member names.</returns>
        public static List<string> ToList()
        {
            return Enum.GetNames(typeof(T)).ToList();
        }
    }

    /// <summary>
    /// Provides additional utilities for working with .NET.
    /// </summary>
    public static class Utilities
    {
        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Returns a fallback string if the input is null, empty, or consists only of whitespace.
        /// </summary>
        /// <param name="Value">
        /// The input string to evaluate.
        /// </param>
        /// <returns>
        /// <c>"N/A"</c> if the input is null, empty, or whitespace; otherwise, returns the original string.
        /// </returns>
        /// <remarks>
        /// This method is useful for normalizing display values in reports, logs, or UI elements.
        /// </remarks>
        public static string FillEmptyString(string Value)
        {
            if (string.IsNullOrWhiteSpace(Value))
                return "N/A";
            else
                return Value;
        }
        #endregion
    }
}
