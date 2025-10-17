using System;
using System.Threading.Tasks;

namespace IotApi.Services
{
    /// <summary>
    /// Redis服务接口
    /// </summary>
    public interface IRedisService
    {
        /// <summary>
        /// 设置字符串值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间（秒）</param>
        /// <returns>是否成功</returns>
        Task<bool> SetStringAsync(string key, string value, int expiry = -1);

        /// <summary>
        /// 获取字符串值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        Task<string> GetStringAsync(string key);

        /// <summary>
        /// 设置对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间（秒）</param>
        /// <returns>是否成功</returns>
        Task<bool> SetAsync<T>(string key, T value, int expiry = -1);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>对象</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteAsync(string key);

        /// <summary>
        /// 检查键是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsAsync(string key);
    }
}