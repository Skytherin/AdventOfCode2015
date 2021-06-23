using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2015.Utils;
using FluentAssertions;

namespace AdventOfCode2015.Days.Day08
{
    public static class Day8
    {
        //private static Day7Input Parse(string input) => StructuredRx.Parse<Day7Input>(input);
        private static string[] Input => File.ReadAllText("Days/Day08/Day8Input.txt").SplitIntoLines().ToArray();

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            var q = "\"";
            var bs = "\\";
            InMemoryLength($"{q}{q}").Should().Be(0);
            InMemoryLength($"{q}abc{q}").Should().Be(3);
            InMemoryLength($"{q}aaa{bs}{q}aaa{q}").Should().Be(7);
            InMemoryLength($"{q}{bs}x27{q}").Should().Be(1);
            InMemoryLength($"{q}{bs}{bs}{q}").Should().Be(1);

            Do1("\"\"").Should().Be(2);
            Do1("\"abc\"").Should().Be(2);
            Do1("\"aaa\\\"aaa\"").Should().Be(3);
            Do1("\"\\x27\"").Should().Be(5);
            Do1(Input[0]).Should().Be(9);
            Do1(Input[1]).Should().Be(2);
            Do1(Input[0], Input[1]).Should().Be(11);
            Do1(Input).Should().Be(1333);
        }
        

        private static void Part2()
        {
            var q = "\"";
            var bs = "\\";
            EncodedLength($"{q}{q}").Should().Be(6);
            EncodedLength($"{q}abc{q}").Should().Be(9);
            EncodedLength($"{q}aaa{bs}{q}aaa{q}").Should().Be(16);
            EncodedLength($"{q}{bs}x27{q}").Should().Be(11);
            EncodedLength($"{q}{bs}{bs}{q}").Should().Be(10);

            Do2(Input).Should().Be(2046);
        }
        

        private static int Do1(params string[] lines)
        {
            return lines
                .Select(l => l.Length - InMemoryLength(l))
                .Sum();
        }

        private static int InMemoryLength(string s)
        {
            s = s.Substring(1, s.Length - 2);
            s = s.Replace("\\\\", "a");
            s = s.Replace("\\\"", "b");
            s = Regex.Replace(s, @"\\x..", "c");
            return s.Length;
        }

        private static int EncodedLength(string s)
        {
            s = s.Replace("\\", "\\\\");
            s = s.Replace("\"", "\\\"");

            return s.Length + 2;
        }

        private static int Do2(params string[] lines)
        {
            return lines
                .Select(l => EncodedLength(l) - l.Length)
                .Sum();
        }
    }
}