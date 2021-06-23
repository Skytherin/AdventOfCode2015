using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;

namespace AdventOfCode2015.Days.Day09
{
    public static class Day9
    {
        private static Day9Input Parse(string input) => StructuredRx.Parse<Day9Input>(input);
        private static Day9Input[] Input => File.ReadAllText("Days/Day09/Day9Input.txt").SplitIntoLines()
            .Select(Parse)
            .ToArray();

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1(Parse("London to Dublin = 464"),
                    Parse("London to Belfast = 518"),
                    Parse("Dublin to Belfast = 141"))
                .Should().Be(605);
            Do1(Input).Should().Be(141);
        }
        

        private static void Part2()
        {
            Do2(Parse("London to Dublin = 464"),
                    Parse("London to Belfast = 518"),
                    Parse("Dublin to Belfast = 141"))
                .Should().Be(982);
            Do2(Input).Should().Be(736);
        }
        

        private static int Do1(params Day9Input[] lines)
        {
            var mappedDistances = MappedDistances(lines);

            return mappedDistances.Min();
        }

        private static int Do2(params Day9Input[] lines)
        {
            var mappedDistances = MappedDistances(lines);

            return mappedDistances.Max();
        }

        private static IEnumerable<int> MappedDistances(Day9Input[] lines)
        {
            var distances = new Dictionary<(string, string), int>();
            var places = new HashSet<string>();
            foreach (var line in lines)
            {
                places.Add(line.Origin);
                places.Add(line.Destination);
                distances.Add((line.Origin, line.Destination), line.Distance);
                distances.Add((line.Destination, line.Origin), line.Distance);
            }

            var mappedDistances = places.Permute()
                .Select(permutation => permutation.Skip(1).Aggregate(new {Distance = 0, Current = permutation[0]},
                    (accum, destination) => new
                        {Distance = accum.Distance + distances[(accum.Current, destination)], Current = destination}))
                .Select(it => it.Distance);
            return mappedDistances;
        }
    }

    internal class Day9Input
    {
        public string Origin { get; set; } = "";

        [RxFormat(Before = "to")]
        public string Destination { get; set; } = "";

        [RxFormat(Before = "=")]
        public int Distance { get; set; }
    }
}