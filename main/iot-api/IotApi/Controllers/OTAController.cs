using Microsoft.AspNetCore.Mvc;
using IotApi.Models;
using IotApi.Services;
using IotApi.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IotApi.Controllers
{
    /// <summary>
    /// OTA管理
    /// </summary>
    [ApiController]
    [Route("xiaozhi/ota")]
    [Produces("application/json")]
    [Tags("设备管理")]
    public class OTAController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public OTAController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        /// <summary>
        /// OTA版本和设备激活状态检查
        /// </summary>
        /// <param name="deviceReportReqDto">设备上报信息</param>
        /// <param name="deviceId">设备ID</param>
        /// <param name="clientId">客户端ID</param>
        /// <returns>检查结果</returns>
        [HttpPost]
        public async Task<ActionResult<string>> CheckOTAVersion(
            [FromBody] DeviceReportReqDto deviceReportReqDto,
            [FromHeader(Name = "Device-Id")] string deviceId,
            [FromHeader(Name = "Client-Id")] string clientId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return BadRequest(CreateErrorResponse("设备ID不能为空"));
            }

            if (string.IsNullOrEmpty(clientId))
            {
                clientId = deviceId;
            }

            bool macAddressValid = IsMacAddressValid(deviceId);
            if (!macAddressValid)
            {
                return BadRequest(CreateErrorResponse("无效的设备ID"));
            }

            var response = await _deviceService.CheckDeviceActiveAsync(deviceId, clientId, deviceReportReqDto);
            return Ok(CreateResponse(response));
        }

        /// <summary>
        /// 设备快速检查激活状态
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <param name="clientId">客户端ID</param>
        /// <returns>激活状态</returns>
        [HttpPost("activate")]
        public async Task<ActionResult<string>> ActivateDevice(
            [FromHeader(Name = "Device-Id")] string deviceId,
            [FromHeader(Name = "Client-Id")] string clientId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return StatusCode(202); // Accepted
            }

            var device = await _deviceService.GetDeviceByMacAddressAsync(deviceId);
            if (device == null)
            {
                return StatusCode(202);
            }

            return Ok("success");
        }

        /// <summary>
        /// 检查OTA服务状态
        /// </summary>
        /// <returns>服务状态</returns>
        [HttpGet]
        public ActionResult<string> GetOTA()
        {
            // 简单返回服务状态
            return Ok("OTA接口运行正常");
        }

        private bool IsMacAddressValid(string macAddress)
        {
            if (string.IsNullOrEmpty(macAddress))
            {
                return false;
            }

            // MAC地址验证
            var macPattern = @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$";
            return System.Text.RegularExpressions.Regex.IsMatch(macAddress, macPattern);
        }

        private string CreateErrorResponse(string message)
        {
            var response = new DeviceReportRespDto
            {
                Status = "error",
                Message = message
            };
            return System.Text.Json.JsonSerializer.Serialize(response);
        }

        private string CreateResponse(DeviceReportRespDto response)
        {
            return System.Text.Json.JsonSerializer.Serialize(response);
        }
    }

    // DTOs for the controller
    public class DeviceReportReqDto
    {
        public string Application { get; set; }
        public string Version { get; set; }
        public string Board { get; set; }
        public string MacAddress { get; set; }
    }

    public class DeviceReportRespDto
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string OtaUrl { get; set; }
        public string Version { get; set; }
    }
}