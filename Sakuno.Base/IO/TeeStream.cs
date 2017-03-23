using System.IO;
using System.Threading;

namespace Sakuno.IO
{
    public class TeeStream : Stream
    {
        Stream r_PrimaryStream, r_SecondaryStream;
        public Stream PrimaryStream => r_PrimaryStream;
        public Stream SecondaryStream => r_SecondaryStream;

        public override bool CanRead => r_PrimaryStream.CanRead;
        public override bool CanSeek => r_PrimaryStream.CanSeek;
        public override bool CanWrite => r_PrimaryStream.CanWrite;

        public override long Length => r_PrimaryStream.Length;

        public override long Position
        {
            get { return r_PrimaryStream.Position; }
            set { r_PrimaryStream.Position = value; }
        }

        public TeeStream(Stream rpPrimary, Stream rpSecondary)
        {
            r_PrimaryStream = rpPrimary;
            r_SecondaryStream = rpSecondary;
        }

        public override int Read(byte[] rpBuffer, int rpOffset, int rpCount)
        {
            var rCount = r_PrimaryStream.Read(rpBuffer, rpOffset, rpCount);

            r_SecondaryStream.Write(rpBuffer, rpOffset, rCount);

            return rCount;
        }

        public override long Seek(long rpOffset, SeekOrigin rpOrigin) => r_PrimaryStream.Seek(rpOffset, rpOrigin);
        public override void SetLength(long rpValue) => r_PrimaryStream.SetLength(rpValue);

        public override void Flush()
        {
            r_PrimaryStream.Flush();
            r_SecondaryStream.Flush();
        }

        public override void Write(byte[] rpBuffer, int rpOffset, int rpCount)
        {
            r_PrimaryStream.Write(rpBuffer, rpOffset, rpCount);
            r_SecondaryStream.Write(rpBuffer, rpOffset, rpCount);
        }

        protected override void Dispose(bool rpDisposing)
        {
            if (!rpDisposing)
                return;

            Interlocked.Exchange(ref r_PrimaryStream, null)?.Dispose();
            Interlocked.Exchange(ref r_SecondaryStream, null)?.Dispose();
        }
    }
}
