using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;
using JetBrains.Annotations;

namespace AdventOfCode2015.Days.Day20
{
    [UsedImplicitly]
    public static class Day20
    {
        [UsedImplicitly]
        public static void Run()
        {
            Console.WriteLine("\n***");
            //Part1();
            Console.WriteLine("***");
            Part2();
        }

        private static void Part1()
        {
            Naive(1).Should().Be(10);
            Naive(2).Should().Be(30);
            Naive(3).Should().Be(40);
            Naive(4).Should().Be(70);
            Naive(5).Should().Be(60);
            Naive(6).Should().Be(120);
            Naive(7).Should().Be(80);
            Naive(8).Should().Be(150);
            Naive(9).Should().Be(130);

            foreach (var i in Enumerable.Range(1, 1000))
            {
                Advanced(i).Should().Be(Naive(i));
            }

            var dt = DateTime.Now;
            foreach (var i in Enumerable.Range(1, 20000))
            {
                Naive(i);
            }
            Console.WriteLine();
            Console.WriteLine((DateTime.Now - dt).TotalSeconds);
            dt = DateTime.Now;
            foreach (var i in Enumerable.Range(1, 20000))
            {
                Advanced(i);
            }
            Console.WriteLine((DateTime.Now - dt).TotalSeconds);

            var houseNumber = 1;
            var targetNumber = 33_100_000;

            while (houseNumber < 2_000_000)
            {
                if (houseNumber % 1000 == 0) Console.WriteLine(houseNumber);
                if (Advanced(houseNumber) >= targetNumber)
                {
                    break;
                }
                houseNumber += 1;
            }

            houseNumber.Should().Be(776_160);
        }

        private static long Naive(long houseNumber)
        {
            return Enumerable.Range(1, (int)houseNumber / 2)
                .Aggregate(0, (accum, current) => houseNumber % current == 0 ? accum + current * 10 : accum) + houseNumber * 10;
        }

        private static Dictionary<long, HashSet<long>> SpecialFactorsCache = new()
        {
            {1, new HashSet<long>{1}}
        };

        private static long Advanced(int houseNumber)
        {
            return SpecialFactors(houseNumber).Sum(it => it) * 10;
        }

        private static HashSet<long> SpecialFactors(int houseNumber)
        {
            if (SpecialFactorsCache.TryGetValue(houseNumber, out var cached)) return cached;
            var specialFactors = new HashSet<long> { 1, houseNumber };
            foreach (var factor in Primes.Factorize(houseNumber).Distinct())
            {
                specialFactors.Add(factor);
                specialFactors.UnionWith(SpecialFactors(houseNumber / factor));
            }

            SpecialFactorsCache[houseNumber] = specialFactors;
            return specialFactors;
        }

        private static long Advanced2(int houseNumber)
        {
            return SpecialFactors(houseNumber).Where(sf => houseNumber <= sf * 50).Sum() * 11;
        }

        private static void Part2()
        {
            var targetNumber = 33_100_000;
            var max = 0L;

            for (var houseNumber = 1; houseNumber < 3_009_090; houseNumber++)
            {
                var x = Advanced2(houseNumber);
                max = Math.Max(x, max);
                if (houseNumber % 1000 == 0) Console.WriteLine($"{houseNumber} {x} {max}");
                if (x >= targetNumber)
                {
                    houseNumber.Should().Be(786240);
                    break;
                }
            }
        }
    }
}