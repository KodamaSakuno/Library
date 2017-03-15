using System;

namespace Sakuno
{
    public static class WeakReferenceExtensions
    {
        public static T GetTargetOrDefault<T>(this WeakReference<T> rpWeakReference) where T : class
        {
            T rResult;
            rpWeakReference.TryGetTarget(out rResult);

            return rResult;
        }
    }
}
