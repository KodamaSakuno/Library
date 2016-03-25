using Sakuno.SystemInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Interop;

namespace Sakuno.UserInterface
{
    public static class WindowUtil
    {
        public static IEnumerable<Window> GetWindowsOrderedByZOrder(IEnumerable<Window> rpWindows)
        {
            var rWindows = rpWindows.Select(r => new { Window = r, Handle = new WindowInteropHelper(r).Handle }).ToDictionary(r => r.Handle, r => r.Window);

            var rResult = new List<Window>();

            var rHandle = NativeMethods.User32.GetTopWindow(IntPtr.Zero);
            while (rHandle != IntPtr.Zero)
            {
                Window rWindow;
                if (rWindows.TryGetValue(rHandle, out rWindow))
                    rResult.Add(rWindow);
                rHandle = NativeMethods.User32.GetWindow(rHandle, NativeConstants.GetWindow.GW_HWNDNEXT);
            }

            return rResult;
        }
    }
}
