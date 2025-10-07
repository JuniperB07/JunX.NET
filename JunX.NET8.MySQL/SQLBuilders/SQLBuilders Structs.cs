using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.MySQL.SQLBuilders
{
    /// <summary>
    /// Represents metadata for aliasing a column in SQL, pairing an enum-defined column with its alias and fully qualified name.
    /// </summary>
    /// <typeparam name="T">
    /// An <see cref="Enum"/> type representing the table whose member defines the column being aliased.
    /// </typeparam>
    /// <remarks>
    /// This struct is used to associate a column with its SQL alias and generate fully qualified references for query composition.
    /// </remarks>
    public struct AliasMetadata<T> where T : Enum
    {
        public T Column { get; set; }
        public string Alias { get; set; }
        public string FullyQualified { get { return typeof(T).Name + "." + Column.ToString(); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliasMetadata{T}"/> struct with the specified column and alias.
        /// </summary>
        /// <param name="Select">
        /// The enum member representing the column to be aliased.
        /// </param>
        /// <param name="As">
        /// The alias to assign to the column in SQL output.
        /// </param>
        /// <remarks>
        /// This constructor pairs a metadata-defined column with its alias, enabling qualified selection and readable output in SQL queries.
        /// </remarks>
        public AliasMetadata(T Select, string As)
        {
            Column = Select;
            Alias = As;
        }
        
    }
    /// <summary>
    /// Represents a typed SQL value for use in <c>INSERT</c> statements, pairing raw input with its associated data type.
    /// </summary>
    /// <remarks>
    /// Values are formatted safely using <c>Construct.SQLSafeValue</c> to ensure proper SQL rendering based on type.
    /// </remarks>
    public struct ValuesMetadata
    {
        private string Values { get; set; }
        private MySQLDataType DataType { get; set; }
        /// <summary>
        /// Gets the SQL-safe representation of the value based on its associated data type.
        /// </summary>
        /// <returns>
        /// A string formatted for safe inclusion in SQL statements, using <c>Construct.SQLSafeValue</c>.
        /// </returns>
        /// <remarks>
        /// This property ensures that the raw value is rendered appropriately for its <see cref="MySQLDataType"/>, preventing injection and formatting errors.
        /// </remarks>
        public string Value
        {
            get
            {
                return Construct.SQLSafeValue(DataType, Values);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValuesMetadata"/> struct with the specified value and data type.
        /// </summary>
        /// <param name="WithValue">
        /// The raw string value to be inserted into the SQL statement.
        /// </param>
        /// <param name="WithDataType">
        /// The SQL data type of the value, defined by <see cref="MySQLDataType"/>.
        /// </param>
        /// <remarks>
        /// This constructor prepares the value for safe SQL rendering using its associated type.
        /// </remarks>
        public ValuesMetadata(string WithValue, MySQLDataType WithDataType)
        {
            Values = WithValue;
            DataType = WithDataType;
        }
    }
}
