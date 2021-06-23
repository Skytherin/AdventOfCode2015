using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;

namespace AdventOfCode2015.Days.Day02
{
    public static class Day2
    {
        private static List<Day2Input> Input => File.ReadAllText("Days/Day02/Day2Input.txt")
            .Split("\n").Select(it => Parse(it.Trim())).ToList();

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1(new[] {Parse("2x3x4")}).Should().Be(58);
            Do1(new[] { Parse("1x1x10") }).Should().Be(43);
            Do1(Input).Should().Be(1606483);
        }

        private static void Part2()
        {
            Do2(new[] {Parse("2x3x4")}).Should().Be(34);
            Do2(new[] { Parse("1x1x10") }).Should().Be(14);
            Do2(Input).Should().Be(3842356);
        }
        

        private static int Do1(IEnumerable<Day2Input> input)
        {
            return input
                .Select(it =>
                {
                    var p = it.D.Pairs().Select(pair => pair.first * pair.second).ToList();
                    return 2 * p.Sum() + p.Min();
                })
                .Sum();
        }

        private static int Do2(IEnumerable<Day2Input> input)
        {
            return input
                .Select(it =>
                {
                    return 2 * it.D.Pairs().Select(pair => pair.first + pair.second).Min() + it.D.Aggregate(1, (a,n) => a * n);
                })
                .Sum();
        }

        private static Day2Input Parse(string s)
        {
            return StructuredRx.Parse<Day2Input>(s);
        }
    }

    public class Day2Input
    {
        [RxFormat(After = "x")]
        public int Length { get; set; }

        [RxFormat(After = "x")]
        public int Width { get; set; }

        public int Height { get; set; }

        public int[] D => new[] {Length, Width, Height};
    }
}