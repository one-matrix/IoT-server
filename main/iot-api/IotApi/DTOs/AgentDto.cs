using System.ComponentModel.DataAnnotations;

namespace IotApi.DTOs
{
    /// <summary>
    /// 智能体数据传输对象
    /// </summary>
    public class AgentDto
    {
        /// <summary>
        /// 智能体代码
        /// </summary>
        [Required(ErrorMessage = "智能体代码不能为空")]
        [StringLength(50, ErrorMessage = "智能体代码长度不能超过50个字符")]
        public string AgentCode { get; set; }
        
        /// <summary>
        /// 智能体名称
        /// </summary>
        [Required(ErrorMessage = "智能体名称不能为空")]
        [StringLength(100, ErrorMessage = "智能体名称长度不能超过100个字符")]
        public string AgentName { get; set; }
        
        /// <summary>
        /// ASR模型ID
        /// </summary>
        public string AsrModelId { get; set; }
        
        /// <summary>
        /// VAD模型ID
        /// </summary>
        public string VadModelId { get; set; }
        
        /// <summary>
        /// LLM模型ID
        /// </summary>
        public string LlmModelId { get; set; }
        
        /// <summary>
        /// VLLM模型ID
        /// </summary>
        public string VllmModelId { get; set; }
        
        /// <summary>
        /// TTS模型ID
        /// </summary>
        public string TtsModelId { get; set; }
        
        /// <summary>
        /// TTS语音ID
        /// </summary>
        public string TtsVoiceId { get; set; }
        
        /// <summary>
        /// 记忆模型ID
        /// </summary>
        public string MemModelId { get; set; }
        
        /// <summary>
        /// 意图模型ID
        /// </summary>
        public string IntentModelId { get; set; }
        
        /// <summary>
        /// 系统提示词
        /// </summary>
        public string SystemPrompt { get; set; }
        
        /// <summary>
        /// 语言代码
        /// </summary>
        public string LangCode { get; set; }
        
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }
        
        /// <summary>
        /// 聊天历史配置
        /// </summary>
        public string ChatHistoryConf { get; set; }
        
        /// <summary>
        /// 摘要记忆
        /// </summary>
        public string SummaryMemory { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }
    }
}