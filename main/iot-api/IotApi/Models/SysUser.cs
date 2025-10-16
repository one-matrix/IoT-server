using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("sys_user")]
    public class SysUser
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        
        [Required]
        [StringLength(50)]
        [Column("username")]
        public string Username { get; set; }
        
        [StringLength(100)]
        [Column("password")]
        public string Password { get; set; }
        
        [Column("super_admin")]
        public int? SuperAdmin { get; set; }
        
        [Column("status")]
        public int? Status { get; set; }
        
        [Column("create_date")]
        public DateTime? CreateDate { get; set; }
        
        [Column("updater")]
        public long? Updater { get; set; }
        
        [Column("creator")]
        public long? Creator { get; set; }
        
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
    }
}