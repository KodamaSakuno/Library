using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class Int32Util
    {
        public static readonly object Zero = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int HighestBit(int rpValue) => UInt32Util.HighestBit((uint)rpValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RotateLeft(int rpValue, int rpCount) => (int)UInt32Util.RotateLeft((uint)rpValue, rpCount);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RotateRight(int rpValue, int rpCount) => (int)UInt32Util.RotateRight((uint)rpValue, rpCount);
    }
}
