namespace IotApi.Services
{
    /// <summary>
    /// 智能体MCP接入点服务接口
    /// </summary>
    public interface IAgentMcpAccessPointService
    {
        /// <summary>
        /// 获取智能体的MCP接入点地址
        /// </summary>
        /// <param name="id">智能体ID</param>
        /// <returns>MCP接入点地址</returns>
        Task<string> GetAgentMcpAccessAddressAsync(string id);

        /// <summary>
        /// 获取智能体的MCP接入点已有的工具列表
        /// </summary>
        /// <param name="id">智能体ID</param>
        /// <returns>工具列表</returns>
        Task<IEnumerable<string>> GetAgentMcpToolsListAsync(string id);
    }
}