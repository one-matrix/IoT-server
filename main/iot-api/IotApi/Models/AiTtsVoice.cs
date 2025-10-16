using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.Models
{
    [Table("ai_tts_voice")]
    public class AiTtsVoice
    {
        [Key]
        [StringLength(32)]
        [Column("id")]
        public string Id { get; set; }
        
        [StringLength(32)]
        [Column("tts_model_id")]
        public string TtsModelId { get; set; }
        
        [StringLength(20)]
        [Column("name")]
        public string Name { get; set; }
        
        [StringLength(50)]
        [Column("tts_voice")]
        public string TtsVoice { get; set; }
        
        [StringLength(50)]
        [Column("languages")]
        public string Languages { get; set; }
        
        [StringLength(500)]
        [Column("voice_demo")]
        public string VoiceDemo { get; set; }
        
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