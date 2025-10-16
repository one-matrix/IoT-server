using Microsoft.AspNetCore.Mvc;
using IotApi.Services;
using IotApi.DTOs;
using System.ComponentModel.DataAnnotations;
using IotApi.Models;

namespace IotApi.Controllers
{
    /// <summary>
    /// 字典类型管理
    /// </summary>
    [ApiController]
    [Route("xiaozhi/admin/dict/type")]
    [Produces("application/json")]
    [Tags("字典类型管理")]
    public class SysDictTypeController : ControllerBase
    {
        private readonly ISysService _sysService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysService">系统服务</param>
        public SysDictTypeController(ISysService sysService)
        {
            _sysService = sysService;
        }
        
        /// <summary>
        /// 获取字典类型分页列表
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <returns>分页结果</returns>
        [HttpGet("page")]
        public async Task<ActionResult<PageResult<SysDictTypeDto>>> GetPage([FromQuery] SysDictTypeQueryDto query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _sysService.GetDictTypePageAsync(query);
            return Ok(new { code = 0, data = result });
        }

        /// <summary>
        /// 获取字典类型详情
        /// </summary>
        /// <param name="id">字典类型ID</param>
        /// <returns>字典类型</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SysDictTypeDto>> Get(long id)
        {
            try
            {
                var dictType = await _sysService.GetDictTypeAsync(id);
                return Ok(new { code = 0, data = dictType });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { code = 404, msg = "字典类型不存在" });
            }
        }

        /// <summary>
        /// 添加字典类型
        /// </summary>
        /// <param name="dto">字典类型</param>
        /// <returns>添加结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SysDictTypeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var id = await _sysService.AddDictTypeAsync(dto);
            return Ok(new { code = 0, data = id });
        }

        /// <summary>
        /// 更新字典类型
        /// </summary>
        /// <param name="dto">字典类型</param>
        /// <returns>更新结果</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SysDictTypeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                await _sysService.UpdateDictTypeAsync(dto);
                return Ok(new { code = 0 });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { code = 404, msg = "字典类型不存在" });
            }
        }

        /// <summary>
        /// 删除字典类型
        /// </summary>
        /// <param name="ids">字典类型ID数组</param>
        /// <returns>删除结果</returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] long[] ids)
        {
            await _sysService.DeleteDictTypesAsync(ids);
            return Ok(new { code = 0 });
        }
    }
}