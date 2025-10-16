using IotApi.DTOs;

namespace IotApi.Services
{
    /// <summary>
    /// 声音克隆服务接口
    /// </summary>
    public interface IVoiceCloneService
    {
        /// <summary>
        /// 分页查询声音克隆列表（包含模型、用户名称）
        /// </summary>
        Task<PageResult<VoiceCloneResponseDto>> GetPageAsync(VoiceCloneQueryDto query);

        /// <summary>
        /// 根据ID获取声音克隆详情（包含模型、用户名称）
        /// </summary>
        Task<VoiceCloneResponseDto> GetByIdAsync(string id);

        /// <summary>
        /// 新增声音克隆记录（批量创建）
        /// </summary>
        Task SaveAsync(VoiceCloneDto dto);

        /// <summary>
        /// 批量删除声音克隆记录
        /// </summary>
        Task DeleteAsync(string[] ids);

        /// <summary>
        /// 上传音频文件
        /// </summary>
        Task UploadVoiceAsync(string id, IFormFile voiceFile);

        /// <summary>
        /// 更新声音名称
        /// </summary>
        Task UpdateNameAsync(string id, string name);

        /// <summary>
        /// 生成音频播放UUID并缓存映射关系
        /// </summary>
        Task<string> GenerateAudioUuidAsync(string id);

        /// <summary>
        /// 根据UUID获取音频二进制数据
        /// </summary>
        Task<byte[]> GetVoiceDataByUuidAsync(string uuid);

        /// <summary>
        /// 触发声音克隆训练流程
        /// </summary>
        Task StartCloneAsync(string cloneId);
    }
}