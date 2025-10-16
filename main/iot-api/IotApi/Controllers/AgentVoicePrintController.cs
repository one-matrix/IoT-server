using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IotApi.Models;

namespace IotApi.Controllers
{
    [ApiController]
    [Route("xiaozhi/agent/voice-print")]
    public class AgentVoicePrintController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AgentVoicePrintController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: xiaozhi/agent/voice-print
        [HttpPost]
        public async Task<ActionResult<object>> Save(AgentVoicePrintSaveDto dto)
        {
            // In a real implementation, we would check if the voiceprint API is configured
            // For now, we'll just create the voiceprint record

            var voiceprint = new AiVoiceprint
            {
                Id = string.IsNullOrEmpty(dto.Id) ? Guid.NewGuid().ToString("N").Substring(0, 32) : dto.Id,
                Name = dto.Name,
                UserId = dto.UserId,
                AgentId = dto.AgentId,
                AgentCode = dto.AgentCode,
                AgentName = dto.AgentName,
                Description = dto.Description,
                Embedding = dto.Embedding,
                Memory = dto.Memory,
                Sort = dto.Sort,
                Creator = dto.Creator,
                CreatedAt = DateTime.UtcNow,
                Updater = dto.Updater,
                UpdatedAt = DateTime.UtcNow
            };

            _context.AiVoiceprints.Add(voiceprint);
            await _context.SaveChangesAsync();

            return Ok(new { code = 0 });
        }

        // PUT: xiaozhi/agent/voice-print
        [HttpPut]
        public async Task<ActionResult<object>> Update(AgentVoicePrintUpdateDto dto)
        {
            var voiceprint = await _context.AiVoiceprints.FindAsync(dto.Id);
            if (voiceprint == null)
            {
                return NotFound(new { code = 1, msg = "声纹不存在" });
            }

            // Update properties
            voiceprint.Name = dto.Name;
            voiceprint.AgentId = dto.AgentId;
            voiceprint.AgentCode = dto.AgentCode;
            voiceprint.AgentName = dto.AgentName;
            voiceprint.Description = dto.Description;
            voiceprint.Embedding = dto.Embedding;
            voiceprint.Memory = dto.Memory;
            voiceprint.Sort = dto.Sort;
            voiceprint.Updater = dto.Updater;
            voiceprint.UpdatedAt = DateTime.UtcNow;

            _context.Entry(voiceprint).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { code = 0 });
        }

        // DELETE: xiaozhi/agent/voice-print/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> Delete(string id)
        {
            var voiceprint = await _context.AiVoiceprints.FindAsync(id);
            if (voiceprint == null)
            {
                return NotFound(new { code = 1, msg = "声纹不存在" });
            }

            _context.AiVoiceprints.Remove(voiceprint);
            await _context.SaveChangesAsync();

            return Ok(new { code = 0 });
        }

        // GET: xiaozhi/agent/voice-print/list/{id}
        [HttpGet("list/{id}")]
        public async Task<ActionResult<object>> List(string id)
        {
            // In a real implementation, we would check if the voiceprint API is configured
            // For now, we'll just return the voiceprint records for the agent

            var voiceprints = await _context.AiVoiceprints
                .Where(v => v.AgentId == id)
                .ToListAsync();

            return Ok(new { code = 0, data = voiceprints });
        }
    }

    // DTOs for the controller
    public class AgentVoicePrintSaveDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long? UserId { get; set; }
        public string AgentId { get; set; }
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public string Description { get; set; }
        public string Embedding { get; set; }
        public string Memory { get; set; }
        public int? Sort { get; set; }
        public long? Creator { get; set; }
        public long? Updater { get; set; }
    }

    public class AgentVoicePrintUpdateDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AgentId { get; set; }
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public string Description { get; set; }
        public string Embedding { get; set; }
        public string Memory { get; set; }
        public int? Sort { get; set; }
        public long? Updater { get; set; }
    }
}