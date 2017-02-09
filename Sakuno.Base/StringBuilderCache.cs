using System;
using System.Text;

namespace Sakuno
{
    public static class StringBuilderCache
    {
        const int MaxLength = 360;

        [ThreadStatic]
        static StringBuilder r_CachedInstance;

        public static StringBuilder Acquire(int rpCapacity = 16)
        {
            if (rpCapacity <= MaxLength)
            {
                var rCachedInstance = r_CachedInstance;
                if (rCachedInstance != null && rpCapacity <= rCachedInstance.Capacity)
                {
                    r_CachedInstance = null;
                    rCachedInstance.Clear();

                    return rCachedInstance;
                }
            }
            return new StringBuilder(rpCapacity);
        }

        public static void Release(StringBuilder rpStringBuilder)
        {
            if (rpStringBuilder.Capacity <= MaxLength)
                r_CachedInstance = rpStringBuilder;
        }
        public static string GetStringAndRelease(this StringBuilder rpStringBuilder)
        {
            var rResult = rpStringBuilder.ToString();
            Release(rpStringBuilder);
            return rResult;
        }
    }
}
