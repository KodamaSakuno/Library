using Microsoft.Win32;
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

        public static int? DotNetFrameworkReleaseNumber
        {
            get
            {
                const string rSubKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

                try
                {
                    using (var rRegisterKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(rSubKey))
                    {
                        var rValue = rRegisterKey?.GetValue("Release");

                        return rValue != null ? (int?)rValue : null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
