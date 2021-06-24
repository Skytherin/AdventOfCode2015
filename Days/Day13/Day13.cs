using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;

namespace AdventOfCode2015.Days.Day13
{
    public static class Day13
    {
        private static Day13Input Parse(string input) => StructuredRx.Parse<Day13Input>(input);
        private static Day13Input[] Input => File.ReadAllText("Days/Day13/Day13Input.txt").SplitIntoLines()
            .Select(Parse)
            .ToArray();

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1(@"Alice would gain 54 happiness units by sitting next to Bob.
Alice would lose 79 happiness units by sitting next to Carol.
Alice would lose 2 happiness units by sitting next to David.
Bob would gain 83 happiness units by sitting next to Alice.
Bob would lose 7 happiness units by sitting next to Carol.
Bob would lose 63 happiness units by sitting next to David.
Carol would lose 62 happiness units by sitting next to Alice.
Carol would gain 60 happiness units by sitting next to Bob.
Carol would gain 55 happiness units by sitting next to David.
David would gain 46 happiness units by sitting next to Alice.
David would lose 7 happiness units by sitting next to Bob.
David would gain 41 happiness units by sitting next to Carol.".SplitIntoLines().Select(Parse).ToArray()).Should().Be(330);

            Do1(Input).Should().Be(709);

            var input = Input.ToList();
            input.AddRange(input.Select(it => it.Subject).ToHashSet().Select(other => new Day13Input
            {
                GainOrLose = Day13Enum.Gain,
                Happiness = 0,
                SatNextTo = other,
                Subject = "Me"
            }).ToList());

            input.AddRange(input.Select(it => it.Subject).ToHashSet().Select(other => new Day13Input
            {
                GainOrLose = Day13Enum.Gain,
                Happiness = 0,
                SatNextTo = "Me",
                Subject = other
            }).ToList());

            Do1(input.ToArray()).Should().Be(668);
        }
        

        private static void Part2()
        {
        }
        

        private static int Do1(params Day13Input[] lines)
        {
            return MaximizeHappiness(lines);
        }

        private static int Do2(params Day13Input[] lines)
        {
            return 0;
        }

        private static int MaximizeHappiness(Day13Input[] lines)
        {
            var happinessChanges = lines.ToDictionary(line => (line.Subject, line.SatNextTo),
                line => line.Happiness * (line.GainOrLose == Day13Enum.Gain ? 1 : -1));
            var people = lines.Select(it => it.Subject).ToHashSet();

            return people.Permute().Select(permutation =>
            {
                var happiness = 0;
                foreach (var index in Enumerable.Range(0, permutation.Count))
                {
                    happiness += happinessChanges[(permutation[index], permutation[(index + 1) % permutation.Count])];
                    if (index == 0) happiness += happinessChanges[(permutation[index], permutation[^1])];
                    else happiness += happinessChanges[(permutation[index], permutation[index - 1])];
                }

                return happiness;
            }).Max();
        }
    }

    internal class Day13Input
    {
        [RxFormat(After = "would")]
        public string Subject { get; set; } = "";

        public Day13Enum GainOrLose { get; set; }

        [RxFormat(After = "happiness units by sitting next to")]
        public int Happiness { get; set; }

        [RxFormat(After = @"\.")]
        public string SatNextTo { get; set; } = "";
    }

    internal enum Day13Enum
    {
        Gain,
        Lose
    }
}