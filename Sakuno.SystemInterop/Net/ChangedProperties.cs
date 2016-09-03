using System;

namespace Sakuno.SystemInterop.Net
{
    [Flags]
    public enum ChangedProperties
    {
        Connection = 1,
        Description = 2,
        Name = 4,
        Icon = 8,
        Category = 16,
    }
}
