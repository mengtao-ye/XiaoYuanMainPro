using System;

namespace Game
{
    public static class DateTimeTools
    {
        /// <summary>
        /// 将秒时间戳转换成显示的时间字符
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string UnixTimeToShowTimeStr(long time)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(time);
            string str = "一分钟前";
            if (DateTime.Now.Year - dateTimeOffset.LocalDateTime.Year > 0)
            {
                str =  dateTimeOffset.LocalDateTime.ToString("yyyy年MMMdd日");
            }
            else if (DateTime.Now.Month - dateTimeOffset.LocalDateTime.Month > 0)
            {
                str = dateTimeOffset.LocalDateTime.ToString("MMMdd日");
            }
            else if (DateTime.Now.Day - dateTimeOffset.LocalDateTime.Day > 0)
            {
                int day = DateTime.Now.Day - dateTimeOffset.LocalDateTime.Day;
                if (day > 3)
                {
                    str = dateTimeOffset.LocalDateTime.ToString("MMMdd日");
                }
                else
                { 
                     str = day + "天前";
                }
            }
            else if (DateTime.Now.Hour - dateTimeOffset.LocalDateTime.Hour > 0)
            {
                str = dateTimeOffset.LocalDateTime.Hour + ":"+ dateTimeOffset.LocalDateTime.Minute;
            }
            else if (DateTime.Now.Minute - dateTimeOffset.LocalDateTime.Minute > 0)
            {
                str = DateTime.Now.Minute - dateTimeOffset.LocalDateTime.Minute + "分钟前";
            }
            return str;
        }
    }
}
