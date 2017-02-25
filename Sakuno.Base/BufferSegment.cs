using Sakuno.Internal;
using System;

namespace Sakuno
{
    public struct BufferSegment : IEquatable<BufferSegment>
    {
        internal Slab Slab { get; }

        public byte[] Buffer { get; }

        public int Offset { get; }
        public int Length { get; }

        public unsafe byte* Address => Slab.Address + Offset;

        internal BufferSegment(byte[] rpBuffer) : this(null, rpBuffer, 0, rpBuffer.Length) { }
        internal BufferSegment(Slab rpSlab, byte[] rpBuffer, int rpOffset, int rpLength)
        {
            Slab = rpSlab;

            Buffer = rpBuffer;

            Offset = rpOffset;
            Length = rpLength;
        }

        public override bool Equals(object obj) => obj is BufferSegment && Equals((BufferSegment)obj);
        public bool Equals(BufferSegment obj) => Buffer == obj.Buffer && Offset == obj.Offset && Length == obj.Length;

        public override int GetHashCode() => Buffer != null ? Buffer.GetHashCode() ^ Offset ^ Length : 0;

        public static bool operator ==(BufferSegment x, BufferSegment y) => x.Equals(y);
        public static bool operator !=(BufferSegment x, BufferSegment y) => !x.Equals(y);
    }
}
