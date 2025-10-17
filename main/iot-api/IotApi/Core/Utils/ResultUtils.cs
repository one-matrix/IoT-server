using System;

namespace IotApi.Core.utils
{
    /// <summary>
    /// 返回响应体工具类
    /// </summary>
    public static class ResultUtils
    {
        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">响应数据</param>
        /// <returns>成功响应结果</returns>
        public static Result<T> Success<T>(T data)
        {
            return new Result<T>().Ok(data);
        }

        /// <summary>
        /// 创建错误响应（使用默认错误码）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>错误响应结果</returns>
        public static Result<T> Error<T>()
        {
            return new Result<T>().Error();
        }

        /// <summary>
        /// 创建错误响应（指定错误消息，使用默认错误码）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="msg">错误消息</param>
        /// <returns>错误响应结果</returns>
        public static Result<T> Error<T>(string msg)
        {
            return new Result<T>().Error(msg);
        }

        /// <summary>
        /// 创建错误响应（指定错误码和消息）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="errorCode">错误码</param>
        /// <param name="msg">错误消息</param>
        /// <returns>错误响应结果</returns>
        public static Result<T> Error<T>(int errorCode, string msg)
        {
            return new Result<T>().Error(errorCode, msg);
        }

        /// <summary>
        /// 创建错误响应（指定错误码）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="errorCode">错误码</param>
        /// <returns>错误响应结果</returns>
        public static Result<T> Error<T>(int errorCode)
        {
            return new Result<T>().Error(errorCode);
        }

        /// <summary>
        /// 创建空响应
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>空响应结果</returns>
        public static Result<T> Empty<T>()
        {
            return new Result<T>();
        }
    }
}