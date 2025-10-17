using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace IotApi.Core.utils
{
    /// <summary>
    /// 敏感数据处理工具类
    /// </summary>
    public static class SensitiveDataUtils
    {
        // 敏感字段列表
        private static readonly HashSet<string> SENSITIVE_FIELDS = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "api_key", "personal_access_token", "access_token", "token",
            "secret", "access_key_secret", "secret_key"
        };

        /// <summary>
        /// 检查字段是否为敏感字段
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <returns>是否为敏感字段</returns>
        public static bool IsSensitiveField(string fieldName)
        {
            return !string.IsNullOrWhiteSpace(fieldName) && SENSITIVE_FIELDS.Contains(fieldName.ToLower());
        }

        /// <summary>
        /// 隐藏字符串中间部分
        /// </summary>
        /// <param name="value">原始字符串</param>
        /// <returns>掩码后的字符串</returns>
        public static string MaskMiddle(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            int length = value.Length;
            if (length <= 8)
            {
                // 短字符串保留前2后2
                return value.Substring(0, 2) + "****" + value.Substring(length - 2);
            }
            else
            {
                // 长字符串保留前4后4
                int maskLength = length - 8;
                return value.Substring(0, 4) + new string('*', maskLength) + value.Substring(length - 4);
            }
        }

        /// <summary>
        /// 判断字符串是否是被掩码处理过的值
        /// </summary>
        /// <param name="value">字符串值</param>
        /// <returns>是否被掩码处理</returns>
        public static bool IsMaskedValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            // 掩码值至少包含4个连续的*
            return value.Contains("****");
        }

        /// <summary>
        /// 处理JsonObject中的敏感字段
        /// </summary>
        /// <param name="jsonObject">JSON对象</param>
        /// <returns>处理后的JSON对象</returns>
        public static JsonObject MaskSensitiveFields(JsonObject jsonObject)
        {
            if (jsonObject == null)
            {
                return null;
            }

            JsonObject result = new JsonObject();

            foreach (var property in jsonObject)
            {
                string key = property.Key;
                JsonNode value = property.Value;

                if (SENSITIVE_FIELDS.Contains(key.ToLower()) && value?.GetValueKind() == JsonValueKind.String)
                {
                    result.Add(key, MaskMiddle(value.GetValue<string>()));
                }
                else if (value?.GetValueKind() == JsonValueKind.Object)
                {
                    result.Add(key, MaskSensitiveFields(value.AsObject()));
                }
                else
                {
                    result.Add(key, value?.DeepClone());
                }
            }

            return result;
        }

        /// <summary>
        /// 比较两个JsonObject的敏感字段是否相同
        /// </summary>
        /// <param name="original">原始JSON对象</param>
        /// <param name="updated">更新后的JSON对象</param>
        /// <returns>敏感字段是否相同</returns>
        public static bool IsSensitiveDataEqual(JsonObject original, JsonObject updated)
        {
            if (original == null && updated == null)
            {
                return true;
            }
            if (original == null || updated == null)
            {
                return false;
            }

            // 提取并比较特定敏感字段
            return CompareSpecificSensitiveFields(original, updated, "api_key") &&
                   CompareSpecificSensitiveFields(original, updated, "personal_access_token") &&
                   CompareSpecificSensitiveFields(original, updated, "access_token") &&
                   CompareSpecificSensitiveFields(original, updated, "token") &&
                   CompareSpecificSensitiveFields(original, updated, "secret") &&
                   CompareSpecificSensitiveFields(original, updated, "access_key_secret") &&
                   CompareSpecificSensitiveFields(original, updated, "secret_key");
        }

        /// <summary>
        /// 比较两个JSON对象中特定敏感字段是否相同
        /// </summary>
        /// <param name="original">原始JSON对象</param>
        /// <param name="updated">更新后的JSON对象</param>
        /// <param name="fieldName">字段名</param>
        /// <returns>特定敏感字段是否相同</returns>
        private static bool CompareSpecificSensitiveFields(JsonObject original, JsonObject updated, string fieldName)
        {
            // 提取原始对象中的指定敏感字段
            Dictionary<string, string> originalFields = new Dictionary<string, string>();
            ExtractSpecificSensitiveField(original, originalFields, fieldName, "");

            // 提取更新对象中的指定敏感字段
            Dictionary<string, string> updatedFields = new Dictionary<string, string>();
            ExtractSpecificSensitiveField(updated, updatedFields, fieldName, "");

            // 如果字段数量不同，说明有增删
            if (originalFields.Count != updatedFields.Count)
            {
                return false;
            }

            // 比较每个字段的值
            foreach (var entry in originalFields)
            {
                string key = entry.Key;
                string originalValue = entry.Value;

                if (!updatedFields.TryGetValue(key, out string updatedValue) || updatedValue != originalValue)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 递归提取JSON对象中指定名称的敏感字段
        /// </summary>
        /// <param name="jsonObject">JSON对象</param>
        /// <param name="fieldsMap">字段映射</param>
        /// <param name="targetFieldName">目标字段名</param>
        /// <param name="parentPath">父路径</param>
        private static void ExtractSpecificSensitiveField(JsonObject jsonObject, Dictionary<string, string> fieldsMap,
            string targetFieldName, string parentPath)
        {
            if (jsonObject == null)
            {
                return;
            }

            foreach (var property in jsonObject)
            {
                string key = property.Key;
                string fullPath = string.IsNullOrEmpty(parentPath) ? key : parentPath + "." + key;
                JsonNode value = property.Value;

                if (value?.GetValueKind() == JsonValueKind.Object)
                {
                    // 递归处理嵌套JSON对象
                    ExtractSpecificSensitiveField(value.AsObject(), fieldsMap, targetFieldName, fullPath);
                }
                else if (value?.GetValueKind() == JsonValueKind.String && 
                         key.Equals(targetFieldName, StringComparison.OrdinalIgnoreCase))
                {
                    // 找到目标敏感字段，保存其路径和值
                    fieldsMap[fullPath] = value.GetValue<string>();
                }
            }
        }
    }
}