using System;
using System.Runtime.Serialization;

namespace Sakuno
{
    public class WeakReference<T> : WeakReference where T : class
    {
        public WeakReference(object rpTarget) : base(rpTarget) { }
        public WeakReference(object rpTarget, bool rpTrackResurrection) : base(rpTarget, rpTrackResurrection) { }
        protected WeakReference(SerializationInfo rpInfo, StreamingContext rpContext) : base(rpInfo, rpContext) { }

        public bool TryGetTarget(out T ropTarget)
        {
            ropTarget = Target as T;
            return ropTarget != null;
        }
        public void SetTarget(T rpTarget) => Target = rpTarget;
    }
}
