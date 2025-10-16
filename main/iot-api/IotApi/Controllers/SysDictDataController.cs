using Microsoft.AspNetCore.Mvc;
using IotApi.Services;
using IotApi.DTOs;
using System.ComponentModel.DataAnnotations;
using IotApi.Models;

namespace IotApi.Controllers
{
    /// <summary>
    /// 字典数据管理
    /// </summary>
    [ApiController]
    [Route("xiaozhi/admin/dict/data")]
    [Produces("application/json")]
    [Tags("字典数据管理")]
    public class SysDictDataController : ControllerBase
    {
        private readonly ISysService _sysService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysService">系统服务</param>
        public SysDictDataController(ISysService sysService)
        {
            _sysService = sysService;
        }

        /// <summary>
        /// 获取字典数据分页列表
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <returns>分页结果</returns>
        // GET: xiaozhi/admin/dict/data/page
        [HttpGet("page")]
        public async Task<ActionResult<PageResult<SysDictDataDto>>> GetPage([FromQuery] SysDictDataQueryDto query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _sysService.GetDictDataPageAsync(query);
            return Ok(new { code = 0, data = result });
        }

        /// <summary>
        /// 获取字典数据详情
        /// </summary>
        /// <param name="id">字典数据ID</param>
        /// <returns>字典数据</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SysDictDataDto>> Get(long id)
        {
            try
            {
                var dictData = await _sysService.GetDictDataAsync(id);
                return Ok(new { code = 0, data = dictData });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { code = 404, msg = "字典数据不存在" });
            }
        }

        /// <summary>
        /// 添加字典数据
        /// </summary>
        /// <param name="dto">字典数据</param>
        /// <returns>添加结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SysDictDataDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var id = await _sysService.AddDictDataAsync(dto);
            return Ok(new { code = 0, data = id });
        }

        /// <summary>
        /// 更新字典数据
        /// </summary>
        /// <param name="dto">字典数据</param>
        /// <returns>更新结果</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SysDictDataDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                await _sysService.UpdateDictDataAsync(dto);
                return Ok(new { code = 0 });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { code = 404, msg = "字典数据不存在" });
            }
        }

        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <param name="ids">字典数据ID数组</param>
        /// <returns>删除结果</returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] long[] ids)
        {
            await _sysService.DeleteDictDatasAsync(ids);
            return Ok(new { code = 0 });
        }
        
        /// <summary>
        /// 根据字典类型获取字典数据列表
        /// </summary>
        /// <param name="dictType">字典类型</param>
        /// <returns>字典数据列表</returns>
        [HttpGet("type/{dictType}")]
        public async Task<ActionResult<List<SysDictDataItem>>> GetDictDataByType(string dictType)
        {
            var list = await _sysService.GetDictDataByTypeAsync(dictType);
            return Ok(new { code = 0, data = list });
        }

        // GET: xiaozhi/admin/dict/data/page
        //[HttpGet("page")]
        //public async Task<ActionResult<IEnumerable<SysDictData>>> GetPage(
        //    [FromQuery] long dictTypeId,
        //    [FromQuery] int page = 1,
        //    [FromQuery] int limit = 10,
        //    [FromQuery] string dictLabel = null,
        //    [FromQuery] string dictValue = null)
        //{
        //    if (dictTypeId <= 0)
        //    {
        //        return BadRequest("dictTypeId cannot be empty");
        //    }

        //    var query = _context.SysDictData
        //        .Where(d => d.DictTypeId == dictTypeId)
        //        .AsQueryable();

        //    if (!string.IsNullOrEmpty(dictLabel))
        //    {
        //        query = query.Where(d => d.DictLabel.Contains(dictLabel));
        //    }

        //    if (!string.IsNullOrEmpty(dictValue))
        //    {
        //        query = query.Where(d => d.DictValue.Contains(dictValue));
        //    }

        //    var dictDataList = await query
        //        .Skip((page - 1) * limit)
        //        .Take(limit)
        //        .ToListAsync();

        //    // In a real implementation, we would return pagination info
        //    return Ok(dictDataList);
        //}

        // GET: xiaozhi/admin/dict/data/{id}
        //[HttpGet("{id}")]
        //public async Task<ActionResult<SysDictData>> Get(long id)
        //{
        //    var dictData = await _context.SysDictData.FindAsync(id);
            
        //    if (dictData == null)
        //    {
        //        return NotFound();
        //    }
            
        //    return Ok(dictData);
        //}

        // 旧版直接使用DbContext的接口已移除，统一通过ISysService实现。
    }
}