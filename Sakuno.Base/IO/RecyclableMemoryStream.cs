using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Sakuno.IO
{
    public sealed class RecyclableMemoryStream : Stream
    {
        BufferPool r_BufferPool;

        volatile int r_DisposeState;
        bool IsOpen => r_DisposeState == 0;

        int r_Length;
        int r_Position;

        public override bool CanRead => IsOpen;
        public override bool CanSeek => IsOpen;
        public override bool CanWrite => IsOpen;

        public override long Length
        {
            get
            {
                CheckDisposed();
                return r_Length;
            }
        }

        public override long Position
        {
            get
            {
                CheckDisposed();
                return r_Position;
            }
            set
            {
                r_Position = (int)value;
            }
        }

        public int Capacity
        {
            get
            {
                CheckDisposed();
                return r_BufferPool.BlockSize * r_Blocks.Count;
            }
            set
            {
                CheckDisposed();
                EnsureCapacity(value);
            }
        }

        readonly byte[] r_ByteBuffer = new byte[1];
        readonly List<BufferSegment> r_Blocks = new List<BufferSegment>(1);

        public RecyclableMemoryStream() : this(BufferPool.Default) { }
        public RecyclableMemoryStream(BufferPool bufferPool)
        {
            r_BufferPool = bufferPool;
        }

        void CheckDisposed()
        {
            if (!IsOpen)
                throw new ObjectDisposedException("The stream is disposed.");
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            CheckDisposed();

            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "The offset cannot be negative.");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "The count cannot be negative.");
            if (offset + count > buffer.Length)
                throw new ArgumentException("The length of buffer should not be less than offset + count.");

            var rCount = ReadCore(buffer, offset, r_Position, count);
            r_Position += rCount;

            return rCount;
        }
        unsafe int ReadCore(byte[] rpBuffer, int rpOffset, int rpStartPosition, int rpCount)
        {
            if (r_Length <= rpStartPosition)
                return 0;

            var rBlockSize = r_BufferPool.BlockSize;
            var rBlockIndex = rpStartPosition / rBlockSize;
            var rBlockOffset = rpStartPosition % rBlockSize;

            var rRemaining = Math.Min(rpCount, r_Length - rpStartPosition);
            var rWritten = 0;

            fixed (byte* rInput = rpBuffer)
                while (rRemaining > 0)
                {
                    var rSource = r_Blocks[rBlockIndex].Address + rBlockOffset;
                    var rDesination = rInput + rpOffset + rWritten;
                    var rCount = Math.Min(rBlockSize - rBlockOffset, rRemaining);

                    Memory.Copy(rSource, rDesination, rCount);

                    rWritten += rCount;
                    rRemaining -= rCount;
                    rBlockIndex++;
                    rBlockOffset = 0;
                }

            return rWritten;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            CheckDisposed();

            if (offset > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(offset), "The offset cannot be greater than " + int.MaxValue);

            int rNewPosition;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    rNewPosition = (int)offset;
                    break;

                case SeekOrigin.Current:
                    rNewPosition = (int)offset + r_Position;
                    break;

                case SeekOrigin.End:
                    rNewPosition = (int)offset;
                    break;

                default:
                    throw new ArgumentException(nameof(origin), "The origin is invalid.");
            }

            if (rNewPosition < 0)
                throw new IOException("The new position is before the beginning.");

            return r_Position = rNewPosition;
        }

        public override void SetLength(long value)
        {
            CheckDisposed();

            if (value < 0 || value > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value), "The value must be non-negative and at most " + int.MaxValue);

            var rLength = (int)value;

            EnsureCapacity(rLength);

            r_Length = rLength;

            if (r_Position > rLength)
                r_Position = rLength;
        }
        void EnsureCapacity(int rpSize)
        {
            var rCapacity = Capacity;
            var rBlockSize = r_BufferPool.BlockSize;

            while (rCapacity < rpSize)
            {
                r_Blocks.Add(r_BufferPool.AcquireBlock());

                rCapacity += rBlockSize;
            }
        }

        public override unsafe void Write(byte[] buffer, int offset, int count)
        {
            CheckDisposed();

            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "The offset cannot be negative.");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "The count cannot be negative.");
            if (offset + count > buffer.Length)
                throw new ArgumentException("The length of buffer should not be less than offset + count.");

            EnsureCapacity(r_Position + count);

            var rBlockSize = r_BufferPool.BlockSize;
            var rBlockIndex = r_Position / rBlockSize;
            var rBlockOffset = r_Position % rBlockSize;

            var rRemaining = count;
            var rWritten = 0;

            fixed (byte* rInput = buffer)
                while (rRemaining > 0)
                {
                    var rSource = rInput + offset + rWritten;
                    var rDesination = r_Blocks[rBlockIndex].Address + rBlockOffset;
                    var rCount = Math.Min(rBlockSize - rBlockOffset, rRemaining);

                    Memory.Copy(rSource, rDesination, rCount);

                    rWritten += rCount;
                    rRemaining -= rCount;
                    rBlockIndex++;
                    rBlockOffset = 0;
                }

            r_Position += count;
            r_Length = Math.Max(r_Length, r_Position);
        }
        public override void WriteByte(byte value)
        {
            CheckDisposed();

            r_ByteBuffer[0] = value;
            Write(r_ByteBuffer, 0, 1);
        }
        public void WriteTo(Stream stream)
        {
            CheckDisposed();

            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var rBlockIndex = 0;
            var rRemaining = r_Length;

            while (rRemaining > 0)
            {
                var rSegment = r_Blocks[rBlockIndex];
                var rCount = Math.Min(rSegment.Length, rRemaining);

                stream.Write(rSegment.Buffer, rSegment.Offset, rCount);

                rRemaining -= rCount;
                rBlockIndex++;
            }
        }

        public override void Flush() { }

        public byte[] ToArray()
        {
            CheckDisposed();

            var rResult = new byte[r_Length];
            ReadCore(rResult, 0, 0, r_Length);

            return rResult;
        }

        ~RecyclableMemoryStream() { Dispose(false); }
        public override void Close() => Dispose(true);
        protected override void Dispose(bool rpDisposing)
        {
            if (Interlocked.Exchange(ref r_DisposeState, 1) != 0)
                return;

            if (rpDisposing)
                GC.SuppressFinalize(this);

            r_BufferPool.Collect(r_Blocks);
            r_Blocks.Clear();

            base.Dispose(rpDisposing);
        }
    }
}
