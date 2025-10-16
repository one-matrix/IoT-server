using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using IotApi.Models;
using IotApi.Services;
using IotApi.DTOs;

namespace IotApi.Controllers
{
    /// <summary>
    /// xiaozhi-server 配置获取
    /// </summary>
    [ApiController]
    [Route("xiaozhi/config")]
    [Produces("application/json")]
    [Tags("参数管理")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigService _configService;

        public ConfigController(IConfigService configService)
        {
            _configService = configService;
        }

        /// <summary>
        /// 服务端获取配置接口
        /// </summary>
        /// <returns>服务器配置信息</returns>
        [HttpPost("server-base")]
        public ActionResult<object> GetConfig()
        {
            var config = _configService.GetConfig(true);
            return Ok(new { code = 0, data = config });
        }

        /// <summary>
        /// 获取智能体模型
        /// </summary>
        /// <param name="dto">智能体模型请求参数</param>
        /// <returns>智能体模型信息</returns>
        [HttpPost("agent-models")]
        public ActionResult<object> GetAgentModels([FromBody] AgentModelsDto dto)
        {
            // 验证数据
            if (string.IsNullOrEmpty(dto.MacAddress))
            {
                return BadRequest(new { code = 1, msg = "MAC地址不能为空" });
            }

            var models = _configService.GetAgentModels(dto.MacAddress, dto.SelectedModule);
            return Ok(new { code = 0, data = models });
        }
    }
}