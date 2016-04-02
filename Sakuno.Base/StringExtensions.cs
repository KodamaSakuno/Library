using System;
using System.Collections.Generic;

namespace Sakuno
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string rpString) => string.IsNullOrEmpty(rpString);

        public static string Join(this IEnumerable<string> rpStrings, string rpSeparator) => string.Join(rpSeparator, rpStrings);

        public static bool OICEquals(this string rpString, string rpValue) => rpString.Equals(rpValue, StringComparison.OrdinalIgnoreCase);

        public static int OICIndexOf(this string rpString, string rpValue) => rpString.IndexOf(rpValue, StringComparison.OrdinalIgnoreCase);
        public static int OICLastIndexOf(this string rpString, string rpValue) => rpString.LastIndexOf(rpValue, StringComparison.OrdinalIgnoreCase);
        public static bool OICContains(this string rpString, string rpValue) => rpString.OICIndexOf(rpValue) >= 0;

        public static bool OICStartsWith(this string rpString, string rpValue) => rpString.StartsWith(rpValue, StringComparison.OrdinalIgnoreCase);
        public static bool OICEndsWith(this string rpString, string rpValue) => rpString.EndsWith(rpValue, StringComparison.OrdinalIgnoreCase);
    }
}
