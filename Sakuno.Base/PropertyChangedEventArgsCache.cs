using System;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Sakuno
{
    public static class PropertyChangedEventArgsCache
    {
        static ConcurrentDictionary<string, PropertyChangedEventArgs> r_Cache = new ConcurrentDictionary<string, PropertyChangedEventArgs>(StringComparer.OrdinalIgnoreCase);

        public static readonly PropertyChangedEventArgs CountPropertyChanged = Get("Count");
        public static readonly PropertyChangedEventArgs IndexerPropertyChanged = Get("Item[]");

        public static PropertyChangedEventArgs Get(string rpPropertyName) => r_Cache.GetOrAdd(rpPropertyName ?? string.Empty, r => new PropertyChangedEventArgs(r));
    }
}
