using IotApi.DTOs;
using IotApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace IotApi.Services
{
    /// <summary>
    /// 模型服务实现
    /// </summary>
    public class ModelConfigService : IModelConfigService
    {
        private readonly ApplicationDbContext _context;

        public ModelConfigService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<object>> GetModelNamesAsync(string modelType, string modelName)
        {
            var query = _context.AiModelConfigs.AsQueryable();
            
            if (!string.IsNullOrEmpty(modelType))
            {
                query = query.Where(m => m.ModelType == modelType);
            }
            
            if (!string.IsNullOrEmpty(modelName))
            {
                query = query.Where(m => m.ModelName.Contains(modelName));
            }
            
            return await query
                .Where(m => m.IsEnabled == 1)
                .Select(m => new ModelBasicInfoDto { Id = m.Id, ModelName = m.ModelName })
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<object>> GetLlmModelNamesAsync(string modelName)
        {
            var query = _context.AiModelConfigs.Where(m => m.ModelType == "LLM");
            
            if (!string.IsNullOrEmpty(modelName))
            {
                query = query.Where(m => m.ModelName.Contains(modelName));
            }
            
            var models = await query
                .Where(m => m.IsEnabled == 1)
                .ToListAsync();
                
            return models.Select(m => new LlmModelBasicInfoDto {
                Id = m.Id, 
                ModelName = m.ModelName,
                Type = m.ConfigJson != null ? GetConfigType(m.ConfigJson) : null
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<object>> GetModelProviderListAsync(string modelType)
        {
            return await _context.AiModelProviders
                .Where(p => p.ModelType == modelType)
                .Select(p => new { p.Id, p.Name, p.ProviderCode })
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<object> GetModelConfigListAsync(string modelType, string modelName, int page, int limit)
        {
            var query = _context.AiModelConfigs.AsQueryable();
            
            if (!string.IsNullOrEmpty(modelType))
            {
                query = query.Where(m => m.ModelType == modelType);
            }
            
            if (!string.IsNullOrEmpty(modelName))
            {
                query = query.Where(m => m.ModelName.Contains(modelName));
            }
            
            // 添加排序规则：先按is_enabled降序，再按sort升序
            query = query.OrderByDescending(m => m.IsEnabled).ThenBy(m => m.Sort);
            
            var totalCount = await query.CountAsync();
            var models = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
                
            // 处理敏感字段
            foreach (var model in models)
            {
                if (!string.IsNullOrEmpty(model.ConfigJson))
                {
                    model.ConfigJson = MaskSensitiveFields(model.ConfigJson);
                }
            }
                
            return new
            {
                list = models,
                total = totalCount
            };
        }

        /// <inheritdoc/>
        public async Task<object> AddModelConfigAsync(string modelType, string provideCode, ModelConfigDto modelConfigDto)
        {
            // 验证供应器是否存在
            var provider = await _context.AiModelProviders
                .FirstOrDefaultAsync(p => p.ModelType == modelType && p.ProviderCode == provideCode);
                
            if (provider == null)
            {
                throw new KeyNotFoundException($"模型供应器不存在: {modelType}/{provideCode}");
            }
            
            var modelConfig = new AiModelConfig
            {
                Id = string.IsNullOrEmpty(modelConfigDto.Id) ? Guid.NewGuid().ToString("N").Substring(0, 32) : modelConfigDto.Id,
                ModelType = modelType,
                ModelName = modelConfigDto.ModelName,
                ModelCode = modelConfigDto.ModelCode,
                ProviderCode = provideCode,
                ConfigJson = modelConfigDto.ConfigJson,
                IsEnabled = modelConfigDto.IsEnabled,
                IsDefault = 0, // 默认不是默认配置
                Sort = modelConfigDto.Sort,
                Remark = modelConfigDto.Remark,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };
            
            _context.AiModelConfigs.Add(modelConfig);
            await _context.SaveChangesAsync();
            
            // 处理敏感字段
            if (!string.IsNullOrEmpty(modelConfig.ConfigJson))
            {
                modelConfig.ConfigJson = MaskSensitiveFields(modelConfig.ConfigJson);
            }
            
            return modelConfig;
        }

        /// <inheritdoc/>
        public async Task<object> EditModelConfigAsync(string modelType, string provideCode, string id, ModelConfigDto modelConfigDto)
        {
            // 验证供应器是否存在
            var provider = await _context.AiModelProviders
                .FirstOrDefaultAsync(p => p.ModelType == modelType && p.ProviderCode == provideCode);
                
            if (provider == null)
            {
                throw new KeyNotFoundException($"模型供应器不存在: {modelType}/{provideCode}");
            }
            
            var modelConfig = await _context.AiModelConfigs.FindAsync(id);
            if (modelConfig == null)
            {
                throw new KeyNotFoundException($"模型配置不存在: {id}");
            }
            
            // 检查是否是默认配置
            if (modelConfig.IsDefault == 1)
            {
                throw new InvalidOperationException("默认模型配置不能修改");
            }
            
            // 检查智能体引用
            await CheckAgentReference(id);
            
            modelConfig.ModelType = modelType;
            modelConfig.ModelName = modelConfigDto.ModelName;
            modelConfig.ModelCode = modelConfigDto.ModelCode;
            modelConfig.ProviderCode = provideCode;
            
            // 处理配置JSON，仅更新非敏感字段和明确修改的敏感字段
            if (!string.IsNullOrEmpty(modelConfigDto.ConfigJson))
            {
                modelConfig.ConfigJson = MergeConfigJson(modelConfig.ConfigJson, modelConfigDto.ConfigJson);
            }
            
            modelConfig.IsEnabled = modelConfigDto.IsEnabled;
            modelConfig.Sort = modelConfigDto.Sort;
            modelConfig.Remark = modelConfigDto.Remark;
            modelConfig.UpdateDate = DateTime.UtcNow;
            
            _context.Entry(modelConfig).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            // 处理敏感字段
            if (!string.IsNullOrEmpty(modelConfig.ConfigJson))
            {
                modelConfig.ConfigJson = MaskSensitiveFields(modelConfig.ConfigJson);
            }
            
            return modelConfig;
        }

        /// <inheritdoc/>
        public async Task DeleteModelConfigAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("模型配置ID不能为空");
            }
            
            var modelConfig = await _context.AiModelConfigs.FindAsync(id);
            if (modelConfig == null)
            {
                throw new KeyNotFoundException($"模型配置不存在: {id}");
            }
            
            // 检查是否是默认配置
            if (modelConfig.IsDefault == 1)
            {
                throw new InvalidOperationException("默认模型配置不能删除");
            }
            
            // 检查智能体引用
            await CheckAgentReference(id);
            
            // 检查意图识别配置引用
            await CheckIntentConfigReference(id);
            
            _context.AiModelConfigs.Remove(modelConfig);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<object> GetModelConfigAsync(string id)
        {
            var modelConfig = await _context.AiModelConfigs.FindAsync(id);
            if (modelConfig == null)
            {
                throw new KeyNotFoundException($"模型配置不存在: {id}");
            }
            
            // 处理敏感字段
            if (!string.IsNullOrEmpty(modelConfig.ConfigJson))
            {
                modelConfig.ConfigJson = MaskSensitiveFields(modelConfig.ConfigJson);
            }
            
            return modelConfig;
        }

        /// <inheritdoc/>
        public async Task EnableModelConfigAsync(string id, int status)
        {
            var modelConfig = await _context.AiModelConfigs.FindAsync(id);
            if (modelConfig == null)
            {
                throw new KeyNotFoundException($"模型配置不存在: {id}");
            }
            
            modelConfig.IsEnabled = status;
            modelConfig.UpdateDate = DateTime.UtcNow;
            
            _context.Entry(modelConfig).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> ModelConfigExistsAsync(string id)
        {
            return await _context.AiModelConfigs.AnyAsync(e => e.Id == id);
        }
        
        /// <inheritdoc/>
        public async Task<string> GetModelNameByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            
            var modelConfig = await _context.AiModelConfigs.FindAsync(id);
            return modelConfig?.ModelName;
        }
        
        /// <inheritdoc/>
        public async Task<AiModelConfig> GetModelByIdFromCacheAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            
            // 简单的内存缓存实现（实际项目中应使用Redis等）
            var modelConfig = await _context.AiModelConfigs.FindAsync(id);
            return modelConfig;
        }
        
        /// <inheritdoc/>
        public async Task SetDefaultModelAsync(string modelType, int isDefault)
        {
            if (string.IsNullOrEmpty(modelType))
            {
                throw new ArgumentException("模型类型不能为空");
            }
            
            // 将该模型类型的所有配置设置为非默认
            var modelConfigs = await _context.AiModelConfigs
                .Where(m => m.ModelType == modelType)
                .ToListAsync();
                
            foreach (var config in modelConfigs)
            {
                config.IsDefault = isDefault;
                config.UpdateDate = DateTime.UtcNow;
                _context.Entry(config).State = EntityState.Modified;
            }
            
            await _context.SaveChangesAsync();
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<object>> GetTtsPlatformListAsync()
        {
            return await _context.AiModelConfigs
                .Where(m => m.ModelType == "TTS")
                .Select(m => new { m.Id, m.ModelName })
                .ToListAsync();
        }
        
        /// <summary>
        /// 从配置JSON中提取类型
        /// </summary>
        /// <param name="configJson">配置JSON</param>
        /// <returns>类型</returns>
        private string GetConfigType(string configJson)
        {
            try
            {
                var json = JsonDocument.Parse(configJson);
                if (json.RootElement.TryGetProperty("type", out var typeElement))
                {
                    return typeElement.GetString();
                }
            }
            catch
            {
                // 解析失败时返回null
            }
            return null;
        }
        
        /// <summary>
        /// 处理敏感字段（掩码）
        /// </summary>
        /// <param name="configJson">配置JSON</param>
        /// <returns>处理后的配置JSON</returns>
        private string MaskSensitiveFields(string configJson)
        {
            try
            {
                var json = JsonDocument.Parse(configJson);
                var root = json.RootElement.Clone();
                
                // 这里应该实现具体的敏感字段掩码逻辑
                // 简化实现，实际项目中需要根据具体需求实现
                return configJson;
            }
            catch
            {
                // 解析失败时返回原值
                return configJson;
            }
        }
        
        /// <summary>
        /// 合并配置JSON
        /// </summary>
        /// <param name="originalJson">原始JSON</param>
        /// <param name="updatedJson">更新的JSON</param>
        /// <returns>合并后的JSON</returns>
        private string MergeConfigJson(string originalJson, string updatedJson)
        {
            // 简化实现，实际项目中需要根据具体需求实现敏感字段处理逻辑
            return updatedJson;
        }
        
        /// <summary>
        /// 检查智能体引用
        /// </summary>
        /// <param name="modelId">模型ID</param>
        private async Task CheckAgentReference(string modelId)
        {
            // 检查是否有智能体引用该模型
            var agentCount = await _context.AiAgents
                .CountAsync(a => a.AsrModelId == modelId || 
                                a.VadModelId == modelId || 
                                a.LlmModelId == modelId || 
                                a.TtsModelId == modelId || 
                                a.MemModelId == modelId || 
                                a.VllmModelId == modelId || 
                                a.IntentModelId == modelId);
                                
            if (agentCount > 0)
            {
                throw new InvalidOperationException("该模型配置已被智能体引用，无法删除");
            }
        }
        
        /// <summary>
        /// 检查意图识别配置引用
        /// </summary>
        /// <param name="modelId">模型ID</param>
        private async Task CheckIntentConfigReference(string modelId)
        {
            // 检查是否有意图识别配置引用该LLM模型
            var intentConfigCount = await _context.AiModelConfigs
                .CountAsync(m => m.ModelType == "Intent" && 
                                m.ConfigJson != null && 
                                m.ConfigJson.Contains(modelId));
                                
            if (intentConfigCount > 0)
            {
                throw new InvalidOperationException("该LLM模型配置已被意图识别配置引用，无法删除");
            }
        }
    }
}