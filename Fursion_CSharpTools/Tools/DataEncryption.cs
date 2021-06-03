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
        /// 对字符串进行加密,文本文件
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
        /// <summary>
        /// 对Byte[]进行加密，
        /// </summary>
        /// <param name="file">明文Byte[]流</param>
        /// <returns>EncryptedByte[] key iv</returns>
        public static Tuple<byte[], byte[], byte[]> RandomAesEncrypt(byte[] file)
        {
            byte[] encrypted;
            byte[] Key, IV;

            using (AesManaged aes = new AesManaged())
            {

                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                aes.GenerateKey();
                aes.GenerateIV();
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {

                        csEncrypt.Write(file, 0, file.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    encrypted = msEncrypt.ToArray(); ;
                }
                Key = aes.Key;
                IV = aes.IV;
            }
            return new Tuple<byte[], byte[], byte[]>(encrypted, Key, IV);
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
        /// 采用SHA256的散列哈希加密，不可逆。返回字符串
        /// </summary>
        /// <param name="Vaule"></param>
        /// <returns></returns>
        public static string SHA256HashString(string Vaule)
        {
            byte[] md;
            using (SHA256 sHA256 = SHA256.Create())
            {
                md = sHA256.ComputeHash(Encoding.UTF8.GetBytes(Vaule));
            }
            return Convert.ToBase64String(md);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Vaule"></param>
        /// <returns></returns>
        public static byte[] SHA256HashBytes(this string Vaule)
        {
            byte[] md;
            using (SHA256 sHA256 = SHA256.Create())
            {
                md = sHA256.ComputeHash(Encoding.UTF8.GetBytes(Vaule));
            }
            return md;
        }
        /// <summary>
        /// 对字符串进行加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="Key">密钥</param>
        /// <param name="IV">偏移量</param>
        /// <returns>加密得到的Byte[]</returns>
        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
            {
                FDebug.Log("密文无效，无法加密");
                return null;
            }
            if (Key == null || Key.Length <= 0)
            {
                FDebug.Log("密钥无效，无法加密");
                return null;
            }
            if (IV == null || IV.Length <= 0)
            {
                FDebug.Log("偏移量无效，无法加密");
                return null;
            }
            byte[] encrypted;
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
            return encrypted;
        }
        /// <summary>
        /// 将字符串加密得到的Byte[]解密并转化为字符串
        /// 默认UTF8编码
        /// </summary>
        /// <param name="EncryptedBytes">密文</param>
        /// <param name="Key">密钥</param>
        /// <param name="IV">偏移量</param>
        /// <returns>解密得到的字符串</returns>
        public static string DecryptStringFromBytes_Aes(byte[] EncryptedBytes, byte[] Key, byte[] IV)
        {
            if (EncryptedBytes == null || EncryptedBytes.Length <= 0)
            {
                FDebug.Log("密文无效，无法解密");
                return null;
            }
            if (Key == null || Key.Length <= 0)
            {
                FDebug.Log("密钥无效，无法解密");
                return null;
            }
            if (IV == null || IV.Length <= 0)
            {
                FDebug.Log("偏移量无效，无法解密");
                return null;
            }
            string plaintext = null;
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(EncryptedBytes))
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
        /// <summary>
        /// 解密经过加密的Byte[]流文件
        /// </summary>
        /// <param name="EncryptedBytes">密文</param>
        /// <param name="Key">密钥</param>
        /// <param name="IV">加密偏移量</param>
        /// <returns>解密后的Byte[]流</returns>
        public static byte[] DecryptBytesFromBytes_Aes(byte[] EncryptedBytes, byte[] Key, byte[] IV)
        {
            if (EncryptedBytes == null || EncryptedBytes.Length <= 0)
            {
                FDebug.Log("密文无效，无法解密");
                return null;
            }
            if (Key == null || Key.Length <= 0)
            {
                FDebug.Log("密钥无效，无法解密");
                return null;
            }
            if (IV == null || IV.Length <= 0)
            {
                FDebug.Log("偏移量无效，无法解密");
                return null;
            }
            byte[] DecrypteBytes;
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                long readlen;
                long totlen = 0;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                MemoryStream ins = new MemoryStream(EncryptedBytes);
                readlen = ins.Length;
                using (MemoryStream msDecrypt = new MemoryStream(EncryptedBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] buffer = new byte[EncryptedBytes.Length];
                        int len = csDecrypt.Read(buffer, 0, buffer.Length);
                        DecrypteBytes = new byte[len];
                        Array.Copy(buffer, 0, DecrypteBytes, 0, len);
                    }
                }
            }
            return DecrypteBytes;
        }

    }

}
