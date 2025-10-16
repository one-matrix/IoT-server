using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IotApi.DTOs;
using IotApi.Models;
using Microsoft.EntityFrameworkCore;

namespace IotApi.Services
{
    /// <summary>
    /// 安全服务实现
    /// </summary>
    public class SecurityService : ISecurityService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISysService _sysService;

        public SecurityService(ApplicationDbContext context, ISysService sysService)
        {
            _context = context;
            _sysService = sysService;
        }

        /// <inheritdoc/>
        public Task<(string captchaCode, string uuid)> GenerateCaptchaAsync(string uuid)
        {
            // 简单生成 4 位数字验证码（示例实现）
            var random = new Random();
            var code = random.Next(1000, 9999).ToString();
            // 此处未持久化验证码，仅返回（前端可直接使用或后续扩展存储）
            return Task.FromResult((code, uuid));
        }

        /// <inheritdoc/>
        public Task<bool> ValidateCaptchaAsync(string uuid, string captchaCode)
        {
            // 由于未持久化验证码，此处返回 true（后续可接入缓存或数据库）
            return Task.FromResult(true);
        }

        /// <inheritdoc/>
        public Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            throw new NotImplementedException("登录功能暂未实现");
        }

        /// <inheritdoc/>
        public Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            throw new NotImplementedException("注册功能暂未实现");
        }

        /// <inheritdoc/>
        public Task<UserDetailDto> GetUserInfoAsync(long userId)
        {
            throw new NotImplementedException("获取用户信息功能暂未实现");
        }

        /// <inheritdoc/>
        public Task<bool> ChangePasswordAsync(long userId, PasswordDto passwordDto)
        {
            throw new NotImplementedException("修改密码功能暂未实现");
        }

        /// <inheritdoc/>
        public Task<bool> RetrievePasswordAsync(RetrievePasswordDto retrievePasswordDto)
        {
            throw new NotImplementedException("找回密码功能暂未实现");
        }

        /// <inheritdoc/>
        public Task<bool> SendSmsVerificationAsync(SmsVerificationDto smsVerificationDto)
        {
            throw new NotImplementedException("短信验证码功能暂未实现");
        }

        /// <inheritdoc/>
        public async Task<ServerConfigDto> GetServerConfigAsync()
        {
            var sysParams = await _context.SysParams.ToListAsync();

            string GetParam(string code, string defaultValue = "")
                => sysParams.FirstOrDefault(p => p.ParamCode == code)?.ParamValue ?? defaultValue;

            bool GetBoolParam(string code, bool defaultValue = false)
            {
                var val = GetParam(code, defaultValue ? "true" : "false");
                if (string.Equals(val, "true", StringComparison.OrdinalIgnoreCase)) return true;
                if (string.Equals(val, "false", StringComparison.OrdinalIgnoreCase)) return false;
                // number-like
                if (int.TryParse(val, out var num)) return num != 0;
                return defaultValue;
            }

            var allowUserRegister = GetBoolParam("server.allow_user_register", false);
            var enableMobileRegister = GetBoolParam("server.enable_mobile_register", false);
            var beianIcpNum = GetParam("server.beian_icp_num", "null");
            var beianGaNum = GetParam("server.beian_ga_num", "null");
            var name = GetParam("server.name", "xiaozhi-esp32-server");
            var sm2PublicKey = GetParam("server.public_key", "");

            // 手机区号列表，来源于字典类型 MOBILE_AREA
            var mobileAreas = await _sysService.GetDictDataByTypeAsync("MOBILE_AREA");

            // 版本与年份（与 Java 行为一致）
            var version = "1.0.0"; // 可改为读取程序集版本
            var year = "©" + DateTime.Now.Year;

            // 若未配置 SM2 公钥，可选择抛出异常或返回空字符串；此处返回空字符串，由控制器统一处理
            var config = new ServerConfigDto
            {
                AllowUserRegister = allowUserRegister,
                EnableMobileRegister = enableMobileRegister,
                Version = version,
                Year = year,
                MobileAreaList = mobileAreas,
                BeianIcpNum = beianIcpNum,
                BeianGaNum = beianGaNum,
                Name = name,
                Sm2PublicKey = sm2PublicKey
            };

            return config;
        }

        /// <inheritdoc/>
        public async Task<long> ValidateTokenAsync(string token)
        {
            var entity = await _context.SysUserTokens.FirstOrDefaultAsync(t => t.Token == token);
            if (entity == null || (entity.ExpireDate.HasValue && entity.ExpireDate.Value < DateTime.UtcNow))
            {
                throw new UnauthorizedAccessException("令牌无效或已过期");
            }
            return entity.UserId;
        }

        /// <inheritdoc/>
        public Task<bool> IsUserRegistrationAllowedAsync()
        {
            // 读取参数 server.allow_user_register
            return _context.SysParams
                .Where(p => p.ParamCode == "server.allow_user_register")
                .Select(p => p.ParamValue)
                .Select(val =>
                {
                    if (string.Equals(val, "true", StringComparison.OrdinalIgnoreCase)) return true;
                    if (string.Equals(val, "false", StringComparison.OrdinalIgnoreCase)) return false;
                    if (int.TryParse(val, out var num)) return num != 0;
                    return false;
                })
                .FirstOrDefaultAsync();
        }
    }
}