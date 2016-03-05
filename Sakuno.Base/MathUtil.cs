namespace Sakuno
{
    public static class MathUtil
    {
        public const double RadOf1Deg = 0.017453292519943295;
        public const double DegOf1Rad = 57.295779513082323;

        public static double DegreesToRadians(double rpAngle) => rpAngle * RadOf1Deg;
        public static double RadiansToDegrees(double rpRadians) => rpRadians * DegOf1Rad;
    }
}
