using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Security;

namespace JunX.NET8.EncryptionService
{
    /// <summary>
    /// Provides methods for encrypting and decrypting text using symmetric encryption with a key derived from a
    /// passphrase.
    /// </summary>
    /// <remarks>The encryption and decryption operations use the AES algorithm with a key and initialization
    /// vector (IV) derived from the provided passphrase. The same passphrase must be used for both encryption and
    /// decryption to ensure correct results. This class is not intended for use with large data streams or files.
    /// Thread safety is not guaranteed; create a separate instance for each thread if used concurrently.</remarks>
    public class EncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        /// <summary>
        /// Initializes a new instance of the EncryptionService class using the specified key material.
        /// </summary>
        /// <remarks>The provided key is hashed internally to generate cryptographic keys. Use a strong,
        /// unpredictable value for best security.</remarks>
        /// <param name="Key">The secret key material used to derive the encryption key and initialization vector. Cannot be null or
        /// empty.</param>
        public EncryptionService(string Key)
        {
            using var sha = SHA256.Create();
            using var md6 = MD5.Create();

            _key = sha.ComputeHash(Encoding.UTF8.GetBytes(Key));
            _iv = md6.ComputeHash(Encoding.UTF8.GetBytes(Key));
        }

        /// <summary>
        /// Encrypts the specified plain text using AES encryption and returns the result as a Base64-encoded string.
        /// </summary>
        /// <param name="Text">The plain text to encrypt. Cannot be null.</param>
        /// <returns>A Base64-encoded string representing the encrypted form of the input text.</returns>
        /// <exception cref="InvalidTextParameterException">Thrown if the input text is invalid or if a cryptographic error occurs during encryption.</exception>
        public string Encrypt(string Text)
        {
            try
            {
                using var aes = Aes.Create();
                aes.Key = _key;
                aes.IV = _iv;

                using var encryptor = aes.CreateEncryptor();
                using var ms = new MemoryStream();
                using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                using var sw = new StreamWriter(cs);

                sw.Write(Text);
                sw.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw new InvalidTextParameterException("Encryption failed due to invalid input or cryptographic error.", ex);
            }
        }
        /// <summary>
        /// Decrypts the specified Base64-encoded string using the configured AES key and IV.
        /// </summary>
        /// <param name="Text">The Base64-encoded string to decrypt. Must represent data encrypted with the corresponding AES key and IV.</param>
        /// <returns>The decrypted plaintext string.</returns>
        /// <exception cref="InvalidTextParameterException">Thrown if the input is not a valid Base64 string or if decryption fails due to an invalid key, IV, or
        /// corrupted data.</exception>
        public string Decrypt(string Text)
        {
            try
            {
                using var aes = Aes.Create();
                aes.Key = _key;
                aes.IV = _iv;

                byte[] buffer;
                try
                {
                    buffer = Convert.FromBase64String(Text);
                }
                catch (FormatException ex)
                {
                    throw new InvalidTextParameterException("The provided text is not a valid Base64 string.", ex);
                }

                using var ms = new MemoryStream(buffer);
                using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch (CryptographicException ex)
            {
                throw new InvalidTextParameterException("Decryption failed due to invalid key, IV, or corrupted data.", ex);
            }
        }
    }

    /// <summary>
    /// The exception that is thrown when a text parameter is invalid or does not meet required criteria.
    /// </summary>
    /// <remarks>Use this exception to indicate that a method or operation has received a text-based argument
    /// that is not valid, such as being null, empty, or failing validation rules. This exception is intended to provide
    /// clearer error handling for invalid text input scenarios.</remarks>
    public class InvalidTextParameterException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidTextParameterException class.
        /// </summary>
        public InvalidTextParameterException() { }
        /// <summary>
        /// Initializes a new instance of the InvalidTextParameterException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidTextParameterException(string message)
            : base(message) { }
        /// <summary>
        /// Initializes a new instance of the InvalidTextParameterException class with a specified error message and a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is
        /// specified.</param>
        public InvalidTextParameterException(string message, Exception inner)
            : base (message, inner) { }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
