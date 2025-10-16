using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("ai_model_provider")]
    public class AiModelProvider
    {
        [Key]
        [StringLength(32)]
        [Column("id")]
        public string Id { get; set; }
        
        [StringLength(20)]
        [Column("model_type")]
        public string ModelType { get; set; }
        
        [StringLength(50)]
        [Column("provider_code")]
        public string ProviderCode { get; set; }
        
        [StringLength(50)]
        [Column("name")]
        public string Name { get; set; }
        
        [Column("fields")]
        public string Fields { get; set; }
        
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