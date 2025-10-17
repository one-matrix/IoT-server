namespace IotApi.Core.Constants
{
    /// <summary>
    /// 常量类
    /// </summary>
    public static class Constant
    {
        /// <summary>
        /// 无记忆模型标识
        /// </summary>
        public const string MEMORY_NO_MEM = "Memory_nomem";

        /// <summary>
        /// 聊天历史配置枚举
        /// </summary>
        public enum ChatHistoryConfEnum
        {
            /// <summary>
            /// 忽略
            /// </summary>
            IGNORE = 0,
            
            /// <summary>
            /// 记录文本和音频
            /// </summary>
            RECORD_TEXT_AUDIO = 1
        }
    }
}