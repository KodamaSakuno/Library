﻿namespace Sakuno.Serialization.MessagePack
{
    public enum ElementType
    {
        Unknown,
        EOF,
        Nil,
        BooleanFalse,
        BooleanTrue,
        FixedPositiveNumber,
        FixedNegativeNumber,
        UInt8,
        UInt16,
        UInt32,
        UInt64,
        Int8,
        Int16,
        Int32,
        Int64,
        Float32,
        Float64,
        FixedString,
        String8,
        String16,
        String32,
        Binary8,
        Binary16,
        Binary32,
        FixedArray,
        Array16,
        Array32,
        FixedMap,
        Map16,
        Map32,
        FixedExtension1,
        FixedExtension2,
        FixedExtension4,
        FixedExtension8,
        FixedExtension16,
        Extension8,
        Extension16,
        Extension32,
    }
}
