using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    partial class NativeMethods
    {
        public static class ComCtl32
        {
            const string DllName = "comctl32.dll";

            [DllImport(DllName, PreserveSig = true)]
            public static extern int TaskDialogIndirect([In] ref NativeStructs.TASKDIALOGCONFIG taskConfig, [Out] out int pnButton, [Out] out int pnRadioButton, [Out][MarshalAs(UnmanagedType.Bool)] out bool pfVerificationFlagChecked);
        }
    }
}
