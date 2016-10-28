using System;

namespace Sakuno.SystemInterop
{
    public class TaskDialogTickEventArgs : EventArgs
    {
        public int Ticks { get; internal set; }

        public bool Reset { get; set; }

        internal TaskDialogTickEventArgs() { }
    }
}
