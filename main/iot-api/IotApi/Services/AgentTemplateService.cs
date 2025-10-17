using Microsoft.EntityFrameworkCore;
using IotApi.Models;
using System.Threading.Tasks;
using System.Linq;

namespace IotApi.Services
{
    /// <summary>
    /// 智能体配置模板服务实现类
    /// </summary>
    public class AgentTemplateService : IAgentTemplateService
    {
        private readonly ApplicationDbContext _dbContext;

        public AgentTemplateService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取默认模板
        /// </summary>
        /// <returns>默认模板实体</returns>
        public async Task<AgentTemplate> GetDefaultTemplateAsync()
        {
            return await _dbContext.AiAgentTemplates
                .FirstOrDefaultAsync(t => t.IsDefault == true);
        }

        /// <summary>
        /// 更新默认模板中的模型ID
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="modelId">模型ID</param>
        /// <returns>操作结果</returns>
        public async Task UpdateDefaultTemplateModelIdAsync(string modelType, string modelId)
        {
            var defaultTemplate = await GetDefaultTemplateAsync();
            if (defaultTemplate == null)
            {
                throw new KeyNotFoundException("未找到默认模板");
            }

            // 根据模型类型更新对应的模型ID
            switch (modelType.ToLower())
            {
                case "llm":
                    defaultTemplate.LlmModelId = modelId;
                    break;
                case "tts":
                    defaultTemplate.TtsModelId = modelId;
                    break;
                case "stt":
                    defaultTemplate.SttModelId = modelId;
                    break;
                default:
                    throw new ArgumentException($"不支持的模型类型: {modelType}");
            }

            _dbContext.AiAgentTemplates.Update(defaultTemplate);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 删除模板后重新排序剩余模板
        /// </summary>
        /// <param name="deletedSort">被删除模板的排序值</param>
        /// <returns>操作结果</returns>
        public async Task ReorderTemplatesAfterDeleteAsync(int deletedSort)
        {
            var templates = await _dbContext.AiAgentTemplates
                .Where(t => t.Sort > deletedSort)
                .ToListAsync();

            foreach (var template in templates)
            {
                template.Sort -= 1;
            }

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 获取下一个可用的排序序号（寻找最小的未使用序号）
        /// </summary>
        /// <returns>下一个可用的排序序号</returns>
        public async Task<int> GetNextAvailableSortAsync()
        {
            var usedSorts = await _dbContext.AiAgentTemplates
                .Select(t => t.Sort)
                .OrderBy(s => s)
                .ToListAsync();

            int nextSort = 1;
            foreach (var sort in usedSorts)
            {
                if (sort == nextSort)
                {
                    nextSort++;
                }
                else if (sort > nextSort)
                {
                    break;
                }
            }

            return nextSort;
        }
    }
}