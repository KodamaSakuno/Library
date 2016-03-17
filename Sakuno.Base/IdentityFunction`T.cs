using System;

namespace Sakuno
{
    public static class IdentityFunction<T>
    {
        public static Func<T, T> Instance { get; } = r => r;
    }
}
