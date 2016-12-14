using System.Collections.Generic;

namespace Sakuno.Collections
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> rpDictionary, TKey rpKey) where TValue : class
        {
            TValue rResult;
            rpDictionary.TryGetValue(rpKey, out rResult);
            return rResult;
        }
    }
}
