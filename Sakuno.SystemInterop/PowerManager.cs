using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Sakuno.SystemInterop
{
    public static class PowerManager
    {
        public static bool IsBatteryPresent => GetSystemBatteryState().BatteryPresent;
        public static bool IsCharging => GetSystemBatteryState().Charging;

        public static event Action<int> BatteryRemainingPercentageChanged;
        public static event Action<PowerSource> PowerSourceChanged;
        public static event Action<bool> BatteryChargeStatusChanged;

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

        static unsafe IntPtr WndProc(IntPtr rpHandle, int rpMessage, IntPtr rpWParam, IntPtr rpLParam, ref bool rrpHandled)
        {
            if ((NativeConstants.WindowMessage)rpMessage == NativeConstants.WindowMessage.WM_POWERBROADCAST)
            {
                switch ((NativeConstants.PBT)rpWParam)
                {
                    case NativeConstants.PBT.PBT_APMPOWERSTATUSCHANGE:
                        var rState = GetSystemBatteryState();

                        BatteryChargeStatusChanged?.Invoke(rState.Charging);
                        break;

                    case NativeConstants.PBT.PBT_POWERSETTINGCHANGE:
                        var rPowerBroadcastSetting = (NativeStructs.POWERBROADCAST_SETTING*)rpLParam;

                        if (rPowerBroadcastSetting->PowerSetting == NativeGuids.GUID_BATTERY_PERCENTAGE_REMAINING)
                            BatteryRemainingPercentageChanged(rPowerBroadcastSetting->Data);
                        else if (rPowerBroadcastSetting->PowerSetting == NativeGuids.GUID_ACDC_POWER_SOURCE)
                            PowerSourceChanged((PowerSource)rPowerBroadcastSetting->Data);
                        break;
                }
            }

            return IntPtr.Zero;
        }

        public static NativeStructs.SYSTEM_BATTERY_STATE GetSystemBatteryState()
        {
            NativeStructs.SYSTEM_BATTERY_STATE rState;
            NativeMethods.PowrProf.CallNtPowerInformation(NativeConstants.POWER_INFORMATION_LEVEL.SystemBatteryState, IntPtr.Zero, 0, out rState, Marshal.SizeOf(typeof(NativeStructs.SYSTEM_BATTERY_STATE)));
            return rState;
        }
    }
}
