using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("sys_params")]
    public class SysParams
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        
        [StringLength(200)]
        [Column("param_code")]
        public string ParamCode { get; set; }
        
        [StringLength(2000)]
        [Column("param_value")]
        public string ParamValue { get; set; }
        
        [Column("param_type")]
        public int? ParamType { get; set; }

        [Column("value_type")]
        public string ValueType { get; set; }

        [StringLength(200)]
        [Column("remark")]
        public string Remark { get; set; }
        
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