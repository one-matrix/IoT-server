using System.ComponentModel.DataAnnotations;

namespace IotApi.DTOs
{
    /// <summary>
    /// 设备注册DTO
    /// </summary>
    public class DeviceRegisterDto
    {
        /// <summary>
        /// MAC地址
        /// </summary>
        [Required(ErrorMessage = "MAC地址不能为空")]
        public string MacAddress { get; set; }
    }

    /// <summary>
    /// 设备更新DTO
    /// </summary>
    public class DeviceUpdateDto
    {
        /// <summary>
        /// 设备别名
        /// </summary>
        [StringLength(100, ErrorMessage = "设备别名长度不能超过100个字符")]
        public string Alias { get; set; }

        /// <summary>
        /// 自动更新开关(0关闭/1开启)
        /// </summary>
        public int? AutoUpdate { get; set; }
    }

    /// <summary>
    /// 设备解绑DTO
    /// </summary>
    public class DeviceUnbindDto
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        [Required(ErrorMessage = "设备ID不能为空")]
        public string DeviceId { get; set; }
    }
    
    /// <summary>
    /// 设备手动添加DTO
    /// </summary>
    public class DeviceManualAddDto
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        [Required(ErrorMessage = "设备名称不能为空")]
        [StringLength(100, ErrorMessage = "设备名称长度不能超过100个字符")]
        public string DeviceName { get; set; }
        
        /// <summary>
        /// MAC地址
        /// </summary>
        [Required(ErrorMessage = "MAC地址不能为空")]
        [StringLength(50, ErrorMessage = "MAC地址长度不能超过50个字符")]
        public string MacAddress { get; set; }
        
        /// <summary>
        /// 设备硬件型号
        /// </summary>
        [Required(ErrorMessage = "设备硬件型号不能为空")]
        [StringLength(50, ErrorMessage = "设备硬件型号长度不能超过50个字符")]
        public string Board { get; set; }
    }

    /// <summary>
    /// 设备上报请求DTO
    /// </summary>
    public class DeviceReportReqDto
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string Application { get; set; }
        
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// 设备硬件型号
        /// </summary>
        public string Board { get; set; }
        
        /// <summary>
        /// MAC地址
        /// </summary>
        public string MacAddress { get; set; }
    }

    /// <summary>
    /// 设备上报响应DTO
    /// </summary>
    public class DeviceReportRespDto
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// OTA更新URL
        /// </summary>
        public string OtaUrl { get; set; }
        
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 创建错误响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>错误响应DTO</returns>
        public static DeviceReportRespDto CreateError(string message)
        {
            return new DeviceReportRespDto
            {
                Status = "error",
                Message = message
            };
        }
    }
}