using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("ai_chat_message")]
    public class AiChatMessage
    {
        [Key]
        [StringLength(32)]
        [Column("id")]
        public string Id { get; set; }
        
        [Column("user_id")]
        public long? UserId { get; set; }
        
        [StringLength(64)]
        [Column("chat_id")]
        public string ChatId { get; set; }
        
        [StringLength(20)]
        [Column("role")]
        public string Role { get; set; }
        
        [Column("content")]
        public string Content { get; set; }
        
        [Column("prompt_tokens")]
        public int? PromptTokens { get; set; }
        
        [Column("total_tokens")]
        public int? TotalTokens { get; set; }
        
        [Column("completion_tokens")]
        public int? CompletionTokens { get; set; }
        
        [Column("prompt_ms")]
        public int? PromptMs { get; set; }
        
        [Column("total_ms")]
        public int? TotalMs { get; set; }
        
        [Column("completion_ms")]
        public int? CompletionMs { get; set; }
        
        [Column("creator")]
        public long? Creator { get; set; }
        
        [Column("create_date")]
        public DateTime? CreateDate { get; set; }
        
        [Column("updater")]
        public long? Updater { get; set; }
        
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
    }
}