using System.ComponentModel.DataAnnotations;

namespace IotApi.DTOs
{
    /// <summary>
    /// 系统参数DTO
    /// </summary>
    public class SysParamsDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long? Id { get; set; }
        
        /// <summary>
        /// 参数编码
        /// </summary>
        [Required(ErrorMessage = "参数编码不能为空")]
        [StringLength(200, ErrorMessage = "参数编码长度不能超过200")]
        public string ParamCode { get; set; }
        
        /// <summary>
        /// 参数值
        /// </summary>
        [Required(ErrorMessage = "参数值不能为空")]
        [StringLength(2000, ErrorMessage = "参数值长度不能超过2000")]
        public string ParamValue { get; set; }
        
        /// <summary>
        /// 参数类型 0：系统参数 1：非系统参数
        /// </summary>
        public int? ParamType { get; set; }

        /// <summary>
        /// 值类型
        /// </summary>
        public string ValueType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200, ErrorMessage = "备注长度不能超过200")]
        public string Remark { get; set; }
    }

    /// <summary>
    /// 系统参数分页查询DTO
    /// </summary>
    public class SysParamsQueryDto
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        [Required(ErrorMessage = "当前页码不能为空")]
        public int Page { get; set; } = 1;
        
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        [Required(ErrorMessage = "每页显示记录数不能为空")]
        public int Limit { get; set; } = 10;
        
        /// <summary>
        /// 参数编码或备注
        /// </summary>
        public string ParamCode { get; set; }
    }

    /// <summary>
    /// 字典类型DTO
    /// </summary>
    public class SysDictTypeDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long? Id { get; set; }
        
        /// <summary>
        /// 字典类型
        /// </summary>
        [Required(ErrorMessage = "字典类型不能为空")]
        [StringLength(100, ErrorMessage = "字典类型长度不能超过100")]
        public string DictType { get; set; }
        
        /// <summary>
        /// 字典名称
        /// </summary>
        [Required(ErrorMessage = "字典名称不能为空")]
        [StringLength(255, ErrorMessage = "字典名称长度不能超过255")]
        public string DictName { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(255, ErrorMessage = "备注长度不能超过255")]
        public string Remark { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }
    }

    /// <summary>
    /// 字典类型分页查询DTO
    /// </summary>
    public class SysDictTypeQueryDto
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        [Required(ErrorMessage = "当前页码不能为空")]
        public int Page { get; set; } = 1;
        
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        [Required(ErrorMessage = "每页显示记录数不能为空")]
        public int Limit { get; set; } = 10;
        
        /// <summary>
        /// 字典类型
        /// </summary>
        public string DictType { get; set; }
        
        /// <summary>
        /// 字典名称
        /// </summary>
        public string DictName { get; set; }
    }

    /// <summary>
    /// 字典数据DTO
    /// </summary>
    public class SysDictDataDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long? Id { get; set; }
        
        /// <summary>
        /// 字典类型ID
        /// </summary>
        [Required(ErrorMessage = "字典类型ID不能为空")]
        public long DictTypeId { get; set; }
        
        /// <summary>
        /// 字典标签
        /// </summary>
        [Required(ErrorMessage = "字典标签不能为空")]
        [StringLength(255, ErrorMessage = "字典标签长度不能超过255")]
        public string DictLabel { get; set; }
        
        /// <summary>
        /// 字典值
        /// </summary>
        [StringLength(255, ErrorMessage = "字典值长度不能超过255")]
        public string DictValue { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(255, ErrorMessage = "备注长度不能超过255")]
        public string Remark { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }
    }

    /// <summary>
    /// 字典数据分页查询DTO
    /// </summary>
    public class SysDictDataQueryDto
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        [Required(ErrorMessage = "当前页码不能为空")]
        public int Page { get; set; } = 1;
        
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        [Required(ErrorMessage = "每页显示记录数不能为空")]
        public int Limit { get; set; } = 10;
        
        /// <summary>
        /// 字典类型ID
        /// </summary>
        [Required(ErrorMessage = "字典类型ID不能为空")]
        public long DictTypeId { get; set; }
        
        /// <summary>
        /// 字典标签
        /// </summary>
        public string DictLabel { get; set; }
        
        /// <summary>
        /// 字典值
        /// </summary>
        public string DictValue { get; set; }
    }

    /// <summary>
    /// 字典数据项
    /// </summary>
    public class SysDictDataItem
    {
        /// <summary>
        /// 字典标签
        /// </summary>
        public string DictLabel { get; set; }
        
        /// <summary>
        /// 字典值
        /// </summary>
        public string DictValue { get; set; }
    }

    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class PageResult<T>
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }
        
        /// <summary>
        /// 列表数据
        /// </summary>
        public List<T> List { get; set; }
        
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page { get; set; }
        
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int Limit { get; set; }
    }
}