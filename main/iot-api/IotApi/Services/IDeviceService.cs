using IotApi.DTOs;
using IotApi.Models;

namespace IotApi.Services
{
    /// <summary>
    /// 设备服务接口
    /// </summary>
    public interface IDeviceService
    {
        /// <summary>
        /// 获取系统参数值
        /// </summary>
        /// <param name="paramCode">参数代码</param>
        /// <returns>参数值</returns>
        string GetSystemParam(string paramCode);
        /// <summary>
        /// 检查设备是否激活
        /// </summary>
        /// <param name="macAddress">MAC地址</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="deviceReport">设备上报信息</param>
        /// <returns>设备上报响应</returns>
        Task<DeviceReportRespDto> CheckDeviceActiveAsync(string macAddress, string clientId, DeviceReportReqDto deviceReport);

        /// <summary>
        /// 获取用户指定智能体的设备列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="agentId">智能体ID</param>
        /// <returns>设备列表</returns>
        Task<IEnumerable<AiDevice>> GetUserDevicesAsync(long userId, string agentId);

        /// <summary>
        /// 解绑设备
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="deviceId">设备ID</param>
        /// <returns>操作结果</returns>
        Task<bool> UnbindDeviceAsync(long userId, string deviceId);

        /// <summary>
        /// 设备激活
        /// </summary>
        /// <param name="agentId">智能体ID</param>
        /// <param name="activationCode">激活码</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否激活成功</returns>
        Task<bool> DeviceActivationAsync(string agentId, string deviceCode, long userId);

        /// <summary>
        /// 根据设备ID获取设备代码
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <returns>设备代码</returns>
        Task<string> GetCodeByDeviceIdAsync(string deviceId);

        /// <summary>
        /// 根据MAC地址获取设备信息
        /// </summary>
        /// <param name="macAddress">MAC地址</param>
        /// <returns>设备信息</returns>
        Task<AiDevice> GetDeviceByMacAddressAsync(string macAddress);

        /// <summary>
        /// 手动添加设备
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="dto">设备信息</param>
        /// <returns>添加的设备</returns>
        Task<AiDevice> ManualAddDeviceAsync(long userId, DeviceManualAddDto dto);

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <param name="dto">设备更新信息</param>
        /// <param name="userId">用户ID</param>
        /// <returns>更新后的设备</returns>
        Task<AiDevice> UpdateDeviceAsync(string id, DeviceUpdateDto dto, long userId);

        /// <summary>
        /// 分页获取全部设备信息
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数量</param>
        /// <param name="keywords">关键词</param>
        /// <returns>分页设备列表</returns>
        Task<(IEnumerable<AiDevice> Items, int Total)> GetAllDevicesAsync(int page, int limit, string keywords = null);
    }
}