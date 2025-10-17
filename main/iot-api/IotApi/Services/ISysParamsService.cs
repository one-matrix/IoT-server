using System.Collections.Generic;
using System.Threading.Tasks;
using IotApi.DTOs;
using IotApi.Models;


namespace IotApi.Services
{
    /// <summary>
    /// 系统服务接口
    /// </summary>
    public interface ISysParamsService
    {
        #region 系统参数
        /// <summary>
        /// 获取系统参数分页列表
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <returns>分页结果</returns>
        Task<PageResult<SysParamsDto>> GetSysParamsPageAsync(SysParamsQueryDto query);

        /// <summary>
        /// 获取系统参数详情
        /// </summary>
        /// <param name="id">参数ID</param>
        /// <returns>系统参数</returns>
        Task<SysParamsDto> GetSysParamAsync(long id);
        
        /// <summary>
        /// 根据参数代码获取参数值
        /// </summary>
        /// <param name="code">参数代码</param>
        /// <returns>参数值</returns>
        Task<string> GetValueAsync(string code);

        /// <summary>
        /// 获取参数列表
        /// </summary>
        /// <param name="codes">参数代码列表</param>
        /// <returns>参数列表</returns>
        Task<List<SysParams>> GetListAsync(List<string> codes);

        /// <summary>
        /// 添加系统参数
        /// </summary>
        /// <param name="dto">系统参数</param>
        /// <returns>添加结果</returns>
        Task<long> AddSysParamAsync(SysParamsDto dto);

        /// <summary>
        /// 更新系统参数
        /// </summary>
        /// <param name="dto">系统参数</param>
        /// <returns>更新结果</returns>
        Task UpdateSysParamAsync(SysParamsDto dto);

        /// <summary>
        /// 删除系统参数
        /// </summary>
        /// <param name="ids">参数ID数组</param>
        /// <returns>删除结果</returns>
        Task DeleteSysParamsAsync(long[] ids);

        /// <summary>
        /// 检查系统参数是否存在
        /// </summary>
        /// <param name="id">参数ID</param>
        /// <returns>是否存在</returns>
        Task<bool> SysParamExistsAsync(long id);
        #endregion

        #region 字典类型
        /// <summary>
        /// 获取字典类型分页列表
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <returns>分页结果</returns>
        Task<PageResult<SysDictTypeDto>> GetDictTypePageAsync(SysDictTypeQueryDto query);

        /// <summary>
        /// 获取字典类型详情
        /// </summary>
        /// <param name="id">字典类型ID</param>
        /// <returns>字典类型</returns>
        Task<SysDictTypeDto> GetDictTypeAsync(long id);

        /// <summary>
        /// 添加字典类型
        /// </summary>
        /// <param name="dto">字典类型</param>
        /// <returns>添加结果</returns>
        Task<long> AddDictTypeAsync(SysDictTypeDto dto);

        /// <summary>
        /// 更新字典类型
        /// </summary>
        /// <param name="dto">字典类型</param>
        /// <returns>更新结果</returns>
        Task UpdateDictTypeAsync(SysDictTypeDto dto);

        /// <summary>
        /// 删除字典类型
        /// </summary>
        /// <param name="ids">字典类型ID数组</param>
        /// <returns>删除结果</returns>
        Task DeleteDictTypesAsync(long[] ids);

        /// <summary>
        /// 检查字典类型是否存在
        /// </summary>
        /// <param name="id">字典类型ID</param>
        /// <returns>是否存在</returns>
        Task<bool> DictTypeExistsAsync(long id);
        #endregion

        #region 字典数据
        /// <summary>
        /// 获取字典数据分页列表
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <returns>分页结果</returns>
        Task<PageResult<SysDictDataDto>> GetDictDataPageAsync(SysDictDataQueryDto query);

        /// <summary>
        /// 获取字典数据详情
        /// </summary>
        /// <param name="id">字典数据ID</param>
        /// <returns>字典数据</returns>
        Task<SysDictDataDto> GetDictDataAsync(long id);

        /// <summary>
        /// 添加字典数据
        /// </summary>
        /// <param name="dto">字典数据</param>
        /// <returns>添加结果</returns>
        Task<long> AddDictDataAsync(SysDictDataDto dto);

        /// <summary>
        /// 更新字典数据
        /// </summary>
        /// <param name="dto">字典数据</param>
        /// <returns>更新结果</returns>
        Task UpdateDictDataAsync(SysDictDataDto dto);

        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <param name="ids">字典数据ID数组</param>
        /// <returns>删除结果</returns>
        Task DeleteDictDatasAsync(long[] ids);

        /// <summary>
        /// 根据字典类型获取字典数据列表
        /// </summary>
        /// <param name="dictType">字典类型</param>
        /// <returns>字典数据列表</returns>
        Task<List<SysDictDataItem>> GetDictDataByTypeAsync(string dictType);

        /// <summary>
        /// 检查字典数据是否存在
        /// </summary>
        /// <param name="id">字典数据ID</param>
        /// <returns>是否存在</returns>
        Task<bool> DictDataExistsAsync(long id);
        #endregion
    }
}