using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class DoubleExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(this double rpValue) => double.IsInfinity(rpValue);
    }
}
