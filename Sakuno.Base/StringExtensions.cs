using System.Collections.Generic;

namespace Sakuno
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string rpString) => string.IsNullOrEmpty(rpString);

        public static string Join(this IEnumerable<string> rpStrings, string rpSeparator) => string.Join(rpSeparator, rpStrings);

        public static bool OICEquals(this string rpString, string rpValue) => rpString.Equals(rpValue, System.StringComparison.OrdinalIgnoreCase);
    }
}
