using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    partial class NativeMethods
    {
        public static class ComCtl32
        {
            const string DllName = "comctl32.dll";

            [DllImport(DllName, PreserveSig = true)]
            public static extern int TaskDialogIndirect(ref NativeStructs.TASKDIALOGCONFIG taskConfig, out int pnButton, out int pnRadioButton, [MarshalAs(UnmanagedType.Bool)] out bool pfVerificationFlagChecked);
        }
    }
}
