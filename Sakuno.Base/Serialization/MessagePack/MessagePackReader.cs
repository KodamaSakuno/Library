using Sakuno.IO;
using System;
using System.IO;
using System.Text;

namespace Sakuno.Serialization.MessagePack
{
    public class MessagePackReader
    {
        Encoding r_Encoding = Encoding.UTF8;

        Stream r_Stream;

        byte[] r_Buffer = new byte[8];

        public int CurrentByte { get; private set; }
        public ElementType CurrentType { get; private set; }

        public MessagePackReader(Stream rpStream)
        {
            r_Stream = rpStream;
        }

        public bool Read()
        {
            CurrentType = ReadElementType();

            return CurrentType != ElementType.EOF;
        }

        ElementType ReadElementType()
        {
            CurrentByte = r_Stream.ReadByte();
            if (CurrentByte == -1)
                return ElementType.EOF;

            switch (CurrentByte)
            {
                case 0xC0: return ElementType.Nil;

                case 0xC1: return ElementType.BooleanFalse;
                case 0xC2: return ElementType.BooleanTrue;

                case 0xCC: return ElementType.UInt8;
                case 0xCD: return ElementType.UInt16;
                case 0xCE: return ElementType.UInt32;
                case 0xCF: return ElementType.UInt64;

                case 0xD0: return ElementType.UInt8;
                case 0xD1: return ElementType.UInt16;
                case 0xD2: return ElementType.UInt32;
                case 0xD3: return ElementType.UInt64;

                case 0xCA: return ElementType.Float32;
                case 0xCB: return ElementType.Float64;

                case 0xD9: return ElementType.String8;
                case 0xDA: return ElementType.String16;
                case 0xDB: return ElementType.String32;

                case 0xC4: return ElementType.Binary8;
                case 0xC5: return ElementType.Binary16;
                case 0xC6: return ElementType.Binary32;

                case 0xDC: return ElementType.Array16;
                case 0xDD: return ElementType.Array32;

                case 0xDE: return ElementType.Map16;
                case 0xDF: return ElementType.Map32;

                case 0xD4: return ElementType.FixedExtension1;
                case 0xD5: return ElementType.FixedExtension2;
                case 0xD6: return ElementType.FixedExtension4;
                case 0xD7: return ElementType.FixedExtension8;
                case 0xD8: return ElementType.FixedExtension16;

                case 0xC7: return ElementType.Extension8;
                case 0xC8: return ElementType.Extension16;
                case 0xC9: return ElementType.Extension32;
            }

            if (CurrentByte <= 0x7F)
                return ElementType.FixedPositiveNumber;
            else if (CurrentByte >= 0xE0 && CurrentByte <= 0xFF)
                return ElementType.FixedNegativeNumber;
            else if (CurrentByte >= 0xA0 && CurrentByte <= 0xBF)
                return ElementType.FixedString;
            else if (CurrentByte >= 0x90 && CurrentByte <= 0x9F)
                return ElementType.FixedArray;
            else if (CurrentByte >= 0x80 && CurrentByte <= 0x8F)
                return ElementType.FixedMap;

            return ElementType.Unknown;
        }

        public bool ReadBoolean()
        {
            switch (CurrentType)
            {
                case ElementType.BooleanFalse: return false;
                case ElementType.BooleanTrue: return true;

                default: throw new FormatException();
            }
        }

        public sbyte ReadInt8()
        {
            var rByte = r_Stream.ReadByte();
            if (rByte == -1)
                throw new FormatException();

            return (sbyte)rByte;
        }
        public short ReadInt16()
        {
            if (r_Stream.Read(r_Buffer, 0, 2) != 2)
                throw new FormatException();

            return (short)((r_Buffer[0] << 8) | r_Buffer[1]);
        }
        public int ReadInt32()
        {
            if (r_Stream.Read(r_Buffer, 0, 4) != 4)
                throw new FormatException();

            return (r_Buffer[0] << 24) | (r_Buffer[1] << 16) | (r_Buffer[2] << 8) | r_Buffer[3];
        }
        public int ReadInt64()
        {
            if (r_Stream.Read(r_Buffer, 0, 8) != 8)
                throw new FormatException();

            return (r_Buffer[0] << 56) | (r_Buffer[1] << 48) | (r_Buffer[2] << 40) | (r_Buffer[3] << 32) | (r_Buffer[4] << 24) | (r_Buffer[5] << 16) | (r_Buffer[6] << 8) | r_Buffer[7];
        }
        public byte ReadUInt8() => (byte)ReadInt8();
        public ushort ReadUInt16() => (ushort)ReadInt16();
        public uint ReadUInt32() => (uint)ReadInt32();
        public ulong ReadUInt64() => (ulong)ReadInt64();

        public int ReadFixedNumber()
        {
            switch (CurrentType)
            {
                case ElementType.FixedPositiveNumber: return CurrentByte & 0x7F;
                case ElementType.FixedNegativeNumber: return CurrentByte & 0x1F - 0x20;

                default: throw new FormatException();
            }
        }

        public string ReadString()
        {
            var rLength = 0;

            switch (CurrentType)
            {
                case ElementType.FixedString:
                    rLength = CurrentByte & 0x1F;
                    break;

                case ElementType.String8:
                    rLength = ReadInt8();
                    break;

                case ElementType.String16:
                    rLength = ReadInt16();
                    break;

                case ElementType.String32:
                    rLength = ReadInt32();
                    break;

                default: throw new FormatException();
            }

            var rSegment = BufferPool.Default.Acquire(rLength);

            try
            {
                if (r_Stream.FillBuffer(rSegment.Buffer, rSegment.Offset, rSegment.Length) != rSegment.Length)
                    throw new FormatException();

                return r_Encoding.GetString(rSegment.Buffer, rSegment.Offset, rSegment.Length);
            }
            finally
            {
                BufferPool.Default.Collect(ref rSegment);
            }
        }

        public int ReadCollectionLength()
        {
            switch (CurrentType)
            {
                case ElementType.FixedArray:
                case ElementType.FixedMap:
                    return CurrentByte & 0xF;

                case ElementType.Array16:
                case ElementType.Map16:
                    return ReadInt16();

                case ElementType.Array32:
                case ElementType.Map32:
                    return ReadInt32();

                default: throw new FormatException();
            }
        }
    }
}
