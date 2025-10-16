using Microsoft.AspNetCore.Mvc;
using IotApi.Services;
using IotApi.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IotApi.Controllers
{
    /// <summary>
    /// 音色管理
    /// </summary>
    [ApiController]
    [Route("ttsVoice")]
    [Produces("application/json")]
    [Tags("音色管理")]
    public class TimbreController : ControllerBase
    {
        private readonly ITimbreService _timbreService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="timbreService">音色服务</param>
        public TimbreController(ITimbreService timbreService)
        {
            _timbreService = timbreService;
        }

        /// <summary>
        /// 获取音色分页列表
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <returns>分页结果</returns>
        [HttpGet]
        public async Task<IActionResult> Page([FromQuery] TimbreQueryDto query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _timbreService.GetPageAsync(query);
            return Ok(new { code = 0, data = result });
        }

        /// <summary>
        /// 新增音色
        /// </summary>
        /// <param name="dto">音色数据</param>
        /// <returns>新增结果</returns>
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] TimbreDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _timbreService.AddAsync(dto);
            return Ok(new { code = 0, data = id });
        }

        /// <summary>
        /// 更新音色
        /// </summary>
        /// <param name="id">音色ID</param>
        /// <param name="dto">音色数据</param>
        /// <returns>更新结果</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TimbreDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dto.Id = id;
            try
            {
                await _timbreService.UpdateAsync(dto);
                return Ok(new { code = 0 });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { code = 404, msg = "音色不存在" });
            }
        }

        /// <summary>
        /// 删除音色
        /// </summary>
        /// <param name="ids">音色ID数组</param>
        /// <returns>删除结果</returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return BadRequest(new { code = 400, msg = "删除的音色ID不能为空" });
            }

            await _timbreService.DeleteAsync(ids);
            return Ok(new { code = 0 });
        }

        /// <summary>
        /// 获取音色详情
        /// </summary>
        /// <param name="id">音色ID</param>
        /// <returns>音色详情</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var data = await _timbreService.GetAsync(id);
            if (data == null)
            {
                return NotFound(new { code = 404, msg = "音色不存在" });
            }
            return Ok(new { code = 0, data });
        }
    }
}