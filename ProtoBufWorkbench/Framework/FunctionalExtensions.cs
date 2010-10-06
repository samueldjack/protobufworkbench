using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProtoBufWorkbench.Framework
{
    /// <summary>
    /// Extension methods implementing concepts from Functional Programming
    /// </summary>
    public static class FunctionalExtensions
    {
        /// <summary>
        /// Generates a sequence by repeatedly applying a generator function to the previous
        /// value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="seed">The seed value to initiate the sequence.</param>
        /// <param name="generator">The generator function to produce the next item.</param>
        /// <returns></returns>
        public static IEnumerable<T> Unfold<T>(this T seed, Func<T, T> generator)
        {
            // include seed in the sequence
            yield return seed;

            T current = seed;

            // now continue the sequence
            while (true)
            {
                current = generator(current);
                yield return current;
            }
        }

        /// <summary>
        /// Memoizes the specified function (i.e. remembers previously seen parameters to the function and keeps the result of the function to avoid repeated calculations)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="function">The function.</param>
        /// <returns></returns>
        public static Func<T, T1, TResult> Memoize<T, T1, TResult>(this Func<T, T1, TResult> function)
        {
            var cache = new Dictionary<Pair<T, T1>, TResult>();

            return (arg, arg1) =>
            {
                TResult result;
                var argumentPair = new Pair<T, T1>(arg, arg1);
                if (cache.TryGetValue(argumentPair, out result))
                {
                    return result;
                }
                result = function(arg, arg1);
                cache.Add(argumentPair, result);
                return result;
            };
        }

        /// <summary>
        /// Memoizes the specified function (i.e. remembers previously seen parameters to the function and keeps the result of the function to avoid repeated calculations)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="function">The function.</param>
        /// <returns></returns>
        public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> function)
        {
            var cache = new Dictionary<T, TResult>();

            return arg =>
            {
                TResult result;
                if (cache.TryGetValue(arg, out result))
                {
                    return result;
                }
                else
                {
                    result = function(arg);
                    cache.Add(arg, result);
                    return result;
                }
            };
        }

        /// <summary>
        /// Memoizes the specified function.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="function">The function.</param>
        /// <returns></returns>
        public static Func<TResult> Memoize<TResult>(this Func<TResult> function)
        {
            bool hasResult = false;
            TResult previous = default(TResult);

            return () =>
            {
                TResult result;

                if (!hasResult)
                {
                    result = function();
                    previous = result;
                    hasResult = true;
                }
                else
                {
                    result = previous;
                }
                return result;
            };
        }

        /// <summary>
        /// <c>Concats</c> the specified sequences.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequences</typeparam>
        /// <param name="sequences">The sequences.</param>
        /// <returns>A flattened sequence.</returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            return sequences.SelectMany(sequence => sequence);
        }

        /// <summary>
        /// Returns a sequence of partial aggregates of the sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulate.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="aggregator">The aggregator.</param>
        /// <returns>a sequence of partial aggregates of the sequence</returns>
        public static IEnumerable<TAccumulate> LazilyAggregate<T, TAccumulate>(this IEnumerable<T> sequence, TAccumulate seed, Func<T, TAccumulate, TAccumulate> aggregator)
        {
            var accumulatedValue = seed;
            yield return seed;

            foreach (var item in sequence)
            {
                accumulatedValue = aggregator(item, accumulatedValue);
                yield return accumulatedValue;
            }
        }

        /// <summary>  
        /// Slices a sequence into a sub-sequences each containing maxItemsPerSlice, except for the last  
        /// which will contain any items left over  
        /// </summary>  
        public static IEnumerable<IList<T>> Slice<T>(this IEnumerable<T> sequence, int maxItemsPerSlice)
        {
            if (maxItemsPerSlice <= 0)
            {
                throw new ArgumentOutOfRangeException("maxItemsPerSlice", "maxItemsPerSlice must be greater than 0");
            }

            List<T> slice = new List<T>(maxItemsPerSlice);

            foreach (var item in sequence)
            {
                slice.Add(item);

                if (slice.Count == maxItemsPerSlice)
                {
                    yield return slice.ToArray();
                    slice.Clear();
                }
            }

            // return the "crumbs" that   
            // didn't make it into a full slice  
            if (slice.Count > 0)
            {
                yield return slice.ToArray();
            }
        }

        /// <summary>
        /// Wraps an IEnumerable around the specified enumerator.
        /// </summary>
        /// <param name="enumerator">The enumerator.</param>
        /// <returns>An instance that wraps the given IEnumerator</returns>
        public static IEnumerable<T> Enumerate<T>(this IEnumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return (T) enumerator.Current;
            }
        }
    }
}
