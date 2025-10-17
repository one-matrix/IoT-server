namespace IotApi.DTOs
{
    /// <summary>
    /// LLM模型基础信息DTO
    /// </summary>
    public class LlmModelBasicInfoDto : ModelBasicInfoDto
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
    }
}