using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    public static class AudioManager
    {
        static NativeInterfaces.IMMDeviceEnumerator r_DeviceEnumerator;
        static NativeInterfaces.IAudioSessionManager2 r_SessionManager;

        static AudioManagerEventSink r_EventSink;
        static IDisposable r_NotificationSubscription;

        public static AudioDevice DefaultDevice { get; internal set; }

        public static event Action<AudioSessionCreatedEventArgs> NewSession = delegate { };

        static AudioManager()
        {
            r_DeviceEnumerator = (NativeInterfaces.IMMDeviceEnumerator)new NativeInterfaces.MMDeviceEnumerator();

            var rDevice = r_DeviceEnumerator.GetDefaultAudioEndpoint(NativeConstants.DataFlow.Render, NativeConstants.Role.Console);
            DefaultDevice = new AudioDevice(rDevice);

            var rAudioSessionManagerGuid = typeof(NativeInterfaces.IAudioSessionManager2).GUID;
            var rObject = rDevice.Activate(ref rAudioSessionManagerGuid, 0, IntPtr.Zero);

            r_SessionManager = (NativeInterfaces.IAudioSessionManager2)rObject;

            r_EventSink = new AudioManagerEventSink();
            r_DeviceEnumerator.RegisterEndpointNotificationCallback(r_EventSink);
        }

        public static AudioDevice ReloadDefaultDevice()
        {
            if (DefaultDevice != null)
                DefaultDevice.Dispose();

            var rDevice = r_DeviceEnumerator.GetDefaultAudioEndpoint(NativeConstants.DataFlow.Render, NativeConstants.Role.Console);

            return DefaultDevice = new AudioDevice(rDevice);
        }

        public static IEnumerable<AudioSession> EnumerateSessions()
        {
            var rSessionEnumerator = r_SessionManager.GetSessionEnumerator();
            var rCount = rSessionEnumerator.GetCount();

            for (var i = 0; i < rCount; i++)
            {
                var rSessionControl = rSessionEnumerator.GetSession(i);

                yield return new AudioSession((NativeInterfaces.IAudioSessionControl2)rSessionControl);
            }

            Marshal.ReleaseComObject(rSessionEnumerator);
        }

        public static IDisposable StartSessionNotification()
        {
            if (r_NotificationSubscription != null)
                return r_NotificationSubscription;

            Marshal.ReleaseComObject(r_SessionManager.GetSessionEnumerator());

            r_SessionManager.RegisterSessionNotification(r_EventSink);

            return r_NotificationSubscription = Disposable.Create(StopSessionNotification);
        }
        public static void StopSessionNotification()
        {
            if (r_NotificationSubscription == null)
                return;

            r_SessionManager.UnregisterSessionNotification(r_EventSink);

            r_NotificationSubscription = null;
        }

        internal static void OnSessionCreated(NativeInterfaces.IAudioSessionControl rpNewSession)
        {
            if (rpNewSession == null)
                return;

            var rEventArgs = new AudioSessionCreatedEventArgs(new AudioSession((NativeInterfaces.IAudioSessionControl2)rpNewSession));

            NewSession(rEventArgs);

            if (rEventArgs.Release)
                rEventArgs.Session.Dispose();
        }
    }
}
