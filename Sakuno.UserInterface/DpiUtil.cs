using Sakuno.SystemInterop;
using System;

namespace Sakuno.UserInterface
{
    public static class DpiUtil
    {
        public const double DefaultDpiX = 96.0;
        public const double DefaultDpiY = 96.0;

        public static double DpiX { get; }
        public static double DpiY { get; }

        public static double ScaleX { get; }
        public static double ScaleY { get; }

        static DpiUtil()
        {
            var rDC = NativeMethods.User32.GetDC(IntPtr.Zero);
            if (rDC != IntPtr.Zero)
            {
                DpiX = NativeMethods.Gdi32.GetDeviceCaps(rDC, NativeConstants.DeviceCap.LOGPIXELSX);
                DpiY = NativeMethods.Gdi32.GetDeviceCaps(rDC, NativeConstants.DeviceCap.LOGPIXELSY);

                NativeMethods.User32.ReleaseDC(IntPtr.Zero, rDC);
            }
            else
            {
                DpiX = DefaultDpiX;
                DpiY = DefaultDpiY;
            }

            ScaleX = DpiX / DefaultDpiX;
            ScaleY = DpiY / DefaultDpiY;
        }
    }
}
