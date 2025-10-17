using Microsoft.EntityFrameworkCore;
using IotApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace IotApi.Services
{
    /// <summary>
    /// Agent与插件映射服务实现类
    /// </summary>
    public class AgentPluginMappingService : IAgentPluginMappingService
    {
        private readonly ApplicationDbContext _dbContext;

        public AgentPluginMappingService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 根据智能体ID获取插件参数
        /// </summary>
        /// <param name="agentId">智能体ID</param>
        /// <returns>插件参数列表</returns>
        public async Task<IEnumerable<AgentPluginMapping>> GetAgentPluginParamsByAgentIdAsync(string agentId)
        {
            return await _dbContext.AgentPluginMappings
                .Where(p => p.AgentId == agentId)
                .ToListAsync();
        }

        /// <summary>
        /// 根据智能体ID删除插件参数
        /// </summary>
        /// <param name="agentId">智能体ID</param>
        /// <returns>操作结果</returns>
        public async Task DeleteByAgentIdAsync(string agentId)
        {
            var mappings = await _dbContext.AgentPluginMappings
                .Where(p => p.AgentId == agentId)
                .ToListAsync();

            if (mappings.Any())
            {
                _dbContext.AgentPluginMappings.RemoveRange(mappings);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}