using System;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;

namespace AdventOfCode2015.Days.Day11
{
    public static class Day11
    {
        //private static Day9Input Parse(string input) => StructuredRx.Parse<Day9Input>(input);
        private static string Input => File.ReadAllText("Days/Day11/Day11Input.txt").SplitIntoLines()[0];

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1("abcdefgh").Should().Be("abcdffaa");
            Do1(Input).Should().Be("cqjxxyzz");
            Do1("cqjxxyzz").Should().Be("cqkaabcc");
        }
        

        private static void Part2()
        {
        }
        

        private static string Do1(string original)
        {
            return original
                .Increments('a', 'z', c =>
                {
                    return c switch
                    {
                        'h' => 'j',
                        'n' => 'p',
                        'k' => 'm',
                        _ => (char) (c+1)
                    };
                })
                .Select(it => it.Join())
                .SkipWhile(it =>
                {
                    var containsRunOf3 = false;
                    var numberOfPairs = 0;
                    var lastWasPairMatch = false;
                    foreach (var index in Enumerable.Range(0, it.Length))
                    {
                        if (index >= 2 && it[index - 2] == it[index - 1] - 1 && it[index - 1] == it[index] - 1)
                        {
                            containsRunOf3 = true;
                        }

                        if (!lastWasPairMatch && index >= 1 && it[index] == it[index - 1])
                        {
                            numberOfPairs += 1;
                            lastWasPairMatch = true;
                        }
                        else
                        {
                            lastWasPairMatch = false;
                        }
                    }

                    return !containsRunOf3 || numberOfPairs < 2;
                })
                .First();
        }
    }
}