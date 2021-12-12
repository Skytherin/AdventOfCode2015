using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;
using JetBrains.Annotations;

namespace AdventOfCode2015.Days.Day24
{
    [UsedImplicitly]
    public static class Day24
    {
        private static List<int> Input => File.ReadAllLines("Days/Day24/Day24Input.txt").Select(it => Convert.ToInt32(it)).ToList();

        private static readonly List<int> Example = new() { 1, 2, 3, 4, 5, 7, 8, 9, 10, 11 };
        // private static readonly List<int> Example = new() { 2, 3, 4, 24, 10, 14 };
        // private static readonly List<int> Example = new() { 1,2,3,4,5 };

        [UsedImplicitly]
        public static void Run()
        {
            //var temp = SubsetsWithTn(new[] { 1, 2, 3, 6 }, 6).ToList();

            Console.WriteLine();
            Part1(Example).Should().Be(99);
            Console.WriteLine(1);
            Part1(Input).Should().Be(11846773891L);
            Console.WriteLine(2);
            Part2(Example).Should().Be(44);
            Console.WriteLine(3);
            Part2(Input).Should().Be(80393059L);
        }

        private static int ChosenLength = int.MaxValue;
        private static long ChosenQE = long.MaxValue;

        private static long Part1(List<int> data)
        {
            ChosenLength = int.MaxValue;
            ChosenQE = long.MaxValue; 
            
            foreach (var group in SplitInto3(data, data.Sum() / 3))
            {
                if (group.Count < ChosenLength)
                {
                    ChosenLength = group.Count;
                    ChosenQE = QE(group);
                }
                else if (group.Count == ChosenLength)
                {
                    var groupQE = QE(group);
                    if (groupQE < ChosenQE)
                    {
                        ChosenQE = groupQE;
                    }
                }
            }
            return ChosenQE;
        }

        private static long Part2(List<int> data)
        {
            ChosenLength = int.MaxValue;
            ChosenQE = long.MaxValue;

            foreach (var group in SplitInto3(data, data.Sum() / 4))
            {
                if (group.Count < ChosenLength)
                {
                    ChosenLength = group.Count;
                    ChosenQE = QE(group);
                }
                else if (group.Count == ChosenLength)
                {
                    var groupQE = QE(group);
                    if (groupQE < ChosenQE)
                    {
                        ChosenQE = groupQE;
                    }
                }
            }
            return ChosenQE;
        }

        public static long QE(List<int> data) => data.Aggregate(1L, ((accum, current) => accum * current));

        public static IEnumerable<List<int>> SplitInto3(List<int> data, int tn)
        {
            foreach (var subset1 in SubsetsWithTn(data, tn))
            {
                yield return subset1;
                //if (SubsetsWithTn(data.Except(subset1), tn).Any()) yield return subset1;
            }
        }

        public static IEnumerable<List<int>> SubsetsWithTn(IEnumerable<int> input, int tn)
        {
            var original = input.ToList();

            if (tn == 0)
            {
                yield return new List<int>();
            }

            foreach (var item in original.WithIndices().Where(it => it.Value <= tn))
            {
                foreach (var list in SubsetsWithTn(original.Skip(item.Index + 1), tn - item.Value))
                {
                    //var nl = list.Append(item.Value).ToList();
                    yield return list.Append(item.Value).ToList();
                }
            }
        }
    }
}