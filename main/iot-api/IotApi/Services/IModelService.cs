using IotApi.DTOs;
using IotApi.Models;

namespace IotApi.Services
{
    /// <summary>
    /// 模型服务接口
    /// </summary>
    public interface IModelService
    {
        /// <summary>
        /// 获取模型名称列表
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="modelName">模型名称（可选）</param>
        /// <returns>模型名称列表</returns>
        Task<IEnumerable<object>> GetModelNamesAsync(string modelType, string modelName);

        /// <summary>
        /// 获取LLM模型名称列表
        /// </summary>
        /// <param name="modelName">模型名称（可选）</param>
        /// <returns>LLM模型列表</returns>
        Task<IEnumerable<object>> GetLlmModelNamesAsync(string modelName);

        /// <summary>
        /// 获取模型供应器列表
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <returns>供应器列表</returns>
        Task<IEnumerable<object>> GetModelProviderListAsync(string modelType);

        /// <summary>
        /// 获取模型配置列表
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="modelName">模型名称（可选）</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数量</param>
        /// <returns>模型配置分页列表</returns>
        Task<object> GetModelConfigListAsync(string modelType, string modelName, int page, int limit);

        /// <summary>
        /// 添加模型配置
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="provideCode">供应器代码</param>
        /// <param name="modelConfigDto">模型配置信息</param>
        /// <returns>创建的模型配置</returns>
        Task<object> AddModelConfigAsync(string modelType, string provideCode, ModelConfigDto modelConfigDto);

        /// <summary>
        /// 编辑模型配置
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="provideCode">供应器代码</param>
        /// <param name="id">模型配置ID</param>
        /// <param name="modelConfigDto">模型配置信息</param>
        /// <returns>更新后的模型配置</returns>
        Task<object> EditModelConfigAsync(string modelType, string provideCode, string id, ModelConfigDto modelConfigDto);

        /// <summary>
        /// 删除模型配置
        /// </summary>
        /// <param name="id">模型配置ID</param>
        Task DeleteModelConfigAsync(string id);

        /// <summary>
        /// 获取模型配置详情
        /// </summary>
        /// <param name="id">模型配置ID</param>
        /// <returns>模型配置详情</returns>
        Task<object> GetModelConfigAsync(string id);

        /// <summary>
        /// 启用/禁用模型配置
        /// </summary>
        /// <param name="id">模型配置ID</param>
        /// <param name="status">状态：1-启用，0-禁用</param>
        Task EnableModelConfigAsync(string id, int status);

        /// <summary>
        /// 检查模型配置是否存在
        /// </summary>
        /// <param name="id">模型配置ID</param>
        /// <returns>是否存在</returns>
        Task<bool> ModelConfigExistsAsync(string id);
    }
}