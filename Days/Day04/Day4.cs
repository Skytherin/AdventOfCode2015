using System;
using System.Security.Cryptography;
using FluentAssertions;

namespace AdventOfCode2015.Days.Day04
{
    public static class Day4
    {
        private static string Input => "ckczppom";

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1("abcdef").Should().Be(609043);
            Do1("pqrstuv").Should().Be(1048970);
            Do1(Input).Should().Be(117946);
        }

        private static void Part2()
        {
            Do1(Input, true).Should().Be(3938038);
        }
        

        private static int Do1(string prefix, bool sixZeroes = false)
        {
            using var md5 = MD5.Create();
            for(var i = 0; i < int.MaxValue; ++i)
            {
                var hash = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes($"{prefix}{i}"));
                if (hash[0] == 0 && hash[1] == 0 && (sixZeroes ? hash[2] == 0 : hash[2] < 0x10))
                {
                    return i;
                }
            }
            throw new ApplicationException();
        }

        private static int Do2(string prefix)
        {
            return 0;
        }
    }
}