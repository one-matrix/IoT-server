using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("ai_device")]
    public class AiDevice
    {
        [Key]
        [StringLength(32)]
        [Column("id")]
        public string Id { get; set; }
        
        [Column("user_id")]
        public long? UserId { get; set; }
        
        [StringLength(50)]
        [Column("mac_address")]
        public string MacAddress { get; set; }
        
        [Column("last_connected_at")]
        public DateTime? LastConnectedAt { get; set; }
        
        [Column("auto_update")]
        public int? AutoUpdate { get; set; }
        
        [StringLength(50)]
        [Column("board")]
        public string Board { get; set; }
        
        [StringLength(64)]
        [Column("alias")]
        public string Alias { get; set; }
        
        [StringLength(32)]
        [Column("agent_id")]
        public string AgentId { get; set; }
        
        [StringLength(20)]
        [Column("app_version")]
        public string AppVersion { get; set; }
        
        [Column("sort")]
        public int? Sort { get; set; }
        
        [Column("creator")]
        public long? Creator { get; set; }
        
        [Column("create_date")]
        public DateTime? CreateDate { get; set; }
        
        [Column("updater")]
        public long? Updater { get; set; }
        
        [Column("update_date")]
        public DateTime? UpdatedAt { get; set; }
        
    }
}