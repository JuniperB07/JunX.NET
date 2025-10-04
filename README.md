# JunX.NET
<img src="JunX.NET8.png" align="right" width="200px" height="200px" /> 

<strong>About This Library</strong>
<br/>
<p align="justify">This library contains multiple namespaces. All of these namespaces are to provide additional functionalities to MySql.Data.MySqlClient for easier MySql Database Querying and Manipulation, additional functionalities for reading enum types, manipulating WinForm Controls, and an Encryption Service that contains functionalities for symmetric encryption.</p>
<br/>
<p>The following are the namespaces found in this library as well as a brief description.</p>
<strong>JunX.NET8.EncryptionService</strong>
<p align="justify">Provides methods for encrypting and decrypting strings using symmetric AES encryption with a key derived from a passphrase.</p>
<ul>
  <li><strong>EncryptionService.EncryptionService(string)</strong> - Initializes a new instance of the class using the specified key material.</li>
  <li><strong>EncryptionService.Encrypt(string)</strong> - Encrypts the specified plain string and returns the result as a Base64-encoded string.</li>
  <li><strong>EncryptionService.Decrypt(string)</strong> - Decrypts the specified Base64-encoded string.</li>
  <li><strong>InvalidTextParameterException</strong> - A custom exception. Throws an exception when a string parameter is invalid or does not meet the required criteria.</li>
</ul>
<strong>JunX.NET8.MySQL</strong>
<p align="justify">Contains classes that provides additional functionalities for easier querying and manipulation of MySql Databases.</p>
<ul>
  <li><strong>Construct Class</strong> - Provide methods that will construct various MySQL Command Strings</li>
    <ul>
      <li><strong>SelectAllCommand</strong> - Generates an SQL SELECT statement that retrieves all columns from a specified table.</li>
      <li><strong>SelectCommand</strong> - Generates an SQL SELECT statement that retrieves the specified column/s from a specified table. An optional WHERE clause is appended if specified.</li>
      <li><strong>SelectAliasCommand</strong> - Generates an SQL SELECT statement that assigns (an) alias(es) to the specified column/s from a specified table. An optional WHERE clause is appended if 
        specified. This method uses the custom struct: <strong>SelectAsMetadata</strong> to map the column/s to the alias/es</li>
      <li><strong>SelectAliasOrderByCommand</strong> - Generates an SQL SELECT statement similar to <strong>SelectAliasCommand</strong>. Appends an optional WHERE clause and an ORDER BY clause.
        This method uses the following custom structs and enums:</li>
        <ul>
          <li><strong>SelectAsMetadata</strong> - a custom struct for column-to-alias mapping.</li>
          <li><strong>MySQLOrderBy</strong> - a custom enum used to specify the ordering direction (ASC or DESC).</li>
        </ul>
      <li><strong>SelectDistinctCommand</strong> - Generates an SQL SELECT DISTINCT command for the specified column/s.</li>
      <li><strong>SelectOrderByCommand</strong> - Similar to <strong>SelectCommand</strong> method but with an ORDER BY clause appended. An optional WHERE clause is appended before the ORDER BY
        clause if specified. This method uses the custom enum: <strong>MySQLOrderBy</strong> to specify the ordering direction (ASC or DESC).</li>
      <li><strong>InsertIntoCommand</strong> - Generates an SQL INSERT INTO command that inserts rows to the specified table. This method uses the custom struct: <strong>InsertColumnMetadata</strong>
        to store the column name, its MySQL data type (uses custom enum <strong>MySQLDataType</strong>), and the corresponding value to insert</li>
      <li><strong>UpdateCommand</strong> - Generates an SQL UPDATE command for one or more columns. An optional but recommended WHERE clause is appended if specified. This method
        uses the custom struct: <strong>UpdateColumnMetadata</strong> to map out the column to be updated, the column's datatype, and update value.</li>
      <li><strong>DeleteCommand</strong> - Generates an SQL DELETE command for the specified table. An optional but recommended WHERE clause is appended if specified.</li>
      <li><strong>InnerJoinCommand</strong> - Generates an SQL SELECT command with an INNER JOIN clause. An optional WHERE clause is appended after the INNER JOIN clause if specified. This
        method uses the custom struct: <strong>JoinMetadata</strong> to create a fully qualified SQL SELECT clause and is also used for the INNER JOIN ... ON clause.</li>
      <li><strong>InnerJoinAliasCommand</strong> - This method is similar to the <strong>InnderJoinCommand</strong> method but allows the use of alias.</li>
      <li><strong>TruncateCommand</strong> - Generates an SQL TRUNCATE command that targets the specified table.</li>
      <li><strong>AppendOrderBy</strong> - Appends an ORDER BY clause to the specified command string.</li>
    </ul>
  <li><strong>MySQLDatabase Class</strong> - Provides additional functionality for manipulating MySQL Databases at the database level. (This Class is unfinished)</li>
    <ul>
      <li><strong>DatabaseExists</strong> - Checks whether a specified database exists on the target server using the provided credentials.</li>
      <li><strong>CreateDatabase</strong> - Creates a new MySQL Database using the specified name, character set (uses the custom enum <strong>MySQLCharsets</strong>), and
        collation (uses the custom enum <strong>MySQLCollation</strong>) if it does not already exists.</li>
    </ul>
  <li><strong>DBConnect Class</strong> - Provides additional functionalities for connecting, querying, and manipulating MySQL Databases.</li>
    <ul>
      <li><strong>DBConnect</strong> - Initializes a new instance of the class using the specified connection string or MySql Connection. This sets the internal connection and/or connection string.</li>
      <li><strong>PROPERTIES</strong></li>
        <ul>
          <li><strong>ConnectionString</strong> - Gets/Sets the internal connection string to be used for opening the internal MySQL Connection.</li>
          <li><strong>Connection</strong> - Exposes the internal MySQL Connection.</li>
          <li><strong>State</strong> - Gets the current connection state of the internal MySQL Connection.</li>
          <li><strong>CommandString</strong> - Gets/Sets the internal SQL Command Text for MySQL Command executions.</li>
          <li><strong>Reader</strong> - Exposes the internal MySQL Data Reader.</li>
          <li><strong>Adapter</strong> - Exposes the internal MySQL Data Adapter.</li>
          <li><string>DataSet</string> - Exposes the internal DataSet.</li>
          <li><string>HasRows</string> - Indicates whether the internal MySQL Data Reader contains one or more rows.</li>
          <li><string>Values</string> - Gets the list of string values extracted from the current MySQL Data Reader.</li>
        </ul>
      <li><strong>FUNCTIONS/METHODS</strong></li>
        <ul>
          <li><strong>Open</strong> - Opens the internal MySQL Connection if it's not already open.</li>
          <li><strong>CloseAll</strong> - Closes the internal MySQL Connection and MySQL Data Reader.</li>
          <li><strong>CloseReader</strong> - Closes the internal MySQL Data Reader.</li>
          <li><strong>Dispose</strong> - Asynchronously disposes all managed database resources of the current instance.</li>
          <li><strong>Reset</strong> - Resets the internal MySQL Connection by closing it and opening it again using the current instance's internal connection string.</li>
          <li><strong>ExecuteReader</strong> - Executes the current internal MySQL Command Text and initializes the internal MySQL Data Reader for result traversal.
            This method contains 3 overloads, 2 of which uses the custom struct: <strong>ParameterMetadata</strong> for parameterized SQL commands.</li>
          <li><strong>ExecuteNonQuery</strong> - Executes the current MySQL Command Text that does not return any result set such as INSERT, UPDATE, and DELETE.
            This method contains 3 overloads, 2 of which uses the custom struct: <strong>ParameterMetadata</strong> for parameterized SQL commands.</li>
          <li><strong>ExecuteAdapter</strong> - Initalizes the internal MySQL Data Adapter with the current MySQL SELECT command stored in the internal command string using the internal MySQL Connection.
            This method contains 3 overloads, 2 of which uses the custom struct: <strong>ParameterMetadata</strong> for parameterized SQL commands.</li>
          <li><strong>ExecuteDataSet</strong> - Executes the current SQL SELECT command stored in the internal command string and fills the internal DataSet with the results.
            This method contains 3 overloads, 2 of which uses the custom struct: <strong>ParameterMetadata</strong> for parameterized SQL commands.</li>
          <li><strong>GetValue</strong> - Extracts all fields from the internal MySQL Data Reader and stores it to the internal list of strings.</li>
        </ul>
    </ul>
  <li><strong>MySQLTables Class</strong> - Provides additional functionalities for creating and manipulating MySQL Database Tables.</li>
  <ul>
    <li><strong>CreateTableCommand</strong> - Generates an MySQL CREATE TABLE command using the provided metadata. This method uses the following custom structs and enums:</li>
      <ul>
        <li><strong>ColumnInformation</strong> - Represents metadata for a single column in a MySQL Table Definition. This parameters accepts an array of this struct.</li>
        <li><strong>AutoIncrement</strong> - Represents auto-increment configuration for a MySQL table column.</li>
        <li><strong>PrimaryKey</strong> - Represents primary key configuration for a MySQL table column.</li>
        <li><strong>MySQLEngine</strong> - A custom enum that specifies the available storage engines supported by MySQL for table creation and data management.</li>
        <li><strong>MySQLCharsets</strong> - A custom enum that defines supported character sets for MySQL database encoding.</li>
        <li><strong>MySQLCollation</strong> - A custom enum that defines supported MySQL collations for character set comparison and sorting behavior.</li>
      </ul>
    <li><strong>GetColumns</strong> - Retrieves the list of column names from a specified table.</li>
    <li><strong>GenerateEnumFromList</strong> - Generates a C# enum definition from a list of string values that contains column names from a MySQL database table.</li>
  </ul>
  <li><strong>SQLBuilder Class</strong> - Provides a fluent interface for constructing SQL Command Strings.</li>
  <ul>
    <li><strong>SQLSelect Subclass</strong> - Provides a fluent interface for constructing an SQL SELECT command string.</li>
      <ul>
        <li><strong>Column</strong> - Appends a column name to the SELECT clause of the SQL query.</li>
        <li><strong>Columns</strong> - Appends multiple column names to the SELECT clause of the SQL query. This method has 1 overload that uses a custom struct <strong>SelectMetadata</strong>
          for a fully qualified SELECT command.</li>
        <li><strong>From</strong> - Appends a FROM clause.</li>
        <li><strong>Where</strong> - Appends a WHERE clause. This method uses the custom enum <strong>MySQLLogicalOperators</strong> for multiple WHERE conditions.</li>
        <li><strong>StartGroupedWhere</strong> - Starts a grouped WHERE clause by appending an open-parenthesis "(".</li>
        <li><strong>EndGroupedWhere</strong> - Ends a grouped WHERE clause by appending a close-parenthesis ")"</li>
        <li><strong>Distinct</strong> - Begins a SELECT DISTINCT SQL command.</li>
        <li><strong>As</strong> - Appends an alias clause.</li>
        <li><strong>OrderBy</strong> - Appends an ORDER BY clause. This method uses the custom enum <strong>MySQLOrderBy</strong> to specify ordering.</li>
        <li><strong>Min</strong> - Begins a SELECT MIN command.</li>
        <li><strong>Max</strong> - Begins a SELECT MAX command.</li>
        <li><strong>Count</strong> - Begins a SELECT COUNT command.</li>
        <li><strong>Sum</strong> - Begins a SELECT SUM command.</li>
        <li><strong>Avg</strong> - Begins a SELECT AVG command.</li>
        <li><strong>InnerJoin</strong> - Appends an INNER JOIN clause. This method uses the custom struct <strong>JoinMetadata</strong>.</li>
        <li><strong>LeftJoin</strong> - Appends a LEFT JOIN clause. This method uses the custom struct <strong>JoinMetadata</strong>.</li>
        <li><strong>RightJoin</strong> - Appends a RIGHT JOIN clause. This method uses the custom struct <strong>JoinMetadata</strong>.</li>
        <li><strong>FullOuterJoin</strong> - Appends a FULL OUTER JOIN clause. This method uses the custom struct <strong>JoinMetadata</strong>.</li>
        <li><strong>ToString</strong> - Returns the complete SQL Command String terminated with a semicolon.</li>
      </ul>
  </ul>
  <li><strong>MySQLExecute Class</strong> - An outdated class succeeded by <strong>DBConnect</strong>. All the methods and properties of this class can be found in <strong>DBConnect</strong> as well.</li>
</ul>
<strong>JunX.NET8.Utilities</strong>
<p align="justify">Contains classes that provides various additional functionalities within .NET8.</p>
<ul>
  <li><strong>ThisEnum Class</strong> - Provides utility methods for dealing with enum types. Uses parameter T: where T is the enum type to operate on.</li>
  <ul>
    <li><strong>ToList</strong> - Returns all member names of the enum type T as a list of strings.</li>
  </ul>
</ul>
<strong>JunX.NET8.WinForms</strong>
<p align="justify">Contains classes, methods, and structs to provide additional functionalities in manipulating System.Windows.Forms Controls.</p>
<ul>
  <li><strong>Forms</strong> - Provides additional functionalities for manipulating System.Windows.Forms Controls.</li>
  <ul>
    <li><strong>SetControlEnabled</strong> - Sets the Enabled state of the specified control/s.</li>
    <li><strong>HasEmptyFields</strong> - Determines whether any control in the specified controls array has an empty Text property.</li>
    <li><strong>ClearControlText</strong> - Clears the Text property of the specified contorl/s.</li>
    <li><strong>ClearComboBox</strong> - Removes all items from the specified ComboBox/es</li>
    <li><strong>FillComboBox</strong> - Fills the specified ComboBox/es with the corresponding string array/list of items.</li>
    <li><strong>AppendComboBox</strong> - Adds the specified collection of items to the specified ComboBox/es.</li>
    <li><strong>ClearListBox</strong> - Removes all items from the specified ListBox/es.</li>
    <li><strong>FillListBox</strong> - Fills the specified ListBox/es with the corresponding specified collection of items.</li>
    <li><strong>AppendListBox</strong> - Adds the specified collection of items to the specified ListBox/es.</li>
    <li><strong>SetDateTimePickerMaxDate</strong> - Sets the MaxDate property of the specified DateTimePicker/s.</li>
    <li><strong>SetDateTimePickerMinDate</strong> - Sets the MinDate property of the specified DateTimePicker/s.</li>
    <li><strong>ResetDataGridView</strong> - Resets the specified DataGridView/s by clearing the rows and removing the data source.</li>
    <li><strong>SetDataGridViewDataSet</strong> - Sets the DataSource property of the specified DataGridView/s by binding it to a table of the specified DataSet/s.</li>
    <li><strong>ControlType Class</strong> - Provides recursive extraction of controls of a specified control type from a container, optionally filtered by prefix. This class uses T where T : Control.</li>
    <ul>
      <li><strong>Extract</strong> - Recursively extracts all cotnrols of the specified type from the specified container and it's optionally filtered by name prefix. This method returns a
        collection of controls of the specified type.</li>
    </ul>
  </ul>
</ul>
<br/>
<br/>
<p align="justify">More functionalities will be added.</p>
