using System;
using System.Text.Json.Serialization;
using IotApi.Core.common;

namespace IotApi.Core.utils
{
    /// <summary>
    /// 响应数据封装类
    /// </summary>
    /// <typeparam name="T">响应数据类型</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// 编码：0表示成功，其他值表示失败
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; } = 0;

        /// <summary>
        /// 消息内容
        /// </summary>
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = "success";

        /// <summary>
        /// 响应数据
        /// </summary>
        [JsonPropertyName("data")]
        public T Data { get; set; }

        /// <summary>
        /// 设置成功响应数据
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <returns>当前对象</returns>
        public Result<T> Ok(T data)
        {
            this.Data = data;
            return this;
        }

        /// <summary>
        /// 设置错误响应（使用默认错误码）
        /// </summary>
        /// <returns>当前对象</returns>
        public Result<T> Error()
        {
            this.Code = ErrorCode.INTERNAL_SERVER_ERROR;
            this.Msg = MessageUtils.GetMessage(this.Code);
            return this;
        }

        /// <summary>
        /// 设置错误响应（指定错误码）
        /// </summary>
        /// <param name="code">错误码</param>
        /// <returns>当前对象</returns>
        public Result<T> Error(int code)
        {
            this.Code = code;
            this.Msg = MessageUtils.GetMessage(this.Code);
            return this;
        }

        /// <summary>
        /// 设置错误响应（指定错误码和消息）
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="msg">错误消息</param>
        /// <returns>当前对象</returns>
        public Result<T> Error(int code, string msg)
        {
            this.Code = code;
            this.Msg = msg;
            return this;
        }

        /// <summary>
        /// 设置错误响应（指定错误消息，使用默认错误码）
        /// </summary>
        /// <param name="msg">错误消息</param>
        /// <returns>当前对象</returns>
        public Result<T> Error(string msg)
        {
            this.Code = ErrorCode.INTERNAL_SERVER_ERROR;
            this.Msg = msg;
            return this;
        }
    }
}