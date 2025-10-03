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
}
