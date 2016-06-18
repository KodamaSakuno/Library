using System;

namespace Sakuno
{
    public struct LazyString
    {
        string r_String;

        public int Index { get; }
        public int Length { get; }

        public LazyString(string rpString) : this(rpString, 0, rpString.Length) { }
        public LazyString(string rpString, int rpIndex, int rpLength)
        {
            r_String = rpString;

            Index = rpIndex;
            Length = rpLength;
        }

        public LazyString Substring(int rpIndex)
        {
            if (rpIndex >= Length)
                throw new ArgumentOutOfRangeException(nameof(rpIndex));

            return new LazyString(r_String, Index + rpIndex, Length - rpIndex);
        }
        public LazyString Substring(int rpIndex, int rpLength)
        {
            if (rpIndex >= Length)
                throw new ArgumentOutOfRangeException(nameof(rpIndex));

            if (rpIndex + rpLength > Length)
                rpLength = Length - rpIndex;

            return new LazyString(r_String, Index + rpIndex, rpLength);
        }

        public int IndexOf(char rpChar)
        {
            var rIndex = r_String.IndexOf(rpChar, Index);
            return rIndex < 0 ? rIndex : rIndex - Index;
        }

        public override string ToString() => r_String.Substring(Index, Length);
    }
}
