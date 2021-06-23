using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;

namespace AdventOfCode2015.Days.Day07
{
    public static class Day7
    {
        private static Day7Input Parse(string input) => StructuredRx.Parse<Day7Input>(input);
        private static Day7Input[] Input => File.ReadAllText("Days/Day07/Day7Input.txt").SplitIntoLines()
            .Select(line => Parse(line)).ToArray();

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1(Parse("123 -> a")).Should().Be(123);
            Do1(Parse("c AND d -> a"), Parse("6 -> c"), Parse("3 -> d")).Should().Be(2);
            Do1(Input).Should().Be(46065);

            var modifiedInput = Input.ToList();
            var setb = modifiedInput.Single(it => it.Output == "b");
            modifiedInput.Remove(setb);
            modifiedInput.Add(Parse("46065 -> b"));

            Do1(modifiedInput.ToArray()).Should().Be(14134);
        }
        

        private static void Part2()
        {
        }
        

        private static int Do1(params Day7Input[] instructions)
        {
            var gates = new Dictionary<string, int>();
            var remainingInstructions = instructions.ToList();
            while (remainingInstructions.Any())
            {
                foreach (var i in remainingInstructions.ToList())
                {
                    var result = i.Process(gates);
                    if (result)
                    {
                        remainingInstructions.Remove(i);
                    }
                }
            }

            return gates["a"];
        }

        private static int Do2(params Day7Input[] instructions)
        {
            return 0;
        }
    }

    public class Day7Input
    {
        [RxAlternate]
        public LogicGate? LogicGate { get; set; }

        [RxAlternate]
        public Input? PlainLhs { get; set; }

        [RxAlternate]
        public NotGate? NotGate { get; set; }

        [RxFormat(Before = "->")]
        public string Output { get; set; } = default!;

        public bool Process(Dictionary<string, int> gates)
        {
            if (!gates.ContainsKey(Output))
            {
                if (LogicGate is { } lg)
                {
                    return lg.Process(gates, Output);
                }

                if (NotGate is { } ng)
                {
                    return ng.Process(gates, Output);
                }

                var value = PlainLhs?.GetValue(gates);
                if (value is { } v)
                {
                    gates[Output] = v;
                    return true;
                }

                return false;
            }

            return true;
        }
    }

    public class Input
    {
        [RxAlternate]
        public string? Gate { get; set; }

        [RxAlternate]
        public int? Literal { get; set; }

        public int? GetValue(Dictionary<string, int> gates)
        {
            if (Literal is { } v) return v;
            if (Gate is { } g && gates.TryGetValue(g, out var gv))
            {
                return gv;
            }

            return null;
        }
    }

    public class NotGate
    {
        [RxFormat(Before = "NOT")]
        public string Input { get; set; }

        public bool Process(Dictionary<string, int> gates, string output)
        {
            if (gates.TryGetValue(Input, out var input))
            {
                gates[output] = ~input;
                return true;
            }

            return false;
        }
    }

    public class LogicGate
    {
        public Input Input1 { get; set; } = default!;

        public LogicGateEnum Command { get; set; }

        public Input Input2 { get; set; } = default!;

        public bool Process(Dictionary<string, int> gates, string output)
        {
            var value1 = Input1.GetValue(gates);
            var value2 = Input2.GetValue(gates);
            if (value1 is {} v1 && value2 is {} v2)
            {
                switch (Command)
                {
                    case LogicGateEnum.And:
                        gates[output] = v1 & v2;
                        break;
                    case LogicGateEnum.Or:
                        gates[output] = v1 | v2;
                        break;
                    case LogicGateEnum.ShiftLeft:
                        gates[output] = v1 << v2;
                        break;
                    case LogicGateEnum.ShiftRight:
                        gates[output] = v1 >> v2;
                        break;
                    default:
                        throw new ApplicationException();
                }

                return true;
            }

            return false;
        }
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