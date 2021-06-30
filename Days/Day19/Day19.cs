using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;
using JetBrains.Annotations;

namespace AdventOfCode2015.Days.Day19
{
    [UsedImplicitly]
    public static class Day19
    {
        private static Day19Input Parse(string input) => StructuredRx.Parse<Day19Input>(input);
        private static Day19Input[] Input => File.ReadAllText("Days/Day19/Day19Input.txt").SplitIntoLines()
            .TakeWhile(line => line != string.Empty)
            .Select(Parse)
            .ToArray();

        private static string Molecule => File.ReadAllText("Days/Day19/Day19Input.txt").SplitIntoLines()
            .SkipWhile(line => line != string.Empty)
            .Skip(1)
            .First();

        [UsedImplicitly]
        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1("HOH", Parse("H => HO"), Parse("H => OH"), Parse("O => HH")).Should().Be(4);
            Do1(Molecule, Input).Should().Be(535);
        }
        

        private static void Part2()
        {
            Do2("HOH", @"e => H
e => O
H => HO
H => OH
O => HH".SplitIntoLines().Select(Parse).ToArray()).Should().Be(3);
            Do2("HOHOHO", @"e => H
e => O
H => HO
H => OH
O => HH".SplitIntoLines().Select(Parse).ToArray()).Should().Be(6);
            Console.WriteLine();
            Console.WriteLine(1);
            Do2(Molecule, Input).Should().Be(212);
        }
        

        private static int Do1(string molecule, params Day19Input[] replacements)
        {
            return MakeReplacements(molecule, replacements).Count();
        }

        private static IEnumerable<string> MakeReplacements(string molecule, IEnumerable<Day19Input> replacements)
        {
            var hs = new HashSet<string>();
            foreach (var replacement in replacements)
            {
                var index = molecule.IndexOf(replacement.Source, StringComparison.Ordinal);
                while (index >= 0)
                {
                    var newMolecule = molecule.Substring(0, index) + replacement.Destination +
                                      molecule.Substring(index + replacement.Source.Length);
                    if (!hs.Contains(newMolecule))
                    {
                        hs.Add(newMolecule);
                        yield return newMolecule;
                    }
                    
                    index = molecule.IndexOf(replacement.Source, index + replacement.Source.Length, StringComparison.Ordinal);
                }
            }
        }

        
        private static int Do2(string goal, params Day19Input[] replacements)
        {
            var open = new SortedList<int, List<OpenItem>>(Comparer<int>.Create((x, y) => x.CompareTo(y)))
            {
                {goal.Length, new List<OpenItem>{new() {Steps = 0, Value = goal}}}
            };
            var closed = new HashSet<string>();

            var reversedReplacements = replacements.Select(it => new Day19Input
            {
                Destination = it.Source,
                Source = it.Destination
            }).ToList();

            while (open.Any())
            {
                var fl = open.ElementAt(0).Value;

                var current = fl.Pop();

                if (!fl.Any())
                {
                    open.RemoveAt(0);
                }

                var newTail = current.Tail.ToList();
                newTail.Add(current.Value);

                var sub = MakeReplacements(current.Value, reversedReplacements);
                foreach (var item in sub)
                {
                    if (closed.Contains(item)) continue;
                    closed.Add(item);
                    if (item == "e")
                    {
                        return current.Steps + 1;
                    }

                    var l = item.Length; // LongestSubstring(item, goal);

                    if (!open.ContainsKey(l))
                    {
                        open.Add(l, new List<OpenItem>());
                    }

                    open.TryGetValue(l, out var temp);
                    temp!.Add(new OpenItem
                    {
                        Steps = current.Steps + 1,
                        Value = item,
                        Tail = newTail
                    });
                }
            }

            throw new ApplicationException();
        }

        internal class OpenItem
        {
            internal string Value { get; set; }
            internal int Steps { get; set; }
            internal List<string> Tail { get; set; } = new();
        }
    }

    internal class Day19Input
    {
        [RxFormat(After = "=>")]
        public string Source { get; set; } = "";

        public string Destination { get; set; } = "";
    }
}