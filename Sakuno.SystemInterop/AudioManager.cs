using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    public class AudioManager
    {
        public static AudioManager Instance { get; } = new AudioManager();

        NativeInterfaces.IMMDeviceEnumerator r_DeviceEnumerator;
        NativeInterfaces.IAudioSessionManager2 r_SessionManager;

        AudioManagerEventSink r_EventSink;
        IDisposable r_NotificationSubscription;

        public AudioDevice DefaultDevice { get; internal set; }

        public event EventHandler<AudioSessionCreatedEventArgs> NewSession = delegate { };

        AudioManager()
        {
            r_DeviceEnumerator = (NativeInterfaces.IMMDeviceEnumerator)new NativeInterfaces.MMDeviceEnumerator();
            NativeInterfaces.IMMDevice rDevice;
            Marshal.ThrowExceptionForHR(r_DeviceEnumerator.GetDefaultAudioEndpoint(NativeConstants.DataFlow.Render, NativeConstants.Role.Console, out rDevice));
            DefaultDevice = new AudioDevice(rDevice);

            var rAudioSessionManagerGuid = typeof(NativeInterfaces.IAudioSessionManager2).GUID;
            object rObject;
            Marshal.ThrowExceptionForHR(rDevice.Activate(ref rAudioSessionManagerGuid, 0, IntPtr.Zero, out rObject));

            r_SessionManager = (NativeInterfaces.IAudioSessionManager2)rObject;

            r_EventSink = new AudioManagerEventSink(this);
            r_DeviceEnumerator.RegisterEndpointNotificationCallback(r_EventSink);
        }

        public AudioDevice ReloadDefaultDevice()
        {
            if (DefaultDevice != null)
                DefaultDevice.Dispose();

            NativeInterfaces.IMMDevice rDevice;
            Marshal.ThrowExceptionForHR(r_DeviceEnumerator.GetDefaultAudioEndpoint(NativeConstants.DataFlow.Render, NativeConstants.Role.Console, out rDevice));

            return DefaultDevice = new AudioDevice(rDevice);
        }

        public IEnumerable<AudioSession> EnumerateSessions()
        {
            NativeInterfaces.IAudioSessionEnumerator rSessionEnumerator;
            Marshal.ThrowExceptionForHR(r_SessionManager.GetSessionEnumerator(out rSessionEnumerator));

            int rCount;
            Marshal.ThrowExceptionForHR(rSessionEnumerator.GetCount(out rCount));

            for (var i = 0; i < rCount; i++)
            {
                NativeInterfaces.IAudioSessionControl rSessionControl;
                Marshal.ThrowExceptionForHR(rSessionEnumerator.GetSession(i, out rSessionControl));

                yield return new AudioSession((NativeInterfaces.IAudioSessionControl2)rSessionControl);
            }

            Marshal.ReleaseComObject(rSessionEnumerator);
        }

        public IDisposable StartSessionNotification()
        {
            if (r_NotificationSubscription != null)
                return r_NotificationSubscription;

            NativeInterfaces.IAudioSessionEnumerator rSessionEnumerator;
            Marshal.ThrowExceptionForHR(r_SessionManager.GetSessionEnumerator(out rSessionEnumerator));
            Marshal.ReleaseComObject(rSessionEnumerator);

            Marshal.ThrowExceptionForHR(r_SessionManager.RegisterSessionNotification(r_EventSink));

            return r_NotificationSubscription = Disposable.Create(StopSessionNotification);
        }
        public void StopSessionNotification()
        {
            if (r_NotificationSubscription == null)
                return;

            Marshal.ThrowExceptionForHR(r_SessionManager.UnregisterSessionNotification(r_EventSink));

            r_NotificationSubscription = null;
        }

        internal void OnSessionCreated(NativeInterfaces.IAudioSessionControl rpNewSession)
        {
            if (rpNewSession == null)
                return;

            var rEventArgs = new AudioSessionCreatedEventArgs(new AudioSession((NativeInterfaces.IAudioSessionControl2)rpNewSession));

            NewSession(this, rEventArgs);

            if (rEventArgs.Release)
                rEventArgs.Session.Dispose();
        }
    }
}
