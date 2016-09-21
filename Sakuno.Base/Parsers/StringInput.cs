using System;

namespace Sakuno.Parsers
{
    public struct StringInput : IInput<char>
    {
        string r_Source;
        int r_Position;

        public char Current => r_Source[r_Position];

        public bool EndOfInput => r_Position == r_Source.Length;

        public int Position => r_Position;

        public StringInput(string rpSource) : this(rpSource, 0) { }
        public StringInput(string rpSource, int rpPosition)
        {
            r_Source = rpSource ?? string.Empty;
            r_Position = rpPosition;
        }

        public IInput<char> MoveNext()
        {
            if (EndOfInput)
                throw new InvalidOperationException("End of input.");

            return new StringInput(r_Source, r_Position + 1);
        }

        public override string ToString() => $"{r_Position}/{r_Source.Length}";
    }
}
