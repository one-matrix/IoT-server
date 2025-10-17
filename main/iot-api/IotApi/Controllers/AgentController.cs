using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IotApi.Models;
using IotApi.Services;
using IotApi.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IotApi.Controllers
{
    /// <summary>
    /// 智能体管理
    /// </summary>
    [ApiController]
    [Route("xiaozhi/[controller]")]
    [Produces("application/json")]
    [Tags("智能体管理")]
    public class AgentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAgentService _agentService;
        private readonly IAgentChatAudioService _agentChatAudioService;
        private readonly IRedisService _redisService;
        private readonly ILogger<AgentController> _logger;

        public AgentController(
            ApplicationDbContext context, 
            IAgentService agentService,
            IAgentChatAudioService agentChatAudioService,
            IRedisService redisService,
            ILogger<AgentController> logger)
        {
            _context = context;
            _agentService = agentService;
            _agentChatAudioService = agentChatAudioService;
            _redisService = redisService;
            _logger = logger;
        }

        /// <summary>
        /// 获取用户智能体列表
        /// </summary>
        /// <returns>智能体列表</returns>
        [HttpGet("list")]
        public async Task<ActionResult<object>> GetUserAgents()
        {
            var userId = GetCurrentUserId();
            var agents = await _agentService.GetUserAgentsAsync(userId);
            return Ok(new { code = 0, data = agents });
        }

        /// <summary>
        /// 管理员获取所有智能体列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数量</param>
        /// <returns>智能体列表</returns>
        [HttpGet("all")]
        public async Task<ActionResult<object>> AdminAgentList([FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            var result = await _agentService.GetAllAgentsAsync(page, limit);
            return Ok(new { code = 0, data = result.Items, total = result.Total });
        }

        /// <summary>
        /// 根据ID获取智能体
        /// </summary>
        /// <param name="id">智能体ID</param>
        /// <returns>智能体信息</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetAgentById(string id)
        {
            var agent = await _agentService.GetAgentByIdAsync(id);
            
            if (agent == null)
            {
                return NotFound(new { code = 1, msg = "智能体不存在" });
            }
            
            return Ok(new { code = 0, data = agent });
        }
        
        /// <summary>
        /// 创建智能体
        /// </summary>
        /// <param name="dto">智能体信息</param>
        /// <returns>创建结果</returns>
        [HttpPost]
        public async Task<ActionResult<object>> CreateAgent([FromBody] AgentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { code = 1, msg = "参数错误", data = ModelState });
            }
            
            var userId = GetCurrentUserId();
            var result = await _agentService.CreateAgentAsync(dto, userId);
            
            return Ok(new { code = 0, msg = "创建成功", data = result });
        }
        
        /// <summary>
        /// 更新智能体
        /// </summary>
        /// <param name="id">智能体ID</param>
        /// <param name="dto">智能体信息</param>
        /// <returns>更新结果</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> UpdateAgent(string id, [FromBody] AgentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { code = 1, msg = "参数错误", data = ModelState });
            }
            
            var userId = GetCurrentUserId();
            var result = await _agentService.UpdateAgentAsync(id, dto, userId);
            
            if (result == null)
            {
                return NotFound(new { code = 1, msg = "智能体不存在或无权限修改" });
            }
            
            return Ok(new { code = 0, msg = "更新成功", data = result });
        }
        
        /// <summary>
        /// 删除智能体
        /// </summary>
        /// <param name="id">智能体ID</param>
        /// <returns>删除结果</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteAgent(string id)
        {
            var userId = GetCurrentUserId();
            var success = await _agentService.DeleteAgentAsync(id, userId);
            
            if (!success)
            {
                return NotFound(new { code = 1, msg = "智能体不存在或无权限删除" });
            }
            
            return Ok(new { code = 0, msg = "删除成功" });
        }
        
        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns>用户ID</returns>
        private string GetCurrentUserId()
        {
            // 在实际实现中，应该从JWT令牌或会话中获取用户ID
            // 这里简单返回一个默认值
            return "";
        }

     

        // PUT: xiaozhi/agent/saveMemory/{macAddress}
        [HttpPut("saveMemory/{macAddress}")]
        public async Task<IActionResult> UpdateByDeviceId(string macAddress, AgentMemoryDto dto)
        {
            try
            {
                // 根据MAC地址获取设备
                var device = await _context.Devices.FirstOrDefaultAsync(d => d.MacAddress == macAddress);
                if (device == null)
                {
                    return NotFound(new { code = 404, msg = "设备不存在" });
                }

                // 更新智能体记忆
                var agent = await _context.AiAgents.FirstOrDefaultAsync(a => a.Id == device.AgentId);
                if (agent == null)
                {
                    return NotFound(new { code = 404, msg = "智能体不存在" });
                }

                // 更新记忆
                agent.SummaryMemory = dto.SummaryMemory;
                _context.AiAgents.Update(agent);
                await _context.SaveChangesAsync();

                return Ok(new { code = 0 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新智能体记忆失败，macAddress={MacAddress}", macAddress);
                return StatusCode(500, new { code = 500, msg = "更新智能体记忆失败" });
            }
        }

        // GET: xiaozhi/agent/template
        [HttpGet("template")]
        public async Task<IActionResult> TemplateList()
        {
            // In a real implementation, we would return agent templates
            // For now, we'll return a placeholder response
            var templates = await _context.AiAgentTemplates.ToListAsync();
            return Ok(new { code = 0, data = templates });
        }

        // GET: xiaozhi/agent/{id}/sessions
        [HttpGet("{id}/sessions")]
        public async Task<IActionResult> GetAgentSessions(string id, [FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            // In a real implementation, we would:
            // 1. Check user permissions
            // 2. Get agent sessions with pagination
            // For now, we'll return a placeholder response
            return Ok(new { code = 0, data = new { list = new object[0], total = 0 } });
        }

        // GET: xiaozhi/agent/{id}/chat-history/{sessionId}
        [HttpGet("{id}/chat-history/{sessionId}")]
        public async Task<IActionResult> GetAgentChatHistory(string id, string sessionId)
        {
            // In a real implementation, we would:
            // 1. Check user permissions
            // 2. Get chat history for the session
            // For now, we'll return a placeholder response
            return Ok(new { code = 0, data = new object[0] });
        }

        // GET: xiaozhi/agent/{id}/chat-history/user
        [HttpGet("{id}/chat-history/user")]
        public async Task<IActionResult> GetRecentlyFiftyByAgentId(string id)
        {
            // In a real implementation, we would:
            // 1. Check user permissions
            // 2. Get recent chat history for the agent
            // For now, we'll return a placeholder response
            return Ok(new { code = 0, data = new object[0] });
        }

        // GET: xiaozhi/agent/{id}/chat-history/audio
        [HttpGet("{id}/chat-history/audio")]
        public async Task<IActionResult> GetContentByAudioId(string id)
        {
            try
            {
                // 获取音频内容
                var audioData = await _agentChatAudioService.GetAudioAsync(id);
                if (audioData == null)
                {
                    return NotFound(new { code = 404, msg = "音频不存在" });
                }
                
                // 将音频数据转换为Base64字符串
                string base64Audio = Convert.ToBase64String(audioData);
                return Ok(new { code = 0, data = base64Audio });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取音频内容失败，audioId={AudioId}", id);
                return StatusCode(500, new { code = 500, msg = "获取音频内容失败" });
            }
        }

        // POST: xiaozhi/agent/audio/{audioId}
        [HttpPost("audio/{audioId}")]
        public async Task<IActionResult> GetAudioId(string audioId)
        {
            try
            {
                // 获取音频数据
                var audioData = await _agentChatAudioService.GetAudioAsync(audioId);
                if (audioData == null)
                {
                    return NotFound(new { code = 404, msg = "音频不存在" });
                }
                
                // 生成UUID并存储在Redis中
                var uuid = Guid.NewGuid().ToString();
                await _redisService.SetStringAsync($"agent:audio:{uuid}", audioId, TimeSpan.FromMinutes(30));
                
                return Ok(new { code = 0, data = uuid });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取音频ID失败，audioId={AudioId}", audioId);
                return StatusCode(500, new { code = 500, msg = "获取音频ID失败" });
            }
        }

        // GET: xiaozhi/agent/play/{uuid}
        [HttpGet("play/{uuid}")]
        public async Task<IActionResult> PlayAudio(string uuid)
        {
            try
            {
                // 从Redis获取音频ID
                var audioId = await _redisService.GetStringAsync($"agent:audio:{uuid}");
                if (string.IsNullOrEmpty(audioId))
                {
                    return NotFound(new { code = 404, msg = "音频链接不存在或已过期" });
                }

                // 获取音频数据
                var audioData = await _agentChatAudioService.GetAudioAsync(audioId);
                if (audioData == null)
                {
                    return NotFound(new { code = 404, msg = "音频不存在" });
                }

                // 删除Redis中的记录，确保链接只能使用一次
                await _redisService.RemoveAsync($"agent:audio:{uuid}");

                // 返回音频文件
                return File(audioData, "audio/wav", "play.wav");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "播放音频失败，uuid={Uuid}", uuid);
                return StatusCode(500, new { code = 500, msg = "播放音频失败" });
            }
        }

        private bool AgentExists(string id)
        {
            return _context.AiAgents.Any(e => e.Id == id);
        }
    }

    // DTOs for the controller
    public class AgentMemoryDto
    {
        public string SummaryMemory { get; set; }
    }
}