using System;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    partial class NativeMethods
    {
        public static class Kernel32
        {
            const string DllName = "kernel32.dll";

            [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool DeleteFileW([MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

            [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern ushort GlobalAddAtomW([MarshalAs(UnmanagedType.LPWStr)] string lpString);
            [DllImport(DllName, SetLastError = true)]
            public static extern ushort GlobalDeleteAtom(short nAtom);

            [DllImport(DllName, SetLastError = true)]
            public static extern IntPtr OpenProcess(NativeEnums.ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

            [DllImport(DllName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CloseHandle(IntPtr hObject);

            [DllImport(DllName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool QueryThreadCycleTime(IntPtr ThreadHandle, out ulong CycleTime);

            [DllImport(DllName)]
            public static extern IntPtr GetCurrentProcess();
            [DllImport(DllName)]
            public static extern IntPtr GetCurrentThread();

            [DllImport(DllName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool IsWow64Process(IntPtr hProcess, [MarshalAs(UnmanagedType.Bool)] out bool Wow64Process);

            [DllImport(DllName)]
            public static extern void GetSystemInfo(out NativeStructs.SYSTEM_INFO lpSystemInfo);
            [DllImport(DllName)]
            public static extern void GetNativeSystemInfo(out NativeStructs.SYSTEM_INFO lpSystemInfo);

            [DllImport(DllName, SetLastError = true)]
            public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out NativeStructs.MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);

            [DllImport(DllName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesRead);
        }
    }
}
