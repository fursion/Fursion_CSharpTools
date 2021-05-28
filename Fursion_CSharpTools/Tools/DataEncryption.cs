using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Fursion_CSharpTools.Tools
{
    /// <summary>
    /// 数据加密工具
    /// </summary>
    public static class DataEncryption
    {
        public static byte[] AesEncrypt(string text)
        {
            byte[] encryped;
            using (Aes myAes = Aes.Create())
            {
                //encryped = AESEncryption.EncryptStringToBytes_Aes(text, MyAes.Key, MyAes.IV);
                byte[] encrypted = AESEncryption.EncryptStringToBytes_Aes(text, myAes.Key, myAes.IV);
                Console.WriteLine("key is {0}", BitConverter.ToString(myAes.Key));
                Console.WriteLine("IV is {0}",BitConverter.ToString(myAes.IV));
                // Decrypt the bytes to a string.
                string roundtrip = AESEncryption.DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);

                //Display the original data and the decrypted data.
                Console.WriteLine("Original:   {0}", text);
                Console.WriteLine("Round Trip: {0}", roundtrip);
                return encrypted;
            }
            
        }
        public static string AesDecrypt(byte[] d)
        {
            string text;
            using (Aes MyAes = Aes.Create())
            {
                text = AESEncryption.DecryptStringFromBytes_Aes(d, MyAes.Key, MyAes.IV);
            }
            return text;
        }
    }
    /// <summary>
    /// AES算法加密
    /// </summary>
    class AESEncryption
    {
        /// <summary>
        /// 采用SHA256的散列加密，不可逆。
        /// </summary>
        /// <param name="Vaule"></param>
        /// <returns></returns>
        public static string SHACode(string Vaule)
        {
            SHA256 sHA256 = SHA256.Create();
            byte[] md = sHA256.ComputeHash(Encoding.UTF8.GetBytes(Vaule));
            sHA256.Clear();
            sHA256.Dispose();
            return Convert.ToBase64String(md);
        }
        public static byte[] SHA256Code(string Vaule)
        {
            SHA256 sHA256 = SHA256.Create();
            byte[] md = sHA256.ComputeHash(Encoding.UTF8.GetBytes(Vaule));
            sHA256.Clear();
            sHA256.Dispose();
            return md;
        }
        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
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
        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
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

    }

}
