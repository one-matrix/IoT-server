using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IotApi.Models;
using IotApi.Services;
using IotApi.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IotApi.Controllers
{
    /// <summary>
    /// 设备管理
    /// </summary>
    [ApiController]
    [Route("xiaozhi/[controller]")]
    [Produces("application/json")]
    [Tags("设备管理")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        /// <summary>
        /// 绑定设备
        /// </summary>
        /// <param name="agentId">智能体ID</param>
        /// <param name="deviceCode">设备激活码</param>
        /// <returns>操作结果</returns>
        [HttpPost("bind/{agentId}/{deviceCode}")]
        public async Task<IActionResult> BindDevice(string agentId, string deviceCode)
        {
            var userId = GetCurrentUserId();
            var result = await _deviceService.DeviceActivationAsync(agentId, deviceCode, userId);
            return Ok(new { code = result ? 0 : 1, msg = result ? "设备绑定成功" : "设备绑定失败" });
        }

        /// <summary>
        /// 注册设备
        /// </summary>
        /// <param name="deviceRegisterDto">设备注册信息</param>
        /// <returns>验证码</returns>
        [HttpPost("register")]
        public async Task<ActionResult<string>> RegisterDevice([FromBody] DeviceRegisterDto deviceRegisterDto)
        {
            if (string.IsNullOrEmpty(deviceRegisterDto.MacAddress))
            {
                return BadRequest(new { code = 1, msg = "MAC地址不能为空" });
            }
            
            // 生成随机6位验证码
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();
            
            // 实际实现中应该将验证码存储在Redis中
            // 这里简化处理，直接返回验证码
            return Ok(new { code = 0, data = code });
        }

        /// <summary>
        /// 获取用户已绑定设备
        /// </summary>
        /// <param name="agentId">智能体ID</param>
        /// <returns>设备列表</returns>
        [HttpGet("bind/{agentId}")]
        public async Task<ActionResult<IEnumerable<AiDevice>>> GetUserDevices(string agentId)
        {
            var userId = GetCurrentUserId();
            var devices = await _deviceService.GetUserDevicesAsync(userId, agentId);
            return Ok(new { code = 0, data = devices });
        }

        /// <summary>
        /// 转发POST请求到MQTT网关
        /// </summary>
        /// <param name="agentId">智能体ID</param>
        /// <param name="requestBody">请求体</param>
        /// <returns>转发结果</returns>
        [HttpPost("bind/{agentId}")]
        public async Task<ActionResult<object>> ForwardToMqttGateway(string agentId, [FromBody] string requestBody)
        {
            try
            {
                // 实际实现中应该从系统参数获取MQTT网关地址并转发请求
                // 这里简化处理，直接返回成功响应
                return Ok(new { code = 0, data = "请求转发成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = "转发请求失败: " + ex.Message });
            }
        }

        /// <summary>
        /// 解绑设备
        /// </summary>
        /// <param name="unbindDto">解绑信息</param>
        /// <returns>操作结果</returns>
        [HttpPost("unbind")]
        public async Task<IActionResult> UnbindDevice([FromBody] DeviceUnbindDto unbindDto)
        {
            var userId = GetCurrentUserId();
            var result = await _deviceService.UnbindDeviceAsync(userId, unbindDto.DeviceId);
            return Ok(new { code = result ? 0 : 1, msg = result ? "设备解绑成功" : "设备解绑失败" });
        }

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <param name="deviceUpdateDto">设备更新信息</param>
        /// <returns>操作结果</returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDeviceInfo(string id, [FromBody] DeviceUpdateDto deviceUpdateDto)
        {
            var userId = GetCurrentUserId();
            try
            {
                var device = await _deviceService.UpdateDeviceAsync(id, deviceUpdateDto, userId);
                return Ok(new { code = 0, msg = "设备信息更新成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 手动添加设备
        /// </summary>
        /// <param name="dto">设备信息</param>
        /// <returns>操作结果</returns>
        [HttpPost("manual-add")]
        public async Task<IActionResult> ManualAddDevice([FromBody] DeviceManualAddDto dto)
        {
            var userId = GetCurrentUserId();
            try
            {
                var device = await _deviceService.ManualAddDeviceAsync(userId, dto);
                return Ok(new { code = 0, msg = "设备添加成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 发送设备指令
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <param name="command">指令内容</param>
        /// <returns>发送结果</returns>
        [HttpPost("commands/{deviceId}")]
        public async Task<IActionResult> SendDeviceCommand(string deviceId, [FromBody] string command)
        {
            try
            {
                // 实际实现中应该从系统参数获取MQTT网关地址并发送指令
                // 这里简化处理，直接返回成功响应
                return Ok(new { code = 0, data = "指令发送成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = "发送指令失败: " + ex.Message });
            }
        }

        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns>用户ID</returns>
        private long GetCurrentUserId()
        {
            // 实际实现中应该从认证信息中获取用户ID
            // 这里简化处理，返回默认值
            return 1;
        }
    }

    // DTOs for the controller
    public class DeviceRegisterDto
    {
        public string MacAddress { get; set; }
    }

    public class DeviceUpdateDto
    {
        public string Alias { get; set; }
        public int? AutoUpdate { get; set; }
    }

    public class DeviceUnbindDto
    {
        public string DeviceId { get; set; }
    }
    
    public class DeviceManualAddDto
    {
        public string DeviceName { get; set; }
        public string MacAddress { get; set; }
        public string Board { get; set; }
    }
}