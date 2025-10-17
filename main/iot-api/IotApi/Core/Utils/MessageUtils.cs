using System.Collections.Generic;

namespace IotApi.Core.utils
{
    /// <summary>
    /// 消息工具类
    /// </summary>
    public static class MessageUtils
    {
        private static readonly Dictionary<int, string> ErrorMessages = new Dictionary<int, string>
        {
            { 400, "请求参数错误" },
            { 401, "未授权" },
            { 403, "禁止访问" },
            { 404, "资源不存在" },
            { 500, "服务器内部错误" },
            { 1000, "数据已存在" },
            { 1001, "账号或密码错误" },
            { 1002, "账号已被锁定" },
            { 1003, "账号不存在" }
        };

        /// <summary>
        /// 获取错误消息
        /// </summary>
        /// <param name="code">错误码</param>
        /// <returns>错误消息</returns>
        public static string GetMessage(int code)
        {
            if (ErrorMessages.TryGetValue(code, out string message))
            {
                return message;
            }
            return "未知错误";
        }
    }
}