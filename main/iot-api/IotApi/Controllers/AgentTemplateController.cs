using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IotApi.Models;

namespace IotApi.Controllers
{
    [ApiController]
    [Route("xiaozhi/agent/template")]
    public class AgentTemplateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AgentTemplateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: xiaozhi/agent/template/page
        [HttpGet("page")]
        public async Task<ActionResult<object>> GetAgentTemplatesPage(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string agentName = null)
        {
            var query = _context.AiAgentTemplates.AsQueryable();

            if (!string.IsNullOrEmpty(agentName))
            {
                query = query.Where(t => t.AgentName.Contains(agentName));
            }

            // Order by sort field
            query = query.OrderBy(t => t.Sort);

            var total = await query.CountAsync();
            var templates = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            var pageData = new
            {
                list = templates,
                total = total
            };

            return Ok(new { code = 0, data = pageData });
        }

        // GET: xiaozhi/agent/template/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetAgentTemplateById(string id)
        {
            var template = await _context.AiAgentTemplates.FindAsync(id);
            if (template == null)
            {
                return NotFound(new { code = 1, msg = "模板不存在" });
            }

            return Ok(new { code = 0, data = template });
        }

        // POST: xiaozhi/agent/template
        [HttpPost]
        public async Task<ActionResult<object>> CreateAgentTemplate(AiAgentTemplate template)
        {
            // Generate ID if not provided
            if (string.IsNullOrEmpty(template.Id))
            {
                template.Id = Guid.NewGuid().ToString("N").Substring(0, 32);
            }

            // Set sort value to next available sort number
            template.Sort = await GetNextAvailableSort();
            template.CreatedAt = DateTime.UtcNow;
            template.UpdatedAt = DateTime.UtcNow;

            _context.AiAgentTemplates.Add(template);
            await _context.SaveChangesAsync();

            return Ok(new { code = 0, data = template });
        }

        // PUT: xiaozhi/agent/template
        [HttpPut]
        public async Task<ActionResult<object>> UpdateAgentTemplate(AiAgentTemplate template)
        {
            template.UpdatedAt = DateTime.UtcNow;

            _context.Entry(template).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentTemplateExists(template.Id))
                {
                    return NotFound(new { code = 1, msg = "模板不存在" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { code = 0, data = template });
        }

        // DELETE: xiaozhi/agent/template/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteAgentTemplate(string id)
        {
            var template = await _context.AiAgentTemplates.FindAsync(id);
            if (template == null)
            {
                return NotFound(new { code = 1, msg = "模板不存在" });
            }

            int? deletedSort = template.Sort;

            _context.AiAgentTemplates.Remove(template);
            await _context.SaveChangesAsync();

            // Reorder remaining templates after delete
            await ReorderTemplatesAfterDelete(deletedSort);

            return Ok(new { code = 0, msg = "删除模板成功" });
        }

        // POST: xiaozhi/agent/template/batch-remove
        [HttpPost("batch-remove")]
        public async Task<ActionResult<object>> BatchRemoveAgentTemplates([FromBody] List<string> ids)
        {
            var templates = await _context.AiAgentTemplates
                .Where(t => ids.Contains(t.Id))
                .ToListAsync();

            _context.AiAgentTemplates.RemoveRange(templates);
            await _context.SaveChangesAsync();

            return Ok(new { code = 0, msg = "批量删除成功" });
        }

        private async Task<int?> GetNextAvailableSort()
        {
            var maxSort = await _context.AiAgentTemplates
                .MaxAsync(t => (int?)t.Sort);

            return (maxSort ?? 0) + 1;
        }

        private async Task ReorderTemplatesAfterDelete(int? deletedSort)
        {
            if (deletedSort.HasValue)
            {
                var templatesToReorder = await _context.AiAgentTemplates
                    .Where(t => t.Sort > deletedSort)
                    .ToListAsync();

                foreach (var template in templatesToReorder)
                {
                    template.Sort -= 1;
                    _context.Entry(template).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
            }
        }

        private bool AgentTemplateExists(string id)
        {
            return _context.AiAgentTemplates.Any(e => e.Id == id);
        }
    }
}