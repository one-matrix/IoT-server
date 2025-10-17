using IotApi.DTOs;
using IotApi.Models;

namespace IotApi.Services
{
    /// <summary>
    /// OTA固件管理服务接口
    /// </summary>
    public interface IOtaService
    {
        /// <summary>
        /// 获取OTA固件分页列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数量</param>
        /// <param name="type">固件类型</param>
        /// <returns>分页结果</returns>
        Task<PageResult<AiOta>> GetPageAsync(int page, int limit, string type = null);

        /// <summary>
        /// 保存OTA固件信息
        /// </summary>
        /// <param name="entity">固件实体</param>
        /// <returns>保存结果</returns>
        Task<bool> SaveAsync(AiOta entity);

        /// <summary>
        /// 更新OTA固件信息
        /// </summary>
        /// <param name="entity">固件实体</param>
        /// <returns>更新结果</returns>
        Task UpdateAsync(AiOta entity);

        /// <summary>
        /// 删除OTA固件
        /// </summary>
        /// <param name="ids">固件ID数组</param>
        /// <returns>删除结果</returns>
        Task DeleteAsync(string[] ids);

        /// <summary>
        /// 获取最新的OTA固件
        /// </summary>
        /// <param name="type">固件类型</param>
        /// <returns>最新固件信息</returns>
        Task<AiOta> GetLatestOtaAsync(string type);
    }
}