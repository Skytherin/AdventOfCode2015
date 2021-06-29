using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;
using JetBrains.Annotations;

namespace AdventOfCode2015.Days.Day16
{
    [UsedImplicitly]
    public static class Day16
    {
        private static Day16Input Parse(string input) => StructuredRx.Parse<Day16Input>(input);
        private static Day16Input[] Input => File.ReadAllText("Days/Day16/Day16Input.txt").SplitIntoLines()
            .Select(Parse)
            .ToArray();

        [UsedImplicitly]
        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1(Input).Should().Be(213);
        }
        

        private static void Part2()
        {
            Do2(Input).Should().Be(323);
        }
        

        private static int Do1(params Day16Input[] lines)
        {
            var evidence = @"children: 3
cats: 7
samoyeds: 2
pomeranians: 3
akitas: 0
vizslas: 0
goldfish: 5
trees: 3
cars: 2
perfumes: 1".SplitIntoLines().Select(it => StructuredRx.Parse<Day16Evidence>(it))
                .ToDictionary(it => it.Key, it => it.Value);

            return lines.Where(line => line.Compounds.All(compound => evidence[compound.Key] == compound.Value))
                .Select(line => line.SueNumber)
                .First();
        }

        private static int Do2(params Day16Input[] lines)
        {
            var evidence = @"children: 3
cats: 7
samoyeds: 2
pomeranians: 3
akitas: 0
vizslas: 0
goldfish: 5
trees: 3
cars: 2
perfumes: 1".SplitIntoLines().Select(it => StructuredRx.Parse<Day16Evidence>(it))
                .ToDictionary(it => it.Key, it => it.Value);


            return lines.Where(line =>
                {
                    return line.Compounds.All(compound =>
                    {
                        var evi = evidence[compound.Key];
                        return compound.Key switch
                        {
                            "cats" => evi < compound.Value,
                            "trees" => evi < compound.Value,
                            "pomeranians" => evi > compound.Value,
                            "goldfish" => evi > compound.Value,
                            _ => evi == compound.Value
                        };
                    });
                })
                .Select(line => line.SueNumber)
                .First();
        }
    }

    internal class Day16Evidence
    {
        [RxFormat(After = ":")]
        public string Key { get; set; }
        public int Value { get; set; }
    }

    internal class Day16Input
    {
        [RxFormat(Before = "Sue", After = ":")]
        public int SueNumber { get; set; }

        public Dictionary<string, int> Compounds { get; set; } = default!;
    }
}