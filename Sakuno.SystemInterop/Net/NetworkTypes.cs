using System;

namespace Sakuno.SystemInterop.Net
{
    [Flags]
    public enum NetworkTypes
    {
        Connected = 1,
        Disconnected,
        All,
    }
}
