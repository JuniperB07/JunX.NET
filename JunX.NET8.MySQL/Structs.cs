using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.MySQL
{
    /// <summary>
    /// Represents metadata for a single column in a MySQL table definition.
    /// </summary>
    public struct ColumnInformation
    {
        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// Gets or sets the MySQL data type assigned to the column.
        /// </summary>
        public MySQLDataType Type { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the column allows <c>NULL</c> values.
        /// </summary>
        public bool Nullable { get; set; }
        /// <summary>
        /// Gets or sets the default value mode for the column.
        /// </summary>
        public MySQLDefaultMode Default { get; set; }
        /// <summary>
        /// Gets or sets the length constraint for the column, applicable to types like <c>VARCHAR</c> or <c>DECIMAL</c>.
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// Gets or sets the literal default value to be applied when <c>Default</c> is set to <c>AsDefined</c>.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <c>ColumnInformation</c> struct with the specified column metadata.
        /// </summary>
        /// <param name="Name">
        /// The name of the column to be defined in the table schema.
        /// </param>
        /// <param name="DataType">
        /// The MySQL data type assigned to the column, represented by the <c>MySQLDataType</c> enum.
        /// </param>
        /// <param name="IsNullable">
        /// Indicates whether the column allows <c>NULL</c> values. Defaults to <c>false</c>.
        /// </param>
        /// <param name="TypeLength">
        /// The length constraint for the column, applicable to types like <c>VARCHAR</c>, <c>DECIMAL</c>, etc. Defaults to <c>0</c>.
        /// </param>
        /// <param name="DefaultMode">
        /// Specifies how the default value should be applied, using the <c>MySQLDefaultMode</c> enum. Defaults to <c>None</c>.
        /// </param>
        /// <param name="DefValue">
        /// The literal default value to assign when <c>DefaultMode</c> is set to <c>AsDefined</c>. Defaults to an empty string.
        /// </param>
        public ColumnInformation(string Name, MySQLDataType DataType, bool IsNullable = false, int TypeLength = 0, MySQLDefaultMode DefaultMode = MySQLDefaultMode.None, string DefValue = "")
        {
            ColumnName = Name;
            Type = DataType;
            Nullable = IsNullable;
            Default = DefaultMode;
            Length = TypeLength;
            DefaultValue = DefValue;
        }
    }
    /// <summary>
    /// Represents auto-increment configuration for a MySQL table column.
    /// </summary>
    public struct AutoIncrement
    {
        /// <summary>
        /// Gets or sets a value indicating whether auto-increment is enabled for the table.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets the name of the column that should be configured as auto-increment.
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// Initializes a new instance of the <c>AutoIncrement</c> struct with the specified auto-increment settings.
        /// </summary>
        /// <param name="AIEnabled">
        /// Indicates whether auto-increment is enabled for the table.
        /// </param>
        /// <param name="ColumnName">
        /// The name of the column to apply auto-increment to. Defaults to an empty string if unspecified.
        /// </param>
        public AutoIncrement(bool AIEnabled, string ColumnName = "")
        {
            Enabled = AIEnabled;
            Column = ColumnName;
        }
    }
    /// <summary>
    /// Represents primary key configuration for a MySQL table column.
    /// </summary>
    public struct PrimaryKey
    {
        /// <summary>
        /// Gets or sets a value indicating whether a primary key is enabled for the table.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets the name of the column designated as the primary key.
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// Initializes a new instance of the <c>PrimaryKey</c> struct with the specified settings.
        /// </summary>
        /// <param name="PKEnabled">Indicates whether the primary key is enabled.</param>
        /// <param name="ColumnName">The name of the column to be used as the primary key. Defaults to an empty string.</param>
        public PrimaryKey(bool PKEnabled, string ColumnName = "")
        {
            Enabled = PKEnabled;
            Column = ColumnName;
        }
    }
    /// <summary>
    /// Represents metadata for a column to be inserted into a SQL table.
    /// </summary>
    public struct InsertColumnMetadata
    {
        /// <summary>
        /// The name of the column to insert into.
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// The SQL data type of the column.
        /// </summary>
        public MySQLDataType DataType { get; set; }
        /// <summary>
        /// The value to insert into the column.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertColumnMetadata"/> struct with the specified column name, data type, and value.
        /// </summary>
        /// <param name="ToColumn">The name of the column to insert into.</param>
        /// <param name="WithDataType">The SQL data type of the column.</param>
        /// <param name="WithValue">The value to insert into the column.</param>
        public InsertColumnMetadata(string ToColumn, MySQLDataType WithDataType, string WithValue)
        {
            Column = ToColumn;
            DataType = WithDataType;
            Value = WithValue;
        }
    }
    /// <summary>
    /// Represents metadata for a column to be updated in a SQL table.
    /// </summary>
    public struct UpdateColumnMetadata
    {
        /// <summary>
        /// Gets or sets the name of the column to update.
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// Gets or sets the SQL data type of the column.
        /// </summary>
        public MySQLDataType DataType { get; set; }
        /// <summary>
        /// Gets or sets the new value to assign to the column.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateColumnMetadata"/> struct with the specified column name, data type, and value.
        /// </summary>
        /// <param name="UpdateColumn">The name of the column to update.</param>
        /// <param name="WithDataType">The SQL data type of the column.</param>
        /// <param name="SetValueTo">The new value to assign to the column.</param>
        public UpdateColumnMetadata(string UpdateColumn, MySQLDataType WithDataType, string SetValueTo)
        {
            Column = UpdateColumn;
            DataType = WithDataType;
            Value = SetValueTo;
        }
    }
    /// <summary>
    /// Represents metadata for the inner join, including table and column to join on.
    /// </summary>
    public struct JoinMetadata
    {
        /// <summary>
        /// Gets or sets the name of the table to join.
        /// </summary>
        public string Table { get; set; }
        /// <summary>
        /// Gets or sets the name of the column used in the join condition.
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// Gets or sets the alias of the column used in the join condition.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinMetadata"/> class with the specified table, column, and optional alias.
        /// </summary>
        /// <param name="FromTable">The source table to join from.</param>
        /// <param name="SelectColumn">The column to select from the source table.</param>
        /// <param name="As">An optional alias for the selected column.</param>
        public JoinMetadata(string FromTable, string SelectColumn, string As = "")
        {
            Table = FromTable;
            Column = SelectColumn;
            Alias = As;
        }
    }
    /// <summary>
    /// Represents metadata for a single SQL parameter, including its name and assigned value.
    /// </summary>
    public struct ParametersMetadata
    {
        /// <summary>
        /// Gets or sets the name of the SQL parameter.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the parameter's name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the value assigned to the SQL parameter.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the parameter's value.
        /// </value>
        public object Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParametersMetadata"/> struct with the specified name and value.
        /// </summary>
        /// <param name="ParameterName">The name of the SQL parameter.</param>
        /// <param name="ParameterValue">The value assigned to the SQL parameter.</param>
        public ParametersMetadata(string ParameterName, object ParameterValue)
        {
            Name = ParameterName;
            Value = ParameterValue;
        }
    }
    /// <summary>
    /// Represents a column-to-alias mapping used in SQL SELECT statements.
    /// </summary>
    public struct SelectAsMetadata
    {
        /// <summary>
        /// The name of the column to be selected.
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// The alias to assign to the selected column.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Initializes a new instance of the <c>SelectAsMetadata</c> struct with the specified column name and alias.
        /// </summary>
        /// <param name="ColumnName">The name of the column to select.</param>
        /// <param name="As">The alias to assign to the selected column.</param>
        public SelectAsMetadata(string ColumnName, string As)
        {
            Column = ColumnName;
            Alias = As;
        }
    }
    /// <summary>
    /// Represents metadata for a column selection in a SQL query, including its table and column name.
    /// </summary>
    public struct SelectMetadata
    {
        /// <summary>
        /// Gets or sets the name of the table containing the column.
        /// </summary>
        public string Table { get; set; }
        /// <summary>
        /// Gets or sets the name of the column to be selected.
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectMetadata"/> struct with the specified table and column names.
        /// </summary>
        /// <param name="SetTable">The name of the table.</param>
        /// <param name="SetColumn">The name of the column.</param>
        public SelectMetadata(string SetTable, string SetColumn)
        {
            Table = SetTable;
            Column = SetColumn;
        }
    }
}
