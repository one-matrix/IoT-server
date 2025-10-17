using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IotApi.Core.common;

namespace IotApi.Core.utils
{
    /// <summary>
    /// 资源处理工具
    /// </summary>
    public class ResourcesUtils
    {
        private readonly ILogger<ResourcesUtils> _logger;
        private readonly string _basePath;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="basePath">基础路径，默认为当前应用程序目录</param>
        public ResourcesUtils(ILogger<ResourcesUtils> logger, string basePath = null)
        {
            _logger = logger;
            _basePath = basePath ?? AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 读取资源，返回字符串
        /// </summary>
        /// <param name="fileName">资源路径：相对于基础路径</param>
        /// <returns>字符串内容</returns>
        public string LoadString(string fileName)
        {
            try
            {
                string filePath = Path.Combine(_basePath, fileName);
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "方法：LoadString()读取资源失败--{Message}", ex.Message);
                throw new ApiException(ErrorCode.RESOURCE_READ_ERROR);
            }
        }

        /// <summary>
        /// 异步读取资源，返回字符串
        /// </summary>
        /// <param name="fileName">资源路径：相对于基础路径</param>
        /// <returns>字符串内容</returns>
        public async Task<string> LoadStringAsync(string fileName)
        {
            try
            {
                string filePath = Path.Combine(_basePath, fileName);
                return await File.ReadAllTextAsync(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "方法：LoadStringAsync()读取资源失败--{Message}", ex.Message);
                throw new ApiException(ErrorCode.RESOURCE_READ_ERROR);
            }
        }

        /// <summary>
        /// 读取嵌入式资源，返回字符串
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <param name="assembly">程序集，默认为当前程序集</param>
        /// <returns>字符串内容</returns>
        public string LoadEmbeddedString(string resourceName, System.Reflection.Assembly assembly = null)
        {
            assembly ??= System.Reflection.Assembly.GetExecutingAssembly();

            try
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    throw new FileNotFoundException($"找不到嵌入式资源: {resourceName}");
                }

                using var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "方法：LoadEmbeddedString()读取嵌入式资源失败--{Message}", ex.Message);
                throw new ApiException(ErrorCode.RESOURCE_READ_ERROR);
            }
        }
    }
}