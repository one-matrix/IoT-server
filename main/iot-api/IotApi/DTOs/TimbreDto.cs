using System.ComponentModel.DataAnnotations;

namespace IotApi.DTOs
{
    /// <summary>
    /// 音色数据传输对象
    /// </summary>
    public class TimbreDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// 对应 TTS 模型主键
        /// </summary>
        [Required(ErrorMessage = "TTS模型ID不能为空")]
        public string TtsModelId { get; set; }
        
        /// <summary>
        /// 音色名称
        /// </summary>
        [Required(ErrorMessage = "音色名称不能为空")]
        [StringLength(20, ErrorMessage = "音色名称长度不能超过20个字符")]
        public string Name { get; set; }
        
        /// <summary>
        /// 音色编码
        /// </summary>
        [Required(ErrorMessage = "音色编码不能为空")]
        [StringLength(50, ErrorMessage = "音色编码长度不能超过50个字符")]
        public string TtsVoice { get; set; }
        
        /// <summary>
        /// 语言
        /// </summary>
        [Required(ErrorMessage = "语言不能为空")]
        [StringLength(50, ErrorMessage = "语言长度不能超过50个字符")]
        public string Languages { get; set; }
        
        /// <summary>
        /// 音频播放地址
        /// </summary>
        [StringLength(500, ErrorMessage = "音频播放地址长度不能超过500个字符")]
        public string VoiceDemo { get; set; }
        
        /// <summary>
        /// 参考音频路径
        /// </summary>
        [StringLength(500, ErrorMessage = "参考音频路径长度不能超过500个字符")]
        public string ReferenceAudio { get; set; }
        
        /// <summary>
        /// 参考文本
        /// </summary>
        public string ReferenceText { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(255, ErrorMessage = "备注长度不能超过255个字符")]
        public string Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public long Sort { get; set; }
        
        /// <summary>
        /// 创建者
        /// </summary>
        public long? Creator { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }
        
        /// <summary>
        /// 更新者
        /// </summary>
        public long? Updater { get; set; }
        
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateDate { get; set; }
    }

    /// <summary>
    /// 音色查询参数
    /// </summary>
    public class TimbreQueryDto
    {
        /// <summary>
        /// 对应 TTS 模型主键
        /// </summary>
        public string TtsModelId { get; set; }
        
        /// <summary>
        /// 音色名称
        /// </summary>
        public string Name { get; set; }
        
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
    }
}