using System;
using System.Collections.Generic;
using System.IO;

namespace Sakuno.Serialization.MessagePack
{
    public static class MessagePack
    {
        public static object Unpack(MessagePackReader rpReader)
        {
            if (!rpReader.Read())
                throw new FormatException();

            switch (rpReader.CurrentType)
            {
                case ElementType.Nil:
                    return null;

                case ElementType.BooleanFalse:
                    return false;

                case ElementType.BooleanTrue:
                    return true;

                case ElementType.FixedPositiveNumber:
                case ElementType.FixedNegativeNumber:
                    return rpReader.ReadFixedNumber();

                case ElementType.UInt8:
                    return rpReader.ReadUInt8();

                case ElementType.UInt16:
                    return rpReader.ReadUInt16();

                case ElementType.UInt32:
                    return rpReader.ReadUInt32();

                case ElementType.UInt64:
                    return rpReader.ReadUInt64();

                case ElementType.Int8:
                    return rpReader.ReadInt8();

                case ElementType.Int16:
                    return rpReader.ReadInt16();

                case ElementType.Int32:
                    return rpReader.ReadInt32();

                case ElementType.Int64:
                    return rpReader.ReadInt64();

                case ElementType.FixedString:
                case ElementType.String8:
                case ElementType.String16:
                case ElementType.String32:
                    return rpReader.ReadString();

                case ElementType.FixedArray:
                case ElementType.Array16:
                case ElementType.Array32:
                    var rArray = new object[rpReader.ReadCollectionLength()];
                    for (var i = 0; i < rArray.Length; i++)
                        rArray[i] = Unpack(rpReader);

                    return rArray;

                case ElementType.FixedMap:
                case ElementType.Map16:
                case ElementType.Map32:
                    var rLength = rpReader.ReadCollectionLength();
                    var rMap = new Dictionary<string, object>();
                    for (var i = 0; i < rLength; i++)
                    {
                        var rKey = Unpack(rpReader);
                        var rValue = Unpack(rpReader);

                        rMap.Add((string)rKey, rValue);
                    }

                    return rMap;

                case ElementType.EOF:
                    throw new EndOfStreamException();

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
