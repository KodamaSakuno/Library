using System;

namespace Sakuno
{
    public static class BufferSegmentExtensions
    {
        public static ArraySegment<byte> ToArraySegment(this BufferSegment rpSegment) => new ArraySegment<byte>(rpSegment.Buffer, rpSegment.Offset, rpSegment.Length);
    }
}
