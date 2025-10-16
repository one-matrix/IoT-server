using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("sys_user_token")]
    public class SysUserToken
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        
        [Required]
        [Column("user_id")]
        public long UserId { get; set; }
        
        [Required]
        [StringLength(100)]
        [Column("token")]
        public string Token { get; set; }
        
        [Column("expire_date")]
        public DateTime? ExpireDate { get; set; }
        
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
        
        [Column("create_date")]
        public DateTime? CreateDate { get; set; }
    }
}