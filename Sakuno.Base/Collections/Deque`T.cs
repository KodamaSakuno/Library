using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Sakuno.Collections
{
    public class Deque<T> : IEnumerable<T>, ICollection, IReadOnlyCollection<T>
    {
        T[] r_Array;

        int r_Count;
        public int Count => r_Count;

        int r_Head, r_Tail;

        public T this[int rpIndex]
        {
            get
            {
                if (rpIndex < 0 || rpIndex >= r_Count)
                    throw new ArgumentOutOfRangeException();

                return GetElement(rpIndex);
            }
            set
            {
                if (rpIndex < 0 || rpIndex >= r_Count)
                    throw new ArgumentOutOfRangeException();

                r_Array[GetOffset(rpIndex)] = value;
                r_Version++;
            }
        }

        int r_Version;

        bool ICollection.IsSynchronized => false;

        object r_ThreadSyncLock;
        object ICollection.SyncRoot => r_ThreadSyncLock ?? Interlocked.CompareExchange(ref r_ThreadSyncLock, new object(), null);

        public Deque()
        {
            r_Array = ArrayUtil.Empty<T>();
        }
        public Deque(int rpCapacity)
        {
            if (rpCapacity < 0)
                throw new ArgumentOutOfRangeException();

            r_Array = new T[rpCapacity];
        }
        public Deque(IEnumerable<T> rpCollection)
        {
            if (rpCollection == null)
                throw new ArgumentNullException();

            r_Array = new T[4];

            foreach (var rItem in rpCollection)
                Enqueue(rItem);
        }

        public void Enqueue(T rpItem)
        {
            GrowCapacityIfNecessary();

            r_Array[r_Tail] = rpItem;

            r_Tail = (r_Tail + 1) % r_Array.Length;
            r_Count++;
            r_Version++;
        }
        public void EnqueueFront(T rpItem)
        {
            GrowCapacityIfNecessary();

            r_Head = (r_Head + r_Array.Length - 1) % r_Array.Length;
            r_Array[r_Head] = rpItem;

            r_Count++;
            r_Version++;
        }
        public void Enqueue(T rpItem, QueueSide rpSide)
        {
            switch (rpSide)
            {
                case QueueSide.Back:
                    Enqueue(rpItem);
                    break;

                case QueueSide.Front:
                    EnqueueFront(rpItem);
                    break;

                default: throw new ArgumentException();
            }
        }

        public T Dequeue()
        {
            if (r_Count == 0)
                throw new InvalidOperationException();

            var rResult = r_Array[r_Head];
            r_Array[r_Head] = default(T);

            r_Head = (r_Head + 1) % r_Array.Length;
            r_Count--;
            r_Version++;

            return rResult;
        }
        public T DequeueBack()
        {
            if (r_Count == 0)
                throw new InvalidOperationException();

            r_Tail = (r_Tail + r_Array.Length - 1) % r_Array.Length;
            var rResult = r_Array[r_Tail];
            r_Array[r_Tail] = default(T);

            r_Count--;
            r_Version++;

            return rResult;
        }
        public T Dequeue(QueueSide rpSide)
        {
            switch (rpSide)
            {
                case QueueSide.Back:
                    return DequeueBack();

                case QueueSide.Front:
                    return Dequeue();

                default: throw new ArgumentException();
            }
        }

        public T Peek()
        {
            if (r_Count == 0)
                throw new InvalidOperationException();

            return r_Array[r_Head];
        }
        public T PeekBack()
        {
            if (r_Count == 0)
                throw new InvalidOperationException();

            var rTail = (r_Tail + r_Array.Length - 1) % r_Array.Length;

            return r_Array[rTail];
        }
        public T Peek(QueueSide rpSide)
        {
            switch (rpSide)
            {
                case QueueSide.Back:
                    return PeekBack();

                case QueueSide.Front:
                    return Peek();

                default: throw new ArgumentException();
            }
        }

        public void Clear()
        {
            if (r_Head < r_Tail)
                Array.Clear(r_Array, r_Head, r_Count);
            else
            {
                Array.Clear(r_Array, r_Head, r_Array.Length - r_Head);
                Array.Clear(r_Array, 0, r_Tail);
            }

            r_Head = r_Tail = 0;
            r_Count = 0;
            r_Version++;
        }

        public bool Contains(T rpItem)
        {
            var rIndex = r_Head;
            var rCount = r_Count;

            var rComparer = EqualityComparer<T>.Default;

            while (rCount-- > 0)
            {
                if (rpItem == null && r_Array[rIndex] == null)
                    return true;
                else if (r_Array[rIndex] != null && rComparer.Equals(r_Array[rIndex], rpItem))
                    return true;

                rIndex = (rIndex + 1) % r_Array.Length;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int GetOffset(int rpIndex) => (r_Head + rpIndex) % r_Array.Length;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T GetElement(int rpIndex) => r_Array[GetOffset(rpIndex)];

        void GrowCapacityIfNecessary()
        {
            if (r_Count != r_Array.Length)
                return;

            var rCapacity = Math.Max(r_Array.Length * 2, r_Array.Length + 4);
            var rArray = new T[rCapacity];

            if (r_Count > 0)
            {
                if (r_Head < r_Tail)
                    Array.Copy(r_Array, r_Head, rArray, 0, r_Count);
                else
                {
                    Array.Copy(r_Array, r_Head, rArray, 0, r_Array.Length - r_Head);
                    Array.Copy(r_Array, 0, rArray, r_Array.Length - r_Head, r_Tail);
                }
            }

            r_Array = rArray;

            r_Head = 0;
            r_Tail = r_Count != rCapacity ? r_Count : 0;
            r_Version++;
        }

        public Enumerator GetEnumerator() => new Enumerator(this);
        public ReverseEnumerator GetReverseEnumerator() => new ReverseEnumerator(this);
        public IEnumerator<T> GetEnumerator(QueueSide rpSide)
        {
            switch (rpSide)
            {
                case QueueSide.Back:
                    return GetReverseEnumerator();

                case QueueSide.Front:
                    return GetEnumerator();

                default: throw new ArgumentException();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        void ICollection.CopyTo(Array rpArray, int rpIndex)
        {
            if (rpArray == null)
                throw new ArgumentNullException();

            if (rpArray.Rank != 1 || rpArray.GetLowerBound(0)!=0)
                throw new RankException();

            var rLength = rpArray.Length;

            if (rpIndex < 0 || rpIndex > rLength)
                throw new ArgumentOutOfRangeException();

            if (rLength - rpIndex < r_Count)
                throw new ArgumentException();

            var rTotalCount = r_Count;
            if (rTotalCount == 0)
                return;

            var rCount = Math.Min(r_Array.Length - r_Head, rTotalCount);
            Array.Copy(r_Array, r_Head, rpArray, rpIndex, rCount);

            rTotalCount -= rCount;
            if (rCount > 0)
                Array.Copy(r_Array, 0, rpArray, rpIndex + r_Array.Length - r_Head, rCount);
        }

        public struct Enumerator : IEnumerator<T>
        {
            Deque<T> r_Owner;

            int r_Version;

            int r_Index;

            T r_Current;
            public T Current
            {
                get
                {
                    if (r_Index < 0)
                        throw new InvalidOperationException();

                    return r_Current;
                }
            }
            object IEnumerator.Current => Current;

            public Enumerator(Deque<T> rpOwner)
            {
                r_Owner = rpOwner;

                r_Version = r_Owner.r_Version;

                r_Index = -1;
                r_Current = default(T);
            }

            public bool MoveNext()
            {
                if (r_Version != r_Owner.r_Version)
                    throw new InvalidOperationException();

                if (r_Index == -2)
                    return false;

                r_Index++;

                if (r_Index == r_Owner.r_Count)
                {
                    Dispose();
                    return false;
                }

                r_Current = r_Owner.GetElement(r_Index);
                return true;
            }

            public void Dispose()
            {
                r_Index = -2;
                r_Current = default(T);
            }
            void IEnumerator.Reset()
            {
                if (r_Version != r_Owner.r_Version)
                    throw new InvalidOperationException();

                r_Index = -1;
                r_Current = default(T);
            }
        }
        public struct ReverseEnumerator : IEnumerator<T>
        {
            Deque<T> r_Owner;

            int r_Version;

            int r_Index;

            T r_Current;
            public T Current
            {
                get
                {
                    if (r_Index < 0)
                        throw new InvalidOperationException();

                    return r_Current;
                }
            }
            object IEnumerator.Current => Current;

            public ReverseEnumerator(Deque<T> rpOwner)
            {
                r_Owner = rpOwner;

                r_Version = r_Owner.r_Version;

                r_Index = r_Owner.r_Count;
                r_Current = default(T);
            }

            public bool MoveNext()
            {
                if (r_Version != r_Owner.r_Version)
                    throw new InvalidOperationException();

                if (r_Index == -2)
                    return false;

                r_Index--;

                if (r_Index == -1)
                {
                    Dispose();
                    return false;
                }

                r_Current = r_Owner.GetElement(r_Index);
                return true;
            }

            public void Dispose()
            {
                r_Index = -2;
                r_Current = default(T);
            }
            void IEnumerator.Reset()
            {
                if (r_Version != r_Owner.r_Version)
                    throw new InvalidOperationException();

                r_Index = -1;
                r_Current = default(T);
            }
        }
    }
}
