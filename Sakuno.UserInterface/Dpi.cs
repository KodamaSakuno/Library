using System;

namespace Sakuno.UserInterface
{
    public struct Dpi : IEquatable<Dpi>
    {
        public static Dpi Default { get; } = new Dpi(96, 96);

        public uint X { get; }
        public uint Y { get; }

        public double ScaleX => X / (double)Default.X;
        public double ScaleY => Y / (double)Default.Y;

        public Dpi(uint x, uint y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Dpi rpOther) => X == rpOther.X && Y == rpOther.Y;
        public override bool Equals(object rpObject) => rpObject != null ? Equals((Dpi)rpObject) : false;
        public override int GetHashCode() => ((int)X * 401) ^ (int)Y;

        public static bool operator ==(Dpi x, Dpi y) => x.Equals(y);
        public static bool operator !=(Dpi x, Dpi y) => !x.Equals(y);

        public override string ToString() => $"{X}, {Y} ({ScaleX}, {ScaleY})";
    }
}
