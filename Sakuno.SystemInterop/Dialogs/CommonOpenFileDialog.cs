using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop.Dialogs
{
    public sealed class CommonOpenFileDialog : CommonFileDialog
    {
        NativeInterfaces.IFileOpenDialog r_Dialog;

        public bool FolderPicker { get; set; }

        public bool AllowMultiselect { get; set; }

        public string Filename => r_Filenames.Count > 0 ? r_Filenames[0] : null;
        public IEnumerable<string> Filenames
        {
            get
            {
                foreach (var rFilename in r_Filenames)
                    yield return rFilename;
            }
        }

        internal override NativeInterfaces.IFileDialog CreateDialog()
        {
            r_Dialog = (NativeInterfaces.IFileOpenDialog)new NativeInterfaces.FileOpenDialogRCW();
            return (NativeInterfaces.IFileDialog)r_Dialog;
        }

        protected override void DisposeNativeResources() => Marshal.FinalReleaseComObject(r_Dialog);

        protected override NativeEnums.FILEOPENDIALOGOPTIONS GetOptionsFromDeviredClass(NativeEnums.FILEOPENDIALOGOPTIONS rpOptions)
        {
            if (FolderPicker)
                rpOptions |= NativeEnums.FILEOPENDIALOGOPTIONS.FOS_PICKFOLDERS;

            if (AllowMultiselect)
                rpOptions |= NativeEnums.FILEOPENDIALOGOPTIONS.FOS_ALLOWMULTISELECT;

            return rpOptions;
        }

        protected override void ProcessResult()
        {
            NativeInterfaces.IShellItemArray rArray;
            r_Dialog.GetResults(out rArray);

            uint rCount;
            rArray.GetCount(out rCount);

            for (var i = 0; i < rCount; i++)
            {
                NativeInterfaces.IShellItem rItem;
                rArray.GetItemAt((uint)i, out rItem);

                r_Filenames.Add(GetFilenameFromShellItem(rItem));
            }
        }
    }
}
