using System;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    partial class NativeMethods
    {
        public static class Ntdll
        {
            const string DllName = "ntdll.dll";

            [DllImport(DllName)]
            public static extern IntPtr NtSuspendProcess(IntPtr ProcessHandle);
            [DllImport(DllName)]
            public static extern IntPtr NtResumeProcess(IntPtr ProcessHandle);
        }
    }
}
