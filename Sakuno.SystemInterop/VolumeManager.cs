using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    public class VolumeManager : NativeInterfaces.IAudioSessionNotification
    {
        public static VolumeManager Instance { get; } = new VolumeManager();

        NativeInterfaces.IAudioSessionManager2 r_SessionManager;

        public event Action<VolumeSession> NewSession = delegate { };

        VolumeManager()
        {
            var rDeviceEnumerator = (NativeInterfaces.IMMDeviceEnumerator)new NativeInterfaces.MMDeviceEnumerator();
            NativeInterfaces.IMMDevice rDevice;
            Marshal.ThrowExceptionForHR(rDeviceEnumerator.GetDefaultAudioEndpoint(NativeConstants.DataFlow.Render, NativeConstants.Role.Console, out rDevice));

            var rAudioSessionManagerGuid = typeof(NativeInterfaces.IAudioSessionManager2).GUID;
            object rObject;
            Marshal.ThrowExceptionForHR(rDevice.Activate(ref rAudioSessionManagerGuid, 0, IntPtr.Zero, out rObject));

            r_SessionManager = (NativeInterfaces.IAudioSessionManager2)rObject;
            Marshal.ThrowExceptionForHR(r_SessionManager.RegisterSessionNotification(this));

            Marshal.ReleaseComObject(rDevice);
            Marshal.ReleaseComObject(rDeviceEnumerator);
        }

        public IEnumerable<VolumeSession> EnumerateSessions()
        {
            NativeInterfaces.IAudioSessionEnumerator rSessionEnumerator;
            Marshal.ThrowExceptionForHR(r_SessionManager.GetSessionEnumerator(out rSessionEnumerator));

            int rCount;
            Marshal.ThrowExceptionForHR(rSessionEnumerator.GetCount(out rCount));
            for (var i = 0; i < rCount; i++)
            {
                NativeInterfaces.IAudioSessionControl rSessionControl;
                Marshal.ThrowExceptionForHR(rSessionEnumerator.GetSession(i, out rSessionControl));

                yield return new VolumeSession((NativeInterfaces.IAudioSessionControl2)rSessionControl);
            }

            Marshal.ReleaseComObject(rSessionEnumerator);
        }

        int NativeInterfaces.IAudioSessionNotification.OnSessionCreated(NativeInterfaces.IAudioSessionControl rpNewSession)
        {
            NewSession(new VolumeSession((NativeInterfaces.IAudioSessionControl2)rpNewSession));

            return 0;
        }
    }
}
