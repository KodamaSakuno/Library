using System;

namespace Sakuno
{
    public static class OS
    {
        static OperatingSystem r_OS = Environment.OSVersion;

        public static bool IsWindows => r_OS.Platform == PlatformID.Win32NT;
        public static bool Is64Bit => IntPtr.Size == 8;

        public static bool IsWinXPOrLater => r_OS.Version.Major >= 6 || (r_OS.Version.Major == 5 && r_OS.Version.Minor >= 1);
        public static bool IsWin7OrLater => r_OS.Version.Major >= 7 || (r_OS.Version.Major == 6 && r_OS.Version.Minor >= 1);
        public static bool IsWin8OrLater => r_OS.Version.Major >= 7 || (r_OS.Version.Major == 6 && r_OS.Version.Minor >= 2);
        public static bool IsWin81OrLater => r_OS.Version.Major >= 7 || (r_OS.Version.Major == 6 && r_OS.Version.Minor >= 3);
        public static bool IsWin10OrLater => r_OS.Version.Major >= 10;
    }
}
