using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IotApi.DTOs;
using IotApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using IotApi.Core.common;

namespace IotApi.Services
{
    /// <summary>
    /// OTA固件管理服务实现类
    /// </summary>
    public class OtaService : IOtaService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OtaService> _logger;

        public OtaService(ApplicationDbContext dbContext, ILogger<OtaService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// 获取OTA固件分页列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数量</param>
        /// <param name="type">固件类型</param>
        /// <returns>分页结果</returns>
        public async Task<PageResult<AiOta>> GetPageAsync(int page, int limit, string type = null)
        {
            try
            {
                _logger.LogInformation("获取OTA固件分页列表, 页码: {Page}, 每页数量: {Limit}, 类型: {Type}", page, limit, type ?? "全部");
                
                if (page < 1) page = 1;
                if (limit < 1) limit = 10;
                
                var query = _dbContext.AiOtas.AsQueryable();
                
                // 按类型筛选
                if (!string.IsNullOrEmpty(type))
                {
                    query = query.Where(o => o.Type == type);
                }

                // 按更新时间降序排序，与Java版本保持一致
                query = query.OrderByDescending(o => o.UpdateDate);

                // 计算总数
                var total = await query.CountAsync();
                
                // 分页查询
                var items = await query
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();

                _logger.LogInformation("获取OTA固件分页列表成功, 总数: {Total}", total);
                
                return new PageResult<AiOta>
                {
                    Total = total,
                    List = items
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取OTA固件分页列表失败");
                throw new ApiException("获取OTA固件分页列表失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 保存OTA固件信息
        /// </summary>
        /// <param name="entity">固件实体</param>
        /// <returns>保存结果</returns>
        public async Task<bool> SaveAsync(AiOta entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "固件信息不能为空");
                }

                if (string.IsNullOrEmpty(entity.Type))
                {
                    throw new ArgumentException("固件类型不能为空", nameof(entity.Type));
                }

                if (string.IsNullOrEmpty(entity.Version))
                {
                    throw new ArgumentException("版本号不能为空", nameof(entity.Version));
                }

                _logger.LogInformation("保存OTA固件信息, 类型: {Type}, 版本: {Version}", entity.Type, entity.Version);

                // 查询同类型的固件
                var existingOta = await _dbContext.AiOtas
                    .Where(o => o.Type == entity.Type)
                    .FirstOrDefaultAsync();

                // 同类固件只保留最新的一条，与Java版本保持一致
                if (existingOta != null)
                {
                    _logger.LogInformation("发现同类型固件，更新现有记录, ID: {Id}", existingOta.Id);
                    
                    // 更新现有记录
                    existingOta.FirmwareName = entity.FirmwareName;
                    existingOta.Version = entity.Version;
                    existingOta.Size = entity.Size;
                    existingOta.Remark = entity.Remark;
                    existingOta.FirmwarePath = entity.FirmwarePath;
                    existingOta.UpdateDate = DateTime.Now;
                    existingOta.Updater = entity.Creator;
                    
                    _dbContext.AiOtas.Update(existingOta);
                    return await _dbContext.SaveChangesAsync() > 0;
                }

                // 新增记录
                entity.Id = Guid.NewGuid().ToString();
                entity.CreateDate = DateTime.Now;
                entity.UpdateDate = DateTime.Now;
                
                await _dbContext.AiOtas.AddAsync(entity);
                var result = await _dbContext.SaveChangesAsync() > 0;
                
                _logger.LogInformation("保存OTA固件信息成功, ID: {Id}", entity.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存OTA固件信息失败");
                throw new ApiException("保存OTA固件信息失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 更新OTA固件信息
        /// </summary>
        /// <param name="entity">固件实体</param>
        /// <returns>更新结果</returns>
        public async Task UpdateAsync(AiOta entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "固件信息不能为空");
                }

                if (string.IsNullOrEmpty(entity.Id))
                {
                    throw new ArgumentException("固件ID不能为空", nameof(entity.Id));
                }

                _logger.LogInformation("更新OTA固件信息, ID: {Id}", entity.Id);

                var existingOta = await _dbContext.AiOtas.FindAsync(entity.Id);
                if (existingOta == null)
                {
                    _logger.LogWarning("未找到ID为{Id}的OTA固件", entity.Id);
                    throw new KeyNotFoundException($"未找到ID为{entity.Id}的OTA固件");
                }

                // 检查是否存在相同类型和版本的固件（排除当前记录）
                var duplicateOta = await _dbContext.AiOtas
                    .Where(o => o.Type == entity.Type && o.Version == entity.Version && o.Id != entity.Id)
                    .FirstOrDefaultAsync();

                if (duplicateOta != null)
                {
                    _logger.LogWarning("已存在相同类型和版本的固件, ID: {Id}", duplicateOta.Id);
                    throw new ApiException("已存在相同类型和版本的固件，请修改后重试");
                }

                // 更新属性
                existingOta.FirmwareName = !string.IsNullOrEmpty(entity.FirmwareName) ? entity.FirmwareName : existingOta.FirmwareName;
                existingOta.Version = !string.IsNullOrEmpty(entity.Version) ? entity.Version : existingOta.Version;
                existingOta.Type = !string.IsNullOrEmpty(entity.Type) ? entity.Type : existingOta.Type;
                existingOta.Size = entity.Size ?? existingOta.Size;
                existingOta.Remark = entity.Remark ?? existingOta.Remark;
                existingOta.FirmwarePath = !string.IsNullOrEmpty(entity.FirmwarePath) ? entity.FirmwarePath : existingOta.FirmwarePath;
                existingOta.UpdateDate = DateTime.Now;
                existingOta.Updater = entity.Updater;

                _dbContext.AiOtas.Update(existingOta);
                await _dbContext.SaveChangesAsync();
                
                _logger.LogInformation("更新OTA固件信息成功, ID: {Id}", entity.Id);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新OTA固件信息失败, ID: {Id}", entity?.Id);
                throw new ApiException("更新OTA固件信息失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 删除OTA固件
        /// </summary>
        /// <param name="ids">固件ID数组</param>
        /// <returns>删除结果</returns>
        public async Task DeleteAsync(string[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                {
                    throw new ArgumentException("删除的固件ID不能为空");
                }

                _logger.LogInformation("删除OTA固件, IDs: {Ids}", string.Join(", ", ids));

                var otaList = await _dbContext.AiOtas
                    .Where(o => ids.Contains(o.Id))
                    .ToListAsync();

                if (otaList.Any())
                {
                    _dbContext.AiOtas.RemoveRange(otaList);
                    await _dbContext.SaveChangesAsync();
                    _logger.LogInformation("删除OTA固件成功, 删除数量: {Count}", otaList.Count);
                }
                else
                {
                    _logger.LogWarning("未找到要删除的OTA固件");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除OTA固件失败");
                throw new ApiException("删除OTA固件失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 获取最新的OTA固件
        /// </summary>
        /// <param name="type">固件类型</param>
        /// <returns>最新固件信息</returns>
        public async Task<AiOta> GetLatestOtaAsync(string type)
        {
            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    _logger.LogWarning("获取最新OTA固件失败: 固件类型不能为空");
                    throw new ArgumentException("固件类型不能为空", nameof(type));
                }

                _logger.LogInformation("获取最新OTA固件, 类型: {Type}", type);

                var latestOta = await _dbContext.AiOtas
                    .Where(o => o.Type == type)
                    .OrderByDescending(o => o.Version)
                    .FirstOrDefaultAsync();

                if (latestOta != null)
                {
                    _logger.LogInformation("获取最新OTA固件成功, ID: {Id}, 版本: {Version}", latestOta.Id, latestOta.Version);
                }
                else
                {
                    _logger.LogInformation("未找到类型为 {Type} 的OTA固件", type);
                }

                return latestOta;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取最新OTA固件失败, 类型: {Type}", type);
                throw new ApiException("获取最新OTA固件失败: " + ex.Message);
            }
        }
    }
}