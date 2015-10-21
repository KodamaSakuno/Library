using System;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    public class VolumeSession : DisposableObject, NativeInterfaces.IAudioSessionEvents
    {
        NativeInterfaces.IAudioSessionControl2 r_Session;
        NativeInterfaces.ISimpleAudioVolume r_SimpleAudioVolume;

        public int ProcessID
        {
            get
            {
                uint rResult;
                Marshal.ThrowExceptionForHR(r_Session.GetProcessId(out rResult));
                return (int)rResult;
            }
        }

        public int Volume
        {
            get
            {
                float rResult;
                Marshal.ThrowExceptionForHR(r_SimpleAudioVolume.GetMasterVolume(out rResult));
                return (int)(rResult * 100);
            }
            set
            {
                var rGuid = Guid.Empty;
                Marshal.ThrowExceptionForHR(r_SimpleAudioVolume.SetMasterVolume((float)(value / 100.0), ref rGuid));
            }
        }
        public bool IsMute
        {
            get
            {
                bool rResult;
                Marshal.ThrowExceptionForHR(r_SimpleAudioVolume.GetMute(out rResult));
                return rResult;
            }
            set
            {
                var rGuid = Guid.Empty;
                Marshal.ThrowExceptionForHR(r_SimpleAudioVolume.SetMute(value, ref rGuid));
            }
        }

        public string DisplayName
        {
            get
            {
                string rResult;
                Marshal.ThrowExceptionForHR(r_Session.GetDisplayName(out rResult));
                return rResult;
            }
            set
            {
                var rGuid = Guid.Empty;
                Marshal.ThrowExceptionForHR(r_Session.SetDisplayName(value, ref rGuid));
            }
        }

        public event Action<VolumeChangedEventArgs> VolumeChanged = delegate { };
        public event Action<AudioSessionState> StateChanged = delegate { };
        public event Action<AudioSessionDisconnectReason> Disconnected = delegate { };

        public VolumeSession(NativeInterfaces.IAudioSessionControl2 rpSession)
        {
            if (rpSession == null)
                throw new ArgumentNullException(nameof(rpSession));

            r_Session = rpSession;
            r_SimpleAudioVolume = (NativeInterfaces.ISimpleAudioVolume)rpSession;

            r_Session.RegisterAudioSessionNotification(this);
        }

        protected override void DisposeNativeResources()
        {
            Marshal.ReleaseComObject(r_Session);
            r_Session = null;
            r_SimpleAudioVolume = null;
        }

        int NativeInterfaces.IAudioSessionEvents.OnDisplayNameChanged(string rpNewDisplayName, ref Guid rrpEventContext) => 0;
        int NativeInterfaces.IAudioSessionEvents.OnIconPathChanged(string rpNewIconPath, ref Guid rrpEventContext) => 0;
        int NativeInterfaces.IAudioSessionEvents.OnChannelVolumeChanged(uint rpChannelCount, IntPtr rpNewChannelVolumeArray, uint rpChangedChannel, ref Guid rrpEventContext) => 0;
        int NativeInterfaces.IAudioSessionEvents.OnGroupingParamChanged(ref Guid rrpNewGroupingParam, ref Guid rrpEventContext) => 0;

        int NativeInterfaces.IAudioSessionEvents.OnSimpleVolumeChanged(float rpNewVolume, bool rpNewMute, ref Guid rrpEventContext)
        {
            VolumeChanged(new VolumeChangedEventArgs(rpNewMute, (int)(100 * rpNewVolume)));

            return 0;
        }
        int NativeInterfaces.IAudioSessionEvents.OnStateChanged(AudioSessionState rpNewState)
        {
            StateChanged(rpNewState);

            return 0;
        }
        int NativeInterfaces.IAudioSessionEvents.OnSessionDisconnected(AudioSessionDisconnectReason rpDisconnectReason)
        {
            Disconnected(rpDisconnectReason);

            return 0;
        }
    }
}
