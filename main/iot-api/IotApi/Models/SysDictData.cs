using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("sys_dict_data")]
    public class SysDictData
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        
        [Required]
        [Column("dict_type_id")]
        public long DictTypeId { get; set; }
        
        [Required]
        [StringLength(255)]
        [Column("dict_label")]
        public string DictLabel { get; set; }
        
        [StringLength(255)]
        [Column("dict_value")]
        public string DictValue { get; set; }
        
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