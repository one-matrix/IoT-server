using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using IotApi.Models;

namespace IotApi.Services
{
    /// <summary>
    /// 智能体聊天音频数据服务实现类
    /// </summary>
    public class AgentChatAudioService : IAgentChatAudioService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AgentChatAudioService> _logger;

        public AgentChatAudioService(
            ApplicationDbContext dbContext,
            ILogger<AgentChatAudioService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// 保存音频数据
        /// </summary>
        /// <param name="audioData">音频数据</param>
        /// <returns>音频ID</returns>
        public async Task<string> SaveAudioAsync(byte[] audioData)
        {
            try
            {
                if (audioData == null || audioData.Length == 0)
                {
                    _logger.LogWarning("尝试保存空的音频数据");
                    return null;
                }

                var audioEntity = new AgentChatAudio
                {
                    Id = Guid.NewGuid().ToString(),
                    AudioData = audioData,
                    CreateTime = DateTime.Now
                };

                await _dbContext.AiAgentChatAudios.AddAsync(audioEntity);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("音频数据保存成功，audioId={AudioId}", audioEntity.Id);
                return audioEntity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "音频数据保存失败");
                throw;
            }
        }

        /// <summary>
        /// 获取音频数据
        /// </summary>
        /// <param name="audioId">音频ID</param>
        /// <returns>音频数据</returns>
        public async Task<byte[]> GetAudioAsync(string audioId)
        {
            try
            {
                if (string.IsNullOrEmpty(audioId))
                {
                    _logger.LogWarning("尝试获取空ID的音频数据");
                    return null;
                }

                var audioEntity = await _dbContext.AiAgentChatAudios
                    .FirstOrDefaultAsync(a => a.Id == audioId);

                if (audioEntity == null)
                {
                    _logger.LogWarning("未找到ID为{AudioId}的音频数据", audioId);
                    return null;
                }

                return audioEntity.AudioData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取音频数据失败，audioId={AudioId}", audioId);
                throw;
            }
        }
    }
}