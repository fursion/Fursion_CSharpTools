using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Fursion_CSharpTools.Tools
{
    public static class EnCode
    {
        public static string file_MD5()
        {
            return null;
        }
    }
    public class EncryptHelp : IDisposable
    {
        bool disposed = false;
        public readonly string Salt1 = "qwertyuioplkjhgfdsamnbvcxz";
        public readonly string Salt2 = "zxcvbnmlkjhgfdsaqwertyuiop";
        public readonly byte[] IV1 = new byte[4] { 0x12, 0x34, 0x45, 0xa3 };
        public readonly byte[] IV2 = new byte[4] { 0x72, 0x37, 0x68, 0xa3 };
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                // Free any other managed objects here.
            }
            disposed = true;
        }
    }
    /// <summary>
    /// 数据加密工具
    /// </summary>
    public static class DataEncryption
    {
        /// <summary>
        /// 解密由Aes算法加密的Byte[]
        /// </summary>
        /// <param name="file"></param>
        /// <param name="Key"></param>
        /// <returns>返回一个解密后的Byte[]</returns>
        public static byte[] AesEncode(this byte[] file, string Key)
        {
            byte[] encryptedByte;
            using (AesManaged aes = new AesManaged())
            {
                using (EncryptHelp help = new EncryptHelp())
                {
                    aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                    aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                    aes.IV = GetAesKey(aes.BlockSize / 8, help.Salt1, Key.SHA256HashString());
                    aes.Key = GetAesKey(aes.KeySize / 8, help.Salt2, Key.SHA256HashString());
                    aes.IV.PrintByteArray();
                    aes.Key.PrintByteArray();
                    encryptedByte = EncryptBytesToBytes_Aes(file, aes.Key, aes.IV);
                }

            }
            return encryptedByte;
        }
        public static byte[] AesDecode(byte[] Aesfile, string Key)
        {
            byte[] DecryptBytes;
            using (AesManaged aes = new AesManaged())
            {
                using (EncryptHelp help = new EncryptHelp())
                {
                    aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                    aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                    aes.IV = GetAesKey(aes.BlockSize / 8, help.Salt1, Key.SHA256HashString());
                    aes.Key = GetAesKey(aes.KeySize / 8, help.Salt2, Key.SHA256HashString());
                    aes.IV.PrintByteArray();
                    aes.Key.PrintByteArray();
                    DecryptBytes = DecryptBytesFromBytes_Aes(Aesfile, aes.Key, aes.IV);
                }
            }
            return DecryptBytes;
        }
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
        public static byte[] GetAesKey(int Size, string saltstring, string Pwd)
        {
            byte[] Aeskey;
            var saltByte = saltstring.SHA256HashBytes();
            using (Rfc2898DeriveBytes Rfc = new Rfc2898DeriveBytes(Pwd, saltByte))
            {
                Aeskey = Rfc.GetBytes(Size);
            }
            return Aeskey;

        }
        /// <summary>
        /// 获取加密IV
        /// </summary>
        /// <param name="size"></param>
        /// <param name="saltstring"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static byte[] GetAesIV(int size, string saltstring, string pwd)
        {
            byte[] IV;
            var saltByte = saltstring.SHA256HashBytes();
            using (Rfc2898DeriveBytes Rfc = new Rfc2898DeriveBytes(pwd, saltByte))
            {
                IV = Rfc.GetBytes(size);
            }
            return IV;
        }
        /// <summary>
        /// 采用SHA256的散列哈希加密，不可逆。返回字符串
        /// </summary>
        /// <param name="Vaule"></param>
        /// <returns></returns>
        public static string SHA256HashString(this string Vaule)
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
        public static byte[] EncryptBytesToBytes_Aes(byte[] plainBytes, byte[] Key, byte[] IV)
        {
            if (plainBytes == null || plainBytes.Length <= 0)
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
            using (AesManaged aes = new AesManaged())
            {
                aes.Key = Key;
                aes.IV = IV;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {

                        csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    encrypted = msEncrypt.ToArray(); ;
                }
            }
            return encrypted;
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
            byte[] DecrypteBytes = null;
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream outs = new MemoryStream())
                {
                    using (MemoryStream msDecrypt = new MemoryStream(EncryptedBytes))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            try
                            {
                                byte[] buffer = new byte[EncryptedBytes.Length];
                                int len = csDecrypt.Read(buffer, 0, buffer.Length);//len为解密流中数据的实际可用长度
                                DecrypteBytes = new byte[len];
                                Array.Copy(buffer, 0, DecrypteBytes, 0, len);
                            }
                            catch (Exception e)
                            {
                                FDebug.Log("解码密钥错误 ：" + e.Message);
                            }

                        }
                    }
                }
            }
            return DecrypteBytes;
        }

    }

}
