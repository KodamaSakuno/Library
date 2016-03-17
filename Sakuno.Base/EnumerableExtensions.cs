using Sakuno.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sakuno
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> rpSource, int rpChunkSize)
            => rpSource.Select((r, i) => new { Index = i, Value = r })
                           .GroupBy(r => r.Index / rpChunkSize)
                           .Select(r => r.Select(rpValue => rpValue.Value));

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> rpEnumerable) => rpEnumerable == null || !rpEnumerable.Any();

        public static HybridDictionary<TKey, TSource> ToHybridDictionary<TSource, TKey>(this IEnumerable<TSource> rpSource, Func<TSource, TKey> rpKeySelector) => rpSource.ToHybridDictionary(rpKeySelector, IdentityFunction<TSource>.Instance, null);
        public static HybridDictionary<TKey, TSource> ToHybridDictionary<TSource, TKey>(this IEnumerable<TSource> rpSource, Func<TSource, TKey> rpKeySelector, IEqualityComparer<TKey> rpComparer) => rpSource.ToHybridDictionary(rpKeySelector, IdentityFunction<TSource>.Instance, rpComparer);
        public static HybridDictionary<TKey, TElement> ToHybridDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> rpSource, Func<TSource, TKey> rpKeySelector, Func<TSource, TElement> rpElementSelector) => rpSource.ToHybridDictionary(rpKeySelector, rpElementSelector, null);
        public static HybridDictionary<TKey, TElement> ToHybridDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> rpSource, Func<TSource, TKey> rpKeySelector, Func<TSource, TElement> rpElementSelector, IEqualityComparer<TKey> rpComparer)
        {
            if (rpSource == null)
                throw new ArgumentNullException(nameof(rpSource));
            if (rpKeySelector == null)
                throw new ArgumentNullException(nameof(rpKeySelector));
            if (rpElementSelector == null)
                throw new ArgumentNullException(nameof(rpElementSelector));

            var rResult = new HybridDictionary<TKey, TElement>(rpComparer);
            foreach (var rItem in rpSource)
                rResult.Add(rpKeySelector(rItem), rpElementSelector(rItem));

            return rResult;
        }
    }
}
