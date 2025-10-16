using IotApi.DTOs;
using IotApi.Models;

namespace IotApi.Services
{
    /// <summary>
    /// 安全服务接口
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="uuid">唯一标识</param>
        /// <returns>验证码信息</returns>
        Task<(string captchaCode, string uuid)> GenerateCaptchaAsync(string uuid);

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="uuid">唯一标识</param>
        /// <param name="captchaCode">验证码</param>
        /// <returns>是否验证成功</returns>
        Task<bool> ValidateCaptchaAsync(string uuid, string captchaCode);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginDto">登录信息</param>
        /// <returns>令牌信息</returns>
        Task<TokenDto> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="registerDto">注册信息</param>
        /// <returns>是否注册成功</returns>
        Task<bool> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户详情</returns>
        Task<UserDetailDto> GetUserInfoAsync(long userId);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="passwordDto">密码信息</param>
        /// <returns>是否修改成功</returns>
        Task<bool> ChangePasswordAsync(long userId, PasswordDto passwordDto);

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="retrievePasswordDto">找回密码信息</param>
        /// <returns>是否找回成功</returns>
        Task<bool> RetrievePasswordAsync(RetrievePasswordDto retrievePasswordDto);

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="smsVerificationDto">短信验证信息</param>
        /// <returns>是否发送成功</returns>
        Task<bool> SendSmsVerificationAsync(SmsVerificationDto smsVerificationDto);

        /// <summary>
        /// 获取服务器配置
        /// </summary>
        /// <returns>服务器配置</returns>
        Task<ServerConfigDto> GetServerConfigAsync();

        /// <summary>
        /// 验证令牌
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns>用户ID</returns>
        Task<long> ValidateTokenAsync(string token);

        /// <summary>
        /// 是否允许用户注册
        /// </summary>
        /// <returns>是否允许</returns>
        Task<bool> IsUserRegistrationAllowedAsync();
    }
}