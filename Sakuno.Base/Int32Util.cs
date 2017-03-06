using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class Int32Util
    {
        public static readonly object Zero = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int HighestBit(int rpValue) => UInt32Util.HighestBit((uint)rpValue);
    }
}
