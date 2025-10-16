using IotApi.DTOs;
using IotApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace IotApi.Services
{
    /// <summary>
    /// 模型服务实现
    /// </summary>
    public class ModelService : IModelService
    {
        private readonly ApplicationDbContext _context;

        public ModelService(ApplicationDbContext context)
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
                .Select(m => new { m.Id, m.ModelName, m.ModelType })
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
            
            return await query
                .Select(m => new { m.Id, m.ModelName, m.ModelType })
                .ToListAsync();
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
            
            var totalCount = await query.CountAsync();
            var models = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
                
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
                Sort = modelConfigDto.Sort,
                Remark = modelConfigDto.Remark,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };
            
            _context.AiModelConfigs.Add(modelConfig);
            await _context.SaveChangesAsync();
            
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
            
            modelConfig.ModelType = modelType;
            modelConfig.ModelName = modelConfigDto.ModelName;
            modelConfig.ModelCode = modelConfigDto.ModelCode;
            modelConfig.ProviderCode = provideCode;
            modelConfig.ConfigJson = modelConfigDto.ConfigJson;
            modelConfig.IsEnabled = modelConfigDto.IsEnabled;
            modelConfig.Sort = modelConfigDto.Sort;
            modelConfig.Remark = modelConfigDto.Remark;
            modelConfig.UpdateDate = DateTime.UtcNow;
            
            _context.Entry(modelConfig).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return modelConfig;
        }

        /// <inheritdoc/>
        public async Task DeleteModelConfigAsync(string id)
        {
            var modelConfig = await _context.AiModelConfigs.FindAsync(id);
            if (modelConfig == null)
            {
                throw new KeyNotFoundException($"模型配置不存在: {id}");
            }
            
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
    }
}