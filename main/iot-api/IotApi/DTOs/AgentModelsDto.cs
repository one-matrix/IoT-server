using System.ComponentModel.DataAnnotations;

namespace IotApi.DTOs
{
    /// <summary>
    /// 智能体模型请求参数
    /// </summary>
    public class AgentModelsDto
    {
        /// <summary>
        /// MAC地址
        /// </summary>
        [Required(ErrorMessage = "MAC地址不能为空")]
        public string MacAddress { get; set; }
        
        /// <summary>
        /// 选择的模块
        /// </summary>
        public string SelectedModule { get; set; }
    }
}