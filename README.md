# JunX.NET

<strong>About This Library</strong>
<br/>
<p align="justify">This libray contains multiple namespaces. All of these namespaces are to provide additional functionalities to MySql.Data.MySqlClient for easier MySql Database Querying and Manipulation, additional functionalities for reading enum types, manipulating WinForm Controls, and an Encryption Service that contains functionalities for symmetric encryption.</p>
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
  <li>Construct Class - Provide methods that will construct various MySQL Command Strings</li>
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
    </ul>
</ul>
