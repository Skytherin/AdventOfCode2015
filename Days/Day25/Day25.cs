using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AdventOfCode2015.Utils;
using FluentAssertions;
using JetBrains.Annotations;

namespace AdventOfCode2015.Days.Day25
{
    [UsedImplicitly]
    public static class Day25
    {
        [UsedImplicitly]
        public static void Run()
        {
            var row = 3010;
            var col = 3019;

            CodeNumber(4, 2).Should().Be(12);
            CodeNumber(1, 5).Should().Be(15);
            CodeNumber(3, 4).Should().Be(19);

            var codeNumber = CodeNumber(row, col);

            Encode(CodeNumber(1, 1)).Should().Be(20151125);
            Encode(CodeNumber(2, 2)).Should().Be(21629792);
            Encode(CodeNumber(6, 6)).Should().Be(27995004);
            Encode(codeNumber).Should().Be(8997277);
        }

        private static long Encode(long codeNumber)
        {
            var n = 20151125L;
            for (var i = 1L; i < codeNumber; ++i)
            {
                n = (n * 252533) % 33554393;
            }

            return n;
        }

        private static long CodeNumber(int row, int col)
        {
            var n = Triangle(col);

            var n2 = Triangle(row + col - 2) - Triangle(col - 1);

            return n + n2;
        }

        private static long Triangle(long n) => n * (n + 1) / 2;
    }
}