using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IotApi.Models;
using IotApi.Services;
using IotApi.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IotApi.Controllers
{
    /// <summary>
    /// 模型配置管理
    /// </summary>
    [ApiController]
    [Route("xiaozhi/[controller]")]
    [Produces("application/json")]
    [Tags("模型管理")]
    public class ModelController : ControllerBase
    {
        private readonly IModelService _modelService;

        public ModelController(IModelService modelService)
        {
            _modelService = modelService;
        }

        /// <summary>
        /// 获取模型名称列表
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="modelName">模型名称（可选）</param>
        /// <returns>模型名称列表</returns>
        [HttpGet("names")]
        public async Task<ActionResult<IEnumerable<object>>> GetModelNames([FromQuery] string modelType, [FromQuery] string modelName = null)
        {
            var models = await _modelService.GetModelNamesAsync(modelType, modelName);
            return Ok(new { code = 0, data = models });
        }

        /// <summary>
        /// 获取LLM模型代码列表
        /// </summary>
        /// <param name="modelName">模型名称（可选）</param>
        /// <returns>LLM模型列表</returns>
        [HttpGet("llm/names")]
        public async Task<ActionResult<IEnumerable<object>>> GetLlmModelCodeList([FromQuery] string modelName = null)
        {
            var models = await _modelService.GetLlmModelNamesAsync(modelName);
            return Ok(new { code = 0, data = models });
        }

        /// <summary>
        /// 获取模型供应器列表
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <returns>供应器列表</returns>
        [HttpGet("{modelType}/provideTypes")]
        public async Task<ActionResult<IEnumerable<object>>> GetModelProviderList(string modelType)
        {
            var providers = await _modelService.GetModelProviderListAsync(modelType);
            return Ok(new { code = 0, data = providers });
        }

        /// <summary>
        /// 获取模型配置列表
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="modelName">模型名称（可选）</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数量</param>
        /// <returns>模型配置分页列表</returns>
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<object>>> GetModelConfigList(
            [FromQuery][Required] string modelType,
            [FromQuery] string modelName = null,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            var pageData = await _modelService.GetModelConfigListAsync(modelType, modelName, page, limit);
            return Ok(new { code = 0, data = pageData });
        }

        /// <summary>
        /// 新增模型配置
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="provideCode">供应器代码</param>
        /// <param name="modelConfigDto">模型配置信息</param>
        /// <returns>创建的模型配置</returns>
        [HttpPost("{modelType}/{provideCode}")]
        public async Task<ActionResult<object>> AddModelConfig(
            string modelType, 
            string provideCode, 
            [FromBody] ModelConfigDto modelConfigDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var modelConfig = await _modelService.AddModelConfigAsync(modelType, provideCode, modelConfigDto);
            return Ok(new { code = 0, data = modelConfig });
        }

        /// <summary>
        /// 编辑模型配置
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="provideCode">供应器代码</param>
        /// <param name="id">模型配置ID</param>
        /// <param name="modelConfigDto">模型配置信息</param>
        /// <returns>更新后的模型配置</returns>
        [HttpPut("{modelType}/{provideCode}/{id}")]
        public async Task<ActionResult<object>> EditModelConfig(
            string modelType,
            string provideCode,
            string id,
            [FromBody] ModelConfigDto modelConfigDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != modelConfigDto.Id)
            {
                return BadRequest(new { code = 1, msg = "ID不匹配" });
            }
            
            try
            {
                var modelConfig = await _modelService.EditModelConfigAsync(modelType, provideCode, id, modelConfigDto);
                return Ok(new { code = 0, data = modelConfig });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { code = 1, msg = "模型配置不存在" });
            }
        }

        /// <summary>
        /// 删除模型配置
        /// </summary>
        /// <param name="id">模型配置ID</param>
        /// <returns>操作结果</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModelConfig(string id)
        {
            try
            {
                await _modelService.DeleteModelConfigAsync(id);
                return Ok(new { code = 0 });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { code = 1, msg = "模型配置不存在" });
            }
        }

        /// <summary>
        /// 获取模型配置详情
        /// </summary>
        /// <param name="id">模型配置ID</param>
        /// <returns>模型配置详情</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetModelConfig(string id)
        {
            try
            {
                var modelConfig = await _modelService.GetModelConfigAsync(id);
                return Ok(new { code = 0, data = modelConfig });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { code = 1, msg = "模型配置不存在" });
            }
        }

        /// <summary>
        /// 启用/禁用模型配置
        /// </summary>
        /// <param name="id">模型配置ID</param>
        /// <param name="status">状态：1-启用，0-禁用</param>
        /// <returns>操作结果</returns>
        [HttpPut("enable/{id}/{status}")]
        public async Task<IActionResult> EnableModelConfig(string id, int status)
        {
            try
            {
                await _modelService.EnableModelConfigAsync(id, status);
                return Ok(new { code = 0 });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { code = 1, msg = "模型配置不存在" });
            }
        }

        // PUT: xiaozhi/models/default/{id}
        [HttpPut("default/{id}")]
        public async Task<IActionResult> SetDefaultModel(string id)
        {
            var modelConfig = await _context.AiModelConfigs.FindAsync(id);
            if (modelConfig == null)
            {
                return NotFound(new { code = 1, msg = "模型配置不存在" });
            }
            
            // Set other models of the same type to non-default
            var otherModels = await _context.AiModelConfigs
                .Where(m => m.ModelType == modelConfig.ModelType && m.Id != id)
                .ToListAsync();
                
            foreach (var otherModel in otherModels)
            {
                otherModel.IsDefault = 0;
                _context.Entry(otherModel).State = EntityState.Modified;
            }
            
            // Set this model as default and enabled
            modelConfig.IsDefault = 1;
            modelConfig.IsEnabled = 1;
            modelConfig.UpdateDate = DateTime.UtcNow;
            
            _context.Entry(modelConfig).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return Ok(new { code = 0 });
        }

        // GET: xiaozhi/models/{modelId}/voices
        [HttpGet("{modelId}/voices")]
        public async Task<ActionResult<IEnumerable<object>>> GetVoiceList(string modelId, [FromQuery] string voiceName = null)
        {
            var query = _context.AiTtsVoices.AsQueryable();
            
            // Filter by model ID
            query = query.Where(v => v.TtsModelId == modelId);
            
            if (!string.IsNullOrEmpty(voiceName))
            {
                query = query.Where(v => v.Name.Contains(voiceName));
            }
            
            var voices = await query
                .Select(v => new { v.Id, v.Name, v.TtsVoice })
                .ToListAsync();
                
            return Ok(new { code = 0, data = voices });
        }

        private bool ModelConfigExists(string id)
        {
            return _context.AiModelConfigs.Any(e => e.Id == id);
        }
    }
}