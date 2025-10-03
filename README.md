# JunX.NET

<strong>About This Library</strong>
<br/>
<p align="justify">This libray contains multiple namespaces. All of these namespaces are to provide additional functionalities to MySql.Data.MySqlClient for easier MySql Database Querying and Manipulation, additional functionalities for reading enum types, manipulating WinForm Controls, and an Encryption Service that contains functionalities for symmetric encryption.</p>
<br/>
<p>The following are the namespaces found in this library as well as a brief description.</p>
<strong>JunX.NET8.EncryptionService</strong>
<p align="justify">Provides methods for encrypting and decrypting strings using symmetric AES encryption with a key derived from a passphrase.</p>
<ul>
  <li>EncryptionService.EncryptionService(string) - Initializes a new instance of the class using the specified key material.</li>
  <li>EncryptionService.Encrypt(string) - Encrypts the specified plain string and returns the result as a Base64-encoded string.</li>
  <li>EncryptionService.Decrypt(string) - Decrypts the specified Base64-encoded string.</li>
  <li>InvalidTextParameterException - A custom exception. Throws an exception when a string parameter is invalid or does not meet the required criteria.</li>
</ul>
<strong>JunX.NET8.MySQL</strong>
<p align="justify">Contains classes that provides additional functionalities for easier querying and manipulation of MySql Databases.</p>
<ul>
  <li>Construct Class - Provide methods that will construct various MySQL Command Strings</li>
    <ul>
      <li>SelectAllCommand - Generates an SQL SELECT statement that retrieves all columns from a specified table.</li>
      <li>SelectCommand - Generates an SQL SELECT statement that retrieves the specified column/s from a specified table. An optional WHERE clause is appended if specified.</li>
      <li>SelectAliasCommand - Generates an SQL SELECT statement that assigns (an) alias(es) to the specified column/s from a specified table. An optional WHERE clause is appended if specified. This method
        uses the custom struct: SelectAsMetadata to map the column/s to the alias/es</li>
      <li></li>
    </ul>
</ul>
