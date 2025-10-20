using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CPC.Infrastructure.CrossCutting.Helpers
{
    public static class ExtensionsMethods
    {
        public static string TextJsonSerializerToString<T>(this T source)
        {
            return System.Text.Json.JsonSerializer.Serialize(source);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string Serialize<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
                {
                    xmlserializer.Serialize(writer, value);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public static List<T> DeserializeList<T>(this string serializedData)
        {
            var serializer = new XmlSerializer(typeof(List<T>));
            var reader = new XmlTextReader(new StringReader(serializedData));

            return (List<T>)serializer.Deserialize(reader);
        }

        public static T DeserializeXML<T>(this string serializedData)
        {
            var serializer = new XmlSerializer(typeof(T));
            var reader = new XmlTextReader(new StringReader(serializedData));
            return (T)serializer.Deserialize(reader);
        }

        public static T Deserialize<T>(this string serializedData)
        {
            var serializer = new XmlSerializer(typeof(T));
            var reader = new XmlTextReader(new StringReader(serializedData.ParseXml("http://schemas.xmlsoap.org/soap/envelope/", "Body")));
            return (T)serializer.Deserialize(reader);
        }

        public static T DeserializeJson<T>(this string serializedData)
        {
            var result = JsonConvert.DeserializeObject<T>(serializedData);
            return result;
        }

        public static string SerializeToJson<T>(this T source)
        {
            return JsonConvert.SerializeObject(source);
        }

        private static object Deserializer<T>(string serializedData)
        {
            throw new NotImplementedException();
        }

        public static List<T> GetListEntity<T>(this Stream stream)
        {
            var entity = default(List<T>);
            var objText = string.Empty;

            using (var reader = new StreamReader(stream))
            {
                objText = reader.ReadToEnd();
                entity = objText.DeserializeList<T>();
            }

            return entity;
        }

        public static T GetEntity<T>(this Stream stream)
        {
            var entity = default(T);
            var objText = string.Empty;

            using (var reader = new StreamReader(stream))
            {
                objText = reader.ReadToEnd();
                entity = objText.Deserialize<T>();
            }

            return entity;
        }

        public static T GetEntityFromJSON<T>(this Stream stream)
        {
            try
            {

                var entity = default(T);
                var objText = string.Empty;

                using (var reader = new StreamReader(stream))
                {
                    objText = reader.ReadToEnd();
                    entity = objText.DeserializeJson<T>();
                }

                return entity;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static string ParseXml(this string sXml, string sNs, string sMethod)
        {
            try
            {
                XDocument xd = XDocument.Parse(sXml);

                if (xd.Root != null)
                {
                    XNamespace xmlns = sNs;
                    var xmlElements = from result in xd.Descendants("{" + xmlns + "}" + sMethod)
                                      select result;

                    return String.Concat(xmlElements.Nodes());
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ToStringWithXmlDeclaration(this XDocument doc)
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);
            doc.Save(writer);
            writer.Flush();
            return builder.ToString();
        }

        public static string ParseSqlIn(this List<string> Lista)
        {
            var cadena = new StringBuilder();
            var Lon = Lista.Count();

            var Con = 1;
            foreach (var item in Lista)
            {
                cadena.Append(string.Format("'{0}'", item.Trim()));
                if (Con < Lon)
                    cadena.Append(",");

                Con += 1;
            }

            return cadena.ToString();
        }

        public static string ParseSqlIn(this List<Guid> Lista)
        {
            var cadena = new StringBuilder();
            var Lon = Lista.Count();

            var Con = 1;
            foreach (var item in Lista)
            {
                cadena.Append(string.Format("'{0}'", item.ToString()));
                if (Con < Lon)
                    cadena.Append(",");

                Con += 1;
            }

            return cadena.ToString();
        }

        public static string GetCalculo(this string XmlString, string Esquema)
        {
            XDocument Xml = XDocument.Parse(XmlString);
            string script = Xml.Descendants("script").SingleOrDefault().Value.ToString();
            script = string.Format("execute calculation ' \n {0} ' {1}", script, Esquema);
            return script;
        }


        #region Security

        /// <summary>
        /// Encrypts specified plaintext using Rijndael symmetric key algorithm
        /// and returns a base64-encoded result.
        /// </summary>
        /// <param name="plainText">
        /// Plaintext value to be encrypted.
        /// </param>
        /// <param name="passPhrase">
        /// Passphrase from which a pseudo-random password will be derived. The
        /// derived password will be used to generate the encryption key.
        /// Passphrase can be any string. In this example we assume that this
        /// passphrase is an ASCII string.
        /// </param>
        /// <param name="saltValue">
        /// Salt value used along with passphrase to generate password. Salt can
        /// be any string. In this example we assume that salt is an ASCII string.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Hash algorithm used to generate password. Allowed values are: "MD5" and
        /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
        /// </param>
        /// <param name="passwordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.
        /// </param>
        /// <param name="initVector">
        /// Initialization vector (or IV). This value is required to encrypt the
        /// first block of plaintext data. For RijndaelManaged class IV must be 
        /// exactly 16 ASCII characters long.
        /// </param>
        /// <param name="keySize">
        /// Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>
        /// Encrypted value formatted as a base64-encoded string.
        /// </returns>

        public static string Encrypt(this string plainText)
        {
            //string plainText = "Hello, World!";       // original plaintext
            //string passPhrase = "Pas5pr@se";          // can be any string
            //string saltValue = "s@1tValue";           // can be any string
            //string hashAlgorithm = "SHA1";            // can be "MD5"
            //int passwordIterations = 2;               // can be any number
            //string initVector = "@1B2c3D4e5F6g7H8";   // must be 16 bytes
            //int keySize = 256;                        // can be 192 or 128
            return Encrypt(plainText, "unamgelito", "Gato.Sandia%&KL", "SHA1", 3, "@OTRO%Vec7809S&%", 256);
        }
        public static string Encrypt(this string plainText,
                                    string passPhrase,
                                    string saltValue,
                                    string hashAlgorithm,
                                    int passwordIterations,
                                    string initVector,
                                    int keySize)
        {
            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and 
            // salt value. The password will be created using the specified hash 
            // algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            using (MemoryStream memoryStream = new MemoryStream())
            {

                // Define cryptographic stream (always use Write mode for encryption).
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                             encryptor,
                                                             CryptoStreamMode.Write))
                {
                    // Start encrypting.
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                    // Finish encrypting.
                    cryptoStream.FlushFinalBlock();

                    // Convert our encrypted data from a memory stream into a byte array.
                    byte[] cipherTextBytes = memoryStream.ToArray();

                    // Close both streams.
                    memoryStream.Close();
                    cryptoStream.Close();

                    // Convert encrypted data into a base64-encoded string.
                    string cipherText = Convert.ToBase64String(cipherTextBytes);

                    // Return encrypted string.
                    return cipherText;
                }
            }
        }

        /// <summary>
        /// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
        /// </summary>
        /// <param name="cipherText">
        /// Base64-formatted ciphertext value.
        /// </param>
        /// <param name="passPhrase">
        /// Passphrase from which a pseudo-random password will be derived. The
        /// derived password will be used to generate the encryption key.
        /// Passphrase can be any string. In this example we assume that this
        /// passphrase is an ASCII string.
        /// </param>
        /// <param name="saltValue">
        /// Salt value used along with passphrase to generate password. Salt can
        /// be any string. In this example we assume that salt is an ASCII string.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Hash algorithm used to generate password. Allowed values are: "MD5" and
        /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
        /// </param>
        /// <param name="passwordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.
        /// </param>
        /// <param name="initVector">
        /// Initialization vector (or IV). This value is required to encrypt the
        /// first block of plaintext data. For RijndaelManaged class IV must be
        /// exactly 16 ASCII characters long.
        /// </param>
        /// <param name="keySize">
        /// Size of encryption key in bits. Allowed values are: 128, 192, and 256.
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>
        /// Decrypted string value.
        /// </returns>
        /// <remarks>
        /// Most of the logic in this function is similar to the Encrypt
        /// logic. In order for decryption to work, all parameters of this function
        /// - except cipherText value - must match the corresponding parameters of
        /// the Encrypt function which was called to generate the
        /// ciphertext.
        /// </remarks>

        public static string Decrypt(this string cipherText)
        {
            return Decrypt(cipherText, "unamgelito", "Gato.Sandia%&KL", "SHA1", 3, "@OTRO%Vec7809S&%", 256);
        }

        public static string Decrypt(this string cipherText,
                                    string passPhrase,
                                    string saltValue,
                                    string hashAlgorithm,
                                    int passwordIterations,
                                    string initVector,
                                    int keySize)
        {
            // Convert strings defining encryption key characteristics into byte
            // arrays. Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            if (cipherText.Contains(" "))
            {
                cipherText = cipherText.Replace(" ", "+");
            }

            // Convert our ciphertext into a byte array.
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            // First, we must create a password, from which the key will be 
            // derived. This password will be generated from the specified 
            // passphrase and salt value. The password will be created using
            // the specified hash algorithm. Password creation can be done in
            // several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
            {
                // Define cryptographic stream (always use Read mode for encryption).
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                              decryptor,
                                                              CryptoStreamMode.Read))
                {
                    // Since at this point we don't know what the size of decrypted data
                    // will be, allocate the buffer long enough to hold ciphertext;
                    // plaintext is never longer than ciphertext.
                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                    // Start decrypting.
                    int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                               0,
                                                               plainTextBytes.Length);

                    // Close both streams.
                    memoryStream.Close();
                    cryptoStream.Close();

                    // Convert decrypted data into a string. 
                    // Let us assume that the original plaintext string was UTF8-encoded.
                    string plainText = Encoding.UTF8.GetString(plainTextBytes,
                                                               0,
                                                               decryptedByteCount);

                    // Return decrypted string.   
                    return plainText;
                }
            }
        }

        #endregion
    }
}