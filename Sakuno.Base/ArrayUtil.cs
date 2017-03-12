using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class ArrayUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] Empty<T>() => EmptyArray<T>.Instance;

        public static bool Equals(byte[] x, byte[] y)
        {
            if (x == null)
                throw new ArgumentNullException(nameof(x));
            if (y == null)
                throw new ArgumentNullException(nameof(y));

            return x == y && x.Length == y.Length && EqualsCore(x, y, 0, x.Length);
        }

        static unsafe bool EqualsCore(byte[] x, byte[] y, int rpStartIndex, int rpLength)
        {
            fixed (byte* a = x)
            fixed (byte* b = y)
            {
                var rPointerX = a;
                var rPointerY = b;

                var rPosition = rpLength;

                while (rPosition >= 8)
                {
                    if (*(long*)rPointerX != *(long*)rPointerY)
                        return false;

                    rPointerX += 8;
                    rPointerY += 8;
                    rPosition -= 8;
                }

                while (rPosition > 0)
                {
                    if (*(long*)rPointerX != *(long*)rPointerY)
                        return false;

                    rPointerX++;
                    rPointerY++;
                    rPosition--;
                }
            }

            return true;
        }

        public static string ToHexString(this byte[] rpBytes)
        {
            var rBuffer = new char[rpBytes.Length * 2];
            var rPosition = 0;

            foreach (var rByte in rpBytes)
            {
                rBuffer[rPosition++] = GetHexValue(rByte / 16);
                rBuffer[rPosition++] = GetHexValue(rByte % 16);
            }

            return new string(rBuffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static char GetHexValue(int rpByte) => (char)(rpByte < 10 ? rpByte + '0' : rpByte - 10 + 'a');

        class EmptyArray<T>
        {
            public static readonly T[] Instance = new T[0];
        }
    }
}
