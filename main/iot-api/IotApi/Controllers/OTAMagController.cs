using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IotApi.Models;
using IotApi.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
        public async Task<IActionResult> GetDownloadUrl(string id, [FromServices] IRedisService redisService)
        {
            var ota = await _context.AiOtas.FindAsync(id);
            if (ota == null)
            {
                return NotFound(new { code = 1, msg = "OTA firmware not found" });
            }
            
            // 生成UUID作为下载标识
            var uuid = Guid.NewGuid().ToString("N");
            
            // 将固件ID存储在Redis中，设置过期时间为1小时
            var downloadInfo = new
            {
                OtaId = id,
                DownloadCount = 0,
                MaxDownloadCount = 5 // 最大下载次数限制
            };
            
            await redisService.SetAsync($"ota:download:{uuid}", downloadInfo, 3600); // 1小时过期
            
            return Ok(new { code = 0, data = uuid, msg = "Success" });
        }

        // GET: otaMag/download/{uuid}
        [HttpGet("download/{uuid}")]
        public async Task<IActionResult> DownloadFirmware(string uuid, [FromServices] IRedisService redisService)
        {
            // 从Redis获取下载信息
            var downloadInfo = await redisService.GetAsync<dynamic>($"ota:download:{uuid}");
            if (downloadInfo == null)
            {
                return NotFound(new { code = 1, msg = "Download link expired or invalid" });
            }
            
            // 检查下载次数
            if (downloadInfo.DownloadCount >= downloadInfo.MaxDownloadCount)
            {
                return BadRequest(new { code = 1, msg = "Download count exceeded" });
            }
            
            // 获取固件信息
            var ota = await _context.AiOtas.FindAsync(downloadInfo.OtaId.ToString());
            if (ota == null)
            {
                return NotFound(new { code = 1, msg = "Firmware not found" });
            }
            
            // 检查固件文件路径
            if (string.IsNullOrEmpty(ota.FirmwarePath))
            {
                return NotFound(new { code = 1, msg = "Firmware file path not found" });
            }
            
            // 构建文件路径
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ota.FirmwarePath);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { code = 1, msg = "Firmware file not found" });
            }
            
            // 更新下载次数
            downloadInfo.DownloadCount++;
            await redisService.SetAsync($"ota:download:{uuid}", downloadInfo, 3600);
            
            // 返回文件
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/octet-stream", Path.GetFileName(filePath));
        }

        // POST: otaMag/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFirmware(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { code = 1, msg = "No file uploaded" });
            }
            
            // 验证文件类型和大小
            var allowedExtensions = new[] { ".bin", ".elf", ".hex" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new { code = 1, msg = "Invalid file type. Only .bin, .elf, and .hex files are allowed." });
            }
            
            // 限制文件大小为10MB
            if (file.Length > 10 * 1024 * 1024)
            {
                return BadRequest(new { code = 1, msg = "File size exceeds the limit (10MB)" });
            }
            
            try
            {
                // 创建上传目录
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadfile");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }
                
                // 生成唯一文件名
                var fileName = $"{Guid.NewGuid():N}{fileExtension}";
                var filePath = Path.Combine(uploadDir, fileName);
                
                // 保存文件
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                
                // 计算MD5哈希值
                string md5Hash;
                using (var md5 = System.Security.Cryptography.MD5.Create())
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] hashBytes = md5.ComputeHash(fileStream);
                    md5Hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
                
                // 返回相对路径
                var relativePath = $"uploadfile/{fileName}";
                
                return Ok(new { 
                    code = 0, 
                    data = relativePath, 
                    md5 = md5Hash,
                    size = file.Length,
                    msg = "Success" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 1, msg = $"Upload failed: {ex.Message}" });
            }
        }
    }
}