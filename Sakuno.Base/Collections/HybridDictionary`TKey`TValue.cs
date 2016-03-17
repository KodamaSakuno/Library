using System;
using System.Collections;
using System.Collections.Generic;

namespace Sakuno.Collections
{
    public class HybridDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        ListDictionary<TKey, TValue> r_ListDictionary;
        Dictionary<TKey, TValue> r_Dictionary;

        public int Count
        {
            get
            {
                if (r_Dictionary != null)
                    return r_Dictionary.Count;

                if (r_ListDictionary != null)
                    return r_ListDictionary.Count;

                return 0;
            }
        }

        public TValue this[TKey rpKey]
        {
            get
            {
                if (r_Dictionary != null)
                    return r_Dictionary[rpKey];

                if (r_ListDictionary != null)
                    return r_ListDictionary[rpKey];

                throw new KeyNotFoundException();
            }
            set
            {
                if (r_Dictionary != null)
                {
                    r_Dictionary[rpKey] = value;
                    return;
                }

                if (r_ListDictionary == null)
                {
                    r_ListDictionary = new ListDictionary<TKey, TValue>(r_Comparer);
                    r_ListDictionary[rpKey] = value;
                    return;
                }
                if (r_ListDictionary.Count < 8)
                {
                    r_ListDictionary[rpKey] = value;
                    return;
                }

                SwitchToDictionary();
                r_Dictionary[rpKey] = value;
            }
        }

        ListDictionary<TKey, TValue> ListDictionary
        {
            get
            {
                if (r_ListDictionary == null)
                    r_ListDictionary = new ListDictionary<TKey, TValue>(r_Comparer);
                return r_ListDictionary;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                if (r_Dictionary != null)
                    return r_Dictionary.Keys;

                return ListDictionary.Keys;
            }
        }
        public ICollection<TValue> Values
        {
            get
            {
                if (r_Dictionary != null)
                    return r_Dictionary.Values;

                return ListDictionary.Values;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        IEqualityComparer<TKey> r_Comparer;

        public HybridDictionary() { }
        public HybridDictionary(IEqualityComparer<TKey> rpComparer)
        {
            r_Comparer = rpComparer;
        }
        public HybridDictionary(int rpCapacity) : this(rpCapacity, null) { }
        public HybridDictionary(int rpCapacity, IEqualityComparer<TKey> rpComparer)
        {
            r_Comparer = rpComparer;

            if (rpCapacity > 5)
                r_Dictionary = new Dictionary<TKey, TValue>(rpCapacity, rpComparer);
        }

        void SwitchToDictionary()
        {
            var rDictionary = new Dictionary<TKey, TValue>(13, r_Comparer);

            foreach (var rItem in r_ListDictionary)
                rDictionary.Add(rItem.Key, rItem.Value);

            r_Dictionary = rDictionary;
            r_ListDictionary = null;
        }

        public void Add(TKey rpKey, TValue rpValue)
        {
            if (r_Dictionary != null)
            {
                r_Dictionary.Add(rpKey, rpValue);
                return;
            }

            if (r_ListDictionary == null)
            {
                r_ListDictionary = new ListDictionary<TKey, TValue>(r_Comparer);
                r_ListDictionary.Add(rpKey, rpValue);
                return;
            }
            if (r_ListDictionary.Count < 8)
            {
                r_ListDictionary.Add(rpKey, rpValue);
                return;
            }

            SwitchToDictionary();
            r_Dictionary.Add(rpKey, rpValue);
        }
        public bool Remove(TKey rpKey)
        {
            if (r_Dictionary != null)
                return r_Dictionary.Remove(rpKey);

            if (r_ListDictionary != null)
                return r_ListDictionary.Remove(rpKey);

            return false;
        }
        public void Clear()
        {
            if (r_Dictionary != null)
            {
                r_Dictionary.Clear();
                r_Dictionary = null;
                return;
            }

            r_ListDictionary?.Clear();
            r_ListDictionary = null;
        }
        public bool ContainsKey(TKey rpKey)
        {
            if (r_Dictionary != null)
                return r_Dictionary.ContainsKey(rpKey);

            if (r_ListDictionary != null)
                return r_ListDictionary.ContainsKey(rpKey);

            return false;
        }
        public bool TryGetValue(TKey rpKey, out TValue ropValue)
        {
            if (r_Dictionary != null)
                return r_Dictionary.TryGetValue(rpKey, out ropValue);

            if (r_ListDictionary == null)
            {
                r_ListDictionary = new ListDictionary<TKey, TValue>(r_Comparer);

                ropValue = default(TValue);
                return false;
            }

            return r_ListDictionary.TryGetValue(rpKey, out ropValue);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (r_Dictionary != null)
                return r_Dictionary.GetEnumerator();

            return ListDictionary.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> rpItem)
        {
            throw new NotImplementedException();
        }
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> rpItem)
        {
            throw new NotImplementedException();
        }
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] roArray, int rpArrayIndex)
        {
            throw new NotImplementedException();
        }
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> rpItem)
        {
            throw new NotImplementedException();
        }
    }
}
