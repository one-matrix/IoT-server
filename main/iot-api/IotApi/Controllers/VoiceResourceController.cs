using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IotApi.Models;

namespace IotApi.Controllers
{
    [ApiController]
    [Route("voiceResource")]
    public class VoiceResourceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VoiceResourceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: voiceResource
        [HttpGet]
        public async Task<IActionResult> Page([FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            // In a real implementation, we would:
            // 1. Get paginated list of voice resources
            // 2. Return the page data
            
            var voiceClones = await _context.AiVoiceClones
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
                
            var totalCount = await _context.AiVoiceClones.CountAsync();
            
            return Ok(new { 
                code = 0, 
                data = new { 
                    list = voiceClones, 
                    total = totalCount,
                    page = page,
                    limit = limit
                }, 
                msg = "Success" 
            });
        }

        // GET: voiceResource/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var voiceClone = await _context.AiVoiceClones.FindAsync(id);
            if (voiceClone == null)
            {
                return NotFound(new { code = 1, msg = "Voice resource not found" });
            }
            
            return Ok(new { code = 0, data = voiceClone, msg = "Success" });
        }

        // POST: voiceResource
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] VoiceCloneDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(new { code = 1, msg = "Voice resource information cannot be empty" });
            }
            
            if (string.IsNullOrWhiteSpace(dto.ModelId))
            {
                return BadRequest(new { code = 1, msg = "Model ID cannot be empty" });
            }
            
            if (dto.VoiceIds == null || dto.VoiceIds.Length == 0)
            {
                return BadRequest(new { code = 1, msg = "Voice IDs cannot be empty" });
            }
            
            if (dto.UserId == null)
            {
                return BadRequest(new { code = 1, msg = "User ID cannot be empty" });
            }
            
            try
            {
                // In a real implementation, we would:
                // 1. Create voice clone records
                // 2. Save to database
                
                return Ok(new { code = 0, msg = "Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = "Save failed: " + ex.Message });
            }
        }

        // DELETE: voiceResource/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return BadRequest(new { code = 1, msg = "Voice resource IDs to delete cannot be empty" });
            }
            
            var voiceClones = await _context.AiVoiceClones
                .Where(vc => ids.Contains(vc.Id))
                .ToListAsync();
                
            if (voiceClones.Any())
            {
                _context.AiVoiceClones.RemoveRange(voiceClones);
                await _context.SaveChangesAsync();
            }
            
            return Ok(new { code = 0, msg = "Success" });
        }

        // GET: voiceResource/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(long userId)
        {
            var voiceClones = await _context.AiVoiceClones
                .Where(vc => vc.UserId == userId)
                .ToListAsync();
                
            return Ok(new { code = 0, data = voiceClones, msg = "Success" });
        }

        // GET: voiceResource/ttsPlatforms
        [HttpGet("ttsPlatforms")]
        public async Task<IActionResult> GetTtsPlatformList()
        {
            // In a real implementation, we would:
            // 1. Get TTS platform list from model configurations
            // 2. Return the list
            
            // For now, we'll just return a placeholder response
            var platforms = new List<object>
            {
                new { id = "1", name = "Microsoft Edge TTS" },
                new { id = "2", name = "Aliyun TTS" },
                new { id = "3", name = "OpenAI TTS" }
            };
            
            return Ok(new { code = 0, data = platforms, msg = "Success" });
        }
    }

    // DTOs for the controller
    public class VoiceCloneDTO
    {
        public string ModelId { get; set; }
        public string[] VoiceIds { get; set; }
        public long? UserId { get; set; }
    }
}