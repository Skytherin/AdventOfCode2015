using System.ComponentModel;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;

namespace AdventOfCode2015.Days.Day7
{
    public static class Day7
    {
        private static Day7Input Parse(string input) => StructuredRx.Parse<Day7Input>(input);
        private static Day7Input[] Input => File.ReadAllText("Days/Day7/Day7Input.txt").SplitIntoLines()
            .Select(line => Parse(line)).ToArray();

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Parse("a AND b");
        }
        

        private static void Part2()
        {
        }
        

        private static int Do1(params Day7Input[] instructions)
        {
            return 0;
        }

        private static int Do2(params Day7Input[] instructions)
        {
            return 0;
        }
    }

    public class Day7Input
    {
        public LogicGate? LogicGate { get; set; }
        //[RxAlternate]
        //public LogicGate? LogicGate { get; set; }

        //[RxAlternate]
        //public NotGate? NotGate { get; set; }

        //[RxFormat(Before = "->")] 
        //public string Output { get; set; } = default!;
    }

    public class NotGate
    {
        [RxFormat(Before = "NOT")]
        public string Input { get; set; }
    }

    public class LogicGate
    {
        [RxAlternate]
        public string Input1 { get; set; } = default!;

        [RxAlternate]
        public int? Value1 { get; set; } = default!;

        public LogicGateEnum Command { get; set; }

        [RxAlternate]
        public string Input2 { get; set; } = default!;
        [RxAlternate]
        public int? Value2 { get; set; } = default!;
    }

    public enum LogicGateEnum
    {
        [Description("LSHIFT")]
        ShiftLeft,
        [Description("RSHIFT")]
        ShiftRight,
        And,
        Or
    }
}