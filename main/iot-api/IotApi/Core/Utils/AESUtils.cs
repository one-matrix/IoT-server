using System;
using System.Security.Cryptography;
using System.Text;

namespace IotApi.Core.Utils
{
    /// <summary>
    /// AES加密工具类
    /// </summary>
    public class AESUtils
    {
        private static readonly string ALGORITHM = "AES";
        private static readonly string TRANSFORMATION = "AES/ECB/PKCS5Padding";

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="key">密钥（16位、24位或32位）</param>
        /// <param name="plainText">待加密字符串</param>
        /// <returns>加密后的Base64字符串</returns>
        public static string Encrypt(string key, string plainText)
        {
            try
            {
                // 确保密钥长度为16、24或32位
                byte[] keyBytes = PadKey(Encoding.UTF8.GetBytes(key));
                
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = keyBytes;
                    aesAlg.Mode = CipherMode.ECB;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (var msEncrypt = new System.IO.MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                            return Convert.ToBase64String(msEncrypt.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AES加密失败", ex);
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="key">密钥（16位、24位或32位）</param>
        /// <param name="encryptedText">待解密的Base64字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string key, string encryptedText)
        {
            try
            {
                // 确保密钥长度为16、24或32位
                byte[] keyBytes = PadKey(Encoding.UTF8.GetBytes(key));
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = keyBytes;
                    aesAlg.Mode = CipherMode.ECB;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (var msDecrypt = new System.IO.MemoryStream(encryptedBytes))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AES解密失败", ex);
            }
        }

        /// <summary>
        /// 填充密钥到指定长度（16、24或32位）
        /// </summary>
        /// <param name="keyBytes">原始密钥字节数组</param>
        /// <returns>填充后的密钥字节数组</returns>
        private static byte[] PadKey(byte[] keyBytes)
        {
            int keyLength = keyBytes.Length;
            if (keyLength == 16 || keyLength == 24 || keyLength == 32)
            {
                return keyBytes;
            }

            // 如果密钥长度不足，用0填充；如果超过，截取前32位
            byte[] paddedKey = new byte[32];
            Array.Copy(keyBytes, 0, paddedKey, 0, Math.Min(keyLength, 32));
            return paddedKey;
        }
    }
}