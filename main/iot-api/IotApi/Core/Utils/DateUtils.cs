using System;
using System.Globalization;

namespace IotApi.Core.utils
{
    /// <summary>
    /// 日期处理工具类
    /// </summary>
    public static class DateUtils
    {
        /// <summary>
        /// 时间格式(yyyy-MM-dd)
        /// </summary>
        public const string DATE_PATTERN = "yyyy-MM-dd";
        
        /// <summary>
        /// 时间格式(yyyy-MM-dd HH:mm:ss)
        /// </summary>
        public const string DATE_TIME_PATTERN = "yyyy-MM-dd HH:mm:ss";
        
        /// <summary>
        /// 时间格式(yyyy-MM-dd HH:mm:ss.fff)
        /// </summary>
        public const string DATE_TIME_MILLIS_PATTERN = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>
        /// 日期格式化 日期格式为：yyyy-MM-dd
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>返回yyyy-MM-dd格式日期</returns>
        public static string Format(DateTime? date)
        {
            return Format(date, DATE_PATTERN);
        }

        /// <summary>
        /// 日期格式化 
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="pattern">格式，如：DateUtils.DATE_TIME_PATTERN</param>
        /// <returns>返回指定格式的日期字符串</returns>
        public static string Format(DateTime? date, string pattern)
        {
            if (date.HasValue)
            {
                return date.Value.ToString(pattern);
            }
            return null;
        }

        /// <summary>
        /// 日期解析
        /// </summary>
        /// <param name="dateStr">日期字符串</param>
        /// <param name="pattern">格式，如：DateUtils.DATE_TIME_PATTERN</param>
        /// <returns>返回DateTime</returns>
        public static DateTime? Parse(string dateStr, string pattern)
        {
            if (string.IsNullOrEmpty(dateStr))
            {
                return null;
            }

            try
            {
                return DateTime.ParseExact(dateStr, pattern, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取当前日期时间字符串，格式为yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <returns>当前日期时间字符串</returns>
        public static string GetDateTimeNow()
        {
            return GetDateTimeNow(DATE_TIME_PATTERN);
        }

        /// <summary>
        /// 获取当前日期时间字符串，使用指定格式
        /// </summary>
        /// <param name="pattern">日期格式</param>
        /// <returns>当前日期时间字符串</returns>
        public static string GetDateTimeNow(string pattern)
        {
            return Format(DateTime.Now, pattern);
        }

        /// <summary>
        /// 将毫秒转换为秒，保留3位小数
        /// </summary>
        /// <param name="mills">毫秒</param>
        /// <returns>秒字符串</returns>
        public static string MillsToSecond(long mills)
        {
            return string.Format("{0:0.000}", mills / 1000.0);
        }

        /// <summary>
        /// 获取简短的时间字符串：10秒前返回刚刚，多少秒前，几小时前，超过一周返回年月日时分秒
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>简短时间描述</returns>
        public static string GetShortTime(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            TimeSpan timeSpan = DateTime.Now - date.Value;
            double totalSeconds = timeSpan.TotalSeconds;

            if (totalSeconds <= 10)
            {
                return "刚刚";
            }
            else if (totalSeconds < 60)
            {
                return $"{(int)totalSeconds}秒前";
            }
            else if (totalSeconds < 60 * 60)
            {
                return $"{(int)(totalSeconds / 60)}分钟前";
            }
            else if (totalSeconds < 86400)
            {
                return $"{(int)(totalSeconds / 3600)}小时前";
            }
            else if (totalSeconds < 604800)
            {
                return $"{(int)(totalSeconds / 86400)}天前";
            }
            else
            {
                // 超过一周，显示完整日期时间
                return Format(date, DATE_TIME_PATTERN);
            }
        }
    }
}