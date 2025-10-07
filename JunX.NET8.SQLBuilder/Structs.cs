using JunX.NET8.MySQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.SQLBuilder
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
    /// Represents a typed SQL value with its associated data type for safe insertion into SQL statements.
    /// </summary>
    /// <remarks>
    /// This struct encapsulates a raw value and its <see cref="MySQLDataType"/>, enabling safe formatting via <c>Construct.SQLSafeValue</c>.
    /// It is typically used in metadata-driven <c>INSERT INTO</c> builders to ensure type-aware value handling.
    /// </remarks>
    public struct ValuesMetadata
    {
        private string Values { get; set; }
        private MySQLDataType DataTypes { get; set; }
        /// <summary>
        /// Gets the SQL-safe representation of the stored value using its associated <see cref="MySQLDataType"/>.
        /// </summary>
        /// <returns>
        /// A string formatted for safe inclusion in SQL statements, based on the value's data type.
        /// </returns>
        /// <remarks>
        /// This property delegates to <c>Construct.SQLSafeValue</c> to ensure proper quoting, escaping, and formatting.
        /// It is intended for use in metadata-driven SQL builders where type-aware safety is critical.
        /// </remarks>
        public string Value
        {
            get
            {
                return Construct.SQLSafeValue(DataTypes, Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValuesMetadata"/> struct with a raw value and its associated SQL data type.
        /// </summary>
        /// <param name="WithValue">
        /// The raw string value to be inserted into the SQL statement.
        /// </param>
        /// <param name="WithDataType">
        /// The <see cref="MySQLDataType"/> representing the SQL type of the value (e.g., <c>VARCHAR</c>, <c>INT</c>, <c>DATE</c>).
        /// </param>
        /// <remarks>
        /// This constructor sets up the internal metadata for safe SQL value formatting via the <c>Value</c> property.
        /// </remarks>
        public ValuesMetadata(string WithValue,  MySQLDataType WithDataType)
        {
            Values = WithValue;
            DataTypes = WithDataType;
        }
    }
    /// <summary>
    /// Represents a typed SQL update target, pairing a column with its corresponding value and data type.
    /// </summary>
    /// <typeparam name="T">
    /// An enum type that identifies the column being updated.
    /// </typeparam>
    /// <remarks>
    /// This struct encapsulates metadata for safe SQL update operations, including the target column, raw value, and its <see cref="MySQLDataType"/>.
    /// The <c>Value</c> property provides a SQL-safe representation via <c>Construct.SQLSafeValue</c>, ensuring proper formatting and escaping.
    /// </remarks>
    public struct UpdateMetadata<T> where T: Enum
    {
        private string Values { get; set; }
        private MySQLDataType DataTypes { get; set; }

        /// <summary>
        /// Gets or sets the enum identifier for the column being updated in the SQL statement.
        /// </summary>
        /// <value>
        /// An enum value of type <typeparamref name="T"/> representing the target column.
        /// </value>
        /// <remarks>
        /// This property provides a strongly typed reference to the column, enabling safer and more expressive update logic in metadata-driven SQL builders.
        /// </remarks>
        public T Column { get; set; }
        /// <summary>
        /// Gets the SQL-safe representation of the update value using its associated <see cref="MySQLDataType"/>.
        /// </summary>
        /// <value>
        /// A string formatted for safe inclusion in SQL <c>SET</c> clauses, based on the value's data type.
        /// </value>
        /// <remarks>
        /// This property delegates to <c>Construct.SQLSafeValue</c> to ensure proper quoting, escaping, and formatting.
        /// It is intended for use in metadata-driven SQL update builders where type-aware safety is critical.
        /// </remarks>
        public string Value { get { return Construct.SQLSafeValue(DataTypes, Values); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMetadata{T}"/> struct with a column, raw value, and associated SQL data type.
        /// </summary>
        /// <param name="UpdateColumn">
        /// The enum value identifying the column to be updated.
        /// </param>
        /// <param name="WithValue">
        /// The raw string value to assign to the column.
        /// </param>
        /// <param name="WithDataType">
        /// The <see cref="MySQLDataType"/> representing the SQL type of the value (e.g., <c>VARCHAR</c>, <c>INT</c>, <c>DATE</c>).
        /// </param>
        /// <remarks>
        /// This constructor sets up the metadata required for safe SQL update composition, enabling type-aware formatting via the <c>Value</c> property.
        /// </remarks>
        public UpdateMetadata(T UpdateColumn, string WithValue, MySQLDataType WithDataType)
        {
            Column = UpdateColumn;
            Values = WithValue;
            DataTypes = WithDataType;
        }
    }
    /// <summary>
    /// Represents a metadata container for SQL update operations, pairing a column name with its value and associated data type.
    /// </summary>
    /// <remarks>
    /// This struct encapsulates the components required to safely construct a SQL <c>SET</c> clause, including the column name,
    /// raw value, and its <see cref="MySQLDataType"/>. The <c>Value</c> property provides a SQL-safe representation using <c>Construct.SQLSafeValue</c>.
    /// Intended for use in dynamic, metadata-driven SQL update builders.
    /// </remarks>
    public struct UpdateMetadata
    {
        private string Values { get; set; }
        private MySQLDataType DataTypes { get; set; }

        /// <summary>
        /// Gets or sets the name of the column to be updated in the SQL statement.
        /// </summary>
        /// <value>
        /// A string representing the target column name, used in the <c>SET</c> clause of an <c>UPDATE</c> command.
        /// </value>
        /// <remarks>
        /// This property provides a flexible, string-based reference to the column, suitable for dynamic or loosely typed update scenarios.
        /// </remarks>
        public string Column { get; set; }
        /// <summary>
        /// Gets the SQL-safe representation of the update value using its associated <see cref="MySQLDataType"/>.
        /// </summary>
        /// <value>
        /// A string formatted for safe inclusion in SQL <c>SET</c> clauses, based on the value's data type.
        /// </value>
        /// <remarks>
        /// This property delegates to <c>Construct.SQLSafeValue</c> to ensure proper quoting, escaping, and formatting.
        /// It is intended for use in dynamic SQL update builders where type-aware safety is essential.
        /// </remarks>
        public string Value { get { return Construct.SQLSafeValue(DataTypes, Values); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMetadata"/> struct with a column name, raw value, and associated SQL data type.
        /// </summary>
        /// <param name="UpdateColumn">
        /// The name of the column to be updated in the SQL statement.
        /// </param>
        /// <param name="WithValue">
        /// The raw string value to assign to the column.
        /// </param>
        /// <param name="WithDataType">
        /// The <see cref="MySQLDataType"/> representing the SQL type of the value (e.g., <c>VARCHAR</c>, <c>INT</c>, <c>DATE</c>).
        /// </param>
        /// <remarks>
        /// This constructor sets up the metadata required for safe SQL update composition, enabling type-aware formatting via the <c>Value</c> property.
        /// </remarks>
        public UpdateMetadata(string UpdateColumn, string WithValue, MySQLDataType WithDataType)
        {
            Column = UpdateColumn;
            Values = WithValue;
            DataTypes = WithDataType;
        }
    }
}
