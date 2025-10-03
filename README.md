# JunX.NET

<strong>About This Library</strong>
<br/>
<p align="justify">This libray contains multiple namespaces. All of these namespaces are to provide additional functionalities to MySql.Data.MySqlClient for easier MySql Database Querying and Manipulation, additional functionalities for reading enum types, manipulating WinForm Controls, and an Encryption Service that contains functionalities for symmetric encryption.</p>
<br/>
<br/>
<p>The following are the namespaces found in this library as well as a brief description.</p>
<strong>JunX.NET8.EncryptionService</strong>
<p align="justify">Provides methods for encrypting and decrypting strings using symmetric AES encryption with a key derived from a passphrase.</p>
<ul>
  <li>EncryptionService.EncryptionService(string) - Initializes a new instance of the class using the specified key material.</li>
  <li>EncryptionService.Encrypt(string) - Encrypts the specified plain string and returns the result as a Base64-encoded string.</li>
  <li>EncryptionService.Decrypt(string) - Decrypts the specified Base64-encoded string.</li>
</ul>
