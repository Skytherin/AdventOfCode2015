using System;
using AdventOfCode2015.Days;
using AdventOfCode2015.Days.Day1;
using AdventOfCode2015.Days.Day2;
using AdventOfCode2015.Days.Day3;
using AdventOfCode2015.Days.Day4;
using AdventOfCode2015.Days.Day5;
using AdventOfCode2015.Days.Day6;
using AdventOfCode2015.Days.Day7;

namespace AdventOfCode2015
{
    class Program
    {
        static void Main(string[] args)
        {
            var regression = true;
            if (regression)
            {
                Console.WriteLine("Day1");
                Day1.Run();
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
            }
            //Console.WriteLine("Day7");
            //Day7.Run();
        }
    }
}
