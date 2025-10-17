using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IotApi.DTOs;
using IotApi.Models;
using IotApi.Core.common;

namespace IotApi.Services
{
    /// <summary>
    /// 设备服务实现类
    /// </summary>
    public class DeviceService : IDeviceService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeviceService> _logger;
        private readonly ISysParamsService _sysService;

        public DeviceService(ApplicationDbContext context, ILogger<DeviceService> logger, ISysParamsService sysService)
        {
            _context = context;
            _logger = logger;
            _sysService = sysService;
        }

        /// <summary>
        /// 获取系统参数值
        /// </summary>
        /// <param name="paramCode">参数代码</param>
        /// <returns>参数值</returns>
        public string GetSystemParam(string paramCode)
        {
            try
            {
                var param = _context.SysParams.FirstOrDefault(p => p.ParamCode == paramCode);
                return param?.ParamValue;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取系统参数[{paramCode}]失败");
                return null;
            }
        }

        /// <summary>
        /// 检查设备是否激活
        /// </summary>
        /// <param name="macAddress">MAC地址</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="deviceReport">设备上报信息</param>
        /// <returns>设备上报响应</returns>
        public async Task<DeviceReportRespDto> CheckDeviceActiveAsync(string macAddress, string clientId, DeviceReportReqDto deviceReport)
        {
            try
            {
                _logger.LogInformation($"检查设备激活状态: MAC={macAddress}, ClientId={clientId}");
                
                // 验证MAC地址
                if (string.IsNullOrEmpty(macAddress))
                {
                    _logger.LogWarning("MAC地址为空");
                    return DeviceReportRespDto.CreateError("MAC地址不能为空");
                }

                // 查找设备
                var device = await _context.AiDevices.FirstOrDefaultAsync(d => d.MacAddress == macAddress);
                if (device == null)
                {
                    _logger.LogWarning($"设备未注册: MAC={macAddress}");
                    return DeviceReportRespDto.CreateError("设备未注册");
                }

                // 检查设备是否已绑定用户
                if (device.UserId == null)
                {
                    _logger.LogWarning($"设备未激活: MAC={macAddress}");
                    return DeviceReportRespDto.CreateError("设备未激活");
                }

                // 更新设备信息
                device.LastConnectedAt = DateTime.UtcNow;
                device.AppVersion = deviceReport.Version;
                device.Board = deviceReport.Board;
                await _context.SaveChangesAsync();

                // 返回成功响应
                return new DeviceReportRespDto
                {
                    Status = "success",
                    Message = "设备已激活"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"检查设备激活状态失败: MAC={macAddress}");
                return DeviceReportRespDto.CreateError("检查设备激活状态失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 获取用户指定智能体的设备列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="agentId">智能体ID</param>
        /// <returns>设备列表</returns>
        public async Task<IEnumerable<AiDevice>> GetUserDevicesAsync(long userId, string agentId)
        {
            try
            {
                _logger.LogInformation($"获取用户设备列表: UserId={userId}, AgentId={agentId}");
                
                var query = _context.AiDevices.AsQueryable();
                
                // 筛选用户ID
                query = query.Where(d => d.UserId == userId);
                
                // 如果指定了智能体ID，则筛选智能体
                if (!string.IsNullOrEmpty(agentId))
                {
                    query = query.Where(d => d.AgentId == agentId);
                }
                
                // 按排序字段和最后连接时间排序
                return await query.OrderBy(d => d.Sort).ThenByDescending(d => d.LastConnectedAt).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取用户设备列表失败: UserId={userId}, AgentId={agentId}");
                throw new ApiException("获取用户设备列表失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 解绑设备
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="deviceId">设备ID</param>
        /// <returns>操作结果</returns>
        public async Task<bool> UnbindDeviceAsync(long userId, string deviceId)
        {
            try
            {
                _logger.LogInformation($"解绑设备: UserId={userId}, DeviceId={deviceId}");
                
                // 查找设备
                var device = await _context.AiDevices.FirstOrDefaultAsync(d => d.Id == deviceId && d.UserId == userId);
                if (device == null)
                {
                    _logger.LogWarning($"设备不存在或不属于当前用户: DeviceId={deviceId}, UserId={userId}");
                    return false;
                }
                
                // 解绑设备（清除用户ID）
                device.UserId = null;
                device.UpdatedAt = DateTime.UtcNow;
                device.Updater = userId;
                
                await _context.SaveChangesAsync();
                _logger.LogInformation($"设备解绑成功: DeviceId={deviceId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"解绑设备失败: UserId={userId}, DeviceId={deviceId}");
                return false;
            }
        }

        /// <summary>
        /// 设备激活
        /// </summary>
        /// <param name="agentId">智能体ID</param>
        /// <param name="deviceCode">激活码</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否激活成功</returns>
        public async Task<bool> DeviceActivationAsync(string agentId, string deviceCode, long userId)
        {
            try
            {
                _logger.LogInformation($"激活设备: AgentId={agentId}, DeviceCode={deviceCode}, UserId={userId}");
                
                // 查找设备
                var device = await _context.AiDevices.FirstOrDefaultAsync(d => d.MacAddress == deviceCode);
                if (device == null)
                {
                    _logger.LogWarning($"设备不存在: DeviceCode={deviceCode}");
                    return false;
                }
                
                // 检查设备是否已被激活
                if (device.UserId != null)
                {
                    _logger.LogWarning($"设备已被激活: DeviceCode={deviceCode}, CurrentUserId={device.UserId}");
                    return false;
                }
                
                // 激活设备
                device.UserId = userId;
                device.AgentId = agentId;
                device.UpdatedAt = DateTime.UtcNow;
                device.Updater = userId;
                
                await _context.SaveChangesAsync();
                _logger.LogInformation($"设备激活成功: DeviceId={device.Id}, UserId={userId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"激活设备失败: AgentId={agentId}, DeviceCode={deviceCode}, UserId={userId}");
                return false;
            }
        }

        /// <summary>
        /// 根据MAC地址获取设备信息
        /// </summary>
        /// <param name="macAddress">MAC地址</param>
        /// <returns>设备信息</returns>
        public async Task<AiDevice> GetDeviceByMacAddressAsync(string macAddress)
        {
            try
            {
                _logger.LogInformation($"根据MAC地址获取设备: MAC={macAddress}");
                return await _context.AiDevices.FirstOrDefaultAsync(d => d.MacAddress == macAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"根据MAC地址获取设备失败: MAC={macAddress}");
                throw new ApiException("根据MAC地址获取设备失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 手动添加设备
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="dto">设备信息</param>
        /// <returns>添加的设备</returns>
        public async Task<AiDevice> ManualAddDeviceAsync(long userId, DeviceManualAddDto dto)
        {
            try
            {
                _logger.LogInformation($"手动添加设备: UserId={userId}, MAC={dto.MacAddress}, Name={dto.DeviceName}");
                
                // 检查MAC地址是否已存在
                var existingDevice = await _context.AiDevices.FirstOrDefaultAsync(d => d.MacAddress == dto.MacAddress);
                if (existingDevice != null)
                {
                    _logger.LogWarning($"设备MAC地址已存在: MAC={dto.MacAddress}");
                    throw new ApiException("设备MAC地址已存在");
                }
                
                // 创建新设备
                var device = new AiDevice
                {
                    Id = Guid.NewGuid().ToString("N"),
                    MacAddress = dto.MacAddress,
                    Alias = dto.DeviceName,
                    Board = dto.Board,
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    Creator = userId,
                    UpdatedAt = DateTime.UtcNow,
                    Updater = userId,
                    Sort = 0,
                    AutoUpdate = 1 // 默认自动更新
                };
                
                _context.AiDevices.Add(device);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation($"设备添加成功: DeviceId={device.Id}");
                return device;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"手动添加设备失败: UserId={userId}, MAC={dto.MacAddress}");
                throw new ApiException("手动添加设备失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <param name="dto">设备更新信息</param>
        /// <param name="userId">用户ID</param>
        /// <returns>更新后的设备</returns>
        public async Task<AiDevice> UpdateDeviceAsync(string id, DeviceUpdateDto dto, long userId)
        {
            try
            {
                _logger.LogInformation($"更新设备信息: DeviceId={id}, UserId={userId}");
                
                // 查找设备
                var device = await _context.AiDevices.FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);
                if (device == null)
                {
                    _logger.LogWarning($"设备不存在或不属于当前用户: DeviceId={id}, UserId={userId}");
                    throw new ApiException("设备不存在或不属于当前用户");
                }
                
                // 更新设备信息
                if (!string.IsNullOrEmpty(dto.Alias))
                {
                    device.Alias = dto.Alias;
                }
                
                if (dto.AutoUpdate.HasValue)
                {
                    device.AutoUpdate = dto.AutoUpdate.Value;
                }
                
                device.UpdatedAt = DateTime.UtcNow;
                device.Updater = userId;
                
                await _context.SaveChangesAsync();
                _logger.LogInformation($"设备信息更新成功: DeviceId={id}");
                return device;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新设备信息失败: DeviceId={id}, UserId={userId}");
                throw new ApiException("更新设备信息失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 分页获取全部设备信息
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数量</param>
        /// <param name="keywords">关键词</param>
        /// <returns>分页设备列表</returns>
        public async Task<(IEnumerable<AiDevice> Items, int Total)> GetAllDevicesAsync(int page, int limit, string keywords = null)
        {
            try
            {
                _logger.LogInformation($"获取全部设备信息: Page={page}, Limit={limit}, Keywords={keywords}");
                
                var query = _context.AiDevices.AsQueryable();
                
                // 关键词搜索
                if (!string.IsNullOrEmpty(keywords))
                {
                    query = query.Where(d => 
                        d.MacAddress.Contains(keywords) || 
                        (d.Alias != null && d.Alias.Contains(keywords)) ||
                        (d.Board != null && d.Board.Contains(keywords))
                    );
                }
                
                // 获取总数
                var total = await query.CountAsync();
                
                // 分页查询
                var items = await query
                    .OrderByDescending(d => d.CreateDate)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();
                
                return (items, total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取全部设备信息失败: Page={page}, Limit={limit}");
                throw new ApiException("获取全部设备信息失败: " + ex.Message);
            }
        }
    }
}