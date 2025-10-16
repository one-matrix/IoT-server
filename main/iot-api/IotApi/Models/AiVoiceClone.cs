using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("ai_voice_clone")]
    public class AiVoiceClone
    {
        [Key]
        [StringLength(32)]
        [Column("id")]
        public string Id { get; set; }
        
        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; }
        
        [StringLength(32)]
        [Column("model_id")]
        public string ModelId { get; set; }
        
        [StringLength(32)]
        [Column("voice_id")]
        public string VoiceId { get; set; }
        
        [Column("user_id")]
        public long? UserId { get; set; }
        
        [Column("voice")]
        public byte[] Voice { get; set; }
        
        [Column("train_status")]
        public int? TrainStatus { get; set; }
        
        [StringLength(500)]
        [Column("train_error")]
        public string TrainError { get; set; }
        
        [Column("creator")]
        public long? Creator { get; set; }
        
        [Column("create_date")]
        public DateTime? CreateDate { get; set; }
    }
}