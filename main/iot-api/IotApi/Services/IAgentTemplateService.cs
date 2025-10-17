using IotApi.Models;

namespace IotApi.Services
{
    /// <summary>
    /// 智能体配置模板服务接口
    /// </summary>
    public interface IAgentTemplateService
    {
        /// <summary>
        /// 获取默认模板
        /// </summary>
        /// <returns>默认模板实体</returns>
        Task<AgentTemplate> GetDefaultTemplateAsync();

        /// <summary>
        /// 更新默认模板中的模型ID
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="modelId">模型ID</param>
        /// <returns>操作结果</returns>
        Task UpdateDefaultTemplateModelIdAsync(string modelType, string modelId);

        /// <summary>
        /// 删除模板后重新排序剩余模板
        /// </summary>
        /// <param name="deletedSort">被删除模板的排序值</param>
        /// <returns>操作结果</returns>
        Task ReorderTemplatesAfterDeleteAsync(int deletedSort);

        /// <summary>
        /// 获取下一个可用的排序序号（寻找最小的未使用序号）
        /// </summary>
        /// <returns>下一个可用的排序序号</returns>
        Task<int> GetNextAvailableSortAsync();
    }
}