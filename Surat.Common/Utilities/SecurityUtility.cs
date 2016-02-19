using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Surat.Common.Data;

namespace Surat.Common.Utilities
{
    public class SecurityUtility
    {
        #region Cryptography


        public static byte[] EncryptUsingAESCryptoService(string plainText, string key, string IV)
        {
            byte[] encrypted;
            using (AesCryptoServiceProvider serviceProviderAES = new AesCryptoServiceProvider())
            {
                serviceProviderAES.Key = Convert.FromBase64String(key);
                serviceProviderAES.IV = Convert.FromBase64String(IV);

                encrypted = EncryptUsingAES(plainText, serviceProviderAES.Key, serviceProviderAES.IV);
            }

            return encrypted;
        }

        public static string EncryptUsingAESCryptoServiceAsString(string plainText, string key, string IV)
        {
            string result;
            byte[] encryptedAsByteArray;

            encryptedAsByteArray = EncryptUsingAESCryptoService(plainText, key, IV);
            result = ConvertEncryptedTextToByteArray(encryptedAsByteArray);

            return result ;
        }

        public static string ConvertEncryptedTextToByteArray(byte[] encrypted)
        {
            string result;

            result = Convert.ToBase64String(encrypted);

            return result;
        }

        public static string DecryptUsingAESCryptoService(string encrypted, string key, string IV)
        {
            string result;
            byte[] encryptedAsByteArray = Convert.FromBase64String(encrypted);

            result = DecryptUsingAESCryptoService(encryptedAsByteArray,key,IV);

            return result;
        }

        public static string DecryptUsingAESCryptoService(byte[] encrypted, string Key, string IV)
        {
            string result;

            using (AesCryptoServiceProvider serviceProviderAES = new AesCryptoServiceProvider())
            {
                serviceProviderAES.Key = Convert.FromBase64String(Key);
                serviceProviderAES.IV = Convert.FromBase64String(IV);

                result = DecryptUsingAES(encrypted, serviceProviderAES.Key, serviceProviderAES.IV);
            }

            return result;
        }

        private static byte[] EncryptUsingAES(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private static string DecryptUsingAES(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        #endregion
    }
}
