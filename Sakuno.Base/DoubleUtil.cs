using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class DoubleUtil
    {
        const double r_Epsilon = 2.2204460492503131E-15;

        public static readonly object Zero = 0.0;

        public static bool IsZero(double rpValue) => Math.Abs(rpValue) < r_Epsilon;
    }
}
