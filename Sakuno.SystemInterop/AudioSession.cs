using System;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    public class AudioSession : DisposableObject
    {
        NativeInterfaces.IAudioSessionControl2 r_Session;
        NativeInterfaces.ISimpleAudioVolume r_SimpleAudioVolume;

        AudioSessionEventSink r_EventSink;

        public bool IsSystemSoundsSession => r_Session.IsSystemSoundsSession();

        public int ProcessID => r_Session.GetProcessId();

        public AudioSessionState State => r_Session.GetState();

        public int Volume
        {
            get { return (int)(r_SimpleAudioVolume.GetMasterVolume() * 100); }
            set
            {
                var rGuid = Guid.Empty;
                r_SimpleAudioVolume.SetMasterVolume((float)(value / 100.0), ref rGuid);
            }
        }
        public bool IsMute
        {
            get { return r_SimpleAudioVolume.GetMute(); }
            set
            {
                var rGuid = Guid.Empty;
                r_SimpleAudioVolume.SetMute(value, ref rGuid);
            }
        }

        public string DisplayName
        {
            get { return r_Session.GetDisplayName(); }
            set
            {
                var rGuid = Guid.Empty;
                r_Session.SetDisplayName(value, ref rGuid);
            }
        }

        public event EventHandler<AudioSessionDisconnectReason> Disconnected = delegate { };
        public event EventHandler<AudioSessionVolumeChangedEventArgs> VolumeChanged = delegate { };
        public event EventHandler<AudioSessionState> StateChanged = delegate { };

        internal AudioSession(NativeInterfaces.IAudioSessionControl2 rpSession)
        {
            if (rpSession == null)
                throw new ArgumentNullException(nameof(rpSession));

            r_Session = rpSession;
            r_SimpleAudioVolume = (NativeInterfaces.ISimpleAudioVolume)rpSession;

            r_EventSink = new AudioSessionEventSink(this);
            r_Session.RegisterAudioSessionNotification(r_EventSink);
        }

        protected override void DisposeNativeResources()
        {
            r_Session.UnregisterAudioSessionNotification(r_EventSink);

            Marshal.ReleaseComObject(r_Session);

            r_Session = null;
            r_SimpleAudioVolume = null;
        }

        internal void OnSessionDisconnected(AudioSessionDisconnectReason rpDisconnectReason) => Disconnected(this, rpDisconnectReason);
        internal void OnVolumeChanged(AudioSessionVolumeChangedEventArgs e) => VolumeChanged(this, e);
        internal void OnStateChanged(AudioSessionState rpState) => StateChanged(this, rpState);
    }
}
