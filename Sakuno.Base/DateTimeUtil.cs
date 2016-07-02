using System;

namespace Sakuno
{
    public static class DateTimeUtil
    {
        public const long UnixEpochSeconds = 62135596800L;
        public const long UnixEpochTicks = 621355968000000000L;

        public static DateTimeOffset UnixEpoch { get; } = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public static DateTimeOffset FromUnixTime(long rpValue) => new DateTimeOffset(rpValue * 10000000 + UnixEpochTicks, TimeSpan.Zero);
        public static long ToUnixTime(this DateTimeOffset rpDateTime) => rpDateTime.UtcDateTime.Ticks / 10000000 - UnixEpochSeconds;
    }
}
