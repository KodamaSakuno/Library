using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class DoubleUtil
    {
        public static readonly object Zero = 0.0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(this double rpValue) => double.IsInfinity(rpValue);
    }
}
