using Sakuno.Internal;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Sakuno
{
    public class BufferPool : DisposableObject
    {
        public const int SlabSize = 4 * 1024 * 1024;

        public static BufferPool Default { get; } = new BufferPool();

        ConcurrentStack<Slab> r_Slabs = new ConcurrentStack<Slab>();

        Slab r_CurrentSlab = new Slab(SlabSize);

        public int Size { get; private set; } = 1;

        public BufferSegment Get(int rpSize)
        {
            ThrowIfDisposed();

            if (rpSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(rpSize));

            if (rpSize > SlabSize)
                return new BufferSegment(new byte[rpSize]);

            while (true)
            {
                var rCurrentSlab = r_CurrentSlab;

                BufferSegment rResult;
                if (r_CurrentSlab.TryAcquireSegment(rpSize, out rResult))
                    return rResult;

                Slab rNextSlab;
                var rIsNewSlab = false;

                if (!r_Slabs.TryPop(out rNextSlab))
                {
                    rNextSlab = new Slab(SlabSize);
                    rIsNewSlab = true;
                }

                if (Interlocked.CompareExchange(ref r_CurrentSlab, rNextSlab, rCurrentSlab) == rCurrentSlab && rIsNewSlab)
                    Size++;
            }
        }

        public void Collect(BufferSegment rpSegment)
        {
            ThrowIfDisposed();

            var rSlab = rpSegment.Slab;
            if (rSlab == null)
                return;

            if (rSlab.ReleaseSegment())
            {
                rSlab.Reset();
                r_Slabs.Push(rSlab);
            }
        }

        protected override void DisposeManagedResources()
        {
            if (Default == this)
                throw new InvalidOperationException("The default pool can't be disposed.");

            r_CurrentSlab?.Dispose();

            foreach (var rSlab in r_Slabs.ToArray())
                rSlab.Dispose();
        }
    }
}
