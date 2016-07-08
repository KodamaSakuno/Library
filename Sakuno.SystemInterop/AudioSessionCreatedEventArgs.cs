using System;

namespace Sakuno.SystemInterop
{
    public class AudioSessionCreatedEventArgs : EventArgs
    {
        public AudioSession Session { get; }

        public bool Release { get; set; } = true;

        internal AudioSessionCreatedEventArgs(AudioSession rpVolumeSession)
        {
            Session = rpVolumeSession;
        }
    }
}
