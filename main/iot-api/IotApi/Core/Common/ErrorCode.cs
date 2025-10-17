namespace IotApi.Core.common
{
    /// <summary>
    /// 错误码常量
    /// </summary>
    public class ErrorCode
    {
        /// <summary>
        /// 内部服务器错误
        /// </summary>
        public const int INTERNAL_SERVER_ERROR = 500;
        
        /// <summary>
        /// 未授权
        /// </summary>
        public const int UNAUTHORIZED = 401;
        
        /// <summary>
        /// 禁止访问
        /// </summary>
        public const int FORBIDDEN = 403;
        
        /// <summary>
        /// 资源不存在
        /// </summary>
        public const int NOT_FOUND = 404;
        
        /// <summary>
        /// 参数错误
        /// </summary>
        public const int BAD_REQUEST = 400;
        
        /// <summary>
        /// 数据已存在
        /// </summary>
        public const int DATA_ALREADY_EXISTS = 1000;
        
        /// <summary>
        /// 账号或密码错误
        /// </summary>
        public const int ACCOUNT_PASSWORD_ERROR = 1001;
        
        /// <summary>
        /// 账号已被锁定
        /// </summary>
        public const int ACCOUNT_LOCKED = 1002;
        
        /// <summary>
        /// 账号不存在
        /// </summary>
        public const int ACCOUNT_NOT_EXIST = 1003;
        
        /// <summary>
        /// 参数不能为空
        /// </summary>
        public const int NOT_NULL = 10001;
        
        /// <summary>
        /// 数据库记录已存在
        /// </summary>
        public const int DB_RECORD_EXISTS = 10002;
        
        /// <summary>
        /// 参数获取错误
        /// </summary>
        public const int PARAMS_GET_ERROR = 10003;
        
        /// <summary>
        /// 上级部门错误
        /// </summary>
        public const int SUPERIOR_DEPT_ERROR = 10011;
        
        /// <summary>
        /// 上级菜单错误
        /// </summary>
        public const int SUPERIOR_MENU_ERROR = 10012;
        
        /// <summary>
        /// 数据范围参数错误
        /// </summary>
        public const int DATA_SCOPE_PARAMS_ERROR = 10013;
        
        /// <summary>
        /// 部门子项删除错误
        /// </summary>
        public const int DEPT_SUB_DELETE_ERROR = 10014;
        
        /// <summary>
        /// 部门用户删除错误
        /// </summary>
        public const int DEPT_USER_DELETE_ERROR = 10015;
        
        /// <summary>
        /// 上传文件为空
        /// </summary>
        public const int UPLOAD_FILE_EMPTY = 10019;
        
        /// <summary>
        /// Token不能为空
        /// </summary>
        public const int TOKEN_NOT_EMPTY = 10020;
        
        /// <summary>
        /// Token无效
        /// </summary>
        public const int TOKEN_INVALID = 10021;
        
        /// <summary>
        /// 账号锁定
        /// </summary>
        public const int ACCOUNT_LOCK = 10022;
        
        /// <summary>
        /// OSS上传文件错误
        /// </summary>
        public const int OSS_UPLOAD_FILE_ERROR = 10024;
        
        /// <summary>
        /// Redis错误
        /// </summary>
        public const int REDIS_ERROR = 10027;
        
        /// <summary>
        /// 任务错误
        /// </summary>
        public const int JOB_ERROR = 10028;
        
        /// <summary>
        /// 无效符号
        /// </summary>
        public const int INVALID_SYMBOL = 10029;
        
        /// <summary>
        /// 密码长度错误
        /// </summary>
        public const int PASSWORD_LENGTH_ERROR = 10030;
        
        /// <summary>
        /// 密码强度不足
        /// </summary>
        public const int PASSWORD_WEAK_ERROR = 10031;
        
        /// <summary>
        /// 不能删除自己
        /// </summary>
        public const int DEL_MYSELF_ERROR = 10032;
        
        /// <summary>
        /// 设备验证码错误
        /// </summary>
        public const int DEVICE_CAPTCHA_ERROR = 10033;
        
        /// <summary>
        /// 参数值为空
        /// </summary>
        public const int PARAM_VALUE_NULL = 10034;
        
        /// <summary>
        /// 参数类型为空
        /// </summary>
        public const int PARAM_TYPE_NULL = 10035;
        
        /// <summary>
        /// 参数类型无效
        /// </summary>
        public const int PARAM_TYPE_INVALID = 10036;
        
        /// <summary>
        /// 参数数值无效
        /// </summary>
        public const int PARAM_NUMBER_INVALID = 10037;
        
        /// <summary>
        /// 参数布尔值无效
        /// </summary>
        public const int PARAM_BOOLEAN_INVALID = 10038;
        
        /// <summary>
        /// 参数数组无效
        /// </summary>
        public const int PARAM_ARRAY_INVALID = 10039;
        
        /// <summary>
        /// 参数JSON无效
        /// </summary>
        public const int PARAM_JSON_INVALID = 10040;
        
        /// <summary>
        /// OTA设备未找到
        /// </summary>
        public const int OTA_DEVICE_NOT_FOUND = 10041;
        
        /// <summary>
        /// OTA设备需要绑定
        /// </summary>
        public const int OTA_DEVICE_NEED_BIND = 10042;
        
        /// <summary>
        /// 删除数据失败
        /// </summary>
        public const int DELETE_DATA_FAILED = 10043;
        
        /// <summary>
        /// 用户未登录
        /// </summary>
        public const int USER_NOT_LOGIN = 10044;
        
        /// <summary>
        /// WebSocket连接失败
        /// </summary>
        public const int WEB_SOCKET_CONNECT_FAILED = 10045;
        
        /// <summary>
        /// 声纹保存错误
        /// </summary>
        public const int VOICE_PRINT_SAVE_ERROR = 10046;
        
        /// <summary>
        /// 当日短信限制已达上限
        /// </summary>
        public const int TODAY_SMS_LIMIT_REACHED = 10047;
        
        /// <summary>
        /// 旧密码错误
        /// </summary>
        public const int OLD_PASSWORD_ERROR = 10048;
        
        /// <summary>
        /// 无效的大语言模型类型
        /// </summary>
        public const int INVALID_LLM_TYPE = 10049;
        
        /// <summary>
        /// Token生成错误
        /// </summary>
        public const int TOKEN_GENERATE_ERROR = 10050;
        
        /// <summary>
        /// 资源未找到
        /// </summary>
        public const int RESOURCE_NOT_FOUND = 10051;
        
        /// <summary>
        /// 默认智能体未找到
        /// </summary>
        public const int DEFAULT_AGENT_NOT_FOUND = 10052;
        
        /// <summary>
        /// 智能体未找到
        /// </summary>
        public const int AGENT_NOT_FOUND = 10053;
        
        /// <summary>
        /// 声纹API未配置
        /// </summary>
        public const int VOICEPRINT_API_NOT_CONFIGURED = 10054;
        
        /// <summary>
        /// 短信发送失败
        /// </summary>
        public const int SMS_SEND_FAILED = 10055;
        
        /// <summary>
        /// 短信连接失败
        /// </summary>
        public const int SMS_CONNECTION_FAILED = 10056;
        
        /// <summary>
        /// 智能体声纹创建失败
        /// </summary>
        public const int AGENT_VOICEPRINT_CREATE_FAILED = 10057;
        
        /// <summary>
        /// 智能体声纹更新失败
        /// </summary>
        public const int AGENT_VOICEPRINT_UPDATE_FAILED = 10058;
        
        /// <summary>
        /// 智能体声纹删除失败
        /// </summary>
        public const int AGENT_VOICEPRINT_DELETE_FAILED = 10059;
        
        /// <summary>
        /// 短信发送过于频繁
        /// </summary>
        public const int SMS_SEND_TOO_FREQUENTLY = 10060;
        
        /// <summary>
        /// 激活码为空
        /// </summary>
        public const int ACTIVATION_CODE_EMPTY = 10061;
        
        /// <summary>
        /// 激活码错误
        /// </summary>
        public const int ACTIVATION_CODE_ERROR = 10062;
        
        /// <summary>
        /// 设备已激活
        /// </summary>
        public const int DEVICE_ALREADY_ACTIVATED = 10063;
        
        /// <summary>
        /// 默认模型删除错误
        /// </summary>
        public const int DEFAULT_MODEL_DELETE_ERROR = 10064;
        
        /// <summary>
        /// Mac地址已存在
        /// </summary>
        public const int MAC_ADDRESS_ALREADY_EXISTS = 10090;
        
        /// <summary>
        /// 供应器不存在
        /// </summary>
        public const int MODEL_PROVIDER_NOT_EXIST = 10091;
        
        /// <summary>
        /// 设置的LLM不存在
        /// </summary>
        public const int LLM_NOT_EXIST = 10092;
        
        /// <summary>
        /// 该模型配置已被智能体引用，无法删除
        /// </summary>
        public const int MODEL_REFERENCED_BY_AGENT = 10093;
        
        /// <summary>
        /// 该LLM模型已被意图识别配置引用，无法删除
        /// </summary>
        public const int LLM_REFERENCED_BY_INTENT = 10094;
        
        /// <summary>
        /// 新增数据失败
        /// </summary>
        public const int ADD_DATA_FAILED = 10065;
        
        /// <summary>
        /// 修改数据失败
        /// </summary>
        public const int UPDATE_DATA_FAILED = 10066;
        
        /// <summary>
        /// 短信验证码错误
        /// </summary>
        public const int SMS_CAPTCHA_ERROR = 10067;
        
        /// <summary>
        /// 未开启手机注册
        /// </summary>
        public const int MOBILE_REGISTER_DISABLED = 10068;
        
        /// <summary>
        /// 用户名不是手机号码
        /// </summary>
        public const int USERNAME_NOT_PHONE = 10069;
        
        /// <summary>
        /// 手机号码已注册
        /// </summary>
        public const int PHONE_ALREADY_REGISTERED = 10070;
        
        /// <summary>
        /// 手机号码未注册
        /// </summary>
        public const int PHONE_NOT_REGISTERED = 10071;
        
        /// <summary>
        /// 不允许用户注册
        /// </summary>
        public const int USER_REGISTER_DISABLED = 10072;
        
        /// <summary>
        /// 未开启找回密码功能
        /// </summary>
        public const int RETRIEVE_PASSWORD_DISABLED = 10073;
        
        /// <summary>
        /// 手机号码格式不正确
        /// </summary>
        public const int PHONE_FORMAT_ERROR = 10074;
        
        /// <summary>
        /// 手机验证码错误
        /// </summary>
        public const int SMS_CODE_ERROR = 10075;
        
        /// <summary>
        /// 字典类型不存在
        /// </summary>
        public const int DICT_TYPE_NOT_EXIST = 10076;
        
        /// <summary>
        /// 字典类型编码重复
        /// </summary>
        public const int DICT_TYPE_DUPLICATE = 10077;
        
        /// <summary>
        /// 读取资源失败
        /// </summary>
        public const int RESOURCE_READ_ERROR = 10078;
        
        /// <summary>
        /// LLM大模型和Intent意图识别，选择参数不匹配
        /// </summary>
        public const int LLM_INTENT_PARAMS_MISMATCH = 10079;
        
        /// <summary>
        /// 此声音声纹已经注册
        /// </summary>
        public const int VOICEPRINT_ALREADY_REGISTERED = 10080;
        
        /// <summary>
        /// 删除声纹出现错误
        /// </summary>
        public const int VOICEPRINT_DELETE_ERROR = 10081;
        
        /// <summary>
        /// 声纹修改不允许，声音已注册
        /// </summary>
        public const int VOICEPRINT_UPDATE_NOT_ALLOWED = 10082;
        
        /// <summary>
        /// 修改声纹错误，请联系管理员
        /// </summary>
        public const int VOICEPRINT_UPDATE_ADMIN_ERROR = 10083;
        
        /// <summary>
        /// 声纹接口地址错误
        /// </summary>
        public const int VOICEPRINT_API_URI_ERROR = 10084;
        
        /// <summary>
        /// 音频数据不属于智能体
        /// </summary>
        public const int VOICEPRINT_AUDIO_NOT_BELONG_AGENT = 10085;
        
        /// <summary>
        /// 音频数据为空
        /// </summary>
        public const int VOICEPRINT_AUDIO_EMPTY = 10086;
        
        /// <summary>
        /// 声纹保存请求失败
        /// </summary>
        public const int VOICEPRINT_REGISTER_REQUEST_ERROR = 10087;
        
        /// <summary>
        /// 声纹保存处理失败
        /// </summary>
        public const int VOICEPRINT_REGISTER_PROCESS_ERROR = 10088;
        
        /// <summary>
        /// 声纹注销请求失败
        /// </summary>
        public const int VOICEPRINT_UNREGISTER_REQUEST_ERROR = 10089;
        
        /// <summary>
        /// 声纹注销处理失败
        /// </summary>
        public const int VOICEPRINT_UNREGISTER_PROCESS_ERROR = 10090;
        
        /// <summary>
        /// 声纹识别请求失败
        /// </summary>
        public const int VOICEPRINT_IDENTIFY_REQUEST_ERROR = 10091;
        
        /// <summary>
        /// 无效服务端操作
        /// </summary>
        public const int INVALID_SERVER_ACTION = 10095;
        
        /// <summary>
        /// 未配置服务端WebSocket地址
        /// </summary>
        public const int SERVER_WEBSOCKET_NOT_CONFIGURED = 10096;
        
        /// <summary>
        /// 目标WebSocket地址不存在
        /// </summary>
        public const int TARGET_WEBSOCKET_NOT_EXIST = 10097;
        
        /// <summary>
        /// WebSocket地址列表不能为空
        /// </summary>
        public const int WEBSOCKET_URLS_EMPTY = 10098;
        
        /// <summary>
        /// WebSocket地址不能使用localhost或127.0.0.1
        /// </summary>
        public const int WEBSOCKET_URL_LOCALHOST = 10099;
        
        /// <summary>
        /// WebSocket地址格式不正确
        /// </summary>
        public const int WEBSOCKET_URL_FORMAT_ERROR = 10100;
        
        /// <summary>
        /// WebSocket连接测试失败
        /// </summary>
        public const int WEBSOCKET_CONNECTION_FAILED = 10101;
        
        /// <summary>
        /// OTA地址不能为空
        /// </summary>
        public const int OTA_URL_EMPTY = 10102;
        
        /// <summary>
        /// OTA地址不能使用localhost或127.0.0.1
        /// </summary>
        public const int OTA_URL_LOCALHOST = 10103;
        
        /// <summary>
        /// OTA地址必须以http或https开头
        /// </summary>
        public const int OTA_URL_PROTOCOL_ERROR = 10104;
        
        /// <summary>
        /// OTA地址必须以/ota/结尾
        /// </summary>
        public const int OTA_URL_FORMAT_ERROR = 10105;
        
        /// <summary>
        /// OTA接口访问失败
        /// </summary>
        public const int OTA_INTERFACE_ACCESS_FAILED = 10106;
        
        /// <summary>
        /// OTA接口返回内容格式不正确
        /// </summary>
        public const int OTA_INTERFACE_FORMAT_ERROR = 10107;
        
        /// <summary>
        /// OTA接口验证失败
        /// </summary>
        public const int OTA_INTERFACE_VALIDATION_FAILED = 10108;
        
        /// <summary>
        /// MCP地址不能为空
        /// </summary>
        public const int MCP_URL_EMPTY = 10109;
        
        /// <summary>
        /// MCP地址不能使用localhost或127.0.0.1
        /// </summary>
        public const int MCP_URL_LOCALHOST = 10110;
        
        /// <summary>
        /// 不是正确的MCP地址
        /// </summary>
        public const int MCP_URL_INVALID = 10111;
        
        /// <summary>
        /// MCP接口访问失败
        /// </summary>
        public const int MCP_INTERFACE_ACCESS_FAILED = 10112;
        
        /// <summary>
        /// MCP接口返回内容格式不正确
        /// </summary>
        public const int MCP_INTERFACE_FORMAT_ERROR = 10113;
        
        /// <summary>
        /// MCP接口验证失败
        /// </summary>
        public const int MCP_INTERFACE_VALIDATION_FAILED = 10114;
        
        /// <summary>
        /// 声纹接口地址不能为空
        /// </summary>
        public const int VOICEPRINT_URL_EMPTY = 10115;
        
        /// <summary>
        /// 声纹接口地址不能使用localhost或127.0.0.1
        /// </summary>
        public const int VOICEPRINT_URL_LOCALHOST = 10116;
        
        /// <summary>
        /// 不是正确的声纹接口地址
        /// </summary>
        public const int VOICEPRINT_URL_INVALID = 10117;
        
        /// <summary>
        /// 声纹接口地址必须以http或https开头
        /// </summary>
        public const int VOICEPRINT_URL_PROTOCOL_ERROR = 10118;
        
        /// <summary>
        /// 声纹接口访问失败
        /// </summary>
        public const int VOICEPRINT_INTERFACE_ACCESS_FAILED = 10119;
        
        /// <summary>
        /// 声纹接口返回内容格式不正确
        /// </summary>
        public const int VOICEPRINT_INTERFACE_FORMAT_ERROR = 10120;
        
        /// <summary>
        /// 声纹接口验证失败
        /// </summary>
        public const int VOICEPRINT_INTERFACE_VALIDATION_FAILED = 10121;
        
        /// <summary>
        /// mqtt密钥不能为空
        /// </summary>
        public const int MQTT_SECRET_EMPTY = 10122;
        
        /// <summary>
        /// mqtt密钥长度不安全
        /// </summary>
        public const int MQTT_SECRET_LENGTH_INSECURE = 10123;
        
        /// <summary>
        /// mqtt密钥必须同时包含大小写字母
        /// </summary>
        public const int MQTT_SECRET_CHARACTER_INSECURE = 10124;
        
        /// <summary>
        /// mqtt密钥包含弱密码
        /// </summary>
        public const int MQTT_SECRET_WEAK_PASSWORD = 10125;
        
        /// <summary>
        /// 字典标签重复
        /// </summary>
        public const int DICT_LABEL_DUPLICATE = 10128;
        
        /// <summary>
        /// SM2密钥未配置
        /// </summary>
        public const int SM2_KEY_NOT_CONFIGURED = 10129;
        
        /// <summary>
        /// SM2解密失败
        /// </summary>
        public const int SM2_DECRYPT_ERROR = 10130;
        
        /// <summary>
        /// modelType和provideCode不能为空
        /// </summary>
        public const int MODEL_TYPE_PROVIDE_CODE_NOT_NULL = 10131;
        
        /// <summary>
        /// 没有权限查看该智能体的聊天记录
        /// </summary>
        public const int CHAT_HISTORY_NO_PERMISSION = 10132;
        
        /// <summary>
        /// 会话ID不能为空
        /// </summary>
        public const int CHAT_HISTORY_SESSION_ID_NOT_NULL = 10133;
        
        /// <summary>
        /// 智能体ID不能为空
        /// </summary>
        public const int CHAT_HISTORY_AGENT_ID_NOT_NULL = 10134;
        
        /// <summary>
        /// 聊天记录下载失败
        /// </summary>
        public const int CHAT_HISTORY_DOWNLOAD_FAILED = 10135;
        
        /// <summary>
        /// 下载链接已过期或无效
        /// </summary>
        public const int DOWNLOAD_LINK_EXPIRED = 10136;
        
        /// <summary>
        /// 下载链接无效
        /// </summary>
        public const int DOWNLOAD_LINK_INVALID = 10137;
        
        /// <summary>
        /// 用户角色
        /// </summary>
        public const int CHAT_ROLE_USER = 10138;
        
        /// <summary>
        /// 智能体角色
        /// </summary>
        public const int CHAT_ROLE_AGENT = 10139;
        
        /// <summary>
        /// 音频文件不能为空
        /// </summary>
        public const int VOICE_CLONE_AUDIO_EMPTY = 10140;
        
        /// <summary>
        /// 只支持音频文件
        /// </summary>
        public const int VOICE_CLONE_NOT_AUDIO_FILE = 10141;
        
        /// <summary>
        /// 音频文件大小不能超过10MB
        /// </summary>
        public const int VOICE_CLONE_AUDIO_TOO_LARGE = 10142;
        
        /// <summary>
        /// 上传失败
        /// </summary>
        public const int VOICE_CLONE_UPLOAD_FAILED = 10143;
        
        /// <summary>
        /// 声音克隆记录不存在
        /// </summary>
        public const int VOICE_CLONE_RECORD_NOT_EXIST = 10144;
        
        /// <summary>
        /// 音色资源信息不能为空
        /// </summary>
        public const int VOICE_RESOURCE_INFO_EMPTY = 10145;
        
        /// <summary>
        /// 平台名称不能为空
        /// </summary>
        public const int VOICE_RESOURCE_PLATFORM_NAME_EMPTY = 10146;
        
        /// <summary>
        /// 音色ID不能为空
        /// </summary>
        public const int VOICE_RESOURCE_ID_EMPTY = 10147;
        
        /// <summary>
        /// 归属账号不能为空
        /// </summary>
        public const int VOICE_RESOURCE_ACCOUNT_EMPTY = 10148;
        
        /// <summary>
        /// 删除的音色资源ID不能为空
        /// </summary>
        public const int VOICE_RESOURCE_DELETE_ID_EMPTY = 10149;
        
        /// <summary>
        /// 您没有权限操作该记录
        /// </summary>
        public const int VOICE_RESOURCE_NO_PERMISSION = 10150;
    }
}