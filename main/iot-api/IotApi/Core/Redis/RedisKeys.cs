namespace IotApi.Core.Redis
{
    /// <summary>
    /// Redis Key 常量类
    /// </summary>
    public class RedisKeys
    {
        /// <summary>
        /// 获取系统配置缓存key
        /// </summary>
        public static string GetServerConfigKey()
        {
            return "server:config";
        }

        /// <summary>
        /// 系统参数Key
        /// </summary>
        public static string GetSysParamsKey()
        {
            return "sys:params";
        }

        /// <summary>
        /// 模型配置的Key
        /// </summary>
        public static string GetModelConfigById(string id)
        {
            return "model:data:" + id;
        }

        /// <summary>
        /// 获取音色详情缓存key
        /// </summary>
        public static string GetTimbreDetailsKey(string id)
        {
            return "timbre:details:" + id;
        }

        /// <summary>
        /// 获取版本号Key
        /// </summary>
        public static string GetVersionKey()
        {
            return "sys:version";
        }
    }
}