using System;

namespace IotApi.Core.common
{
    /// <summary>
    /// Custom API exception class for the IoT system
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// Gets or sets the error code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the error message
        /// </summary>
        public new string Message { get; set; }

        /// <summary>
        /// Creates a new instance of ApiException with a code
        /// </summary>
        /// <param name="code">The error code</param>
        public ApiException(int code) : base()
        {
            Code = code;
            Message = GetMessage(code);
        }

        /// <summary>
        /// Creates a new instance of ApiException with a code and parameters
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="params">Parameters for message formatting</param>
        public ApiException(int code, params string[] @params) : base()
        {
            Code = code;
            Message = GetMessage(code, @params);
        }

        /// <summary>
        /// Creates a new instance of ApiException with a code and inner exception
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="innerException">The inner exception</param>
        public ApiException(int code, Exception innerException) : base(GetMessage(code), innerException)
        {
            Code = code;
            Message = GetMessage(code);
        }

        /// <summary>
        /// Creates a new instance of ApiException with a code, inner exception, and parameters
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="innerException">The inner exception</param>
        /// <param name="params">Parameters for message formatting</param>
        public ApiException(int code, Exception innerException, params string[] @params) : base(GetMessage(code, @params), innerException)
        {
            Code = code;
            Message = GetMessage(code, @params);
        }

        /// <summary>
        /// Creates a new instance of ApiException with a message
        /// </summary>
        /// <param name="message">The error message</param>
        public ApiException(string message) : base(message)
        {
            Code = ErrorCode.INTERNAL_SERVER_ERROR;
            Message = message;
        }

        /// <summary>
        /// Creates a new instance of ApiException with a message and inner exception
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="innerException">The inner exception</param>
        public ApiException(string message, Exception innerException) : base(message, innerException)
        {
            Code = ErrorCode.INTERNAL_SERVER_ERROR;
            Message = message;
        }

        /// <summary>
        /// Gets a message for the specified error code
        /// </summary>
        /// <param name="code">The error code</param>
        /// <returns>The error message</returns>
        private static string GetMessage(int code)
        {
            // In the Java version, this would call MessageUtils.getMessage(code)
            // For now, we'll return a default message based on the code
            return GetDefaultMessage(code);
        }

        /// <summary>
        /// Gets a message for the specified error code with parameters
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="params">Parameters for message formatting</param>
        /// <returns>The error message</returns>
        private static string GetMessage(int code, params string[] @params)
        {
            // In the Java version, this would call MessageUtils.getMessage(code, params)
            // For now, we'll return a default message based on the code
            return string.Format(GetDefaultMessage(code), @params);
        }

        /// <summary>
        /// Gets a default message for the specified error code
        /// </summary>
        /// <param name="code">The error code</param>
        /// <returns>The default error message</returns>
        private static string GetDefaultMessage(int code)
        {
            // This would typically retrieve messages from a resource file
            // For now, we'll provide basic messages based on common codes
            return code switch
            {
                ErrorCode.INTERNAL_SERVER_ERROR => "Internal server error",
                ErrorCode.UNAUTHORIZED => "Unauthorized",
                ErrorCode.FORBIDDEN => "Forbidden",
                ErrorCode.NOT_FOUND => "Resource not found",
                ErrorCode.BAD_REQUEST => "Bad request",
                ErrorCode.DATA_ALREADY_EXISTS => "Data already exists",
                ErrorCode.ACCOUNT_PASSWORD_ERROR => "Account or password error",
                ErrorCode.ACCOUNT_LOCKED => "Account locked",
                ErrorCode.ACCOUNT_NOT_EXIST => "Account does not exist",
                _ => "An error occurred"
            };
        }
    }
}