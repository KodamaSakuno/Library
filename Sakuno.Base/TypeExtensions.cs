using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class TypeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAssignableFrom<T>(this Type rpType) => rpType.IsAssignableFrom(typeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSubclassOf<T>(this Type rpType) => rpType.IsSubclassOf(typeof(T));
    }
}
