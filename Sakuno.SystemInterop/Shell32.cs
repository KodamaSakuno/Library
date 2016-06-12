using System;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    partial class NativeMethods
    {
        public static class Shell32
        {
            const string DllName = "shell32.dll";

            [DllImport(DllName, PreserveSig = false)]
            public static extern void SHCreateItemFromParsingName([In][MarshalAs(UnmanagedType.LPWStr)] string pszPath, [In] IntPtr pbc, [In] ref Guid riid, [Out][MarshalAs(UnmanagedType.Interface)] out NativeInterfaces.IShellItem ppv);
        }
    }
}
