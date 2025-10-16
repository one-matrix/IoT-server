using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IotApi.Models;

namespace IotApi.Controllers
{
    [ApiController]
    [Route("otaMag")]
    public class OTAMagController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OTAMagController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: otaMag
        [HttpGet]
        public async Task<IActionResult> Page([FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            // In a real implementation, we would:
            // 1. Get paginated list of OTA firmware
            // 2. Return the page data
            
            // For now, we'll just return a placeholder response
            var otas = await _context.AiOtas
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
                
            var totalCount = await _context.AiOtas.CountAsync();
            
            return Ok(new { 
                code = 0, 
                data = new { 
                    list = otas, 
                    total = totalCount,
                    page = page,
                    limit = limit
                }, 
                msg = "Success" 
            });
        }

        // GET: otaMag/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var ota = await _context.AiOtas.FindAsync(id);
            if (ota == null)
            {
                return NotFound(new { code = 1, msg = "OTA firmware not found" });
            }
            
            return Ok(new { code = 0, data = ota, msg = "Success" });
        }

        // POST: otaMag
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AiOta entity)
        {
            if (entity == null)
            {
                return BadRequest(new { code = 1, msg = "Firmware information cannot be empty" });
            }
            
            if (string.IsNullOrWhiteSpace(entity.FirmwareName))
            {
                return BadRequest(new { code = 1, msg = "Firmware name cannot be empty" });
            }
            
            if (string.IsNullOrWhiteSpace(entity.Type))
            {
                return BadRequest(new { code = 1, msg = "Firmware type cannot be empty" });
            }
            
            if (string.IsNullOrWhiteSpace(entity.Version))
            {
                return BadRequest(new { code = 1, msg = "Version cannot be empty" });
            }
            
            try
            {
                entity.Id = Guid.NewGuid().ToString("N");
                entity.CreateDate = DateTime.UtcNow;
                entity.UpdateDate = DateTime.UtcNow;
                
                _context.AiOtas.Add(entity);
                await _context.SaveChangesAsync();
                
                return Ok(new { code = 0, msg = "Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = ex.Message });
            }
        }

        // DELETE: otaMag/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return BadRequest(new { code = 1, msg = "Firmware IDs to delete cannot be empty" });
            }
            
            var otas = await _context.AiOtas
                .Where(o => ids.Contains(o.Id))
                .ToListAsync();
                
            if (otas.Any())
            {
                _context.AiOtas.RemoveRange(otas);
                await _context.SaveChangesAsync();
            }
            
            return Ok(new { code = 0, msg = "Success" });
        }

        // PUT: otaMag/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] AiOta entity)
        {
            if (entity == null)
            {
                return BadRequest(new { code = 1, msg = "Firmware information cannot be empty" });
            }
            
            var existingOta = await _context.AiOtas.FindAsync(id);
            if (existingOta == null)
            {
                return NotFound(new { code = 1, msg = "OTA firmware not found" });
            }
            
            try
            {
                // Update properties
                existingOta.FirmwareName = entity.FirmwareName ?? existingOta.FirmwareName;
                existingOta.Type = entity.Type ?? existingOta.Type;
                existingOta.Version = entity.Version ?? existingOta.Version;
                existingOta.Size = entity.Size ?? existingOta.Size;
                existingOta.Remark = entity.Remark ?? existingOta.Remark;
                existingOta.FirmwarePath = entity.FirmwarePath ?? existingOta.FirmwarePath;
                existingOta.Sort = entity.Sort ?? existingOta.Sort;
                existingOta.Updater = entity.Updater ?? existingOta.Updater;
                existingOta.UpdateDate = DateTime.UtcNow;
                
                _context.Entry(existingOta).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                return Ok(new { code = 0, msg = "Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = ex.Message });
            }
        }

        // GET: otaMag/getDownloadUrl/{id}
        [HttpGet("getDownloadUrl/{id}")]
        public async Task<IActionResult> GetDownloadUrl(string id)
        {
            // In a real implementation, we would:
            // 1. Generate UUID
            // 2. Store ID in Redis with download count tracking
            // 3. Return the UUID as download identifier
            
            // For now, we'll just return a placeholder response
            var uuid = Guid.NewGuid().ToString();
            return Ok(new { code = 0, data = uuid, msg = "Success" });
        }

        // GET: otaMag/download/{uuid}
        [HttpGet("download/{uuid}")]
        public async Task<IActionResult> DownloadFirmware(string uuid)
        {
            // In a real implementation, we would:
            // 1. Get firmware ID from Redis using uuid
            // 2. Check download count
            // 3. Retrieve firmware file
            // 4. Return file for download
            
            // For now, we'll just return a placeholder response
            return Ok(new { code = 0, msg = "Download firmware" });
        }

        // POST: otaMag/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFirmware()
        {
            // In a real implementation, we would:
            // 1. Handle file upload
            // 2. Validate file type and size
            // 3. Calculate MD5 hash
            // 4. Save file to storage
            // 5. Return file path
            
            // For now, we'll just return a placeholder response
            return Ok(new { code = 0, data = "uploadfile/firmware.bin", msg = "Success" });
        }
    }
}