using System;
using System.Security.Cryptography;
using System.Text;

namespace IotApi.Core.utils
{
    /// <summary>
    /// 哈希工具类
    /// </summary>
    public static class HashUtils
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="text">待加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="text">待加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Sha1(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="text">待加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Sha256(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// SHA512加密
        /// </summary>
        /// <param name="text">待加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Sha512(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 使用指定算法进行哈希计算
        /// </summary>
        /// <param name="text">待加密字符串</param>
        /// <param name="algorithm">哈希算法名称 (MD5, SHA1, SHA256, SHA512)</param>
        /// <returns>加密后的字符串</returns>
        public static string Hash(string text, string algorithm)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(algorithm))
            {
                return null;
            }

            HashAlgorithm hashAlgorithm = null;
            switch (algorithm.ToUpper())
            {
                case "MD5":
                    hashAlgorithm = MD5.Create();
                    break;
                case "SHA1":
                    hashAlgorithm = SHA1.Create();
                    break;
                case "SHA256":
                    hashAlgorithm = SHA256.Create();
                    break;
                case "SHA512":
                    hashAlgorithm = SHA512.Create();
                    break;
                default:
                    throw new ArgumentException($"不支持的哈希算法: {algorithm}");
            }

            using (hashAlgorithm)
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = hashAlgorithm.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取文件的MD5哈希值
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件的MD5哈希值</returns>
        public static string GetFileMd5(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            using (MD5 md5 = MD5.Create())
            using (var stream = System.IO.File.OpenRead(filePath))
            {
                byte[] hashBytes = md5.ComputeHash(stream);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取文件的SHA256哈希值
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件的SHA256哈希值</returns>
        public static string GetFileSha256(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            using (SHA256 sha256 = SHA256.Create())
            using (var stream = System.IO.File.OpenRead(filePath))
            {
                byte[] hashBytes = sha256.ComputeHash(stream);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}