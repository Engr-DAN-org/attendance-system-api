using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Utils
{
    public class DateTimeUtils
    {
        public static DateTime OneHourAfter()
        {
            return DateTime.UtcNow.AddHours(1);
        }

        public static DateTime TwoMinutesAfter()
        {
            return DateTime.UtcNow.AddMinutes(2);
        }

        public static DateTime DateTimeNow()
        {
            return DateTime.UtcNow;
        }

        public static string DateTimeNowFormattedString()
        {
            TimeZoneInfo philippineTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
            DateTime philippineTime = TimeZoneInfo.ConvertTimeFromUtc(DateTimeNow(), philippineTimeZone);
            return philippineTime.ToString("MMM dd, yyyy 'at' h:mm tt");
        }


        public static DateOnly DateNow()
        {
            return DateOnly.FromDateTime(DateTime.UtcNow);
        }

        public static TimeOnly TimeNow()
        {
            return TimeOnly.FromDateTime(DateTime.UtcNow);
        }

        public static bool IsExpired(DateTime expiry)
        {
            return DateTimeNow() > expiry;
        }

    }
}