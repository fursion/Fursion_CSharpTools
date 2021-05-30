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
        /// <summary>
        /// 
        /// </summary>
        /// <returns>返回加密后的数据，密钥，偏移量</returns>
        public static Tuple<byte[], byte[], byte[]> RandomAesEncrypt(string file)
        {
            byte[] encrypted;
            byte[] Key, IV;
            using (AesManaged aes = new AesManaged())
            {

                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                aes.GenerateKey();
                aes.GenerateIV();
                encrypted = EncryptStringToBytes_Aes(file, aes.Key, aes.IV);
                Key = aes.Key;
                IV = aes.IV;
            }
            return new Tuple<byte[], byte[], byte[]>(encrypted, Key, IV);
        }
        public static byte[] AesEncrypt(string text, string Paw, string salt)
        {
            byte[] encrypted;
            using (AesManaged myAes = new AesManaged())
            {
                byte[] salt1 = Encoding.UTF8.GetBytes(salt);
                myAes.BlockSize = myAes.LegalBlockSizes[0].MaxSize;
                myAes.KeySize = myAes.LegalKeySizes[0].MaxSize;
                myAes.Key = GetAesKey(myAes.KeySize / 8, salt1, Paw);
                myAes.IV = GetAesKey(myAes.BlockSize / 8, salt1, salt);
                encrypted = EncryptStringToBytes_Aes(text, myAes.Key, myAes.IV);
            }
            return encrypted;
        }
        /// <summary>
        /// 派生指定长度的密钥
        /// </summary>
        /// <param name="Size"></param>
        /// <param name="salt1"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static byte[] GetAesKey(int Size, byte[] salt1, string Pwd)
        {
            byte[] Aeskey;
            using (Rfc2898DeriveBytes Rfc = new Rfc2898DeriveBytes(Pwd, salt1))
            {
                Aeskey = Rfc.GetBytes(Size);
            }
            return Aeskey;

        }
        /// <summary>
        /// Aes解码
        /// </summary>
        /// <param name="d"></param>
        /// <param name="Paw"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string AesDecrypt(byte[] d, string Paw, string salt)
        {
            string text;
            using (AesManaged myAes = new AesManaged())
            {
                byte[] salt1 = Encoding.UTF8.GetBytes(salt);
                myAes.BlockSize = myAes.LegalBlockSizes[0].MaxSize;
                myAes.KeySize = myAes.LegalKeySizes[0].MaxSize;
                myAes.Key = GetAesKey(myAes.KeySize / 8, salt1, Paw);
                myAes.IV = GetAesKey(myAes.BlockSize / 8, salt1, salt);
                text = DecryptStringFromBytes_Aes(d, myAes.Key, myAes.IV);
            }
            return text;
        }
        public static byte[] AesDeCode(byte[] key)
        {

            return null;
        }
        /// <summary>
        /// 采用SHA256的散列加密，不可逆。
        /// </summary>
        /// <param name="Vaule"></param>
        /// <returns></returns>
        public static string SHACode(string Vaule)
        {
            byte[] md;
            using (SHA256 sHA256 = SHA256.Create())
            {
                md = sHA256.ComputeHash(Encoding.UTF8.GetBytes(Vaule));
                sHA256.Clear();
                sHA256.Dispose();
            }
            return Convert.ToBase64String(md);
        }
        public static byte[] SHA256Code(string Vaule)
        {
            byte[] md;
            using (SHA256 sHA256 = SHA256.Create())
            {
                md = sHA256.ComputeHash(Encoding.UTF8.GetBytes(Vaule));
                sHA256.Clear();
                sHA256.Dispose();
            }
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
            using (AesManaged aesAlg = new AesManaged())
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
                Console.WriteLine(aesAlg.Mode);
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
            using (AesManaged aesAlg = new AesManaged())
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
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

    }

}
