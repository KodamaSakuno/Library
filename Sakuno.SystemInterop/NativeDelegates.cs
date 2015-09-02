using System;

namespace Sakuno.SystemInterop
{
    public static partial class NativeDelegates
    {
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    }
}
