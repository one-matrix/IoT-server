using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("ai_agent")]
    public class AiAgent
    {
        [Key]
        [StringLength(32)]
        [Column("id")]
        public string Id { get; set; }
        
        [Column("user_id")]
        public long? UserId { get; set; }
        
        [StringLength(36)]
        [Column("agent_code")]
        public string AgentCode { get; set; }
        
        [StringLength(64)]
        [Column("agent_name")]
        public string AgentName { get; set; }
        
        [StringLength(32)]
        [Column("asr_model_id")]
        public string AsrModelId { get; set; }
        
        [StringLength(64)]
        [Column("vad_model_id")]
        public string VadModelId { get; set; }
        
        [StringLength(32)]
        [Column("llm_model_id")]
        public string LlmModelId { get; set; }
        
        [StringLength(32)]
        [Column("vllm_model_id")]
        public string VllmModelId { get; set; }
        
        [StringLength(32)]
        [Column("tts_model_id")]
        public string TtsModelId { get; set; }
        
        [StringLength(32)]
        [Column("tts_voice_id")]
        public string TtsVoiceId { get; set; }
        
        [StringLength(32)]
        [Column("mem_model_id")]
        public string MemModelId { get; set; }
        
        [StringLength(32)]
        [Column("intent_model_id")]
        public string IntentModelId { get; set; }
        
        [Column("chat_history_conf")]
        public int? ChatHistoryConf { get; set; }
        
        [Column("system_prompt")]
        public string SystemPrompt { get; set; }
        
        [Column("summary_memory")]
        public string SummaryMemory { get; set; }
        
        [StringLength(10)]
        [Column("lang_code")]
        public string LangCode { get; set; }
        
        [StringLength(10)]
        [Column("language")]
        public string Language { get; set; }
        
        [Column("sort")]
        public int? Sort { get; set; }
        
        [Column("creator")]
        public long? Creator { get; set; }
        
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [Column("updater")]
        public long? Updater { get; set; }
        
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}