﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Sakuno.SystemInterop
{
    partial class NativeMethods
    {
        public static class User32
        {
            const string DllName = "user32.dll";

            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport(DllName, SetLastError = true)]
            public static extern bool PostMessageW(IntPtr hWnd, NativeConstants.WindowMessage Msg, IntPtr wParam, IntPtr lParam);

            [DllImport(DllName)]
            public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport(DllName)]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, NativeEnums.SetWindowPosition uFlags);

            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport(DllName, SetLastError = true)]
            public static extern bool GetWindowRect(IntPtr hWnd, out NativeStructs.RECT lpRect);

            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport(DllName, SetLastError = true)]
            public static extern bool GetWindowPlacement(IntPtr hWnd, out NativeStructs.WINDOWPLACEMENT lpwndpl);
            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport(DllName, SetLastError = true)]
            public static extern bool SetWindowPlacement(IntPtr hWnd, ref NativeStructs.WINDOWPLACEMENT lpwndpl);

            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport(DllName)]
            public static extern bool GetCursorPos(out NativeStructs.POINT lpPoint);

            [DllImport(DllName, SetLastError = true)]
            public static extern IntPtr GetTopWindow(IntPtr hWnd);

            [DllImport(DllName, SetLastError = true)]
            public static extern IntPtr GetWindow(IntPtr hWnd, NativeConstants.GetWindow uCmd);

            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport(DllName)]
            public static extern bool EnumChildWindows(IntPtr hwndParent, NativeDelegates.EnumWindowsProc lpEnumFunc, IntPtr lParam);

            [DllImport(DllName, CharSet = CharSet.Unicode)]
            public static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpClassName, int nMaxCount);
            [DllImport(DllName, CharSet = CharSet.Unicode)]
            public static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpText, int nMaxCount);

            #region Window Long
            public static IntPtr GetWindowLongPtr(IntPtr hWnd, NativeConstants.GetWindowLong nIndex)
            {
                return new IntPtr(OS.Is64Bit ? WindowLong.GetWindowLongPtrW(hWnd, nIndex) : WindowLong.GetWindowLongW(hWnd, nIndex));
            }
            public static IntPtr SetWindowLongPtr(IntPtr hWnd, NativeConstants.GetWindowLong nIndex, IntPtr dwNewLong)
            {
                return OS.Is64Bit ? WindowLong.SetWindowLongPtrW(hWnd, nIndex, dwNewLong) : new IntPtr(WindowLong.SetWindowLongW(hWnd, nIndex, dwNewLong.ToInt32()));
            }
            public static IntPtr GetClassLongPtr(IntPtr hWnd, NativeConstants.GetClassLong nIndex)
            {
                return new IntPtr(OS.Is64Bit ? WindowLong.GetClassLongPtrW(hWnd, nIndex) : WindowLong.GetClassLongW(hWnd, nIndex));
            }
            public static IntPtr SetClassLongPtr(IntPtr hWnd, NativeConstants.GetClassLong nIndex, IntPtr dwNewLong)
            {
                return OS.Is64Bit ? WindowLong.SetClassLongPtrW(hWnd, nIndex, dwNewLong) : new IntPtr(WindowLong.SetClassLongW(hWnd, nIndex, dwNewLong.ToInt32()));
            }
            static class WindowLong
            {
                [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
                public static extern int GetWindowLongW(IntPtr hWnd, NativeConstants.GetWindowLong nIndex);
                [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
                public static extern long GetWindowLongPtrW(IntPtr hWnd, NativeConstants.GetWindowLong nIndex);

                [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
                public static extern int SetWindowLongW(IntPtr hWnd, NativeConstants.GetWindowLong nIndex, int dwNewLong);
                [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
                public static extern IntPtr SetWindowLongPtrW(IntPtr hWnd, NativeConstants.GetWindowLong nIndex, IntPtr dwNewLong);

                [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
                public static extern int GetClassLongW(IntPtr hWnd, NativeConstants.GetClassLong nIndex);
                [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
                public static extern long GetClassLongPtrW(IntPtr hWnd, NativeConstants.GetClassLong nIndex);

                [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
                public static extern int SetClassLongW(IntPtr hWnd, NativeConstants.GetClassLong nIndex, int dwNewLong);
                [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
                public static extern IntPtr SetClassLongPtrW(IntPtr hWnd, NativeConstants.GetClassLong nIndex, IntPtr dwNewLong);
            }
            #endregion

            #region SystemParametersInfo
            [DllImport(DllName, CharSet = CharSet.Auto)]
            public static extern bool SystemParametersInfo(NativeConstants.SPI uiAction, int uiParam, ref NativeStructs.RECT pvParam, int fWinIni);

            #endregion

            #region Device Context
            [DllImport(DllName, SetLastError = true)]
            public static extern IntPtr GetDC(IntPtr hWnd);
            [DllImport(DllName)]
            public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);
            #endregion

            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport(DllName)]
            public static extern bool ClientToScreen(IntPtr hWnd, ref NativeStructs.POINT lpPoint);
            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport(DllName)]
            public static extern bool ScreenToClient(IntPtr hWnd, ref NativeStructs.POINT lpPoint);

            [DllImport(DllName, SetLastError = true)]
            public static extern IntPtr RegisterPowerSettingNotification(IntPtr hRecipient, ref Guid PowerSettingGuid, int Flags);

            [DllImport(DllName)]
            public static extern IntPtr MonitorFromWindow(IntPtr hwnd, NativeConstants.MFW dwFlags);
            [DllImport(DllName)]
            public static extern bool GetMonitorInfo(IntPtr hMonitor, ref NativeStructs.MONITORINFO lpmi);

        }
    }
}
