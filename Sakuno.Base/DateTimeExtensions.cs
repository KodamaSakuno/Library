using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class DateTimeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTimeOffset AsOffset(this DateTime rpDateTime) => new DateTimeOffset(rpDateTime);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTimeOffset DateAsOffset(this DateTimeOffset rpDateTime) => new DateTimeOffset(rpDateTime.Date, rpDateTime.Offset);

        public static DateTime Tomorrow(this DateTime rpDateTime) =>
            new DateTime(TomorrowTicks(rpDateTime.Ticks), rpDateTime.Kind);
        public static DateTimeOffset Tomorrow(this DateTimeOffset rpDateTime) =>
            new DateTimeOffset(TomorrowTicks(rpDateTime.Ticks), rpDateTime.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static long TomorrowTicks(long rpTicks) => rpTicks - rpTicks % DateTimeUtil.TicksPerDay + DateTimeUtil.TicksPerDay;

        public static DateTime LastMonday(this DateTime rpDateTime) =>
            new DateTime(LastMondayTicks(rpDateTime.Ticks), rpDateTime.Kind);
        public static DateTimeOffset LastMonday(this DateTimeOffset rpDateTime) =>
            new DateTimeOffset(LastMondayTicks(rpDateTime.Ticks), rpDateTime.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static long LastMondayTicks(long rpTicks)
        {
            var rDays = (int)(rpTicks / DateTimeUtil.TicksPerDay);

            return (rDays - rDays % 7) * DateTimeUtil.TicksPerDay;
        }

        public static DateTime NextMonday(this DateTime rpDateTime) =>
            new DateTime(NextMondayTicks(rpDateTime.Ticks), rpDateTime.Kind);
        public static DateTimeOffset NextMonday(this DateTimeOffset rpDateTime) =>
            new DateTimeOffset(NextMondayTicks(rpDateTime.Ticks), rpDateTime.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static long NextMondayTicks(long rpTicks)
        {
            var rDays = (int)(rpTicks / DateTimeUtil.TicksPerDay);

            return (rDays - rDays % 7 + 7) * DateTimeUtil.TicksPerDay;
        }

        public static DateTime StartOfLastMonth(this DateTime rpDateTime) =>
            new DateTime(StartOfLastMonthTicks(rpDateTime.Ticks), rpDateTime.Kind);
        public static DateTimeOffset StartOfLastMonth(this DateTimeOffset rpDateTime) =>
            new DateTimeOffset(StartOfLastMonthTicks(rpDateTime.Ticks), rpDateTime.Offset);
        static long StartOfLastMonthTicks(long rpTicks)
        {
            int rYear, rMonth, rDay;
            bool rIsLeapYear;
            DateTimeUtil.ExtractDate(rpTicks, out rYear, out rMonth, out rDay, out rIsLeapYear);

            if (rDay > 1)
                rpTicks -= (rDay - 1) * DateTimeUtil.TicksPerDay;

            return rpTicks - rpTicks % DateTimeUtil.TicksPerDay;
        }

        public static DateTime StartOfNextMonth(this DateTime rpDateTime) =>
            new DateTime(StartOfNextMonthTicks(rpDateTime.Ticks), rpDateTime.Kind);
        public static DateTimeOffset StartOfNextMonth(this DateTimeOffset rpDateTime) =>
            new DateTimeOffset(StartOfNextMonthTicks(rpDateTime.Ticks), rpDateTime.Offset);
        static long StartOfNextMonthTicks(long rpTicks)
        {
            int rYear, rMonth, rDay;
            bool rIsLeapYear;
            DateTimeUtil.ExtractDate(rpTicks, out rYear, out rMonth, out rDay, out rIsLeapYear);

            var rDays = rIsLeapYear ? DateTimeUtil.DaysToMonth366 : DateTimeUtil.DaysToMonth365;

            rDay = rDays[rMonth] - rDays[rMonth - 1] - rDay + 1;
            rpTicks += rDay * DateTimeUtil.TicksPerDay;

            return rpTicks - rpTicks % DateTimeUtil.TicksPerDay;
        }
    }
}
