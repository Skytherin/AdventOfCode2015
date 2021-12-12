using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2015.Utils
{
    public static class Primes
    {
        private static readonly List<int> KnownPrimes = new() {2, 3, 5, 7, 11};

        public static IEnumerable<int> Generate()
        {
            var proposed = 0;
            foreach (var p in KnownPrimes)
            {
                yield return p;
                proposed = p;
            }

            while (true)
            {
                proposed += 2;
                if (KnownPrimes.All(prime => proposed % prime != 0))
                {
                    KnownPrimes.Add(proposed);
                    yield return proposed;
                }
            }
        }

        public static IEnumerable<int> Factorize(int number)
        {
            foreach (var prime in Generate())
            {
                while (prime <= number && number % prime == 0)
                {
                    yield return prime;
                    number /= prime;
                }
                if (prime > number) break;
            }
        }
    }
}