using System.ComponentModel.DataAnnotations;

namespace IotApi.DTOs
{
    /// <summary>
    /// 模型配置请求体DTO
    /// </summary>
    public class ModelConfigBodyDto
    {
        /// <summary>
        /// 模型名称
        /// </summary>
        [Required(ErrorMessage = "模型名称不能为空")]
        [StringLength(100, ErrorMessage = "模型名称长度不能超过100个字符")]
        public string ModelName { get; set; }

        /// <summary>
        /// 模型代码
        /// </summary>
        [Required(ErrorMessage = "模型代码不能为空")]
        [StringLength(50, ErrorMessage = "模型代码长度不能超过50个字符")]
        public string ModelCode { get; set; }

        /// <summary>
        /// 配置JSON
        /// </summary>
        public string ConfigJson { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int IsEnabled { get; set; } = 1;

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
        public string Remark { get; set; }
    }
}