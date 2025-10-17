using System;
using Microsoft.Extensions.DependencyInjection;

namespace IotApi.Core.utils
{
    /// <summary>
    /// 应用程序上下文工具类，用于在非依赖注入环境中获取服务
    /// </summary>
    public static class ApplicationContextUtils
    {
        private static IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化服务提供者
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>服务实例</returns>
        public static T GetService<T>() where T : class
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("IServiceProvider未初始化，请先调用Configure方法");
            }
            return _serviceProvider.GetService<T>();
        }

        /// <summary>
        /// 获取必需的服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>服务实例</returns>
        /// <exception cref="InvalidOperationException">如果服务未注册则抛出异常</exception>
        public static T GetRequiredService<T>() where T : class
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("IServiceProvider未初始化，请先调用Configure方法");
            }
            return _serviceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns>服务实例</returns>
        public static object GetService(Type serviceType)
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("IServiceProvider未初始化，请先调用Configure方法");
            }
            return _serviceProvider.GetService(serviceType);
        }

        /// <summary>
        /// 获取必需的服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns>服务实例</returns>
        /// <exception cref="InvalidOperationException">如果服务未注册则抛出异常</exception>
        public static object GetRequiredService(Type serviceType)
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("IServiceProvider未初始化，请先调用Configure方法");
            }
            return _serviceProvider.GetRequiredService(serviceType);
        }

        /// <summary>
        /// 检查是否包含指定类型的服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>是否包含服务</returns>
        public static bool ContainsService<T>() where T : class
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("IServiceProvider未初始化，请先调用Configure方法");
            }
            return _serviceProvider.GetService<T>() != null;
        }

        /// <summary>
        /// 检查是否包含指定类型的服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns>是否包含服务</returns>
        public static bool ContainsService(Type serviceType)
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("IServiceProvider未初始化，请先调用Configure方法");
            }
            return _serviceProvider.GetService(serviceType) != null;
        }
    }
}