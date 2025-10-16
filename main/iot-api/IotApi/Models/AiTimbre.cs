using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("ai_timbre")]
    public class AiTimbre
    {
        [Key]
        [StringLength(32)]
        [Column("id")]
        public string Id { get; set; }
        
        [StringLength(64)]
        [Column("timbre_name")]
        public string TimbreName { get; set; }
        
        [StringLength(255)]
        [Column("description")]
        public string Description { get; set; }
        
        [Column("timbre_data")]
        public string TimbreData { get; set; }
        
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