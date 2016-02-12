using System;

namespace Sakuno
{
    public class EventListener<T> : DisposableObject where T : class
    {
        Action<T> r_Add, r_Remove;
        T r_Handler;

        protected EventListener() { }

        public EventListener(Action<T> rpAdd, Action<T> rpRemove, T rpHandler)
        {
            Initialize(rpAdd, rpRemove, rpHandler);
        }

        protected void Initialize(Action<T> rpAdd, Action<T> rpRemove, T rpHandler)
        {
            r_Add = rpAdd;
            r_Remove = rpRemove;
            r_Handler = rpHandler;
            r_Add(rpHandler);
        }

        protected override void DisposeManagedResources()
        {
            r_Remove(r_Handler);
            r_Add = null;
            r_Remove = null;
            r_Handler = null;
        }
    }
}
