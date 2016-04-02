using System;
using System.Collections;
using System.Collections.Generic;

namespace Sakuno.Collections
{
    public class ListDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        Node r_Head;

        int r_Count;
        public int Count => r_Count;

        public TValue this[TKey rpKey]
        {
            get
            {
                if (rpKey == null)
                    throw new ArgumentNullException(nameof(rpKey));

                Node rNode = r_Head;
                if (r_Comparer == null)
                    while(rNode != null)
                    {
                        var rKey = rNode.Key;
                        if (rKey != null && rKey.Equals(rpKey))
                            return rNode.Value;

                        rNode = rNode.Next;
                    }
                else
                    while (rNode != null)
                    {
                        var rKey = rNode.Key;
                        if (rKey != null && r_Comparer.Equals(rpKey, rKey))
                            return rNode.Value;

                        rNode = rNode.Next;
                    }

                throw new KeyNotFoundException();
            }
            set
            {
                if (rpKey == null)
                    throw new ArgumentNullException(nameof(rpKey));

                AddOrSet(rpKey, value, false);
            }
        }

        KeyCollection r_Keys;
        public ICollection<TKey> Keys
        {
            get
            {
                if (r_Keys == null)
                    r_Keys = new KeyCollection(this);
                return r_Keys;
            }
        }

        ValueCollection r_Values;
        public ICollection<TValue> Values
        {
            get
            {
                if (r_Values == null)
                    r_Values = new ValueCollection(this);
                return r_Values;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        int r_Version;

        IEqualityComparer<TKey> r_Comparer;

        public ListDictionary() { }
        public ListDictionary(IEqualityComparer<TKey> rpComparer)
        {
            r_Comparer = rpComparer;
        }

        public void Add(TKey rpKey, TValue rpValue) => AddOrSet(rpKey, rpValue, true);
        void AddOrSet(TKey rpKey, TValue rpValue, bool rpThrowExceptionOnDuplicatedKey)
        {
            r_Version++;

            Node rNode = null;
            Node rCurrent;
            for (rCurrent = r_Head; rCurrent != null; rCurrent = rCurrent.Next)
            {
                var rKey = rCurrent.Key;
                if (r_Comparer == null ? rKey.Equals(rpKey) : r_Comparer.Equals(rpKey, rKey))
                    if (rpThrowExceptionOnDuplicatedKey)
                        throw new ArgumentException();
                    else
                        break;

                rNode = rCurrent;
            }

            if (rCurrent != null)
            {
                rCurrent.Value = rpValue;
                return;
            }

            var rNewNode = new Node() { Key = rpKey, Value = rpValue };

            if (rNode != null)
                rNode.Next = rNewNode;
            else
                r_Head = rNewNode;

            r_Count++;
        }
        public bool Remove(TKey rpKey)
        {
            if (rpKey == null)
                throw new ArgumentNullException(nameof(rpKey));

            r_Version++;

            Node rNode = null;
            Node rCurrent;
            for (rCurrent = r_Head; rCurrent != null; rCurrent = rCurrent.Next)
            {
                var rKey = rCurrent.Key;
                if (r_Comparer == null ? rKey.Equals(rpKey) : r_Comparer.Equals(rpKey, rKey))
                    break;

                rNode = rCurrent;
            }

            if (rCurrent == null)
                return false;

            if (rCurrent == r_Head)
                r_Head = rCurrent.Next;
            else
                rNode.Next = rCurrent.Next;

            r_Count--;

            return true;
        }
        public void Clear()
        {
            r_Count = 0;
            r_Head = null;
            r_Version++;
        }

        public bool ContainsKey(TKey rpKey)
        {
            if (rpKey == null)
                throw new ArgumentNullException(nameof(rpKey));

            for (var rCurrent = r_Head; rCurrent != null; rCurrent = rCurrent.Next)
            {
                var rKey = rCurrent.Key;
                if (r_Comparer == null ? rKey.Equals(rpKey) : r_Comparer.Equals(rpKey, rKey))
                    return true;
            }

            return false;
        }


        public bool TryGetValue(TKey rpKey, out TValue ropValue)
        {
            Node rCurrent;
            for (rCurrent = r_Head; rCurrent != null; rCurrent = rCurrent.Next)
            {
                var rKey = rCurrent.Key;
                if (r_Comparer == null ? rKey.Equals(rpKey) : r_Comparer.Equals(rpKey, rKey))
                    break;
            }

            if (rCurrent == null)
            {
                ropValue = default(TValue);
                return false;
            }

            ropValue = rCurrent.Value;
            return true;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (var rCurrent = r_Head; rCurrent != null; rCurrent = rCurrent.Next)
                yield return new KeyValuePair<TKey, TValue>(rCurrent.Key, rCurrent.Value);
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> rpItem) => Add(rpItem.Key, rpItem.Value);
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> rpItem)
        {
            TValue rValue;
            if (TryGetValue(rpItem.Key, out rValue) && EqualityComparer<TValue>.Default.Equals(rValue, rpItem.Value))
            {
                Remove(rpItem.Key);
                return true;
            }

            return false;
        }
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> rpItem)
        {
            TValue rValue;
            if (TryGetValue(rpItem.Key, out rValue))
                return EqualityComparer<TValue>.Default.Equals(rValue, rpItem.Value);

            return false;
        }
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] rpArray, int rpIndex)
        {
            if (rpArray == null)
                throw new ArgumentNullException(nameof(rpArray));
            if (rpIndex < 0 || rpArray.Length - rpIndex < r_Count)
                throw new ArgumentOutOfRangeException(nameof(rpIndex));

            for (var rCurrent = r_Head; rCurrent != null; rCurrent = rCurrent.Next)
                rpArray[rpIndex++] = new KeyValuePair<TKey, TValue>(rCurrent.Key, rCurrent.Value);
        }

        class Node
        {
            public TKey Key;
            public TValue Value;

            public Node Next;
        }

        public class KeyCollection : ICollection<TKey>
        {
            ListDictionary<TKey, TValue> r_Owner;

            public int Count => r_Owner.Count;

            bool ICollection<TKey>.IsReadOnly => true;

            internal KeyCollection(ListDictionary<TKey, TValue> rpOwner)
            {
                r_Owner = rpOwner;
            }

            public IEnumerator<TKey> GetEnumerator()
            {
                for (var rCurrent = r_Owner.r_Head; rCurrent != null; rCurrent = rCurrent.Next)
                    yield return rCurrent.Key;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public void CopyTo(TKey[] rpArray, int rpIndex)
            {
                if (rpArray == null)
                    throw new ArgumentNullException(nameof(rpArray));
                if (rpIndex < 0 || rpIndex > rpArray.Length)
                    throw new ArgumentOutOfRangeException(nameof(rpIndex));
                if (rpArray.Length - rpIndex < Count)
                    throw new ArgumentException(nameof(rpIndex));

                for (var rCurrent = r_Owner.r_Head; rCurrent != null; rCurrent = rCurrent.Next)
                    rpArray[rpIndex++] = rCurrent.Key;
            }

            void ICollection<TKey>.Add(TKey rpItem)
            {
                throw new NotSupportedException();
            }
            bool ICollection<TKey>.Remove(TKey rpItem)
            {
                throw new NotSupportedException();
            }
            void ICollection<TKey>.Clear()
            {
                throw new NotSupportedException();
            }
            bool ICollection<TKey>.Contains(TKey rpItem) => r_Owner.ContainsKey(rpItem);
        }
        public class ValueCollection : ICollection<TValue>
        {
            ListDictionary<TKey, TValue> r_Owner;

            public int Count => r_Owner.Count;

            bool ICollection<TValue>.IsReadOnly => true;

            internal ValueCollection(ListDictionary<TKey, TValue> rpOwner)
            {
                r_Owner = rpOwner;
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                for (var rCurrent = r_Owner.r_Head; rCurrent != null; rCurrent = rCurrent.Next)
                    yield return rCurrent.Value;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public void CopyTo(TValue[] rpArray, int rpIndex)
            {
                if (rpArray == null)
                    throw new ArgumentNullException(nameof(rpArray));
                if (rpIndex < 0 || rpIndex > rpArray.Length)
                    throw new ArgumentOutOfRangeException(nameof(rpIndex));
                if (rpArray.Length - rpIndex < Count)
                    throw new ArgumentException(nameof(rpIndex));

                for (var rCurrent = r_Owner.r_Head; rCurrent != null; rCurrent = rCurrent.Next)
                    rpArray[rpIndex++] = rCurrent.Value;
            }

            void ICollection<TValue>.Add(TValue rpItem)
            {
                throw new NotSupportedException();
            }
            bool ICollection<TValue>.Remove(TValue rpItem)
            {
                throw new NotSupportedException();
            }
            void ICollection<TValue>.Clear()
            {
                throw new NotSupportedException();
            }
            bool ICollection<TValue>.Contains(TValue rpItem)
            {
                throw new NotSupportedException();
            }
        }
    }
}
