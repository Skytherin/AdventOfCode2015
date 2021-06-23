using System;
using System.IO;
using System.Linq;
using FluentAssertions;

namespace AdventOfCode2015.Days.Day01
{
    public static class Day01
    {
        private static string Input = File.ReadAllText("Days/Day01/Day1Input.txt");

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part2()
        {
            Do2(")").Should().Be(1);
            Do2("()())").Should().Be(5);
            Do2(Input).Should().Be(1783);
        }

        private static void Part1()
        {
            Do1("(())").Should().Be(0);
            Do1("()()").Should().Be(0);
            Do1("(((").Should().Be(3);
            Do1("(()(()(").Should().Be(3);
            Do1("))(((((").Should().Be(3);
            Do1("())").Should().Be(-1);
            Do1("))(").Should().Be(-1);
            Do1(")))").Should().Be(-3);
            Do1(")())())").Should().Be(-3);
            Do1(Input).Should().Be(232);
        }

        private static int Do1(string s)
        {
            return s.Count(c => c == '(') - s.Count(c => c == ')');
        }

        private static int Do2(string s)
        {
            var floor = 0;
            foreach (var c in s.Select((it,index)=>(it,index+1)))
            {
                if (c.it == '(') floor += 1;
                if (c.it == ')') floor -= 1;

                if (floor == -1) return c.Item2;
            }

            throw new ApplicationException();
        }
    }
}