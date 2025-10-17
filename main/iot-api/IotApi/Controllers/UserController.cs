using Microsoft.AspNetCore.Mvc;
using IotApi.Services;
using IotApi.DTOs;
using System.ComponentModel.DataAnnotations;
using IotApi.Models;

namespace IotApi.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [ApiController]
[Route("xiaozhi/user-management")]
[Produces("application/json")]
[Tags("用户管理")]
public class UserController : ControllerBase
    {
        private readonly ISecurityService _securityService;

        public UserController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginDto">登录信息</param>
        /// <returns>登录结果</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { code = 1, msg = "请求参数错误" });
            }

            try
            {
                var tokenDto = await _securityService.LoginAsync(loginDto);
                return Ok(new { code = 0, data = tokenDto });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { code = 1, msg = ex.Message });
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns>用户信息</returns>
        [HttpGet("info")]
        public async Task<IActionResult> GetUserInfo()
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

        // POST: xiaozhi/user/register
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
    }
}