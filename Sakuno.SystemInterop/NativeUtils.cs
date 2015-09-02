using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Sakuno.SystemInterop
{
    public static class NativeUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Succeeded(int rpResult) => rpResult >= 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Failed(int rpResult) => rpResult < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort HiWord(IntPtr rpValue) => (ushort)(rpValue.ToInt64() >> 0x10 & 0xFFFFL);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort LoWord(IntPtr rpValue) => (ushort)(rpValue.ToInt64() & 0xFFFFL);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point ToPoint(this IntPtr rpHandle) => new Point(NativeUtils.LoWord(rpHandle), NativeUtils.HiWord(rpHandle));
    }
}
