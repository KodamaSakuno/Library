using Sakuno.Internal;

namespace Sakuno
{
    public struct BufferSegment
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
    }
}
