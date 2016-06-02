using System;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    partial class NativeMethods
    {
        public static class PowrProf
        {
            const string DllName = "powrprof.dll";

            [DllImport(DllName)]
            public static extern int CallNtPowerInformation(NativeConstants.POWER_INFORMATION_LEVEL InformationLevel, IntPtr lpInputBuffer, uint nInputBufferSize, [Out] out NativeStructs.SYSTEM_BATTERY_STATE lpOutputBuffer, uint nOutputBufferSize);
        }
    }
}
