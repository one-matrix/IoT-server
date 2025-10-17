using System;

namespace IotApi.Core.Exceptions
{
    /// <summary>
    /// API异常类
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        public ApiException(string message) : base(message)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        /// <param name="message">错误消息</param>
        public ApiException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}