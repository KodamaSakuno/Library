using System;

namespace Sakuno.SystemInterop
{
    public static partial class NativeDelegates
    {
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        public delegate int TaskDialogCallbackProc(IntPtr hwnd, NativeConstants.TASKDIALOG_NOTIFICATIONS uNotification, IntPtr wParam, IntPtr lParam, IntPtr dwRefData);

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
    }
}
