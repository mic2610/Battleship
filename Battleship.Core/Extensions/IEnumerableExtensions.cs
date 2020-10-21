using Battleship.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Delegate factory used for TrySelect extension. Attempts to transform a TIn into a TOut, with the bool result
        /// representing the success or failure of this operation.
        /// </summary>
        public delegate bool TrySelectDelegate<TIn, TOut>(TIn input, out TOut result);

        public static bool IsNullOrEmpty(this IEnumerable value)
        {
            return value == null || !value.GetEnumerator().MoveNext();
        }

        public static bool HasMinimumCount(this IEnumerable value, uint minimumCount)
        {
            if (value == null)
                return false;

            if (value is ICollection collection)
                return collection.Count >= minimumCount;

            return value.HasMinimumCountInternal(minimumCount);
        }

        public static bool HasMinimumCount<T>(this IEnumerable<T> value, uint minimumCount)
        {
            if (value == null)
                return false;

            if (value is ICollection<T> collection)
                return collection.Count >= minimumCount;

            return value.HasMinimumCountInternal(minimumCount);
        }

        private static bool HasMinimumCountInternal(this IEnumerable value, uint minimumCount)
        {
            foreach (var discard in value)
            {
                if (--minimumCount <= 0)
                    return true;
            }

            return false;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> value)
        {
            return value.Shuffle(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> value, Random random)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (random == null)
                throw new ArgumentNullException(nameof(random));

            return value.ShuffleIterator(random);
        }

        private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> value, Random random)
        {
            var buffer = value.ToList();
            if (buffer.IsNullOrEmpty())
                yield return default;

            for (var i = 0; i < buffer.Count; i++)
            {
                var j = random.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }

        /// <summary>
        /// Almost verbatim copy of https://stackoverflow.com/a/5423024 (just switched to queue instead of stack so enumerable order is preserved & fixed so it will compile)
        /// </summary>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childSelector)
        {
            var queue = new Queue<T>(source);

            while (queue.Count > 0)
            {
                T item = queue.Dequeue();

                yield return item;

                // Push all of the children on the queue.
                foreach (T child in childSelector(item))
                    queue.Enqueue(child);
            }
        }

        public static IEnumerable<TResult> SelectNotNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
            where TResult : class
        {
            foreach (var src in source)
            {
                var current = selector(src);
                if (current != null)
                    yield return current;
            }
        }

        public static IEnumerable<TSource> SafeUnion<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (first == null)
                return second;

            if (second == null)
                return first;

            return first.Union(second);
        }

        /// <summary>
        /// Find the item within the sequence with the maximum value returned from the selector function
        /// </summary>
        public static TSource MaxBy<TSource, TValue>(this IEnumerable<TSource> source, Func<TSource, TValue> selector)
            where TValue : IComparable<TValue>
        {
            var isFirst = true;
            return source
                .Select(i => (Item: i, Value: selector(i)))
                .Aggregate(
                    default((TSource Item, TValue Value)),
                    (max, next) => isFirst && !(isFirst = false) || next.Value.CompareTo(max.Value) > 0 ? next : max).Item;
        }

        /// <summary>
        /// Find the item within the sequence with the minimum value returned from the selector function
        /// </summary>
        public static TSource MinBy<TSource, TValue>(this IEnumerable<TSource> source, Func<TSource, TValue> selector)
            where TValue : IComparable<TValue>
        {
            var isFirst = true;
            return source
                .Select(i => (Item: i, Value: selector(i)))
                .Aggregate(
                    default((TSource Item, TValue Value)),
                    (min, next) => isFirst && !(isFirst = false) || next.Value.CompareTo(min.Value) < 0 ? next : min).Item;
        }

        /// <summary>
        /// Creates a dictionary from the given enumerable. Safely handles a null enumerable without throwing exceptions.
        /// </summary>
        public static Dictionary<TKey, TSource> SafeToDictionary<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer = null)
        {
            if (source == null)
                return null;

            var dictionary = comparer == null ? new Dictionary<TKey, TSource>() : new Dictionary<TKey, TSource>(comparer);
            foreach (var current in source)
                dictionary[keySelector(current)] = current;

            return dictionary;
        }

        /// <summary>
        /// Creates a dictionary from the given enumerable. Safely handles a null enumerable without throwing exceptions.
        /// </summary>
        public static Dictionary<TKey, TValue> SafeToDictionary<TSource, TKey, TValue>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> valueSelector,
            IEqualityComparer<TKey> comparer = null)
        {
            if (source == null || keySelector == null || valueSelector == null)
                return null;

            var dictionary = comparer == null ? new Dictionary<TKey, TValue>() : new Dictionary<TKey, TValue>(comparer);
            foreach (var current in source)
                dictionary[keySelector(current)] = valueSelector(current);

            return dictionary;
        }

        /// <summary>
        /// Convert IEnumerable of KeyValuePairs to Dictionary. If there are any duplicate keys in the IEnumerable, ArgumentException will be thrown
        /// </summary>
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            return keyValuePairs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        /// <summary>
        /// Convert IEnumerable of KeyValuePairs to Dictionary without throwing exceptions. If there are any duplicate keys in the IEnumerable, the last KeyValuePair wins!
        /// </summary>
        public static IDictionary<TKey, TValue> SafeToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            if (keyValuePairs == null)
                return null;

            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var current in keyValuePairs)
                dictionary[current.Key] = current.Value;

            return dictionary;
        }

        /// <summary>
        /// Batches the given source into multiple provided size batches
        /// </summary>
        /// <param name="size">Size of each batch</param>
        /// <param name="max">Maximum number of batches</param>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, uint size, uint max = byte.MaxValue)
        {
            var enumerator = source?.GetEnumerator();

            if (enumerator == null)
                yield break;

            IEnumerable<T> CreateBatch()
            {
                var batch = size;
                while (batch-- > 0 && enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }

            while (max-- > 0)
            {
                yield return CreateBatch();
            }
        }

        /// <summary>
        /// Returns an enumeration of results which satisfy the conditions of the input TryGetDelegate
        /// </summary>
        public static IEnumerable<TOut> TrySelect<TIn, TOut>(this IEnumerable<TIn> sequence, TrySelectDelegate<TIn, TOut> selector)
        {
            if (sequence == null)
                yield break;

            foreach (var element in sequence)
            {
                if (selector(element, out var result))
                    yield return result;
            }
        }

        /// <summary>
        /// Returns an empty enumeration of results if the enumeration is null
        /// </summary>
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Caches the results of enumerating over a given enumerable so that evaulated positions don't require computation a second time.
        /// Designed for use on the results of 'generator' methods (the ones with <c>yield return</c> in them).
        /// </summary>
        /// <returns>
        /// Either a new enumerable object that caches enumerated results, or the original, <paramref name="sequence"/>
        /// object if no caching is necessary to avoid additional CPU work.
        /// </returns>
        /// <remarks>
        /// <para>This is designed for use on the results of generator methods (the ones with <c>yield return</c> in them)
        /// so that only those elements in the sequence that are needed are ever generated, while not requiring
        /// regeneration of elements that are enumerated over multiple times.</para>
        /// <para>This can be a huge performance gain if enumerating multiple times over an expensive generator method.</para>
        /// <para>Some enumerable types such as collections, lists, and already-cached generators do not require
        /// any (additional) caching, and this method will simply return those objects rather than caching them
        /// to avoid double-caching.</para>
        /// </remarks>
        public static IEnumerable<T> ToCached<T>(this IEnumerable<T> sequence, int? capacity = null)
        {
            // Don't create a cache for types that don't need it.
            if (sequence is IList<T> || sequence is ICollection<T> || sequence is Array || sequence is EnumerableCache<T>)
                return sequence;

            return new EnumerableCache<T>(sequence, capacity);
        }

        /// <summary>
        /// A wrapper for enumerable types and returns a cached IEnumerator from its <see cref="IEnumerable&lt;T&gt;.GetEnumerator"/> method.
        /// </summary>
        private class EnumerableCache<T> : IEnumerable<T>
        {
            /// <summary>
            /// The results from enumeration of the live object that have been collected thus far.
            /// </summary>
            private List<T> cache;

            /// <summary>
            /// The original enumerable object whose contents should only be enumerated only once.
            /// </summary>
            private IEnumerable<T> sequence;

            /// <summary>
            /// The original enumerable object whose contents should only be enumerated only once.
            /// </summary>
            private int? capacity;

            /// <summary>
            /// The enumerator we're using over the sequence's results.
            /// </summary>
            private IEnumerator<T> sequenceEnumerator;

            /// <summary>
            /// Sync object our caching enumerators use when adding a new enumerated result to the cache, allows multiple threads to concurrently enumerate.
            /// </summary>
            private object sequenceLock = new object();

            /// <summary>
            /// Initializes a new instance of the EnumerableCache class.
            /// </summary>
            internal EnumerableCache(IEnumerable<T> sequence, int? capacity = null)
            {
                this.sequence = sequence ?? throw new ArgumentNullException(nameof(sequence));
                this.capacity = capacity;
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            public IEnumerator<T> GetEnumerator()
            {
                if (sequenceEnumerator == null)
                {
                    cache = capacity.HasValue ? new List<T>(capacity.Value) : new List<T>();
                    sequenceEnumerator = sequence.GetEnumerator();
                }

                return new EnumeratorCache(this);
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            /// An enumerator that uses cached enumeration results whenever they are available,
            /// and caches whatever results it has to pull from the original <see cref="IEnumerable&lt;T&gt;"/> object.
            /// </summary>
            private class EnumeratorCache : IEnumerator<T>
            {
                /// <summary>
                /// The parent enumeration wrapper class that stores the cached results.
                /// </summary>
                private EnumerableCache<T> parent;

                /// <summary>
                /// The position of this enumerator in the cached list.
                /// </summary>
                private int cachePosition = -1;

                /// <summary>
                /// Initializes a new instance of the <see cref="EnumerableCache&lt;T&gt;.EnumeratorCache"/> class.
                /// </summary>
                internal EnumeratorCache(EnumerableCache<T> parent)
                {
                    this.parent = parent ?? throw new ArgumentNullException("parent");
                }

                /// <summary>
                /// Gets the element in the collection at the current position of the enumerator.
                /// </summary>
                /// <returns>
                /// The element in the collection at the current position of the enumerator.
                /// </returns>
                public T Current
                {
                    get
                    {
                        if (cachePosition < 0 || cachePosition >= parent.cache.Count)
                        {
                            throw new InvalidOperationException();
                        }

                        return parent.cache[cachePosition];
                    }
                }

                /// <summary>
                /// Gets the element in the collection at the current position of the enumerator.
                /// </summary>
                /// <returns>
                /// The element in the collection at the current position of the enumerator.
                /// </returns>
                object IEnumerator.Current
                {
                    get { return Current; }
                }

                /// <summary>
                /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
                /// </summary>
                public void Dispose()
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }

                /// <summary>
                /// Advances the enumerator to the next element of the collection.
                /// </summary>
                /// <returns>
                /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
                /// </returns>
                /// <exception cref="T:System.InvalidOperationException">
                /// The collection was modified after the enumerator was created.
                /// </exception>
                public bool MoveNext()
                {
                    cachePosition++;
                    if (cachePosition >= parent.cache.Count)
                    {
                        lock (parent.sequenceLock)
                        {
                            if (cachePosition >= parent.cache.Count)
                            {
                                if (parent.sequenceEnumerator.MoveNext())
                                {
                                    parent.cache.Add(parent.sequenceEnumerator.Current);
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    return true;
                }

                /// <summary>
                /// Sets the enumerator to its initial position, which is before the first element in the collection.
                /// </summary>
                /// <exception cref="T:System.InvalidOperationException">
                /// The collection was modified after the enumerator was created.
                /// </exception>
                public void Reset()
                {
                    cachePosition = -1;
                }

                /// <summary>
                /// Releases unmanaged and - optionally - managed resources
                /// </summary>
                protected virtual void Dispose(bool disposing)
                {
                    // Nothing to do here.
                }
            }
        }
    }
}