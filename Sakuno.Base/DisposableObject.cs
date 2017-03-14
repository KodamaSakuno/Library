using System;
using System.Threading;

namespace Sakuno
{
    public class DisposableObject : IDisposable
    {
        int r_IsDisposed;
        public bool IsDisposed => Thread.VolatileRead(ref r_IsDisposed) == 1;

        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        ~DisposableObject()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            if (Interlocked.Exchange(ref r_IsDisposed, 1) != 0)
                return;

            try
            {
                Dispose(true);
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }
        protected void Dispose(bool rpDisposing)
        {
            if (rpDisposing)
                DisposeManagedResources();

            DisposeNativeResources();
        }

        protected virtual void DisposeManagedResources() { }
        protected virtual void DisposeNativeResources() { }
    }
    public class DisposableObjectWithEvent : DisposableObject
    {
        Action r_Disposing;

        public event Action Disposing
        {
            add
            {
                ThrowIfDisposed();
                r_Disposing = (Action)Delegate.Combine(r_Disposing, value);
            }
            remove
            {
                ThrowIfDisposed();
                r_Disposing = (Action)Delegate.Remove(r_Disposing, value);
            }
        }
    }
}
