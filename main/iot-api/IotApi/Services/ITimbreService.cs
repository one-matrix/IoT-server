using IotApi.DTOs;
using IotApi.Models;

namespace IotApi.Services
{
    /// <summary>
    /// 音色服务接口
    /// </summary>
    public interface ITimbreService
    {
        /// <summary>
        /// 获取音色分页列表
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <returns>分页结果</returns>
        Task<PageResult<TimbreDto>> GetPageAsync(TimbreQueryDto query);
        
        /// <summary>
        /// 获取音色详情
        /// </summary>
        /// <param name="id">音色ID</param>
        /// <returns>音色详情</returns>
        Task<TimbreDto> GetAsync(string id);
        
        /// <summary>
        /// 添加音色
        /// </summary>
        /// <param name="dto">音色数据</param>
        /// <returns>新增音色的ID</returns>
        Task<string> AddAsync(TimbreDto dto);
        
        /// <summary>
        /// 更新音色
        /// </summary>
        /// <param name="dto">音色数据</param>
        /// <returns>更新结果</returns>
        Task UpdateAsync(TimbreDto dto);
        
        /// <summary>
        /// 删除音色
        /// </summary>
        /// <param name="ids">音色ID数组</param>
        /// <returns>删除结果</returns>
        Task DeleteAsync(string[] ids);
        
        /// <summary>
        /// 根据TTS模型ID获取音色列表
        /// </summary>
        /// <param name="ttsModelId">TTS模型ID</param>
        /// <param name="voiceName">音色名称（可选）</param>
        /// <returns>音色列表</returns>
        Task<List<TimbreDto>> GetVoiceNamesAsync(string ttsModelId, string voiceName);
        
        /// <summary>
        /// 根据ID获取音色名称
        /// </summary>
        /// <param name="id">音色ID</param>
        /// <returns>音色名称</returns>
        Task<string> GetTimbreNameByIdAsync(string id);
    }
}