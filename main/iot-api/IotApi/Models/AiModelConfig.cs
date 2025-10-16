using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("ai_model_config")]
    public class AiModelConfig
    {
        [Key]
        [StringLength(32)]
        [Column("id")]
        public string Id { get; set; }
        
        [StringLength(20)]
        [Column("model_type")]
        public string ModelType { get; set; }
        
        [StringLength(50)]
        [Column("model_code")]
        public string ModelCode { get; set; }
        
        [StringLength(50)]
        [Column("model_name")]
        public string ModelName { get; set; }
        
        [Column("is_default")]
        public int? IsDefault { get; set; }
        
        [Column("is_enabled")]
        public int? IsEnabled { get; set; }
        
        [Column("config_json")]
        public string ConfigJson { get; set; }
        
        [StringLength(200)]
        [Column("doc_link")]
        public string DocLink { get; set; }
        
        [StringLength(255)]
        [Column("remark")]
        public string Remark { get; set; }
        
        [Column("sort")]
        public int? Sort { get; set; }
        
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