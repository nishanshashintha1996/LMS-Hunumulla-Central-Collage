using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS___Hunumulla_Central_Collage
{
    class DateTimeCalculatoin
    {
        public static DateTime getToday(DateTime date)
        {
            DateTime baseDate = date;
            var today = baseDate;
            return today;
        }

        public static DateTime getYesterday(DateTime date)
        {
            DateTime baseDate = date;
            var yesterday = baseDate.AddDays(-1);
            return yesterday;
        }

        public static DateTime getWeekStartDate(DateTime date)
        {
            DateTime baseDate = date;
            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            return thisWeekStart;
        }

        public static DateTime getWeekEndDate(DateTime date)
        {
            DateTime baseDate = date;
            var thisWeekEnd = getWeekStartDate(date).AddDays(7).AddSeconds(-1).AddDays(-1);
            return thisWeekEnd;
        }

        public static DateTime getLastWeekStart(DateTime date)
        {
            DateTime baseDate = date;
            var lastWeekStart = getWeekStartDate(date).AddDays(-7);
            return lastWeekStart;
        }

        public static DateTime getLastWeekEnd(DateTime date)
        {
            DateTime baseDate = date;
            var lastWeekEnd = getWeekStartDate(date).AddSeconds(-1).AddDays(-1);
            return lastWeekEnd;
        }

        public static DateTime getMonthStart(DateTime date)
        {
            DateTime baseDate = date;
            var today = baseDate;
            var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
            return thisMonthStart;
        }

        public static DateTime getMonthEnd(DateTime date)
        {
            DateTime baseDate = date;
            var today = baseDate;
            var thisMonthEnd = getMonthStart(date).AddMonths(1).AddSeconds(-1).AddDays(-1);
            return thisMonthEnd;
        }
        public static DateTime getlastMonthStart(DateTime date)
        {
            DateTime baseDate = date;
            var today = baseDate;
            var lastMonthStart = getMonthStart(date).AddMonths(-1);
            return lastMonthStart;
        }
        public static DateTime getlastMonthEnd(DateTime date)
        {
            DateTime baseDate = date;
            var today = baseDate;
            var lastMonthEnd = getMonthStart(date).AddSeconds(-1).AddDays(-1);
            return lastMonthEnd;
        }
    }
}
