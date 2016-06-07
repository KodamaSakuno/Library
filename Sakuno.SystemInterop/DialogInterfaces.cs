using Sakuno.SystemInterop.Dialogs;
using System;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    partial class NativeInterfaces
    {
        [ComImport]
        [Guid("42F85136-DB7E-439C-85F1-E4075D135FC8")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IFileDialog
        {
            [PreserveSig]
            int Show([In] IntPtr parent);
            [PreserveSig]
            int SetFileTypes([In] uint cFileTypes, [In][MarshalAs(UnmanagedType.LPArray)] NativeStructs.COMDLG_FILTERSPEC[] rgFilterSpec);
            [PreserveSig]
            int SetFileTypeIndex([In] uint iFileType);
            [PreserveSig]
            int GetFileTypeIndex(out uint piFileType);
            [PreserveSig]
            int Advise([In][MarshalAs(UnmanagedType.Interface)] IFileDialogEvents pfde, out uint pdwCookie);
            [PreserveSig]
            int Unadvise([In] uint dwCookie);
            [PreserveSig]
            int SetOptions([In] NativeEnums.FILEOPENDIALOGOPTIONS fos);
            [PreserveSig]
            int GetOptions([Out] out NativeEnums.FILEOPENDIALOGOPTIONS pfos);
            [PreserveSig]
            int SetDefaultFolder([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi);
            [PreserveSig]
            int SetFolder([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi);
            [PreserveSig]
            int GetFolder([Out][MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
            [PreserveSig]
            int GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
            [PreserveSig]
            int SetFileName([In][MarshalAs(UnmanagedType.LPWStr)] string pszName);
            [PreserveSig]
            int GetFileName([Out][MarshalAs(UnmanagedType.LPWStr)] out string pszName);
            [PreserveSig]
            int SetTitle([In][MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
            [PreserveSig]
            int SetOkButtonLabel([In][MarshalAs(UnmanagedType.LPWStr)] string pszText);
            [PreserveSig]
            int SetFileNameLabel([In][MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
            [PreserveSig]
            int GetResult([Out][MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
            [PreserveSig]
            int AddPlace([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi, CommonFileDialogAddPlaceLocation fdap);
            [PreserveSig]
            int SetDefaultExtension([In][MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);
            [PreserveSig]
            int Close([MarshalAs(UnmanagedType.Error)] int hr);
            [PreserveSig]
            int SetClientGuid([In] ref Guid guid);
            [PreserveSig]
            int ClearClientData();
            [PreserveSig]
            int SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);
        }

        [ComImport]
        [Guid("D57C7288-D4AD-4768-BE02-9D969532D960")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IFileOpenDialog
        {
            [PreserveSig]
            int Show([In] IntPtr parent);
            [PreserveSig]
            int SetFileTypes([In] uint cFileTypes, [In][MarshalAs(UnmanagedType.LPArray)] NativeStructs.COMDLG_FILTERSPEC[] rgFilterSpec);
            [PreserveSig]
            int SetFileTypeIndex([In] uint iFileType);
            [PreserveSig]
            int GetFileTypeIndex(out uint piFileType);
            [PreserveSig]
            int Advise([In][MarshalAs(UnmanagedType.Interface)] IFileDialogEvents pfde, out uint pdwCookie);
            [PreserveSig]
            int Unadvise([In] uint dwCookie);
            [PreserveSig]
            int SetOptions([In] NativeEnums.FILEOPENDIALOGOPTIONS fos);
            [PreserveSig]
            int GetOptions([Out] out NativeEnums.FILEOPENDIALOGOPTIONS pfos);
            [PreserveSig]
            int SetDefaultFolder([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi);
            [PreserveSig]
            int SetFolder([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi);
            [PreserveSig]
            int GetFolder([Out][MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
            [PreserveSig]
            int GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
            [PreserveSig]
            int SetFileName([In][MarshalAs(UnmanagedType.LPWStr)] string pszName);
            [PreserveSig]
            int GetFileName([Out][MarshalAs(UnmanagedType.LPWStr)] out string pszName);
            [PreserveSig]
            int SetTitle([In][MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
            [PreserveSig]
            int SetOkButtonLabel([In][MarshalAs(UnmanagedType.LPWStr)] string pszText);
            [PreserveSig]
            int SetFileNameLabel([In][MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
            [PreserveSig]
            int GetResult([Out][MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
            [PreserveSig]
            int AddPlace([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi, CommonFileDialogAddPlaceLocation fdap);
            [PreserveSig]
            int SetDefaultExtension([In][MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);
            [PreserveSig]
            int Close([MarshalAs(UnmanagedType.Error)] int hr);
            [PreserveSig]
            int SetClientGuid([In] ref Guid guid);
            [PreserveSig]
            int ClearClientData();
            [PreserveSig]
            int SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);
            [PreserveSig]
            int GetResults([MarshalAs(UnmanagedType.Interface)] out IShellItemArray ppenum);
            [PreserveSig]
            int GetSelectedItems([MarshalAs(UnmanagedType.Interface)] out IShellItemArray ppsai);
        }

        [ComImport]
        [Guid("84BCCD23-5FDE-4CDB-AEA4-AF64B83D78AB")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IFileSaveDialog
        {
            [PreserveSig]
            int Show([In] IntPtr parent);
            [PreserveSig]
            int SetFileTypes([In] uint cFileTypes, [In][MarshalAs(UnmanagedType.LPArray)] NativeStructs.COMDLG_FILTERSPEC[] rgFilterSpec);
            [PreserveSig]
            int SetFileTypeIndex([In] uint iFileType);
            [PreserveSig]
            int GetFileTypeIndex(out uint piFileType);
            [PreserveSig]
            int Advise([In][MarshalAs(UnmanagedType.Interface)] IFileDialogEvents pfde, out uint pdwCookie);
            [PreserveSig]
            int Unadvise([In] uint dwCookie);
            [PreserveSig]
            int SetOptions([In] NativeEnums.FILEOPENDIALOGOPTIONS fos);
            [PreserveSig]
            int GetOptions([Out] out NativeEnums.FILEOPENDIALOGOPTIONS pfos);
            [PreserveSig]
            int SetDefaultFolder([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi);
            [PreserveSig]
            int SetFolder([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi);
            [PreserveSig]
            int GetFolder([Out][MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
            [PreserveSig]
            int GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
            [PreserveSig]
            int SetFileName([In][MarshalAs(UnmanagedType.LPWStr)] string pszName);
            [PreserveSig]
            int GetFileName([Out][MarshalAs(UnmanagedType.LPWStr)] out string pszName);
            [PreserveSig]
            int SetTitle([In][MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
            [PreserveSig]
            int SetOkButtonLabel([In][MarshalAs(UnmanagedType.LPWStr)] string pszText);
            [PreserveSig]
            int SetFileNameLabel([In][MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
            [PreserveSig]
            int GetResult([Out][MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
            [PreserveSig]
            int AddPlace([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi, CommonFileDialogAddPlaceLocation fdap);
            [PreserveSig]
            int SetDefaultExtension([In][MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);
            [PreserveSig]
            int Close([MarshalAs(UnmanagedType.Error)] int hr);
            [PreserveSig]
            int SetClientGuid([In] ref Guid guid);
            [PreserveSig]
            int ClearClientData();
            [PreserveSig]
            int SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);
            [PreserveSig]
            void SetSaveAsItem([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi);
            [PreserveSig]
            void SetProperties([In][MarshalAs(UnmanagedType.Interface)] IntPtr pStore);
            [PreserveSig]
            int SetCollectedProperties([In] IPropertyDescriptionList pList, [In] bool fAppendDefault);
            [PreserveSig]
            int GetProperties(out IPropertyStore ppStore);
            [PreserveSig]
            int ApplyProperties([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In][MarshalAs(UnmanagedType.Interface)] IntPtr pStore, [In] ref IntPtr hwnd, [In][MarshalAs(UnmanagedType.Interface)] IntPtr pSink);
        }

        [ComImport]
        [Guid("DC1C5A9C-E88A-4DDE-A5A1-60F82A20AEF7")]
        [ClassInterface(ClassInterfaceType.None)]
        [TypeLibType(TypeLibTypeFlags.FCanCreate)]
        public class FileOpenDialogRCW { }
        [ComImport]
        [Guid("C0B4E2F3-BA21-4773-8DBA-335EC946EB8B")]
        [ClassInterface(ClassInterfaceType.None)]
        [TypeLibType(TypeLibTypeFlags.FCanCreate)]
        public class FileSaveDialogRCW { }

        [ComImport]
        [Guid("973510DB-7D7F-452B-8975-74A85828D354")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IFileDialogEvents
        {
            [PreserveSig]
            int OnFileOk([In][MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);
            [PreserveSig]
            int OnFolderChanging([In][MarshalAs(UnmanagedType.Interface)] IFileDialog pfd, [In][MarshalAs(UnmanagedType.Interface)] IShellItem psiFolder);
            [PreserveSig]
            int OnFolderChange([In][MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);
            [PreserveSig]
            int OnSelectionChange([In][MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);
            [PreserveSig]
            int OnShareViolation([In][MarshalAs(UnmanagedType.Interface)] IFileDialog pfd, [In][MarshalAs(UnmanagedType.Interface)] IShellItem psi, out NativeConstants.FDE_SHAREVIOLATION_RESPONSE pResponse);
            [PreserveSig]
            int OnTypeChange([In][MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);
            [PreserveSig]
            int OnOverwrite([In][MarshalAs(UnmanagedType.Interface)] IFileDialog pfd, [In][MarshalAs(UnmanagedType.Interface)] IShellItem psi, out NativeConstants.FDE_OVERWRITE_RESPONSE pResponse);
        }
    }
}
