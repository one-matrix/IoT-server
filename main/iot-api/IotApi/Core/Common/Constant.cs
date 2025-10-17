namespace IotApi.Core.Common
{
    /// <summary>
    /// 常量
    /// </summary>
    public static class Constant
    {
        /// <summary>
        /// 成功
        /// </summary>
        public const int SUCCESS = 1;
        
        /// <summary>
        /// 失败
        /// </summary>
        public const int FAIL = 0;
        
        /// <summary>
        /// OK
        /// </summary>
        public const string OK = "OK";
        
        /// <summary>
        /// 用户标识
        /// </summary>
        public const string USER_KEY = "userId";
        
        /// <summary>
        /// 菜单根节点标识
        /// </summary>
        public const long MENU_ROOT = 0L;
        
        /// <summary>
        /// 部门根节点标识
        /// </summary>
        public const long DEPT_ROOT = 0L;
        
        /// <summary>
        /// 数据字典根节点标识
        /// </summary>
        public const long DICT_ROOT = 0L;
        
        /// <summary>
        /// 升序
        /// </summary>
        public const string ASC = "asc";
        
        /// <summary>
        /// 降序
        /// </summary>
        public const string DESC = "desc";
        
        /// <summary>
        /// 创建时间字段名
        /// </summary>
        public const string CREATE_DATE = "create_date";
        
        /// <summary>
        /// 创建时间字段名
        /// </summary>
        public const string ID = "id";
        
        /// <summary>
        /// 数据权限过滤
        /// </summary>
        public const string SQL_FILTER = "sqlFilter";
        
        /// <summary>
        /// 当前页码
        /// </summary>
        public const string PAGE = "page";
        
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        public const string LIMIT = "limit";
        
        /// <summary>
        /// 排序字段
        /// </summary>
        public const string ORDER_FIELD = "orderField";
        
        /// <summary>
        /// 排序方式
        /// </summary>
        public const string ORDER = "order";
        
        /// <summary>
        /// 请求头授权标识
        /// </summary>
        public const string AUTHORIZATION = "Authorization";
        
        /// <summary>
        /// 服务器密钥
        /// </summary>
        public const string SERVER_SECRET = "server.secret";
        
        /// <summary>
        /// SM2公钥
        /// </summary>
        public const string SM2_PUBLIC_KEY = "server.public_key";
        
        /// <summary>
        /// SM2私钥
        /// </summary>
        public const string SM2_PRIVATE_KEY = "server.private_key";
        
        /// <summary>
        /// websocket地址
        /// </summary>
        public const string SERVER_WEBSOCKET = "server.websocket";
        
        /// <summary>
        /// mqtt gateway 配置
        /// </summary>
        public const string SERVER_MQTT_GATEWAY = "server.mqtt_gateway";
        
        /// <summary>
        /// ota地址
        /// </summary>
        public const string SERVER_OTA = "server.ota";
        
        /// <summary>
        /// 是否允许用户注册
        /// </summary>
        public const string SERVER_ALLOW_USER_REGISTER = "server.allow_user_register";
        
        /// <summary>
        /// 下发六位验证码时显示的控制面板地址
        /// </summary>
        public const string SERVER_FRONTED_URL = "server.fronted_url";
        
        /// <summary>
        /// 路径分割符
        /// </summary>
        public const string FILE_EXTENSION_SEG = ".";
        
        /// <summary>
        /// mcp接入点路径
        /// </summary>
        public const string SERVER_MCP_ENDPOINT = "server.mcp_endpoint";
        
        /// <summary>
        /// 声纹接口地址
        /// </summary>
        public const string SERVER_VOICE_PRINT = "server.voice_print";
        
        /// <summary>
        /// mqtt密钥
        /// </summary>
        public const string SERVER_MQTT_SECRET = "server.mqtt_signature_key";
        
        /// <summary>
        /// 无记忆
        /// </summary>
        public const string MEMORY_NO_MEM = "Memory_nomem";
        
        /// <summary>
        /// 无效固件URL
        /// </summary>
        public const string INVALID_FIRMWARE_URL = "http://xiaozhi.server.com:8002/xiaozhi/otaMag/download/NOT_ACTIVATED_FIRMWARE_THIS_IS_A_INVALID_URL";
        
        /// <summary>
        /// 版本号
        /// </summary>
        public const string VERSION = "0.8.4";
        
        /// <summary>
        /// 系统基础参数
        /// </summary>
        public enum SysBaseParam
        {
            /// <summary>
            /// ICP备案号
            /// </summary>
            BEIAN_ICP_NUM,
            
            /// <summary>
            /// GA备案号
            /// </summary>
            BEIAN_GA_NUM,
            
            /// <summary>
            /// 系统名称
            /// </summary>
            SERVER_NAME
        }
        
        /// <summary>
        /// 系统短信参数
        /// </summary>
        public enum SysMSMParam
        {
            /// <summary>
            /// 阿里云授权keyID
            /// </summary>
            ALIYUN_SMS_ACCESS_KEY_ID,
            
            /// <summary>
            /// 阿里云授权密钥
            /// </summary>
            ALIYUN_SMS_ACCESS_KEY_SECRET,
            
            /// <summary>
            /// 阿里云短信签名
            /// </summary>
            ALIYUN_SMS_SIGN_NAME,
            
            /// <summary>
            /// 阿里云短信模板
            /// </summary>
            ALIYUN_SMS_SMS_CODE_TEMPLATE_CODE,
            
            /// <summary>
            /// 单号码最大短信发送条数
            /// </summary>
            SERVER_SMS_MAX_SEND_COUNT,
            
            /// <summary>
            /// 是否开启手机注册
            /// </summary>
            SERVER_ENABLE_MOBILE_REGISTER
        }
        
        /// <summary>
        /// 数据操作状态
        /// </summary>
        public enum DataOperation
        {
            /// <summary>
            /// 插入
            /// </summary>
            INSERT,
            
            /// <summary>
            /// 已修改
            /// </summary>
            UPDATE,
            
            /// <summary>
            /// 已删除
            /// </summary>
            DELETE
        }
        
        /// <summary>
        /// 聊天历史配置枚举
        /// </summary>
        public enum ChatHistoryConfEnum
        {
            /// <summary>
            /// 不记录
            /// </summary>
            IGNORE = 0,
            
            /// <summary>
            /// 记录文本
            /// </summary>
            RECORD_TEXT = 1,
            
            /// <summary>
            /// 文本音频都记录
            /// </summary>
            RECORD_TEXT_AUDIO = 2
        }
        
        /// <summary>
        /// 字典类型
        /// </summary>
        public enum DictType
        {
            /// <summary>
            /// 手机区号
            /// </summary>
            MOBILE_AREA
        }
        
        /// <summary>
        /// 获取系统基础参数的值
        /// </summary>
        /// <param name="param">参数枚举</param>
        /// <returns>参数值</returns>
        public static string GetValue(SysBaseParam param)
        {
            return param switch
            {
                SysBaseParam.BEIAN_ICP_NUM => "server.beian_icp_num",
                SysBaseParam.BEIAN_GA_NUM => "server.beian_ga_num",
                SysBaseParam.SERVER_NAME => "server.name",
                _ => string.Empty
            };
        }
        
        /// <summary>
        /// 获取系统短信参数的值
        /// </summary>
        /// <param name="param">参数枚举</param>
        /// <returns>参数值</returns>
        public static string GetValue(SysMSMParam param)
        {
            return param switch
            {
                SysMSMParam.ALIYUN_SMS_ACCESS_KEY_ID => "aliyun.sms.access_key_id",
                SysMSMParam.ALIYUN_SMS_ACCESS_KEY_SECRET => "aliyun.sms.access_key_secret",
                SysMSMParam.ALIYUN_SMS_SIGN_NAME => "aliyun.sms.sign_name",
                SysMSMParam.ALIYUN_SMS_SMS_CODE_TEMPLATE_CODE => "aliyun.sms.sms_code_template_code",
                SysMSMParam.SERVER_SMS_MAX_SEND_COUNT => "server.sms_max_send_count",
                SysMSMParam.SERVER_ENABLE_MOBILE_REGISTER => "server.enable_mobile_register",
                _ => string.Empty
            };
        }
        
        /// <summary>
        /// 获取数据操作状态的值
        /// </summary>
        /// <param name="operation">操作枚举</param>
        /// <returns>操作值</returns>
        public static string GetValue(DataOperation operation)
        {
            return operation switch
            {
                DataOperation.INSERT => "I",
                DataOperation.UPDATE => "U",
                DataOperation.DELETE => "D",
                _ => string.Empty
            };
        }
        
        /// <summary>
        /// 获取聊天历史配置枚举的代码
        /// </summary>
        /// <param name="config">配置枚举</param>
        /// <returns>配置代码</returns>
        public static int GetCode(ChatHistoryConfEnum config)
        {
            return (int)config;
        }
        
        /// <summary>
        /// 获取字典类型的值
        /// </summary>
        /// <param name="type">字典类型枚举</param>
        /// <returns>类型值</returns>
        public static string GetValue(DictType type)
        {
            return type switch
            {
                DictType.MOBILE_AREA => "MOBILE_AREA",
                _ => string.Empty
            };
        }
    }
}