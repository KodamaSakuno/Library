using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class DoubleUtil
    {
        const double r_Epsilon = 2.2204460492503131E-15;

        public static readonly object Zero = 0.0;
        public static readonly object One = 1.0;
        public static readonly object NaN = double.NaN;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(double rpValue) => Math.Abs(rpValue) < r_Epsilon;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOne(double rpValue) => Math.Abs(rpValue - 1.0) < r_Epsilon;
    }
}
