//using System;
//using System.IO;
//using System.Linq;
//using AdventOfCode2015.Utils;
//using FluentAssertions;
//using JetBrains.Annotations;

//namespace AdventOfCode2015.Days.Day17
//{
//    [UsedImplicitly]
//    public static class Day17
//    {
//        //private static int Parse(string input) => StructuredRx.Parse<int>(input);
//        private static int[] Input => File.ReadAllText("Days/Day17/Day17Input.txt").SplitIntoLines()
//            .Select(it => Convert.ToInt32(it))
//            .ToArray();

//        [UsedImplicitly]
//        public static void Run()
//        {
//            Part1();
//            Part2();
//        }

//        private static void Part1()
//        {
//            Do1(25, 20, 15, 10, 5, 5).Should().Be(4);
//            Do1(150, Input).Should().Be(1304);
//        }
        

//        private static void Part2()
//        {
//            Do2(25, 20, 15, 10, 5, 5).Should().Be(3);
//            Do2(150, Input).Should().Be(18);
//        }
        

//        private static int Do1(int capacity, params int[] lines)
//        {
//            return lines.Subsets().Count(subset => subset.Sum() == capacity);
//        }

//        private static int Do2(int capacity, params int[] lines)
//        {
//            var fits = lines.Subsets().Where(subset => subset.Sum() == capacity).ToList();
//            var minfit = fits.Select(fit => fit.Count).Min();
//            return fits.Count(fit => fit.Count == minfit);
//        }
//    }
//}