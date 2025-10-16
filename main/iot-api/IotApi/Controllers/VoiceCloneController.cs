using Microsoft.AspNetCore.Mvc;
using IotApi.Services;
using IotApi.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IotApi.Controllers
{
    /// <summary>
    /// 音色资源管理（声音克隆）
    /// </summary>
    [ApiController]
    [Route("voiceClone")]
    [Produces("application/json")]
    [Tags("音色资源管理")]
    public class VoiceCloneController : ControllerBase
    {
        private readonly IVoiceCloneService _voiceCloneService;
        private readonly ILogger<VoiceCloneController> _logger;

        public VoiceCloneController(IVoiceCloneService voiceCloneService, ILogger<VoiceCloneController> logger)
        {
            _voiceCloneService = voiceCloneService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询声音克隆列表
        /// </summary>
        /// <param name="query">查询参数</param>
        [HttpGet]
        public async Task<IActionResult> Page([FromQuery] VoiceCloneQueryDto query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _voiceCloneService.GetPageAsync(query);
            return Ok(new { code = 0, data = result });
        }

        /// <summary>
        /// 上传音频文件
        /// </summary>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadVoice(UploadVoiceRequest input)
        {
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                return BadRequest(new { code = 400, msg = "声音克隆ID不能为空" });
            }
            if (input.VoiceFile == null || input.VoiceFile.Length == 0)
            {
                return BadRequest(new { code = 400, msg = "音频文件不能为空" });
            }
            if (!input.VoiceFile.ContentType.StartsWith("audio/"))
            {
                return BadRequest(new { code = 400, msg = "文件必须是音频类型" });
            }
            if (input.VoiceFile.Length > 10 * 1024 * 1024)
            {
                return BadRequest(new { code = 400, msg = "音频文件过大（最大10MB）" });
            }

            try
            {
                await _voiceCloneService.UploadVoiceAsync(input.Id, input.VoiceFile);
                return Ok(new { code = 0 });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { code = 404, msg = "声音克隆记录不存在" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "上传音频失败");
                return BadRequest(new { code = 500, msg = "上传失败: " + ex.Message });
            }
        }

        /// <summary>
        /// 更新声音名称
        /// </summary>
        [HttpPost("updateName")]
        public async Task<IActionResult> UpdateName([FromBody] UpdateNameRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
            {
                return BadRequest(new { code = 400, msg = "ID不能为空" });
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new { code = 400, msg = "名称不能为空" });
            }

            try
            {
                await _voiceCloneService.UpdateNameAsync(request.Id, request.Name);
                return Ok(new { code = 0 });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { code = 404, msg = "声音克隆记录不存在" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新声音名称失败");
                return BadRequest(new { code = 500, msg = "更新失败: " + ex.Message });
            }
        }

        /// <summary>
        /// 生成音频播放UUID
        /// </summary>
        [HttpPost("audio/{id}")]
        public async Task<IActionResult> GetAudioId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new { code = 400, msg = "ID不能为空" });
            }

            var uuid = await _voiceCloneService.GenerateAudioUuidAsync(id);
            return Ok(new { code = 0, data = uuid });
        }

        /// <summary>
        /// 根据UUID播放音频
        /// </summary>
        [HttpGet("play/{uuid}")]
        public async Task<IActionResult> PlayVoice(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
            {
                return BadRequest(new { code = 400, msg = "UUID不能为空" });
            }

            var bytes = await _voiceCloneService.GetVoiceDataByUuidAsync(uuid);
            if (bytes == null || bytes.Length == 0)
            {
                return NotFound(new { code = 404, msg = "音频数据不存在或已过期" });
            }
            return File(bytes, "audio/mpeg");
        }

        /// <summary>
        /// 触发声音克隆训练
        /// </summary>
        [HttpPost("cloneAudio")]
        public async Task<IActionResult> CloneAudio([FromBody] CloneAudioRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CloneId))
            {
                return BadRequest(new { code = 400, msg = "克隆ID不能为空" });
            }
            try
            {
                await _voiceCloneService.StartCloneAsync(request.CloneId);
                return Ok(new { code = 0 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "启动声音克隆失败");
                return BadRequest(new { code = 500, msg = "启动失败: " + ex.Message });
            }
        }
    }

    // DTOs for the controller
    public class UpdateNameRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    
    public class CloneAudioRequest
    {
        public string CloneId { get; set; }
    }
    public class UploadVoiceRequest
    {
        public string Id { get; set; }
        public IFormFile VoiceFile { get; set; }
    }
}