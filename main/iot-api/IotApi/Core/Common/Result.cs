namespace IotApi.Core.common
{
    /// <summary>
    /// 统一API响应结果封装
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }
        
        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data { get; set; }
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Result()
        {
            Code = 0;
            Message = "success";
        }
        
        /// <summary>
        /// 成功响应
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <returns>结果对象</returns>
        public static Result<T> Success(T data)
        {
            return new Result<T>
            {
                Code = 0,
                Message = "success",
                Data = data
            };
        }
        
        /// <summary>
        /// 失败响应
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="message">错误消息</param>
        /// <returns>结果对象</returns>
        public static Result<T> Error(int code, string message)
        {
            return new Result<T>
            {
                Code = code,
                Message = message,
                Data = default(T)
            };
        }
        
        /// <summary>
        /// 失败响应（使用默认错误码）
        /// </summary>
        /// <returns>结果对象</returns>
        public static Result<T> Error()
        {
            return new Result<T>
            {
                Code = ErrorCode.INTERNAL_SERVER_ERROR,
                Message = "Internal server error",
                Data = default(T)
            };
        }
    }
    
    /// <summary>
    /// 统一API响应结果封装（无数据）
    /// </summary>
    public class Result : Result<object>
    {
        /// <summary>
        /// 成功响应（无数据）
        /// </summary>
        /// <returns>结果对象</returns>
        public static new Result Success()
        {
            return new Result
            {
                Code = 0,
                Message = "success",
                Data = null
            };
        }
        
        /// <summary>
        /// 成功响应
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <returns>结果对象</returns>
        public static Result Success(object data)
        {
            return new Result
            {
                Code = 0,
                Message = "success",
                Data = data
            };
        }
        
        /// <summary>
        /// 失败响应
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="message">错误消息</param>
        /// <returns>结果对象</returns>
        public static new Result Error(int code, string message)
        {
            return new Result
            {
                Code = code,
                Message = message,
                Data = null
            };
        }
        
        /// <summary>
        /// 失败响应（使用默认错误码）
        /// </summary>
        /// <returns>结果对象</returns>
        public static new Result Error()
        {
            return new Result
            {
                Code = ErrorCode.INTERNAL_SERVER_ERROR,
                Message = "Internal server error",
                Data = null
            };
        }
    }
}