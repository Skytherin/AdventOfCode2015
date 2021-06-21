using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;

namespace AdventOfCode2015.Days.Day6
{
    public static class Day6
    {
        private static Day6Input[] Input => File.ReadAllText("Days/Day6/Day6Input.txt").SplitIntoLines()
            .Select(line => Parse(line)).ToArray();

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1(Parse("turn on 0,0 through 999,999")).Should().Be(1_000_000);
            Do1(Parse("toggle 0,0 through 999,0")).Should().Be(1000);
            Do1(Parse("turn on 0,0 through 999,999"),
                Parse("turn off 499,499 through 500,500")).Should().Be(1_000_000 - 4);
            Do1(Input).Should().Be(400410);
        }

        private static Day6Input Parse(string input) => StructuredRx.Parse<Day6Input>(input);

        private static void Part2()
        {
            Do2(Parse("turn on 0,0 through 0,0")).Should().Be(1);
            Do2(Parse("toggle 0,0 through 999,999")).Should().Be(2_000_000);
            Do2(Input).Should().Be(15343601);
        }
        

        private static int Do1(params Day6Input[] instructions)
        {
            var lights = new int[1000, 1000];

            var commands = new Dictionary<Day6Enum, Action<int, int>>
            {
                [Day6Enum.TurnOff] = (x, y) => lights[x, y] = 0,
                [Day6Enum.TurnOn] = (x, y) => lights[x, y] = 1,
                [Day6Enum.Toggle] = (x, y) => lights[x, y] = lights[x, y] == 1 ? 0 : 1
            };

            foreach (var instruction in instructions)
            {
                var c = commands[instruction.Instruction];
                foreach (var x in Enumerable.Range(instruction.X1, instruction.X2 - instruction.X1 + 1))
                {
                    foreach (var y in Enumerable.Range(instruction.Y1, instruction.Y2 - instruction.Y1 + 1))
                    {
                        c(x, y);
                    }
                }
            }

            return Enumerable.Range(0, 1000).Sum(x => Enumerable.Range(0, 1000).Sum(y => lights[x, y]));
        }

        private static int Do2(params Day6Input[] instructions)
        {
            var lights = new int[1000, 1000];

            var commands = new Dictionary<Day6Enum, Action<int, int>>
            {
                [Day6Enum.TurnOff] = (x, y) => lights[x, y] = lights[x, y] > 0 ? lights[x, y] - 1 : 0,
                [Day6Enum.TurnOn] = (x, y) => lights[x, y] = lights[x, y] + 1,
                [Day6Enum.Toggle] = (x, y) => lights[x, y] = lights[x, y] += 2
            };

            foreach (var instruction in instructions)
            {
                var c = commands[instruction.Instruction];
                foreach (var x in Enumerable.Range(instruction.X1, instruction.X2 - instruction.X1 + 1))
                {
                    foreach (var y in Enumerable.Range(instruction.Y1, instruction.Y2 - instruction.Y1 + 1))
                    {
                        c(x, y);
                    }
                }
            }

            return Enumerable.Range(0, 1000).Sum(x => Enumerable.Range(0, 1000).Sum(y => lights[x, y]));
        }
    }

    public class Day6Input
    {
        public Day6Enum Instruction { get; set; }

        [RxFormat(After=",")]
        public int X1 { get; set; }
        [RxFormat(After = "through")]
        public int Y1 { get; set; }

        [RxFormat(After = ",")]
        public int X2 { get; set; }
        public int Y2 { get; set; }
    }

    public enum Day6Enum
    {
        [Description("turn on")]
        TurnOn,
        [Description("toggle")]
        Toggle,
        [Description("turn off")]
        TurnOff
    }
}