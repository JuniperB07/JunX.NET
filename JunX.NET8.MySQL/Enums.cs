using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.MySQL
{
    /// <summary>
    /// Specifies the available storage engines supported by MySQL for table creation and data management.
    /// </summary>
    public enum MySQLEngine
    {
        /// <summary>
        /// Hash based, stored in memory, useful for temporary tables
        /// </summary>
        MEMORY,
        /// <summary>
        /// 	Collection of identical MyISAM tables
        /// </summary>
        MRG_MYISAM,
        /// <summary>
        /// 	CSV storage engine
        /// </summary>
        CSV,
        /// <summary>
        /// Federated MySQL storage engine
        /// </summary>
        FEDERATED,
        /// <summary>
        /// Performance Schema
        /// </summary>
        PERFORMANCE_SCHEMA,
        /// <summary>
        /// 	MyISAM storage engine
        /// </summary>
        MyISAM,
        /// <summary>
        /// Supports transactions, row-level locking, and foreign keys
        /// </summary>
        InnoDB,
        /// <summary>
        /// MySQL Cluster system information storage engine
        /// </summary>
        ndbinfo,
        /// <summary>
        /// 	/dev/null storage engine (anything you write to it disappears)
        /// </summary>
        BLACKHOLE,
        /// <summary>
        /// Archive storage engine
        /// </summary>
        ARCHIVE,
        /// <summary>
        /// Clustered, fault-tolerant tables
        /// </summary>
        ndbcluster
    }
    /// <summary>
    /// Defines supported character sets for MySQL database encoding.
    /// </summary>
    public enum MySQLCharsets
    {
        utf8mb4
    }
    /// <summary>
    /// Defines supported MySQL collations for character set comparison and sorting behavior.
    /// </summary>
    public enum MySQLCollation
    {
        utf8mb4_0900_ai_ci
    }
    /// <summary>
    /// Defines supported MySQL data types for column definitions during table creation.
    /// </summary>
    public enum MySQLDataType
    {
        /// <summary>
        /// 	A FIXED length string (can contain letters, numbers, and special characters).
        /// </summary>
        Char,
        /// <summary>
        /// A VARIABLE length string (can contain letters, numbers, and special characters). 
        /// </summary>
        VarChar,
        /// <summary>
        /// Holds a string with a maximum length of 255 characters
        /// </summary>
        TinyText,
        /// <summary>
        /// Holds a string with a maximum length of 65,535 bytes
        /// </summary>
        Text,
        /// <summary>
        /// Holds a string with a maximum length of 4,294,967,295 characters
        /// </summary>
        LongText,
        /// <summary>
        /// 	Holds a string with a maximum length of 16,777,215 characters
        /// </summary>
        MediumText,

        /// <summary>
        /// 	A very small integer. Signed range is from -128 to 127. Unsigned range is from 0 to 255.
        /// </summary>
        TinyInt,
        /// <summary>
        /// Zero is considered as false, nonzero values are considered as true.
        /// </summary>
        Bool,
        /// <summary>
        /// A small integer. Signed range is from -32768 to 32767. Unsigned range is from 0 to 65535.
        /// </summary>
        SmallInt,
        /// <summary>
        /// 	A medium integer. Signed range is from -8388608 to 8388607. Unsigned range is from 0 to 16777215.
        /// </summary>
        MediumInt,
        /// <summary>
        /// A medium integer. Signed range is from -2147483648 to 2147483647. Unsigned range is from 0 to 4294967295.
        /// </summary>
        Int,
        /// <summary>
        /// A large integer. Signed range is from -9223372036854775808 to 9223372036854775807. Unsigned range is from 0 to 18446744073709551615.
        /// </summary>
        BigInt,
        /// <summary>
        /// A floating point number. The total number of digits is specified in size. 
        /// The number of digits after the decimal point is specified in the d parameter. 
        /// This syntax is deprecated in MySQL 8.0.17, and it will be removed in future MySQL versions
        /// </summary>
        Float,
        /// <summary>
        /// A normal-size floating point number. The total number of digits is specified in size.
        /// The number of digits after the decimal point is specified in the <c>d</c> parameter
        /// </summary>
        Double,
        /// <summary>
        /// An exact fixed-point number.
        /// The total number of digits is specified in <c>size</c>.
        /// The number of digits after the decimal point is specified in the <c>d</c> parameter.
        /// The maximum number for size is 65.
        /// The maximum number for <c>d</c> is 30.
        /// The default value for <c>size</c> is 10.
        /// The default value for <c>d</c> is 0.
        /// </summary>
        Decimal,

        /// <summary>
        /// A date. Format: YYYY-MM-DD.
        /// The supported range is from '1000-01-01' to '9999-12-31'
        /// </summary>
        Date,
        /// <summary>
        /// A date and time combination.
        /// Format: YYYY-MM-DD hh:mm:ss.
        /// The supported range is from '1000-01-01 00:00:00' to '9999-12-31 23:59:59'.
        /// Adding DEFAULT and ON UPDATE in the column definition to get automatic initialization and updating to the current date and time
        /// </summary>
        DateTime,
        /// <summary>
        /// A timestamp.
        /// <c>TIMESTAMP</c> values are stored as the number of seconds since the Unix epoch ('1970-01-01 00:00:00' UTC).
        /// Format: YYYY-MM-DD hh:mm:ss.
        /// The supported range is from '1970-01-01 00:00:01' UTC to '2038-01-09 03:14:07' UTC.
        /// Automatic initialization and updating to the current date and time can be specified using DEFAULT CURRENT_TIMESTAMP and ON UPDATE CURRENT_TIMESTAMP in the column definition
        /// </summary>
        Timestamp,
        /// <summary>
        /// A time. Format: hh:mm:ss.
        /// The supported range is from '-838:59:59' to '838:59:59'
        /// </summary>
        Time,
        /// <summary>
        /// A year in four-digit format. Values allowed in four-digit format: 1901 to 2155, and 0000.
        /// MySQL 8.0 does not support year in two-digit format.
        /// </summary>
        Year
    }
    /// <summary>
    /// Specifies how default values should be applied to MySQL column definitions during table creation.
    /// </summary>
    public enum MySQLDefaultMode
    {
        /// <summary>
        /// Sets no default value for the column. The column will follow MySQL's implicit default behavior.
        /// </summary>
        None,
        /// <summary>
        /// Explicitly sets the column's default value to <c>NULL</c>.
        /// </summary>
        Null,
        /// <summary>
        /// Applies a user-defined default value, such as a literal or function (e.g., <c>'active'</c>, <c>NOW()</c>).
        /// </summary>
        AsDefined
    }
    /// <summary>
    /// Specifies the ordering mode to be used in generating MySQL <c>SELECT</c> commands.
    /// </summary>
    public enum MySQLOrderBy
    {
        /// <summary>
        /// Sets the ordering mode to Ascending.
        /// </summary>
        ASC,
        /// <summary>
        /// Sets the ordering mode to Descending
        /// </summary>
        DESC
    }
    /// <summary>
    /// Represents logical operators used in MySQL conditional expressions.
    /// </summary>
    public enum MySQLLogicalOperators
    {
        None,
        AND,
        OR,
        NOT,
        IS
    }
}
