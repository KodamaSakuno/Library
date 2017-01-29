using System;
using System.Reflection;

namespace Sakuno
{
    public static class DateTimeUtil
    {
        public const long UnixEpochSeconds = 62135596800L;
        public const long UnixEpochTicks = 621355968000000000L;

        public const long TicksPerDay = 864000000000L;

        public const int DaysPerYear = 365;
        public const int DaysPer4Years = DaysPerYear * 4 + 1;
        public const int DaysPer100Years = DaysPer4Years * 25 - 1;
        public const int DaysPer400Years = DaysPer100Years * 4 + 1;

        internal static readonly int[] DaysToMonth365, DaysToMonth366;

        public static DateTimeOffset UnixEpoch { get; } = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        static DateTimeUtil()
        {
            var rType = typeof(DateTime);
            var rFlags = BindingFlags.NonPublic | BindingFlags.Static;

            DaysToMonth365 = (int[])rType.GetField(nameof(DaysToMonth365), rFlags).GetValue(null);
            DaysToMonth366 = (int[])rType.GetField(nameof(DaysToMonth366), rFlags).GetValue(null);
        }

        public static DateTimeOffset FromUnixTime(long rpValue) => new DateTimeOffset(rpValue * 10000000 + UnixEpochTicks, TimeSpan.Zero);
        public static long ToUnixTime(this DateTimeOffset rpDateTime) => rpDateTime.UtcDateTime.Ticks / 10000000 - UnixEpochSeconds;

        internal static void ExtractDate(long rpTicks, out int ropYear, out int ropMonth, out int ropDay, out bool ropIsLeapYear)
        {
            var rpTotalDays = (int)(rpTicks / TicksPerDay);

            var rPeriod400Years = rpTotalDays / DaysPer400Years;
            rpTotalDays -= rPeriod400Years * DaysPer400Years;

            var rPeriod100Years = rpTotalDays / DaysPer100Years;
            if (rPeriod100Years == 4)
                rPeriod100Years = 3;
            rpTotalDays -= rPeriod100Years * DaysPer100Years;

            var rPeriod4Years = rpTotalDays / DaysPer4Years;
            rpTotalDays -= rPeriod4Years * DaysPer4Years;

            var rPeriod1Year = rpTotalDays / DaysPerYear;
            if (rPeriod1Year == 4)
                rPeriod1Year = 3;

            ropYear = rPeriod400Years * 400 + rPeriod100Years * 100 + rPeriod4Years * 4 + rPeriod1Year + 1;

            rpTotalDays -= rPeriod1Year * DaysPerYear;

            ropIsLeapYear = rPeriod1Year == 3 && (rPeriod4Years != 24 || rPeriod100Years == 3);

            var rDays = ropIsLeapYear ? DaysToMonth366 : DaysToMonth365;

            ropMonth = rpTotalDays >> 5 + 1;
            while (rpTotalDays >= rDays[ropMonth])
                ropMonth++;

            ropDay = rpTotalDays - rDays[ropMonth - 1] + 1;
        }
    }
}
