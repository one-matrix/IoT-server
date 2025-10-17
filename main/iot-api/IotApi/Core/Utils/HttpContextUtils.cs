using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using IotApi.Core.common;
using IotApi.Core.Utils;

namespace IotApi.Core.utils
{
    /// <summary>
    /// Http上下文工具类
    /// </summary>
    public static class HttpContextUtils
    {
        private static IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 初始化HttpContext访问器
        /// </summary>
        /// <param name="httpContextAccessor">Http上下文访问器</param>
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取当前HttpContext
        /// </summary>
        /// <returns>HttpContext</returns>
        public static HttpContext GetHttpContext()
        {
            return _httpContextAccessor?.HttpContext;
        }

        /// <summary>
        /// 从授权头中获取Token
        /// </summary>
        /// <param name="authorization">授权头</param>
        /// <returns>Token字符串</returns>
        public static string GetToken(string authorization)
        {
            string token;
            if (string.IsNullOrWhiteSpace(authorization) || !authorization.Contains("Bearer "))
            {
                throw new ApiException(ErrorCode.UNAUTHORIZED);
            }
            token = authorization.Replace("Bearer ", "");
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ApiException(ErrorCode.TOKEN_NOT_EMPTY);
            }
            return token;
        }

        /// <summary>
        /// 获取请求参数映射
        /// </summary>
        /// <param name="request">Http请求</param>
        /// <returns>参数字典</returns>
        public static Dictionary<string, string> GetParameterMap(HttpRequest request)
        {
            var parameters = request.Query;
            Dictionary<string, string> paramMap = new Dictionary<string, string>();

            foreach (var param in parameters)
            {
                if (!string.IsNullOrWhiteSpace(param.Value))
                {
                    paramMap.Add(param.Key, param.Value);
                }
            }

            return paramMap;
        }

        /// <summary>
        /// 获取域名
        /// </summary>
        /// <returns>域名</returns>
        public static string GetDomain()
        {
            var request = GetHttpContext()?.Request;
            if (request == null)
            {
                return null;
            }

            return $"{request.Scheme}://{request.Host}";
        }

        /// <summary>
        /// 获取来源
        /// </summary>
        /// <returns>来源</returns>
        public static string GetOrigin()
        {
            var request = GetHttpContext()?.Request;
            if (request == null)
            {
                return null;
            }

            return request.Headers["Origin"];
        }

        /// <summary>
        /// 获取语言
        /// </summary>
        /// <returns>语言</returns>
        public static string GetLanguage()
        {
            // 默认语言
            string defaultLanguage = "zh-CN";
            
            var request = GetHttpContext()?.Request;
            if (request == null)
            {
                return defaultLanguage;
            }

            // 请求语言
            if (request.Headers.TryGetValue("Accept-Language", out StringValues language))
            {
                defaultLanguage = language.ToString();
            }

            return defaultLanguage;
        }

        /// <summary>
        /// 获取客户端的唯一标识
        /// </summary>
        /// <returns>客户端唯一标识</returns>
        public static string GetClientCode()
        {
            var request = GetHttpContext()?.Request;
            if (request == null)
            {
                return null;
            }

            string userAgent = request.Headers["User-Agent"].ToString().ToLower();
            string ipAddr = IpUtils.GetIpAddr(GetHttpContext());
            string date = DateUtils.Format(DateTime.Now);
            
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(ipAddr + date + userAgent);
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
        /// 获取请求URI
        /// </summary>
        /// <returns>请求URI</returns>
        public static string GetRequestURI()
        {
            var request = GetHttpContext()?.Request;
            if (request == null)
            {
                return null;
            }

            return request.Path.ToString();
        }
    }
}