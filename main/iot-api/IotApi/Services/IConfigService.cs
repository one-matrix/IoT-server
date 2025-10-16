namespace IotApi.Services
{
    /// <summary>
    /// 配置服务接口
    /// </summary>
    public interface IConfigService
    {
        /// <summary>
        /// 获取服务器配置
        /// </summary>
        /// <param name="isServer">是否为服务器端请求</param>
        /// <returns>配置信息</returns>
        object GetConfig(bool isServer);

        /// <summary>
        /// 获取智能体模型
        /// </summary>
        /// <param name="macAddress">MAC地址</param>
        /// <param name="selectedModule">选择的模块</param>
        /// <returns>智能体模型信息</returns>
        object GetAgentModels(string macAddress, string selectedModule);
    }
}