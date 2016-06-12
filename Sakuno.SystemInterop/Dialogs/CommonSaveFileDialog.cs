using System;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop.Dialogs
{
    public sealed class CommonSaveFileDialog : CommonFileDialog
    {
        NativeInterfaces.IFileSaveDialog r_Dialog;

        public bool CreatePrompt { get; set; } = true;
        public bool OverwritePrompt { get; set; } = true;

        internal override NativeInterfaces.IFileDialog CreateDialog()
        {
            r_Dialog = (NativeInterfaces.IFileSaveDialog)new NativeInterfaces.FileSaveDialogRCW();
            return (NativeInterfaces.IFileDialog)r_Dialog;
        }

        protected override void DisposeNativeResources() => Marshal.FinalReleaseComObject(r_Dialog);

        protected override NativeEnums.FILEOPENDIALOGOPTIONS GetOptionsFromDeviredClass(NativeEnums.FILEOPENDIALOGOPTIONS rpOptions)
        {
            if (CreatePrompt)
                rpOptions |= NativeEnums.FILEOPENDIALOGOPTIONS.FOS_CREATEPROMPT;
            if (OverwritePrompt)
                rpOptions |= NativeEnums.FILEOPENDIALOGOPTIONS.FOS_OVERWRITEPROMPT;

            return rpOptions;
        }

        protected override void ProcessResult()
        {
            NativeInterfaces.IShellItem rItem;
            r_Dialog.GetResult(out rItem);

            if (rItem == null)
                throw new InvalidOperationException("Save with null item");

            r_Filenames.Add(GetFilenameFromShellItem(rItem));
        }
    }
}
