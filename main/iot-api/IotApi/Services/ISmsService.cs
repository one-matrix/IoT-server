namespace IotApi.Services
{
    /// <summary>
    /// 短信服务接口
    /// </summary>
    public interface ISmsService
    {
        /// <summary>
        /// 发送验证码短信
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="verificationCode">验证码</param>
        /// <returns>发送结果</returns>
        Task<bool> SendVerificationCodeSmsAsync(string phone, string verificationCode);
    }
}