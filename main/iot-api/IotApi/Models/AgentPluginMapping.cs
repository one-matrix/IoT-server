using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("ai_agent_plugin_mapping")]
    public class AgentPluginMapping
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        
        [StringLength(32)]
        [Column("agent_id")]
        public string AgentId { get; set; }
        
        [StringLength(64)]
        [Column("plugin_id")]
        public string PluginId { get; set; }
        
        [Column("enabled")]
        public bool Enabled { get; set; }
        
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
        
        [StringLength(64)]
        [Column("provider_code")]
        public string ProviderCode { get; set; }
        
        [Column("param_info")]
        public string ParamInfo { get; set; }
    }
}