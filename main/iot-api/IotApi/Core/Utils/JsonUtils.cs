using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IotApi.Core.utils
{
    /// <summary>
    /// JSON 工具类
    /// </summary>
    public static class JsonUtils
    {
        private static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        /// <summary>
        /// 将对象转换为JSON字符串
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>JSON字符串</returns>
        public static string ToJsonString(object obj)
        {
            try
            {
                return JsonSerializer.Serialize(obj, DefaultOptions);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("JSON序列化失败", ex);
            }
        }

        /// <summary>
        /// 将JSON字符串解析为指定类型的对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="text">JSON字符串</param>
        /// <returns>解析后的对象</returns>
        public static T ParseObject<T>(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return default;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(text, DefaultOptions);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("JSON反序列化失败", ex);
            }
        }

        /// <summary>
        /// 将字节数组解析为指定类型的对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="bytes">字节数组</param>
        /// <returns>解析后的对象</returns>
        public static T ParseObject<T>(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return default;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(bytes, DefaultOptions);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("JSON反序列化失败", ex);
            }
        }

        /// <summary>
        /// 将JSON字符串解析为对象列表
        /// </summary>
        /// <typeparam name="T">列表元素类型</typeparam>
        /// <param name="text">JSON字符串</param>
        /// <returns>对象列表</returns>
        public static List<T> ParseArray<T>(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new List<T>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<T>>(text, DefaultOptions);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("JSON数组反序列化失败", ex);
            }
        }
    }
}