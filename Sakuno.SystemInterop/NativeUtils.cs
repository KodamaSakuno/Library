using System;
using System.Windows;

namespace Sakuno.SystemInterop
{
    public static class NativeUtils
    {
        public static bool Succeeded(int rpResult) => rpResult >= 0;
        public static bool Failed(int rpResult) => rpResult < 0;

        public static ushort HiWord(IntPtr rpValue) => (ushort)(rpValue.ToInt64() >> 0x10 & 0xFFFFL);
        public static ushort LoWord(IntPtr rpValue) => (ushort)(rpValue.ToInt64() & 0xFFFFL);

        public static Point ToPoint(this IntPtr rpHandle) => new Point(NativeUtils.LoWord(rpHandle), NativeUtils.HiWord(rpHandle));

    }
}
