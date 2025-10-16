using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("ai_agent_chat_history")]
    public class AiAgentChatHistory
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        
        [StringLength(64)]
        [Column("mac_address")]
        public string MacAddress { get; set; }
        
        [StringLength(32)]
        [Column("agent_id")]
        public string AgentId { get; set; }
        
        [StringLength(64)]
        [Column("session_id")]
        public string SessionId { get; set; }
        
        [Column("chat_type")]
        public byte ChatType { get; set; }
        
        [Column("content")]
        public string Content { get; set; }
        
        [StringLength(64)]
        [Column("audio_id")]
        public string AudioId { get; set; }
        
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}