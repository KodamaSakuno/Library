using Sakuno.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Sakuno
{
    public class BufferPool : DisposableObject
    {
        public const int DefaultBlockSize = 128 * 1024;
        public const int DefaultMaximumBufferSize = 128 * 1024 * 1024;

        public static BufferPool Default { get; } = new BufferPool();

        public int BlockSize { get; }

        int r_SmallBlockCount;
        int r_SmallBlockTotalUsage;
        public int SmallBlockCount => r_SmallBlockCount;
        public int SmallBlockTotalUsage => r_SmallBlockTotalUsage;
        public int CurrentSmallBlockFreeSize => BlockSize - r_SmallBlockTotalUsage % BlockSize;

        ConcurrentStack<Slab> r_Slabs = new ConcurrentStack<Slab>();

        Slab r_CurrentSlab;

        public BufferPool() : this(DefaultBlockSize) { }
        public BufferPool(int blockSize)
        {
            BlockSize = blockSize;

            r_CurrentSlab = new Slab(BlockSize);
        }

        public BufferSegment Acquire(int size)
        {
            ThrowIfDisposed();

            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), "The size cannot be negative.");

            return AcquireCore(size);
        }
        BufferSegment AcquireCore(int rpSize)
        {
            for (;;)
            {
                var rCurrentSlab = r_CurrentSlab;

                BufferSegment rSegment;
                if (rCurrentSlab.TryAcquireSegment(rpSize, out rSegment))
                {
                    Interlocked.Add(ref r_SmallBlockTotalUsage, rpSize);
                    return rSegment;
                }

                var rIsNewSlab = false;
                var rNextSlab = GetSlab(out rIsNewSlab);

                if (Interlocked.CompareExchange(ref r_CurrentSlab, rNextSlab, rCurrentSlab) == rCurrentSlab && rIsNewSlab)
                    Interlocked.Increment(ref r_SmallBlockCount);
            }
        }

        public BufferSegment AcquireBlock()
        {
            var rIsNewSlab = false;
            var rSlab = GetSlab(out rIsNewSlab);

            BufferSegment rSegment;
            rSlab.TryAcquireSegment(BlockSize, out rSegment);

            Interlocked.Add(ref r_SmallBlockTotalUsage, BlockSize);

            return rSegment;
        }

        Slab GetSlab(out bool ropIsNewSlab)
        {
            Slab rResult;
            if (r_Slabs.TryPop(out rResult))
                ropIsNewSlab = false;
            else
            {
                rResult = new Slab(BlockSize);
                ropIsNewSlab = true;
            }

            return rResult;
        }

        public void Collect(BufferSegment segment) => Collect(ref segment);
        public void Collect(ref BufferSegment segment)
        {
            ThrowIfDisposed();

            CollectCore(ref segment);
        }
        public void Collect(IEnumerable<BufferSegment> segments)
        {
            ThrowIfDisposed();

            foreach (var rSegment in segments)
                CollectCore(rSegment);
        }
        void CollectCore(BufferSegment rpSegment) => CollectCore(ref rpSegment);
        unsafe void CollectCore(ref BufferSegment rpSegment)
        {
            var rSlab = rpSegment.Slab;
            if (rSlab == null)
                return;

            Memory.Clear(rpSegment.Address, rpSegment.Length);

            Interlocked.Add(ref r_SmallBlockTotalUsage, -rpSegment.Length);

            if (rSlab.ReleaseSegment())
            {
                rSlab.Reset();
                r_Slabs.Push(rSlab);
            }
        }

        public void Resize(ref BufferSegment segment, int size)
        {
            ThrowIfDisposed();

            if (size < segment.Length)
                throw new ArgumentOutOfRangeException(nameof(size), "The size is less than the original one.");

            if (size == segment.Length)
                return;

            CollectCore(ref segment);
            segment = AcquireCore(size);
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
