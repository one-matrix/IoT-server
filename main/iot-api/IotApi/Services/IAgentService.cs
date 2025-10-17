using IotApi.DTOs;
using IotApi.Models;

namespace IotApi.Services
{
    /// <summary>
    /// 智能体服务接口
    /// </summary>
    public interface IAgentService
    {
        /// <summary>
        /// 获取用户的智能体列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>智能体列表</returns>
        Task<IEnumerable<Agent>> GetUserAgentsAsync(string userId);
        
        /// <summary>
        /// 获取所有智能体（分页）
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数量</param>
        /// <returns>分页智能体列表</returns>
        Task<(IEnumerable<Agent> Items, int Total)> GetAllAgentsAsync(int page, int limit);
        
        /// <summary>
        /// 根据ID获取智能体
        /// </summary>
        /// <param name="id">智能体ID</param>
        /// <returns>智能体信息</returns>
        Task<Agent> GetAgentByIdAsync(string id);
        
        /// <summary>
        /// 创建智能体
        /// </summary>
        /// <param name="agentDto">智能体数据</param>
        /// <param name="userId">用户ID</param>
        /// <returns>创建的智能体</returns>
        Task<Agent> CreateAgentAsync(AgentDto agentDto, string userId);
        
        /// <summary>
        /// 更新智能体
        /// </summary>
        /// <param name="id">智能体ID</param>
        /// <param name="agentDto">智能体数据</param>
        /// <param name="userId">用户ID</param>
        /// <returns>更新后的智能体</returns>
        Task<Agent> UpdateAgentAsync(string id, AgentDto agentDto, string userId);
        
        /// <summary>
        /// 删除智能体
        /// </summary>
        /// <param name="id">智能体ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteAgentAsync(string id, string userId);
    }
}