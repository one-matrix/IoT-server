using IotApi.Models;

namespace IotApi.Services
{
    /// <summary>
    /// Agent与插件映射服务接口
    /// </summary>
    public interface IAgentPluginMappingService
    {
        /// <summary>
        /// 根据智能体ID获取插件参数
        /// </summary>
        /// <param name="agentId">智能体ID</param>
        /// <returns>插件参数列表</returns>
        Task<IEnumerable<AgentPluginMapping>> GetAgentPluginParamsByAgentIdAsync(string agentId);

        /// <summary>
        /// 根据智能体ID删除插件参数
        /// </summary>
        /// <param name="agentId">智能体ID</param>
        /// <returns>操作结果</returns>
        Task DeleteByAgentIdAsync(string agentId);
    }
}