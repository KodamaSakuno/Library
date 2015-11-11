using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Sakuno.SystemInterop
{
    public static class PowerMonitor
    {
        public static event Action<int> BatteryRemainingPercentageChanged = delegate { };
        public static event Action<PowerSource> PowerSourceChanged = delegate { };

        public static void RegisterMonitor(Window rpWindow)
        {
            var rHwndSource = PresentationSource.FromVisual(rpWindow) as HwndSource;
            if (rHwndSource == null)
                return;

            Guid rGuid;

            rGuid = NativeGuids.GUID_BATTERY_PERCENTAGE_REMAINING;
            NativeMethods.User32.RegisterPowerSettingNotification(rHwndSource.Handle, ref rGuid, 0);

            rGuid = NativeGuids.GUID_ACDC_POWER_SOURCE;
            NativeMethods.User32.RegisterPowerSettingNotification(rHwndSource.Handle, ref rGuid, 0);

            rHwndSource.AddHook(WndProc);
        }

        static IntPtr WndProc(IntPtr rpHandle, int rpMessage, IntPtr rpWParam, IntPtr rpLParam, ref bool rrpHandled)
        {
            if ((NativeConstants.WindowMessage)rpMessage == NativeConstants.WindowMessage.WM_POWERBROADCAST && (NativeConstants.PBT)rpWParam == NativeConstants.PBT.PBT_POWERSETTINGCHANGE)
            {
                var rPowerBroadcastSetting = (NativeStructs.POWERBROADCAST_SETTING)Marshal.PtrToStructure(rpLParam, typeof(NativeStructs.POWERBROADCAST_SETTING));

                if (rPowerBroadcastSetting.PowerSetting == NativeGuids.GUID_BATTERY_PERCENTAGE_REMAINING)
                    BatteryRemainingPercentageChanged(rPowerBroadcastSetting.Data);
                else if (rPowerBroadcastSetting.PowerSetting == NativeGuids.GUID_ACDC_POWER_SOURCE)
                    PowerSourceChanged((PowerSource)rPowerBroadcastSetting.Data);
            }

            return IntPtr.Zero;
        }
    }
}
