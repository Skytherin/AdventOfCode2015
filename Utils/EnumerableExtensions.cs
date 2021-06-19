using System.Collections.Generic;
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

        public static string Join<T>(this IEnumerable<T> self, string separator) =>
            string.Join(separator, self.Select(it => it?.ToString()));
    }
}