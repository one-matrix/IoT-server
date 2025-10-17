using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace IotApi.Core.utils
{
    /// <summary>
    /// 转换工具类
    /// </summary>
    public static class ConvertUtils
    {
        private static readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger(typeof(ConvertUtils).Name);

        /// <summary>
        /// 将源对象属性值复制到目标类型的新实例
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>目标类型的新实例</returns>
        public static T SourceToTarget<T>(object source) where T : new()
        {
            if (source == null)
            {
                return default;
            }

            T targetObject = default;
            try
            {
                targetObject = new T();
                var properties = typeof(T).GetProperties();
                
                foreach (var targetProperty in properties)
                {
                    var sourceProperty = source.GetType().GetProperty(targetProperty.Name);
                    if (sourceProperty != null && sourceProperty.CanRead && targetProperty.CanWrite)
                    {
                        var value = sourceProperty.GetValue(source);
                        if (value != null && targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                        {
                            targetProperty.SetValue(targetObject, value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Convert error");
            }

            return targetObject;
        }

        /// <summary>
        /// 将源对象集合转换为目标类型的集合
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="sourceList">源对象集合</param>
        /// <returns>目标类型的集合</returns>
        public static List<T> SourceToTarget<T>(IEnumerable<object> sourceList) where T : new()
        {
            if (sourceList == null)
            {
                return null;
            }

            List<T> targetList = new List<T>();
            try
            {
                foreach (var source in sourceList)
                {
                    T targetObject = SourceToTarget<T>(source);
                    targetList.Add(targetObject);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Convert error");
            }

            return targetList;
        }
    }
}