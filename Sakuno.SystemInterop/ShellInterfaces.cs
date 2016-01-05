using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Sakuno.SystemInterop
{
    partial class NativeInterfaces
    {

        [ComImport]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IShellLinkW
        {
            [PreserveSig]
            int GetPath([Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, ref NativeStructs.WIN32_FIND_DATAW pfd, NativeEnums.SLGP fFlags);
            [PreserveSig]
            int GetIDList(out IntPtr ppidl);
            [PreserveSig]
            int SetIDList(IntPtr pidl);
            [PreserveSig]
            int GetDescription([Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            [PreserveSig]
            int SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            [PreserveSig]
            int GetWorkingDirectory([Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            [PreserveSig]
            int SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            [PreserveSig]
            int GetArguments([Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            [PreserveSig]
            int SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            [PreserveSig]
            int GetHotKey(out ushort pwHotkey);
            [PreserveSig]
            int SetHotKey(ushort wHotKey);
            [PreserveSig]
            int GetShowCmd(out int piShowCmd);
            [PreserveSig]
            int SetShowCmd(int iShowCmd);
            [PreserveSig]
            int GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            [PreserveSig]
            int SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            [PreserveSig]
            int SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);
            [PreserveSig]
            int Resolve(IntPtr hwnd, uint fFlags);
            [PreserveSig]
            int SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }
        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        [ClassInterface(ClassInterfaceType.None)]
        public class CShellLink { }

    }
}
