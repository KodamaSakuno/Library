using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Sakuno.SystemInterop
{
    partial class NativeInterfaces
    {
        [ComImport]
        [Guid("000214E6-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IShellFolder
        {
            [PreserveSig]
            int ParseDisplayName(IntPtr hwnd, [In][MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In][MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, [In][Out] ref uint pchEaten, [Out] IntPtr ppidl, [In][Out] ref uint pdwAttributes);
            [PreserveSig]
            int EnumObjects([In] IntPtr hwnd, [In] NativeEnums.SHCONTF grfFlags, [Out][MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenumIDList);
            [PreserveSig]
            int BindToObject([In] IntPtr pidl, IntPtr pbc, [In] ref Guid riid, [Out][MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);
            [PreserveSig]
            int BindToStorage([In] ref IntPtr pidl, [In][MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In] ref Guid riid, [Out] out IntPtr ppv);
            [PreserveSig]
            int CompareIDs([In] IntPtr lParam, [In] ref IntPtr pidl1, [In] ref IntPtr pidl2);
            [PreserveSig]
            int CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid, [Out] out IntPtr ppv);
            [PreserveSig]
            int GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In][Out] ref uint rgfInOut);
            [PreserveSig]
            int GetUIObjectOf([In] IntPtr hwndOwner, [In] uint cidl, [In] IntPtr apidl, [In] ref Guid riid, [In][Out] ref uint rgfReserved, out IntPtr ppv);
            [PreserveSig]
            int GetDisplayNameOf([In] ref IntPtr pidl, [In] uint uFlags, [Out][MarshalAs(UnmanagedType.LPWStr)] out string pName);
            [PreserveSig]
            int SetNameOf([In] IntPtr hwnd, [In] ref IntPtr pidl, [In][MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] uint uFlags, [Out] out IntPtr ppidlOut);
        }

        [ComImport]
        [Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IShellItem
        {
            [PreserveSig]
            int BindToHandler([In] IntPtr pbc, [In] ref Guid bhid, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IShellFolder ppvOut);
            [PreserveSig]
            int GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
            [PreserveSig]
            int GetDisplayName([In] NativeEnums.SIGDN sigdnName, [Out][MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
            [PreserveSig]
            int GetAttributes([In] NativeEnums.SFGAO sfgaoMask, out NativeEnums.SFGAO psfgaoAttribs);
            [PreserveSig]
            int Compare([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] NativeEnums.SICHINTF hint, [Out] out int piOrder);
        }

        [ComImport]
        [Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IShellItemArray
        {
            [PreserveSig]
            int BindToHandler([In][MarshalAs(UnmanagedType.Interface)] IntPtr pbc, [In] ref Guid bhid, [In] ref Guid riid, [Out] out IntPtr ppvOut);
            [PreserveSig]
            int GetPropertyStore([In] int flags, [In] ref Guid riid, out IntPtr ppv);
            [PreserveSig]
            int GetPropertyDescriptionList([In] ref NativeStructs.PROPERTYKEY keyType, [In] ref Guid riid, [Out] out IntPtr ppv);
            [PreserveSig]
            int GetAttributes([In] NativeConstants.SIATTRIBFLAGS dwAttribFlags, [In] NativeEnums.SFGAO sfgaoMask, [Out] out NativeEnums.SFGAO psfgaoAttribs);
            [PreserveSig]
            int GetCount(out uint pdwNumItems);
            [PreserveSig]
            int GetItemAt([In] uint dwIndex, [Out][MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
            [PreserveSig]
            int EnumItems([Out][MarshalAs(UnmanagedType.Interface)] out IntPtr ppenumShellItems);
        }

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

        [ComImport]
        [Guid("B4DB1657-70D7-485E-8E3E-6FCB5A5C1802")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IModalWindow
        {
            [PreserveSig]
            int Show([In] IntPtr hwndOwner);
        }
    }
}
