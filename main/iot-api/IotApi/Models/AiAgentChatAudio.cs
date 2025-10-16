using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("ai_agent_chat_audio")]
    public class AiAgentChatAudio
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }
        
        [Column("audio_data")]
        public string AudioData { get; set; }
        
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}