using System;

namespace Sakuno
{
    public class DisposableObject : IDisposable
    {
        public bool IsDisposed { get; private set; }

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
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Dispose(bool rpDisposing)
        {
            if (IsDisposed)
                return;

            if (rpDisposing)
                DisposeManagedResources();
            DisposeNativeResources();

            IsDisposed = true;
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
