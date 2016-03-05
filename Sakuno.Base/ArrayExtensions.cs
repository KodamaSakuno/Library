using System;
using System.Collections.ObjectModel;

namespace Sakuno
{
    public static class ArrayExtensions
    {
        public static ReadOnlyCollection<T> AsReadOnly<T>(this T[] rpArray) => Array.AsReadOnly(rpArray);
    }
}
