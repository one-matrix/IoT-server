using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace IotApi.Services
{
    /// <summary>
    /// Redis服务实现类
    /// </summary>
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisService> _logger;

        public RedisService(IDistributedCache cache, ILogger<RedisService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<bool> SetStringAsync(string key, string value, int expiry = -1)
        {
            try
            {
                var options = new DistributedCacheEntryOptions();
                if (expiry > 0)
                {
                    options.SetAbsoluteExpiration(TimeSpan.FromSeconds(expiry));
                }

                await _cache.SetStringAsync(key, value, options);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"设置Redis缓存失败: {key}");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<string> GetStringAsync(string key)
        {
            try
            {
                return await _cache.GetStringAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取Redis缓存失败: {key}");
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> SetAsync<T>(string key, T value, int expiry = -1)
        {
            try
            {
                var options = new DistributedCacheEntryOptions();
                if (expiry > 0)
                {
                    options.SetAbsoluteExpiration(TimeSpan.FromSeconds(expiry));
                }

                string jsonValue = JsonSerializer.Serialize(value);
                await _cache.SetStringAsync(key, jsonValue, options);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"设置Redis缓存对象失败: {key}");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                string jsonValue = await _cache.GetStringAsync(key);
                if (string.IsNullOrEmpty(jsonValue))
                {
                    return default;
                }

                return JsonSerializer.Deserialize<T>(jsonValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取Redis缓存对象失败: {key}");
                return default;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"删除Redis缓存失败: {key}");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                var value = await _cache.GetStringAsync(key);
                return !string.IsNullOrEmpty(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"检查Redis缓存键是否存在失败: {key}");
                return false;
            }
        }
    }
}