using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IotApi.Models;

namespace IotApi.Controllers
{
    [ApiController]
    [Route("xiaozhi/models/provider")]
    public class ModelProviderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ModelProviderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: xiaozhi/models/provider
        [HttpGet]
        public async Task<ActionResult<object>> GetListPage(
            [FromQuery] string modelType = null,
            [FromQuery] string providerCode = null,
            [FromQuery] string name = null,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            var query = _context.AiModelProviders.AsQueryable();

            if (!string.IsNullOrEmpty(modelType))
            {
                query = query.Where(p => p.ModelType == modelType);
            }

            if (!string.IsNullOrEmpty(providerCode))
            {
                query = query.Where(p => p.ProviderCode.Contains(providerCode));
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            var total = await query.CountAsync();
            var providers = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            var pageData = new
            {
                list = providers,
                total = total
            };

            return Ok(new { code = 0, data = pageData });
        }

        // POST: xiaozhi/models/provider
        [HttpPost]
        public async Task<ActionResult<object>> Add(AiModelProvider provider)
        {
            // Generate ID if not provided
            if (string.IsNullOrEmpty(provider.Id))
            {
                provider.Id = Guid.NewGuid().ToString("N").Substring(0, 32);
            }

            provider.CreateDate = DateTime.UtcNow;
            provider.UpdateDate = DateTime.UtcNow;

            _context.AiModelProviders.Add(provider);
            await _context.SaveChangesAsync();

            return Ok(new { code = 0, data = provider });
        }

        // PUT: xiaozhi/models/provider
        [HttpPut]
        public async Task<ActionResult<object>> Edit(AiModelProvider provider)
        {
            provider.UpdateDate = DateTime.UtcNow;

            _context.Entry(provider).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelProviderExists(provider.Id))
                {
                    return NotFound(new { code = 1, msg = "模型供应器不存在" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { code = 0, data = provider });
        }

        // POST: xiaozhi/models/provider/delete
        [HttpPost("delete")]
        public async Task<ActionResult<object>> Delete([FromBody] List<string> ids)
        {
            var providers = await _context.AiModelProviders
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            _context.AiModelProviders.RemoveRange(providers);
            await _context.SaveChangesAsync();

            return Ok(new { code = 0 });
        }

        // GET: xiaozhi/models/provider/plugin/names
        [HttpGet("plugin/names")]
        public async Task<ActionResult<object>> GetPluginNameList()
        {
            // In a real implementation, this would return a list of plugin names
            // For now, we'll return all model providers as a placeholder
            var providers = await _context.AiModelProviders.ToListAsync();
            
            var pluginList = providers.Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                ProviderCode = p.ProviderCode
            }).ToList();

            return Ok(new { code = 0, data = pluginList });
        }

        private bool ModelProviderExists(string id)
        {
            return _context.AiModelProviders.Any(e => e.Id == id);
        }
    }
}