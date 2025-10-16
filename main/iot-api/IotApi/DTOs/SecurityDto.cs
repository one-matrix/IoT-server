using System.ComponentModel.DataAnnotations;

namespace IotApi.DTOs
{
    /// <summary>
    /// 登录请求DTO
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        /// <summary>
        /// 验证码ID
        /// </summary>
        public string CaptchaId { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Captcha { get; set; }
    }

    /// <summary>
    /// 注册请求DTO
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        /// <summary>
        /// 验证码ID
        /// </summary>
        public string CaptchaId { get; set; }

        /// <summary>
        /// 手机验证码
        /// </summary>
        public string MobileCaptcha { get; set; }
    }

    /// <summary>
    /// 令牌DTO
    /// </summary>
    public class TokenDto
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireDate { get; set; }
    }

    /// <summary>
    /// 用户详情DTO
    /// </summary>
    public class UserDetailDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public int? SuperAdmin { get; set; }
    }

    /// <summary>
    /// 密码修改DTO
    /// </summary>
    public class PasswordDto
    {
        /// <summary>
        /// 当前密码
        /// </summary>
        [Required(ErrorMessage = "当前密码不能为空")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "新密码不能为空")]
        [MinLength(6, ErrorMessage = "密码长度不能少于6位")]
        public string NewPassword { get; set; }
    }

    /// <summary>
    /// 找回密码DTO
    /// </summary>
    public class RetrievePasswordDto
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "手机号不能为空")]
        [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "手机号格式不正确")]
        public string Phone { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "验证码不能为空")]
        public string Code { get; set; }

        /// <summary>
        /// 验证码ID（用于图形验证码或SM2解密校验）
        /// </summary>
        public string CaptchaId { get; set; }

        /// <summary>
        /// 新密码（与Java实现对齐，字段名为Password）
        /// </summary>
        [Required(ErrorMessage = "新密码不能为空")]
        [MinLength(6, ErrorMessage = "密码长度不能少于6位")]
        public string Password { get; set; }
    }

    /// <summary>
    /// 短信验证DTO
    /// </summary>
    public class SmsVerificationDto
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "手机号不能为空")]
        [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "手机号格式不正确")]
        public string Phone { get; set; }

        /// <summary>
        /// 验证码ID
        /// </summary>
        public string CaptchaId { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Captcha { get; set; }
    }

    /// <summary>
    /// 服务器配置DTO
    /// </summary>
    public class ServerConfigDto
    {
        /// <summary>
        /// 是否允许用户注册
        /// </summary>
        public bool AllowUserRegister { get; set; }

        /// <summary>
        /// 是否开启手机注册
        /// </summary>
        public bool EnableMobileRegister { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 年份版权信息
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// 手机区号列表
        /// </summary>
        public List<SysDictDataItem> MobileAreaList { get; set; }

        /// <summary>
        /// 备案ICP号
        /// </summary>
        public string BeianIcpNum { get; set; }

        /// <summary>
        /// 备案公安号
        /// </summary>
        public string BeianGaNum { get; set; }

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// SM2公钥
        /// </summary>
        public string Sm2PublicKey { get; set; }
    }

    /// <summary>
    /// 手机区号项
    /// </summary>
    public class MobileAreaItem
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 键值
        /// </summary>
        public string Key { get; set; }
    }
}