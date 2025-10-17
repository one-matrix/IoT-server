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
        private readonly ISysParamsService _sysService;

        public SecurityService(ApplicationDbContext context, ISysParamsService sysService)
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
        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            // 验证参数
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
            {
                throw new ArgumentException("用户名或密码不能为空");
            }

            // 验证验证码
            if (!string.IsNullOrEmpty(loginDto.CaptchaId) && !string.IsNullOrEmpty(loginDto.Captcha))
            {
                var isValid = await ValidateCaptchaAsync(loginDto.CaptchaId, loginDto.Captcha);
                if (!isValid)
                {
                    throw new UnauthorizedAccessException("验证码错误");
                }
            }

            // 获取用户
            var user = await _context.SysUsers
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null)
            {
                throw new UnauthorizedAccessException("账号或密码错误");
            }

            // 验证密码
            // 注意：实际应用中应使用加密算法比对密码
            if (user.Password != loginDto.Password)
            {
                throw new UnauthorizedAccessException("账号或密码错误");
            }

            // 检查用户状态
            if (user.Status != 1)
            {
                throw new UnauthorizedAccessException("账号已被禁用");
            }

            // 生成token
            var token = Guid.NewGuid().ToString("N");
            var expireTime = DateTime.UtcNow.AddHours(24); // 24小时有效期

            // 查找是否已有token
            var userToken = await _context.SysUserTokens
                .FirstOrDefaultAsync(t => t.UserId == user.Id);

            if (userToken == null)
            {
                // 创建新token
                userToken = new SysUserToken
                {
                    UserId = user.Id,
                    Token = token,
                    ExpireDate = expireTime,
                    UpdateDate = DateTime.UtcNow
                };
                _context.SysUserTokens.Add(userToken);
            }
            else
            {
                // 更新token
                userToken.Token = token;
                userToken.ExpireDate = expireTime;
                userToken.UpdateDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            // 返回token信息
            return new TokenDto
            {
                Token = token,
                ExpireTime = expireTime
            };
        }

        /// <inheritdoc/>
        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            // 验证参数
            if (registerDto == null || string.IsNullOrEmpty(registerDto.Username) || string.IsNullOrEmpty(registerDto.Password))
            {
                throw new ArgumentException("用户名或密码不能为空");
            }

            // 检查是否允许注册
            var isAllowed = await IsUserRegistrationAllowedAsync();
            if (!isAllowed)
            {
                throw new InvalidOperationException("用户注册已禁用");
            }

            // 验证验证码
            if (!string.IsNullOrEmpty(registerDto.CaptchaId) && !string.IsNullOrEmpty(registerDto.Captcha))
            {
                var isValid = await ValidateCaptchaAsync(registerDto.CaptchaId, registerDto.Captcha);
                if (!isValid)
                {
                    throw new InvalidOperationException("验证码错误");
                }
            }

            // 检查用户是否已存在
            var existingUser = await _context.SysUsers
                .FirstOrDefaultAsync(u => u.Username == registerDto.Username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("用户名已被注册");
            }

            // 创建新用户
            var user = new SysUser
            {
                Username = registerDto.Username,
                Password = registerDto.Password, // 实际应用中应加密存储
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                Status = 1 // 默认启用
            };

            _context.SysUsers.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc/>
        public async Task<UserDetailDto> GetUserInfoAsync(long userId)
        {
            var user = await _context.SysUsers
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user == null)
            {
                throw new KeyNotFoundException("用户不存在");
            }

            return new UserDetailDto
            {
                UserId = user.Id,
                Username = user.Username,
                Status = user.Status,
                CreateDate = user.CreateDate,
                UpdateDate = user.UpdateDate
            };
        }

        /// <inheritdoc/>
        public async Task<bool> ChangePasswordAsync(long userId, PasswordDto passwordDto)
        {
            if (passwordDto == null || string.IsNullOrEmpty(passwordDto.OldPassword) || string.IsNullOrEmpty(passwordDto.NewPassword))
            {
                throw new ArgumentException("密码参数不能为空");
            }

            var user = await _context.SysUsers
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user == null)
            {
                throw new KeyNotFoundException("用户不存在");
            }

            // 验证旧密码
            if (user.Password != passwordDto.OldPassword) // 实际应用中应使用加密比对
            {
                throw new InvalidOperationException("原密码错误");
            }

            // 更新密码
            user.Password = passwordDto.NewPassword; // 实际应用中应加密存储
            user.UpdateDate = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> RetrievePasswordAsync(RetrievePasswordDto retrievePasswordDto)
        {
            if (retrievePasswordDto == null || string.IsNullOrEmpty(retrievePasswordDto.Username) || 
                string.IsNullOrEmpty(retrievePasswordDto.NewPassword))
            {
                throw new ArgumentException("用户名或新密码不能为空");
            }

            // 查找用户
            var user = await _context.SysUsers
                .FirstOrDefaultAsync(u => u.Username == retrievePasswordDto.Username);
            
            if (user == null)
            {
                throw new KeyNotFoundException("用户不存在");
            }

            // 验证验证码（如果提供）
            if (!string.IsNullOrEmpty(retrievePasswordDto.CaptchaId) && !string.IsNullOrEmpty(retrievePasswordDto.Captcha))
            {
                var isValid = await ValidateCaptchaAsync(retrievePasswordDto.CaptchaId, retrievePasswordDto.Captcha);
                if (!isValid)
                {
                    throw new InvalidOperationException("验证码错误");
                }
            }

            // 更新密码
            user.Password = retrievePasswordDto.NewPassword; // 实际应用中应加密存储
            user.UpdateDate = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            return true;
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