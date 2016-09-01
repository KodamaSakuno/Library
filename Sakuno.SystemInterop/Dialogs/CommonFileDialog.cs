using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Interop;

namespace Sakuno.SystemInterop.Dialogs
{
    public abstract class CommonFileDialog : DisposableObject
    {
        static Guid r_ShellItemGuid = typeof(NativeInterfaces.IShellItem).GUID;

        NativeInterfaces.IFileDialog r_Dialog;

        IntPtr r_OwnerWindowHandle;

        public bool ShowPlacesList { get; set; } = true;
        public bool AddToRecent { get; set; } = true;
        public bool PathMustExist { get; set; } = true;
        public bool FileMustExist { get; set; } = true;

        public bool ShowHiddenItems { get; set; }

        public string Title { get; set; }
        public string OKButtonText { get; set; }

        public string DefaultFilename { get; set; }
        public string DefaultDirectory { get; set;}
        public string DefaultExtension { get; set; }

        public string InitialFolder { get; set; }
        public string DefaultFolder { get; set; }

        public IList<CommonFileDialogFileType> FileTypes { get; } = new List<CommonFileDialogFileType>();
        bool r_IsFileTypesSet;

        protected List<string> r_Filenames = new List<string>();
        public string Filename
        {
            get
            {
                if (r_Filenames.Count == 0)
                    throw new InvalidOperationException();

                var rResult = r_Filenames[0];

                if (!DefaultExtension.IsNullOrEmpty())
                    rResult = Path.ChangeExtension(rResult, DefaultExtension);

                return rResult;
            }
        }

        public CommonFileDialogResult Show()
        {
            EnsureDialog();

            if (r_OwnerWindowHandle == IntPtr.Zero)
            {
                var rOwnerWindow = Application.Current?.MainWindow;
                if (rOwnerWindow != null)
                    r_OwnerWindowHandle = new WindowInteropHelper(rOwnerWindow).Handle;
            }

            var rOptions = GetOptions();
            r_Dialog.SetOptions(rOptions);

            if (!Title.IsNullOrEmpty())
                r_Dialog.SetTitle(Title);

            if (!OKButtonText.IsNullOrEmpty())
                r_Dialog.SetOkButtonLabel(OKButtonText);

            if (!DefaultExtension.IsNullOrEmpty())
                r_Dialog.SetDefaultExtension(DefaultExtension);

            if (!DefaultFilename.IsNullOrEmpty())
                r_Dialog.SetFileName(DefaultFilename);

            if (!InitialFolder.IsNullOrEmpty())
            {
                var rInitialFolder = GetShellItemFromFilename(InitialFolder);
                if (rInitialFolder != null)
                    r_Dialog.SetFolder(rInitialFolder);
            }
            if (!DefaultFolder.IsNullOrEmpty())
            {
                var rDefaultFolder = GetShellItemFromFilename(DefaultFolder);
                if (rDefaultFolder != null)
                    r_Dialog.SetDefaultFolder(rDefaultFolder);
            }

            if ((rOptions & NativeEnums.FILEOPENDIALOGOPTIONS.FOS_PICKFOLDERS) == 0 && !r_IsFileTypesSet && FileTypes.Count > 0)
            {
                r_Dialog.SetFileTypes(FileTypes.Count, FileTypes.Select(r => r.ToFilterSpec()).ToArray());

                r_IsFileTypesSet = true;
            }

            return ShowCore();
        }
        public CommonFileDialogResult Show(IntPtr rpOwnerWindowHandle)
        {
            if (rpOwnerWindowHandle == IntPtr.Zero)
                throw new ArgumentException("Invalid handle");

            r_OwnerWindowHandle = rpOwnerWindowHandle;
            return Show();
        }
        public CommonFileDialogResult Show(Window rpOwnerWindow)
        {
            if (rpOwnerWindow == null)
                throw new ArgumentNullException(nameof(rpOwnerWindow));

            r_OwnerWindowHandle = new WindowInteropHelper(rpOwnerWindow).Handle;
            return Show();
        }
        CommonFileDialogResult ShowCore()
        {
            var rResult = r_Dialog.Show(r_OwnerWindowHandle);

            r_Filenames.Clear();
            if (rResult == 0)
            {
                ProcessResult();

                return CommonFileDialogResult.OK;
            }

            return CommonFileDialogResult.Cancel;
        }

        public void AddPlaceList(string rpPath, CommonFileDialogAddPlaceLocation rpLocation)
        {
            EnsureDialog();

            r_Dialog.AddPlace(GetShellItemFromFilename(rpPath), rpLocation);
        }

        internal void EnsureDialog()
        {
            if (r_Dialog == null)
                r_Dialog = CreateDialog();
        }
        internal abstract NativeInterfaces.IFileDialog CreateDialog();

        NativeEnums.FILEOPENDIALOGOPTIONS GetOptions()
        {
            var rOptions = GetOptionsFromDeviredClass(NativeEnums.FILEOPENDIALOGOPTIONS.FOS_NOTESTFILECREATE);

            if (!ShowPlacesList)
                rOptions |= NativeEnums.FILEOPENDIALOGOPTIONS.FOS_HIDEPINNEDPLACES;

            if (!AddToRecent)
                rOptions |= NativeEnums.FILEOPENDIALOGOPTIONS.FOS_DONTADDTORECENT;

            if (PathMustExist)
                rOptions |= NativeEnums.FILEOPENDIALOGOPTIONS.FOS_PATHMUSTEXIST;
            if (FileMustExist)
                rOptions |= NativeEnums.FILEOPENDIALOGOPTIONS.FOS_FILEMUSTEXIST;

            if (ShowHiddenItems)
                rOptions |= NativeEnums.FILEOPENDIALOGOPTIONS.FOS_FORCESHOWHIDDEN;

            return rOptions;
        }
        protected abstract NativeEnums.FILEOPENDIALOGOPTIONS GetOptionsFromDeviredClass(NativeEnums.FILEOPENDIALOGOPTIONS rpOptions);

        protected abstract void ProcessResult();

        internal static string GetFilenameFromShellItem(NativeInterfaces.IShellItem rpItem) => rpItem.GetDisplayName(NativeEnums.SIGDN.SIGDN_DESKTOPABSOLUTEPARSING);
        internal static NativeInterfaces.IShellItem GetShellItemFromFilename(string rpFilename)
        {
            NativeInterfaces.IShellItem rResult;
            NativeMethods.Shell32.SHCreateItemFromParsingName(rpFilename, IntPtr.Zero, ref r_ShellItemGuid, out rResult);

            return rResult;
        }
    }
}
