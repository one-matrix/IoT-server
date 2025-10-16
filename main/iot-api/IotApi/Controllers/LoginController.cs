using Microsoft.AspNetCore.Mvc;
using IotApi.Services;
using IotApi.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IotApi.Controllers
{
    /// <summary>
    /// 登录控制器
    /// </summary>
    [ApiController]
    [Route("xiaozhi/user")]
    [Produces("application/json")]
    [Tags("用户认证")]
    public class LoginController : ControllerBase
    {
        private readonly ISecurityService _securityService;

        public LoginController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="uuid">唯一标识</param>
        /// <returns>验证码信息</returns>
        [HttpGet("captcha")]
        public async Task<IActionResult> Captcha(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
            {
                return BadRequest(new { code = 1, msg = "UUID不能为空" });
            }

            var (captchaCode, generatedUuid) = await _securityService.GenerateCaptchaAsync(uuid);
            return Ok(new { code = 0, data = new { uuid = generatedUuid, captcha = captchaCode } });
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="login">登录信息</param>
        /// <returns>登录结果</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { code = 1, msg = "请求参数错误" });
            }

            try
            {
                var tokenDto = await _securityService.LoginAsync(login);
                return Ok(new { code = 0, data = tokenDto });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { code = 1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="registerDto">注册信息</param>
        /// <returns>注册结果</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { code = 1, msg = "请求参数错误" });
            }

            try
            {
                var isAllowed = await _securityService.IsUserRegistrationAllowedAsync();
                if (!isAllowed)
                {
                    return BadRequest(new { code = 1, msg = "用户注册已禁用" });
                }

                var result = await _securityService.RegisterAsync(registerDto);
                return Ok(new { code = 0 });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns>用户信息</returns>
        [HttpGet("info")]
        public async Task<IActionResult> Info()
        {
            try
            {
                // 从请求头中获取Token
                var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(new { code = 1, msg = "未授权" });
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();
                var userId = await _securityService.ValidateTokenAsync(token);
                var userDetail = await _securityService.GetUserInfoAsync(userId);

                return Ok(new { code = 0, data = userDetail });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { code = 1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="passwordDto">密码信息</param>
        /// <returns>修改结果</returns>
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordDto passwordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { code = 1, msg = "请求参数错误" });
            }

            try
            {
                // 从请求头中获取Token
                var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(new { code = 1, msg = "未授权" });
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();
                var userId = await _securityService.ValidateTokenAsync(token);
                var result = await _securityService.ChangePasswordAsync(userId, passwordDto);

                return Ok(new { code = 0 });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = ex.Message });
            }
        }

        // PUT: xiaozhi/user/retrieve-password（与 Java 兼容）
        [HttpPut("retrieve-password")]
        public async Task<IActionResult> RetrievePasswordPut([FromBody] RetrievePasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { code = 1, msg = "请求参数错误" });
            }

            try
            {
                var result = await _securityService.RetrievePasswordAsync(dto);
                return Ok(new { code = 0 });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="retrievePasswordDto">找回密码信息</param>
        /// <returns>找回结果</returns>
        [HttpPost("retrieve-password")]
        public async Task<IActionResult> RetrievePassword([FromBody] RetrievePasswordDto retrievePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { code = 1, msg = "请求参数错误" });
            }

            try
            {
                var result = await _securityService.RetrievePasswordAsync(retrievePasswordDto);
                return Ok(new { code = 0 });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="smsVerificationDto">短信验证信息</param>
        /// <returns>发送结果</returns>
        [HttpPost("sms-verification")]
        public async Task<IActionResult> SmsVerification([FromBody] SmsVerificationDto smsVerificationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { code = 1, msg = "请求参数错误" });
            }

            try
            {
                var result = await _securityService.SendSmsVerificationAsync(smsVerificationDto);
                return Ok(new { code = 0 });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 发送短信验证码（兼容 Java 路由）
        /// </summary>
        /// <param name="smsVerificationDto">短信验证信息</param>
        /// <returns>发送结果</returns>
        [HttpPost("smsVerification")]
        public async Task<IActionResult> SmsVerificationCompat([FromBody] SmsVerificationDto smsVerificationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { code = 1, msg = "请求参数错误" });
            }

            try
            {
                var result = await _securityService.SendSmsVerificationAsync(smsVerificationDto);
                return Ok(new { code = 0 });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 获取服务器配置
        /// </summary>
        /// <returns>服务器配置</returns>
        [HttpGet("server-config")]
        public async Task<IActionResult> ServerConfig()
        {
            try
            {
                var config = await _securityService.GetServerConfigAsync();
                return Ok(new { code = 0, data = config });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 公共配置（兼容 Java 路由）
        /// </summary>
        /// <returns>公共配置</returns>
        [HttpGet("pub-config")]
        public async Task<IActionResult> PubConfig()
        {
            try
            {
                var config = await _securityService.GetServerConfigAsync();
                return Ok(new { code = 0, data = config });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 1, msg = ex.Message });
            }
        }
    }
}