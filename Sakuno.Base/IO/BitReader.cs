using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Sakuno.IO
{
    public unsafe class BitReader : DisposableObject
    {
        const int CharBufferSize = 128;

        Stream r_Stream;
        public Stream BaseStream => r_Stream;

        bool r_LeaveOpen;

        byte r_CurrentByte;
        int r_BitPosition;

        BufferSegment r_Segment;
        byte* r_Buffer;

        Decoder r_Decoder;

        char[] r_Chars;
        GCHandle r_CharBufferHandle;
        char* r_CharBuffer;

        public BitReader(Stream rpInput) : this(rpInput, Encoding.UTF8, false) { }
        public BitReader(Stream rpInput, Encoding rpEncoding, bool rpLeaveOpen)
        {
            if (rpInput == null)
                throw new ArgumentNullException(nameof(rpInput));

            r_Stream = rpInput;
            r_LeaveOpen = rpLeaveOpen;

            r_Segment = BufferPool.Default.Acquire(16);
            r_Buffer = r_Segment.Address;

            r_Decoder = rpEncoding.GetDecoder();
        }

        public void Skip(long rpCount)
        {
            if (rpCount < 0)
                return;

            Align();

            if (r_Stream.CanSeek)
            {
                r_Stream.Position += rpCount;
                return;
            }

            for (var i = 0; i < rpCount; i++)
                r_Stream.ReadByte();
        }

        public byte ReadByte()
        {
            ThrowIfDisposed();

            var rByte = r_Stream.ReadByte();
            if (rByte == -1)
                throw new EndOfStreamException();

            return (byte)rByte;
        }
        public byte[] ReadBytes(int rpCount)
        {
            ThrowIfDisposed();

            if (rpCount == 0)
                return ArrayUtil.Empty<byte>();

            var rBuffer = new byte[rpCount];

            var rOffset = 0;
            do
            {
                var rCount = r_Stream.Read(rBuffer, rOffset, rpCount);
                if (rCount == 0)
                    break;

                rOffset += rCount;
                rpCount -= rCount;
            } while (rpCount > 0);

            if (rOffset != rBuffer.Length)
            {
                var rNewBuffer = new byte[rOffset];
                Memory.Copy(rBuffer, rNewBuffer, rOffset);

                return rNewBuffer;
            }

            return rBuffer;
        }

        public bool ReadBit()
        {
            ThrowIfDisposed();

            if (r_BitPosition == 0)
                r_CurrentByte = ReadByte();

            var rResult = ((r_CurrentByte >> (7 - r_BitPosition)) & 1) == 1;

            r_BitPosition++;

            if (r_BitPosition == 8)
                r_BitPosition = 0;

            return rResult;
        }
        public long ReadBits(int rpCount)
        {
            ThrowIfDisposed();

            if (rpCount == 0)
                return 0L;

            var rResult = 0L;

            if (r_BitPosition == 0)
                r_CurrentByte = ReadByte();

            for (var i = 0; i < rpCount; i++)
            {
                var rBit = (r_CurrentByte >> (7 - r_BitPosition)) & 1;
                rResult += rBit << (rpCount - i - 1);

                r_BitPosition++;

                if (r_BitPosition == 8)
                {
                    if (i != rpCount - 1)
                        r_CurrentByte = ReadByte();

                    r_BitPosition = 0;
                }
            }

            return rResult;
        }

        public void Align() => r_BitPosition = 0;

        public int ReadEncodedInt()
        {
            var rResult = 0;
            var rPosition = 0;

            while (rPosition != 35)
            {
                var rCurrent = ReadByte();

                rResult |= (rCurrent & 127) << rPosition;
                rPosition += 7;

                if ((rCurrent & 128) == 0)
                    return rResult;
            }

            throw new FormatException("Failed to read encoded integer.");
        }

        void FillBuffer(int rpBytes)
        {
            ThrowIfDisposed();

            if (rpBytes < 0 || rpBytes > 16)
                throw new ArgumentOutOfRangeException(nameof(rpBytes));

            if (rpBytes == 1)
            {
                var rByte = r_Stream.ReadByte();
                if (rByte == -1)
                    throw new EndOfStreamException();

                *r_Buffer = (byte)rByte;
                return;
            }

            var rRemaining = rpBytes;
            var rOffset = 0;

            while (rRemaining > 0)
            {
                var rCount = r_Stream.Read(r_Segment.Buffer, r_Segment.Offset + rOffset, rRemaining);
                if (rCount == 0)
                    throw new EndOfStreamException();

                rRemaining -= rCount;
                rOffset += rCount;
            }
        }

        public bool ReadBoolean()
        {
            ThrowIfDisposed();

            return ReadByte() == 1;
        }
        public short ReadInt16()
        {
            FillBuffer(2);

            return *(short*)r_Buffer;
        }
        public int ReadInt32()
        {
            FillBuffer(4);

            return *(int*)r_Buffer;
        }
        public long ReadInt64()
        {
            FillBuffer(8);

            return *(long*)r_Buffer;
        }
        public ushort ReadUInt16()
        {
            FillBuffer(2);

            return *(ushort*)r_Buffer;
        }
        public uint ReadUInt32()
        {
            FillBuffer(4);

            return *(uint*)r_Buffer;
        }
        public ulong ReadUInt64()
        {
            FillBuffer(8);

            return *(ulong*)r_Buffer;
        }
        public unsafe float ReadSingle()
        {
            FillBuffer(4);

            return *(float*)r_Buffer;
        }
        public unsafe double ReadDouble()
        {
            FillBuffer(8);

            return *(double*)r_Buffer;
        }

        public string ReadNullTerminatedString()
        {
            ThrowIfDisposed();

            InitializeCharBuffer();

            StringBuilder rBuilder = null;

            var rPosition = 0;

            for (;;)
            {
                var rByte = ReadByte();
                if (rByte == 0)
                    break;

                r_Buffer[rPosition] = rByte;

                rPosition++;

                if (rPosition == CharBufferSize)
                {
                    if (rBuilder == null)
                        rBuilder = StringBuilderCache.Acquire();

                    DecodeCharsAndAppend(rBuilder, rPosition);

                    rPosition = 0;
                }
            }

            if (rBuilder != null)
            {
                if (rPosition > 0)
                    DecodeCharsAndAppend(rBuilder, rPosition);

                return rBuilder.GetStringAndRelease();
            }

            var rCharCount = r_Decoder.GetChars(r_Buffer, rPosition, r_CharBuffer, CharBufferSize, false);

            return new string(r_CharBuffer, 0, rCharCount);
        }

        public string ReadStringStartingWithEncodedLength()
        {
            ThrowIfDisposed();

            var rLength = ReadEncodedInt();
            if (rLength < 0)
                throw new IOException("Invalid string length.");

            InitializeCharBuffer();

            StringBuilder rBuilder = null;

            var rPosition = 0;
            var rCharCount = 0;

            for (;;)
            {
                var rCount = r_Stream.Read(r_Segment.Buffer, r_Segment.Offset, rLength);
                if (rCount == 0)
                    throw new EndOfStreamException();

                rCharCount = r_Decoder.GetChars(r_Buffer, rCount, r_CharBuffer, CharBufferSize, false);

                if (rPosition == 0 && rLength == rCharCount)
                    break;

                if (rBuilder == null)
                    rBuilder = StringBuilderCache.Acquire();

                rBuilder.Append(r_Chars, 0, rCharCount);

                rPosition += rCount;
                if (rPosition >= rLength)
                    break;
            }

            if (rBuilder != null)
                return rBuilder.GetStringAndRelease();

            return new string(r_CharBuffer, 0, rCharCount);
        }

        void InitializeCharBuffer()
        {
            if (r_Segment.Length < CharBufferSize)
            {
                BufferPool.Default.Resize(ref r_Segment, CharBufferSize);
                r_Buffer = r_Segment.Address;
            }

            if (r_CharBuffer == null)
            {
                r_Chars = new char[CharBufferSize];
                r_CharBufferHandle = GCHandle.Alloc(r_Chars, GCHandleType.Pinned);
                r_CharBuffer = (char*)r_CharBufferHandle.AddrOfPinnedObject();
            }
        }
        void DecodeCharsAndAppend(StringBuilder rpBuilder, int rpPosition)
        {
            var rCharCount = r_Decoder.GetChars(r_Buffer, rpPosition, r_CharBuffer, CharBufferSize, false);

            rpBuilder.Append(r_Chars, 0, rCharCount);
        }

        protected override void DisposeManagedResources()
        {
            var rStream = Interlocked.Exchange(ref r_Stream, null);
            if (rStream != null && !r_LeaveOpen)
                rStream.Close();
        }
        protected override void DisposeNativeResources()
        {
            BufferPool.Default.Collect(ref r_Segment);

            if (r_CharBufferHandle.IsAllocated)
                r_CharBufferHandle.Free();

            r_Stream = null;
            r_Decoder = null;
            r_Chars = null;
        }
    }
}
