using System;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    class AudioSessionEventSink : NativeInterfaces.IAudioSessionEvents
    {
        AudioSession r_Owner;

        public AudioSessionEventSink(AudioSession rpAudioSession)
        {
            r_Owner = rpAudioSession;
        }

        public void OnSessionDisconnected(AudioSessionDisconnectReason rpDisconnectReason)
        {
            r_Owner.OnSessionDisconnected(rpDisconnectReason);
        }

        public void OnSimpleVolumeChanged(float rpVolume, bool rpMute, ref Guid rpEventContext)
        {
            r_Owner.OnVolumeChanged(new AudioSessionVolumeChangedEventArgs(rpMute, (int)(rpVolume * 100)));
        }

        public void OnStateChanged(AudioSessionState rpState)
        {
            r_Owner.OnStateChanged(rpState);
        }

        public void OnChannelVolumeChanged([In] uint rpChannelCount, [In] IntPtr rpNewChannelVolumeArray, [In] uint rpChangedChannel, [In] ref Guid rpEventContext) { }
        public void OnDisplayNameChanged([In, MarshalAs(UnmanagedType.LPWStr)] string rpNewDisplayName, [In] ref Guid rpEventContext) { }
        public void OnGroupingParamChanged([In] ref Guid rpNewGroupingParam, [In] ref Guid rpEventContext) { }
        public void OnIconPathChanged([In, MarshalAs(UnmanagedType.LPWStr)] string rpNewIconPath, [In] ref Guid rpEventContext) { }
    }
}
