using System;

namespace Sakuno.SystemInterop
{
    public class AudioSessionVolumeChangedEventArgs : EventArgs
    {
        public bool IsMute { get; set; }
        public int Volume { get; set; }

        public AudioSessionVolumeChangedEventArgs(bool rpIsMute, int rpVolumn)
        {
            IsMute = rpIsMute;
            Volume = rpVolumn;
        }
    }
}
