using System.ComponentModel.DataAnnotations;

namespace IotApi.DTOs
{
    /// <summary>
    /// 声音克隆创建DTO
    /// </summary>
    public class VoiceCloneDto
    {
        /// <summary>
        /// 模型ID
        /// </summary>
        [Required(ErrorMessage = "模型ID不能为空")]
        public string ModelId { get; set; }

        /// <summary>
        /// 音色ID列表
        /// </summary>
        [Required(ErrorMessage = "音色ID列表不能为空")]
        [MinLength(1, ErrorMessage = "至少需要一个音色ID")]
        public string[] VoiceIds { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required(ErrorMessage = "用户ID不能为空")]
        public long? UserId { get; set; }
    }

    /// <summary>
    /// 声音克隆查询参数
    /// </summary>
    public class VoiceCloneQueryDto
    {
        /// <summary>
        /// 页码
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "页码必须大于0")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每页记录数
        /// </summary>
        [Range(1, 1000, ErrorMessage = "每页记录数必须在1-1000之间")]
        public int Limit { get; set; } = 10;

        /// <summary>
        /// 可选：用户ID过滤
        /// </summary>
        public long? UserId { get; set; }
    }

    /// <summary>
    /// 声音克隆响应DTO
    /// </summary>
    public class VoiceCloneResponseDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 声音名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模型id
        /// </summary>
        public string ModelId { get; set; }

        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 声音id
        /// </summary>
        public string VoiceId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 训练状态：0待训练 1训练中 2训练成功 3训练失败
        /// </summary>
        public int? TrainStatus { get; set; }

        /// <summary>
        /// 训练错误原因
        /// </summary>
        public string TrainError { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 是否有音频数据
        /// </summary>
        public bool? HasVoice { get; set; }
    }
}