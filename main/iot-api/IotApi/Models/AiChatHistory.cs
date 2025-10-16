using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("ai_chat_history")]
    public class AiChatHistory
    {
        [Key]
        [StringLength(32)]
        [Column("id")]
        public string Id { get; set; }
        
        [Column("user_id")]
        public long? UserId { get; set; }
        
        [StringLength(32)]
        [Column("agent_id")]
        public string AgentId { get; set; }
        
        [StringLength(32)]
        [Column("device_id")]
        public string DeviceId { get; set; }
        
        [Column("message_count")]
        public int? MessageCount { get; set; }
        
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