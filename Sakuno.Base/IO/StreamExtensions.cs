using System.IO;

namespace Sakuno.IO
{
    public static class StreamExtensions
    {
        public static int FillBuffer(this Stream rpStream, byte[] rpBuffer, int rpOffset, int rpLength)
        {
            var rRemaining = rpLength;

            do
            {
                var rLength = rpStream.Read(rpBuffer, rpOffset, rRemaining);
                if (rLength == 0)
                    break;

                rRemaining -= rLength;
                rpOffset += rLength;
            } while (rRemaining > 0);

            return rpLength - rRemaining;
        }
    }
}
