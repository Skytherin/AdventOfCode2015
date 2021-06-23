using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;

namespace AdventOfCode2015.Days.Day03
{
    public static class Day3
    {
        private static string Input => File.ReadAllText("Days/Day03/Day3Input.txt").Trim();

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1(">").Should().Be(2);
            Do1("^>v<").Should().Be(4);
            Do1("^v^v^v^v^v").Should().Be(2);
            Do1(Input).Should().Be(2081);
        }

        private static void Part2()
        {
            Do2("^v").Should().Be(3);
            Do2("^>v<").Should().Be(3);
            Do2("^v^v^v^v^v").Should().Be(11);
            Do2(Input).Should().Be(2341);
        }
        

        private static int Do1(string input)
        {
            return Walk(input).Count;
        }

        private static int Do2(string input)
        {
            var split = input.Select((it,index)=>(it,index)).GroupBy(it => it.index % 2, it => it.it).ToDictionary(it => it.Key, it => it.Join(""));
            var p1 = Walk(split[0]);
            if (split.ContainsKey(1))
            {
                var p2 = Walk(split[1]);
                p1.UnionWith(p2);
            }
            
            return p1.Count;
        }

        private static HashSet<Position> Walk(string input)
        {
            var position = Position.Zero;
            var hs = new HashSet<Position> {position};
            foreach (var c in input)
            {
                if (c == '<') position += Vector.West;
                if (c == '>') position += Vector.East;
                if (c == '^') position += Vector.North;
                if (c == 'v') position += Vector.South;
                hs.Add(position);
            }

            return hs;
        }
    }
}