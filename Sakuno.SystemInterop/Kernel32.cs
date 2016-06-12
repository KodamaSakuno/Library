﻿using System.Runtime.InteropServices;

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
        }
    }
}
