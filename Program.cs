using System;
using AdventOfCode2015.Days;
using AdventOfCode2015.Days.Day01;
using AdventOfCode2015.Days.Day02;
using AdventOfCode2015.Days.Day03;
using AdventOfCode2015.Days.Day04;
using AdventOfCode2015.Days.Day05;
using AdventOfCode2015.Days.Day06;
using AdventOfCode2015.Days.Day07;
using AdventOfCode2015.Days.Day08;
using AdventOfCode2015.Days.Day09;
using AdventOfCode2015.Days.Day10;
using AdventOfCode2015.Days.Day11;
using AdventOfCode2015.Days.Day12;

namespace AdventOfCode2015
{
    class Program
    {
        static void Main(string[] args)
        {
            var regression = false;
            if (regression)
            {
                Console.WriteLine("Day01");
                Day01.Run();
                Console.WriteLine("Day2");
                Day2.Run();
                Console.WriteLine("Day3");
                Day3.Run();
                Console.WriteLine("Day4");
                Day4.Run();
                Console.WriteLine("Day5");
                Day5.Run();
                Console.WriteLine("Day6");
                Day6.Run();
                Console.WriteLine("Day7");
                Day7.Run();
                Console.WriteLine("Day8");
                Day8.Run();
                Console.WriteLine("Day9");
                Day9.Run();
                Console.WriteLine("Day10");
                Day10.Run();
                Console.WriteLine("Day11");
                Day11.Run();
            }
            Console.WriteLine("Day12");
            Day12.Run();
        }
    }
}
