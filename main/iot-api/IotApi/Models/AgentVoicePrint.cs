using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("ai_agent_voice_print")]
    public class AgentVoicePrint
    {
        [Key]
        [StringLength(32)]
        [Column("id")]
        public string Id { get; set; }
        
        [StringLength(64)]
        [Column("name")]
        public string Name { get; set; }
        
        [Column("user_id")]
        public long? UserId { get; set; }
        
        [StringLength(32)]
        [Column("agent_id")]
        public string AgentId { get; set; }
        
        [StringLength(36)]
        [Column("agent_code")]
        public string AgentCode { get; set; }
        
        [StringLength(36)]
        [Column("agent_name")]
        public string AgentName { get; set; }
        
        [StringLength(255)]
        [Column("description")]
        public string Description { get; set; }
        
        [Column("embedding")]
        public string Embedding { get; set; }
        
        [Column("memory")]
        public string Memory { get; set; }
        
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