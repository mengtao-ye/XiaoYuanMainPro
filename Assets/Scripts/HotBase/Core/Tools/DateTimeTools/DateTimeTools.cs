using System;

namespace Game
{
    public static class DateTimeTools
    {
        public static long GetCurUnixTime() 
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }
        
        public static long DateTimeToLongByUnix(DateTime dateTime) 
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeSeconds();
        }
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
                str = dateTimeOffset.LocalDateTime.ToString("yyyy年MMMdd日");
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
                str = dateTimeOffset.LocalDateTime.Hour + ":" + dateTimeOffset.LocalDateTime.Minute;
            }
            else if (DateTime.Now.Minute - dateTimeOffset.LocalDateTime.Minute > 0)
            {
                str = DateTime.Now.Minute - dateTimeOffset.LocalDateTime.Minute + "分钟前";
            }
            return str;
        }
        /// <summary>
        /// 获取当前月份有多少天
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetDaysInCurrentMonth(int year, int month)
        {
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
            TimeSpan duration = firstDayOfNextMonth - firstDayOfMonth;
            return duration.Days;
        }
        public static long GetValueByDateTime(DateTime dateTime)
        {
            return GetValueByDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute ,dateTime.Second);
          
        }
        public static long GetValueByDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            return year * 10000000000 + month * 100000000 + day * 1000000 + hour * 10000 + minute * 100 + second;
        }
        public static DateTime GetDateTimeByValue(long value)
        {
            int year = (int)((value / 10000000000));
            int month = (int)((value / 100000000) % 100);
            int day = (int)((value / 1000000) % 100);
            int hour = (int)((value / 10000) % 100);
            int minute = (int)((value / 100) % 100);
            int second = (int)(value % 100);
            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}
