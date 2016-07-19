using System;
using System.Collections.Concurrent;

namespace Sakuno
{
    public static class EnumUtil
    {
        public static object GetBoxed<T>(T rpValue) where T : struct, IComparable, IFormattable, IConvertible
        {
            return BoxedEnum<T>.Get(rpValue);
        }

        static class BoxedEnum<T> where T : struct, IComparable, IFormattable, IConvertible
        {
            static readonly ConcurrentDictionary<T, object> r_Boxes = new ConcurrentDictionary<T, object>();

            public static object Get(T rpValue) => r_Boxes.GetOrAdd(rpValue, r => r);
        }
    }
}
