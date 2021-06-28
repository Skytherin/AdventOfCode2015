using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;

namespace AdventOfCode2015.Days.Day15
{
    public static class Day15
    {
        private static Day15Input Parse(string input) => StructuredRx.Parse<Day15Input>(input);
        private static Day15Input[] Input => File.ReadAllText("Days/Day15/Day15Input.txt").SplitIntoLines()
            .Select(Parse)
            .ToArray();

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1(Parse("Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8"),
                    Parse("Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3"))
                .Should().Be(62842880);
            Do1(Input).Should().Be(18965440);
        }
        

        private static void Part2()
        {
            Do2(Parse("Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8"),
                    Parse("Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3"))
                .Should().Be(57600000);
            Do2(Input).Should().Be(15862900);
        }
        

        private static int Do1(params Day15Input[] lines)
        {
            return CountingPermute(100, lines.Length)
                .Select(permutation =>
                {
                    var capacity = 0;
                    var durability = 0;
                    var flavor = 0;
                    var texture = 0;

                    foreach (var (count, item) in permutation.Zip(lines))
                    {
                        capacity += count * item.Capacity;
                        durability += count * item.Durability;
                        flavor += count * item.Flavor;
                        texture += count * item.Texture;
                    }

                    return Math.Max(0, capacity) * Math.Max(0, durability) * Math.Max(0, flavor) * Math.Max(0, texture);
                })
                .Max();
        }

        private static int Do2(params Day15Input[] lines)
        {
            return CountingPermute(100, lines.Length)
                .Select(permutation =>
                {
                    var capacity = 0;
                    var durability = 0;
                    var flavor = 0;
                    var texture = 0;
                    var calories = 0;

                    foreach (var (count, item) in permutation.Zip(lines))
                    {
                        capacity += count * item.Capacity;
                        durability += count * item.Durability;
                        flavor += count * item.Flavor;
                        texture += count * item.Texture;
                        calories += count * item.Calories;
                    }

                    if (calories == 500)
                    {
                        return Math.Max(0, capacity) * Math.Max(0, durability) * Math.Max(0, flavor) * Math.Max(0, texture);
                    }

                    return 0;
                })
                .Max();
        }

        private static IEnumerable<List<int>> CountingPermute(int max, int numberOfElements)
        {
            if (numberOfElements == 0)
            {
                yield return new List<int>();
                yield break;
            }

            if (numberOfElements == 1)
            {
                yield return new List<int> {max};
                yield break;
            }

            max.Should().BeGreaterThan(0);
            foreach (var current in Enumerable.Range(1, max - numberOfElements + 1))
            {
                var permutations = CountingPermute(max - current, numberOfElements - 1);
                foreach (var permutation in permutations)
                {
                    permutation.Insert(0, current);
                    yield return permutation;
                }
            }
        }
    }

    internal class Day15Input
    {
        [RxFormat(After = ":")]
        public string Ingredient { get; set; } = "";

        [RxFormat(Before = "capacity", After = ",")]
        public int Capacity { get; set; }

        [RxFormat(Before = "durability", After = ",")]
        public int Durability { get; set; }

        [RxFormat(Before = "flavor", After = ",")]
        public int Flavor { get; set; }

        [RxFormat(Before = "texture", After = ",")]
        public int Texture { get; set; }

        [RxFormat(Before = "calories")]
        public int Calories { get; set; }
    }
}