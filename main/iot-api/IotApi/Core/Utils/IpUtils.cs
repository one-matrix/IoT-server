using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace IotApi.Core.Utils
{
    /// <summary>
    /// IP地址工具类
    /// </summary>
    public class IpUtils
    {
        private static readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<IpUtils>();

        /// <summary>
        /// 获取IP地址
        /// 使用Nginx等反向代理软件，则不能通过HttpContext.Connection.RemoteIpAddress获取IP地址
        /// 如果使用了多级反向代理的话，X-Forwarded-For的值并不止一个，而是一串IP地址，X-Forwarded-For中第一个非unknown的有效IP字符串，则为真实IP地址
        /// </summary>
        /// <param name="context">HTTP上下文</param>
        /// <returns>IP地址</returns>
        public static string GetIpAddr(HttpContext context)
        {
            const string unknown = "unknown";
            string ip = null;
            try
            {
                ip = context.Request.Headers["x-forwarded-for"];
                if (string.IsNullOrEmpty(ip) || unknown.Equals(ip, StringComparison.OrdinalIgnoreCase))
                {
                    ip = context.Request.Headers["Proxy-Client-IP"];
                }
                if (string.IsNullOrEmpty(ip) || ip.Length == 0 || unknown.Equals(ip, StringComparison.OrdinalIgnoreCase))
                {
                    ip = context.Request.Headers["WL-Proxy-Client-IP"];
                }
                if (string.IsNullOrEmpty(ip) || unknown.Equals(ip, StringComparison.OrdinalIgnoreCase))
                {
                    ip = context.Request.Headers["HTTP_CLIENT_IP"];
                }
                if (string.IsNullOrEmpty(ip) || unknown.Equals(ip, StringComparison.OrdinalIgnoreCase))
                {
                    ip = context.Request.Headers["HTTP_X_FORWARDED_FOR"];
                }
                if (string.IsNullOrEmpty(ip) || unknown.Equals(ip, StringComparison.OrdinalIgnoreCase))
                {
                    ip = context.Connection.RemoteIpAddress?.ToString();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IpUtils ERROR");
            }

            return ip;
        }
    }
}