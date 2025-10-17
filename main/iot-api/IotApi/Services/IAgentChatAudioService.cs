namespace IotApi.Services
{
    /// <summary>
    /// 智能体聊天音频数据服务接口
    /// </summary>
    public interface IAgentChatAudioService
    {
        /// <summary>
        /// 保存音频数据
        /// </summary>
        /// <param name="audioData">音频数据</param>
        /// <returns>音频ID</returns>
        Task<string> SaveAudioAsync(byte[] audioData);

        /// <summary>
        /// 获取音频数据
        /// </summary>
        /// <param name="audioId">音频ID</param>
        /// <returns>音频数据</returns>
        Task<byte[]> GetAudioAsync(string audioId);
    }
}