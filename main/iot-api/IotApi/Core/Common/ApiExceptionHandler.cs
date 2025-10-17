using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IotApi.Core.common
{
    /// <summary>
    /// Global exception handler for the API
    /// </summary>
    public class ApiExceptionHandler : IExceptionFilter
    {
        /// <summary>
        /// Handles exceptions that occur during request processing
        /// </summary>
        /// <param name="context">The exception context</param>
        public void OnException(ExceptionContext context)
        {
            object result = null;
            
            if (context.Exception is ApiException apiException)
            {
                // Handle custom API exceptions
                result = new 
                {
                    code = apiException.Code,
                    message = apiException.Message,
                    data = (object)null
                };
            }
            else if (context.Exception is InvalidOperationException && 
                     context.Exception.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
            {
                // Handle duplicate key exceptions
                result = new 
                {
                    code = ErrorCode.DATA_ALREADY_EXISTS,
                    message = "Data already exists",
                    data = (object)null
                };
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                // Handle unauthorized access exceptions
                result = new 
                {
                    code = ErrorCode.FORBIDDEN,
                    message = "Access forbidden",
                    data = (object)null
                };
            }
            else if (context.Exception is KeyNotFoundException)
            {
                // Handle resource not found exceptions
                result = new 
                {
                    code = ErrorCode.NOT_FOUND,
                    message = "Resource not found",
                    data = (object)null
                };
            }
            else
            {
                // Handle all other exceptions
                result = new 
                {
                    code = ErrorCode.INTERNAL_SERVER_ERROR,
                    message = "An internal server error occurred",
                    data = (object)null
                };
            }

            context.Result = new ObjectResult(result)
            {
                StatusCode = GetHttpStatusCode(result.GetType().GetProperty("code")?.GetValue(result) as int? ?? ErrorCode.INTERNAL_SERVER_ERROR)
            };
            
            // Mark exception as handled
            context.ExceptionHandled = true;
        }

        /// <summary>
        /// Maps error codes to HTTP status codes
        /// </summary>
        /// <param name="errorCode">The error code</param>
        /// <returns>The HTTP status code</returns>
        private static int GetHttpStatusCode(int errorCode)
        {
            return errorCode switch
            {
                ErrorCode.UNAUTHORIZED => 401,
                ErrorCode.FORBIDDEN => 403,
                ErrorCode.NOT_FOUND => 404,
                ErrorCode.BAD_REQUEST => 400,
                ErrorCode.DATA_ALREADY_EXISTS => 409,
                _ => 500
            };
        }
    }
}