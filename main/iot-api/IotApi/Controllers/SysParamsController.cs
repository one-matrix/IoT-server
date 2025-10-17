using Microsoft.AspNetCore.Mvc;
using IotApi.Services;
using IotApi.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IotApi.Controllers
{
    /// <summary>
    /// 系统参数管理
    /// </summary>
    [ApiController]
    [Route("xiaozhi/admin/[controller]")]
    [Produces("application/json")]
    [Tags("系统参数管理")]
    public class SysParamsController : ControllerBase
    {
        private readonly ISysParamsService _sysService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysService">系统服务</param>
        public SysParamsController(ISysParamsService sysService)
        {
            _sysService = sysService;
        }

        /// <summary>
        /// 获取系统参数分页列表
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <returns>分页结果</returns>
        [HttpGet("page")]
        public async Task<ActionResult<PageResult<SysParamsDto>>> GetPage([FromQuery] SysParamsQueryDto query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _sysService.GetSysParamsPageAsync(query);
            return Ok(new { code = 0, data = result });
        }

        /// <summary>
        /// 获取系统参数详情
        /// </summary>
        /// <param name="id">参数ID</param>
        /// <returns>系统参数</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SysParamsDto>> Get(long id)
        {
            try
            {
                var param = await _sysService.GetSysParamAsync(id);
                return Ok(new { code = 0, data = param });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { code = 404, msg = "系统参数不存在" });
            }
        }

        /// <summary>
        /// 添加系统参数
        /// </summary>
        /// <param name="dto">系统参数</param>
        /// <returns>添加结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SysParamsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var id = await _sysService.AddSysParamAsync(dto);
            return Ok(new { code = 0, data = id });
        }

        /// <summary>
        /// 更新系统参数
        /// </summary>
        /// <param name="dto">系统参数</param>
        /// <returns>更新结果</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SysParamsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                await _sysService.UpdateSysParamAsync(dto);
                return Ok(new { code = 0 });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { code = 404, msg = "系统参数不存在" });
            }
        }

        /// <summary>
        /// 删除系统参数
        /// </summary>
        /// <param name="ids">参数ID数组</param>
        /// <returns>删除结果</returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] long[] ids)
        {
            await _sysService.DeleteSysParamsAsync(ids);
            return Ok(new { code = 0 });
        }
    }
}