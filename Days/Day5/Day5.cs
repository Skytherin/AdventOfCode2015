using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;

namespace AdventOfCode2015.Days.Day5
{
    public static class Day5
    {
        private static List<string> Input => File.ReadAllText("Days/Day5/Day5Input.txt").SplitIntoLines();

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1("ugknbfddgicrmopn").Should().Be(1);
            Do1("aaa").Should().Be(1);
            Do1("jchzalrnumimnmhp").Should().Be(0);
            Do1("haegwjzuvuyypxyu").Should().Be(0);
            Do1("dvszwmarrgswjxmb").Should().Be(0);
            Do1(Input.ToArray()).Should().Be(258);
        }

        private static void Part2()
        {
            Do2("qjhvhtzxzqqjkmpb").Should().Be(1);
            Do2("xxyxx").Should().Be(1);
            Do2("uurcxstgmygtbstg").Should().Be(0);
            Do2("ieodomkazucvgmuy").Should().Be(0);
            Do2(Input.ToArray()).Should().Be(53);
        }
        

        private static int Do1(params string[] lines)
        {
            return lines.Count(line => IsNice1(line));
        }

        private static int Do2(params string[] lines)
        {
            return lines.Count(line => IsNice2(line));
        }
        

        private static bool IsNice1(string line)
        {
            var vowelCount = 0;
            var containsDoubled = false;
            var vowels = new[] { 'a', 'e', 'i', 'o', 'u' };
            var illegals = new[] { "ab", "cd", "pq", "xy" };
            foreach (var (c,index) in line.Select((c,index)=>(c,index)))
            {
                if (vowels.Contains(c)) vowelCount += 1;
                if (index > 0)
                {
                    if (c == line[index-1]) containsDoubled = true;
                    if (illegals.Contains($"{line[index-1]}{c}")) return false;
                }
            }

            return vowelCount >= 3 && containsDoubled;
        }

        private static bool IsNice2(string line)
        {
            var pairs = new Dictionary<string, int>();
            var legalPair = false;
            var legalStepped = false;

            foreach (var (c,index) in line.Select((c,index)=>(c,index)))
            {
                if (index > 0)
                {
                    var pair = $"{c}{line[index - 1]}";
                    if (pairs.TryGetValue(pair, out var oldIndex))
                    {
                        if (oldIndex < index - 2)
                            legalPair = true;
                    }
                    else
                    {
                        pairs.Add(pair, index-1);
                    }
                }

                if (index > 1)
                {
                    if (c == line[index - 2])
                    {
                        legalStepped = true;
                    }
                }
            }

            return legalStepped && legalPair;
        }
    }
}