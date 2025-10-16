using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("sys_dict_type")]
    public class SysDictType
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        
        [Required]
        [StringLength(100)]
        [Column("dict_type")]
        public string DictType { get; set; }
        
        [Required]
        [StringLength(255)]
        [Column("dict_name")]
        public string DictName { get; set; }
        
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