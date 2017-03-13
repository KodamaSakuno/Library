using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class UInt32Util
    {
        static readonly int[] r_MultiplyDeBruijnBitPosition = new int[]
        {
            0, 9, 1, 10, 13, 21, 2, 29, 11, 14, 16, 18, 22, 25, 3, 30,
            8, 12, 20, 28, 15, 17, 24, 7, 19, 27, 23, 6, 26, 5, 4, 31,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int HighestBit(uint rpValue)
        {
            rpValue |= rpValue >> 1;
            rpValue |= rpValue >> 2;
            rpValue |= rpValue >> 4;
            rpValue |= rpValue >> 8;
            rpValue |= rpValue >> 16;

            return r_MultiplyDeBruijnBitPosition[rpValue * 0x07C4ACDD >> 27];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(uint rpValue, int rpCount)
        {
            if (rpCount < 0)
                return RotateRight(rpValue, -rpCount);

            var rShift = rpCount & 0x1F;

            return rpValue << rShift | rpValue >> 32 - rShift;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateRight(uint rpValue, int rpCount)
        {
            if (rpCount < 0)
                return RotateLeft(rpValue, -rpCount);

            var rShift = rpCount & 0x1F;

            return rpValue >> rShift | rpValue << 32 - rShift;
        }
    }
}
