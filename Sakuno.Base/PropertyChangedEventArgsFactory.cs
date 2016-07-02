using System.Collections.Concurrent;
using System.ComponentModel;

namespace Sakuno
{
    static class PropertyChangedEventArgsFactory
    {
        static ConcurrentDictionary<string, PropertyChangedEventArgs> r_Cache = new ConcurrentDictionary<string, PropertyChangedEventArgs>();

        public static PropertyChangedEventArgs Get(string rpPropertyName) => r_Cache.GetOrAdd(rpPropertyName, r => new PropertyChangedEventArgs(r));
    }
}
