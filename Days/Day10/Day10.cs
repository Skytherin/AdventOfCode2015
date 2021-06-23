using System;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;

namespace AdventOfCode2015.Days.Day10
{
    public static class Day10
    {
        //private static Day9Input Parse(string input) => StructuredRx.Parse<Day9Input>(input);
        private static string Input => File.ReadAllText("Days/Day10/Day10Input.txt").SplitIntoLines()[0];

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1("1", 1).Should().Be("11");
            Do1("1", 2).Should().Be("21");
            Do1("1", 3).Should().Be("1211");
            Do1("1", 4).Should().Be("111221");
            Do1("1", 5).Should().Be("312211");
            Do1(Input, 40).Length.Should().Be(252594);
            Do1(Input, 50).Length.Should().Be(3579328);
        }
        

        private static void Part2()
        {
        }
        

        private static string Do1(string line, int iterations)
        {
            foreach (var _ in Enumerable.Range(0, iterations))
            {
                line = DoSay(line);
            }
            return line;
        }

        private static string DoSay(string line)
        {
            return line.Runs()
                .Select(run => $"{run.Count}{run[0]}")
                .Join();
        }

        private static int Do2(string line)
        {
            return 0;
        }
    }
}