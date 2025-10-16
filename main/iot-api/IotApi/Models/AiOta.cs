using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("ai_ota")]
    public class AiOta
    {
        [Key]
        [StringLength(32)]
        [Column("id")]
        public string Id { get; set; }
        
        [StringLength(255)]
        [Column("firmware_name")]
        public string FirmwareName { get; set; }
        
        [StringLength(50)]
        [Column("type")]
        public string Type { get; set; }
        
        [StringLength(50)]
        [Column("version")]
        public string Version { get; set; }
        
        [Column("size")]
        public long? Size { get; set; }
        
        [StringLength(500)]
        [Column("remark")]
        public string Remark { get; set; }
        
        [StringLength(500)]
        [Column("firmware_path")]
        public string FirmwarePath { get; set; }
        
        [Column("sort")]
        public int? Sort { get; set; }
        
        [Column("updater")]
        public long? Updater { get; set; }
        
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
        
        [Column("creator")]
        public long? Creator { get; set; }
        
        [Column("create_date")]
        public DateTime? CreateDate { get; set; }
    }
}