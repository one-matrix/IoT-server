using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotApi.DTOs
{
    /// <summary>
    /// 智能体声纹数据传输对象
    /// </summary>
    public class AgentVoicePrintDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// 音频文件id
        /// </summary>
        public string AudioId { get; set; }
        public string AgentId { get; set; }
        
        /// <summary>
        /// 声纹来源的人姓名
        /// </summary>
        [StringLength(100, ErrorMessage = "声纹来源的人姓名长度不能超过100个字符")]
        public string SourceName { get; set; }
        
        /// <summary>
        /// 描述声纹来源的人
        /// </summary>
        [StringLength(500, ErrorMessage = "描述声纹来源的人长度不能超过500个字符")]
        public string Introduce { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

 
        public string Name { get; set; }

        public long? UserId { get; set; }

       
        public string AgentCode { get; set; }

        public string AgentName { get; set; }

        public string Description { get; set; }

        public string Embedding { get; set; }

        public string Memory { get; set; }

        public int? Sort { get; set; }

    }
}