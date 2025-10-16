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
        private readonly IAgentService _agentService;

        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
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
            // In a real implementation, we would:
            // 1. Get device by MAC address
            // 2. Update agent memory
            // For now, we'll just return a success response
            return Ok(new { code = 0 });
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
            // In a real implementation, we would:
            // 1. Get audio content by ID
            // For now, we'll return a placeholder response
            return Ok(new { code = 0, data = "" });
        }

        // POST: xiaozhi/agent/audio/{audioId}
        [HttpPost("audio/{audioId}")]
        public async Task<IActionResult> GetAudioId(string audioId)
        {
            // In a real implementation, we would:
            // 1. Get audio data
            // 2. Generate and store UUID in Redis
            // 3. Return the UUID
            var uuid = Guid.NewGuid().ToString();
            return Ok(new { code = 0, data = uuid });
        }

        // GET: xiaozhi/agent/play/{uuid}
        [HttpGet("play/{uuid}")]
        public async Task<IActionResult> PlayAudio(string uuid)
        {
            // In a real implementation, we would:
            // 1. Get audio ID from Redis using uuid
            // 2. Retrieve audio data
            // 3. Return audio file for playback
            return Ok(new { code = 0, msg = "Play audio" });
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