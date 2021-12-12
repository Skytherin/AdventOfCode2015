using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace AdventOfCode2015.Utils
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T first, T second)> Pairs<T>(this IEnumerable<T> self)
        {
            var l = self.ToList();
            for (var first = 0; first < l.Count - 1; first++)
            {
                for (var second = first + 1; second < l.Count; second++)
                {
                    yield return (l[first], l[second]);
                }
            }
        }

        public static string Join<T>(this IEnumerable<T> self, string separator = "") =>
            string.Join(separator, self.Select(it => it?.ToString()));

        public static List<string> SplitIntoLines(this string input) =>
            input.Split("\n")
                .Select(it => it.Trim()).ToList();

        public static IEnumerable<List<T>> Permute<T>(this IEnumerable<T> input)
        {
            var original = input.ToList();
            if (original.Count == 0)
            {
                yield return new List<T>();
                yield break;
            }
            foreach (var index in Enumerable.Range(0, original.Count))
            {
                var modified = original.ToList();
                var first = original[index];
                modified.RemoveAt(index);
                foreach (var permutation in Permute(modified))
                {
                    permutation.Insert(0, first);
                    yield return permutation;
                }
            }
        }

        public static IEnumerable<List<T>> Runs<T>(this IEnumerable<T> input)
        {
            var original = input.ToList();
            if (original.Count == 0)
            {
                yield return new List<T>();
                yield break;
            }

            var runKey = original[0];
            var count = 1;
            foreach (var current in original.Skip(1))
            {
                if (current.Equals(runKey))
                {
                    count += 1;
                }
                else
                {
                    yield return Enumerable.Repeat(runKey, count).ToList();
                    runKey = current;
                    count = 1;
                }
            }
            yield return Enumerable.Repeat(runKey, count).ToList();
        }

        public static IEnumerable<List<T>> Increments<T>(this IEnumerable<T> input,
            T firstElement, T lastElement, Func<T, T> incrementFunc)
        {
            var original = input.ToArray();
            while (true)
            {
                var i = 0;
                while (i < original.Length)
                {
                    if (original[i].Equals(lastElement))
                    {
                        original[i] = firstElement;
                        i += 1;
                        if (i >= original.Length) yield break;
                        continue;
                    }

                    original[i] = incrementFunc(original[i]);
                    yield return original.ToList();
                    break;
                }
            }
        }

        public static IEnumerable<List<T>> Increments<T>(this IEnumerable<T> input,
            T firstElement, Func<int, T> lastElement, Func<T, T> incrementFunc)
        {
            var original = input.ToList();
            yield return original;
            while (true)
            {
                var i = original.Count - 1;
                while (i >= 0)
                {
                    if (original[i].Equals(lastElement(i)))
                    {
                        original[i] = firstElement;
                        i -= 1;
                        if (i < 0) yield break;
                        continue;
                    }

                    original[i] = incrementFunc(original[i]);
                    yield return original.ToList();
                    break;
                }
            }
        }

        public static IEnumerable<(T Value, int Index)> WithIndices<T>(this IEnumerable<T> self) =>
            self.Select((it, index) => (it, index));

        public static T Pop<T>(this List<T> self)
        {
            if (self.Any())
            {
                var result = self.Last();
                self.RemoveAt(self.Count - 1);
                return result;
            }
            throw new ApplicationException("Attempt to shift empty list.");
        }

        public static IEnumerable<(T Value, int Length)> RunLengthEncode<T>(this IEnumerable<T> self)
        {
            var temp = new List<T>();
            var length = 0;
            foreach (var t in self)
            {
                if (!temp.Any())
                {
                    temp.Add(t);
                    length = 1;
                }
                else if (temp[0]!.Equals(t))
                {
                    length += 1;
                }
                else
                {
                    yield return (temp[0], length);
                    temp.Clear();
                    temp.Add(t);
                    length = 1;
                }
            }
            if (length > 0) yield return (temp[0], length);
        }

        public static TSource MinBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector) where TSource : notnull
        {
            var comparer = Comparer<TKey>.Default;
            return source.ArgBy(keySelector, lag => comparer.Compare(lag.Current, lag.Previous) < 0);
        }

        public static TSource ArgBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<(TKey Current, TKey Previous), bool> predicate) where TSource : notnull
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (!source.Any()) new InvalidOperationException("Sequence contains no elements");

            var value = source.First();
            var key = keySelector(value);

            bool hasValue = false;
            foreach (var other in source)
            {
                var otherKey = keySelector(other);
                if (otherKey == null) continue;

                if (hasValue)
                {
                    if (predicate((otherKey, key)))
                    {
                        value = other;
                        key = otherKey;
                    }
                }
                else
                {
                    value = other;
                    key = otherKey;
                    hasValue = true;
                }
            }
            if (hasValue)
            {
                return value;
            }
            throw new InvalidOperationException("Sequence contains no elements");
        }
    }
}