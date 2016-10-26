using System.Runtime.InteropServices;
using System.Threading;

namespace Sakuno.Internal
{
    sealed class Slab : DisposableObject
    {
        readonly object r_ThreadSyncLock = new object();

        int r_Size;

        byte[] r_Buffer;
        GCHandle r_Handle;

        int r_Head;

        int r_AllocationCount;

        public bool IsInsufficient => r_Head >= r_Size;

        public Slab(int rpSize)
        {
            r_Size = rpSize;
        }

        public bool TryAcquireSegment(int rpSize, out BufferSegment ropSegment)
        {
            ThrowIfDisposed();

            EnsureAllocation();

            var rHead = Interlocked.Add(ref r_Head, rpSize);
            if (rHead <= r_Size)
            {
                Interlocked.Increment(ref r_AllocationCount);

                ropSegment = new BufferSegment(this, r_Buffer, rHead - rpSize, rpSize);

                return true;
            }

            ropSegment = default(BufferSegment);

            return false;
        }
        void EnsureAllocation()
        {
            if (r_Buffer != null)
                return;

            lock (r_ThreadSyncLock)
            {
                if (r_Buffer != null)
                    return;

                r_Buffer = new byte[r_Size];
                r_Handle = GCHandle.Alloc(r_Buffer, GCHandleType.Pinned);
            }
        }

        public bool ReleaseSegment()
        {
            ThrowIfDisposed();

            return Interlocked.Decrement(ref r_AllocationCount) == 0 && IsInsufficient;
        }

        public void Reset() => r_Head = 0;

        protected override void DisposeManagedResources()
        {
            if (!r_Handle.IsAllocated)
                return;

            r_Handle.Free();
            r_Buffer = null;
        }
    }
}
