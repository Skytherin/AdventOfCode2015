using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;
using JetBrains.Annotations;

namespace AdventOfCode2015.Days.Day23
{
    [UsedImplicitly]
    public static class Day23
    {
        private static List<string> Input => File.ReadAllText("Days/Day23/Day23Input.txt").SplitIntoLines();

        [UsedImplicitly]
        public static void Run()
        {
            Console.WriteLine();
            Part1();
            Console.WriteLine();
            Part2();
        }

        private static void Part1()
        {
            var mc = RunMachine(Input);
            mc.B.Should().Be(255);
        }

        private static void Part2()
        {
            var mc = RunMachine(Input, 1);
            mc.B.Should().Be(334);
        }

        private static MachineState RunMachine(List<string> input, int a = 0)
        {
            var machineState = new MachineState(a, 0, 0);
            while (machineState.PC < input.Count)
            {
                var loc = input[machineState.PC];
                var instruction = loc.Split(" ").First();
                var arg0 = loc.Replace(",", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).FirstOrDefault() ?? "";
                var arg1 = loc.Replace(",", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(2).FirstOrDefault() ?? "";
                if (instruction == "jio")
                {
                    if (arg0 == "a" && (machineState.A == 1))
                    {
                        machineState = machineState with { PC = machineState.PC + Convert.ToInt32(arg1) };
                        continue;
                    }
                    if (arg0 == "b" && (machineState.B == 1))
                    {
                        machineState = machineState with { PC = machineState.PC + Convert.ToInt32(arg1) };
                        continue;
                    }
                }
                if (instruction == "jmp")
                {
                    machineState = machineState with { PC = machineState.PC + Convert.ToInt32(arg0) };
                    continue;
                }
                if (instruction == "jie")
                {
                    if (arg0 == "a" && (machineState.A % 2 == 0))
                    {
                        machineState = machineState with { PC = machineState.PC + Convert.ToInt32(arg1) };
                        continue;
                    }
                    if (arg0 == "b" && (machineState.B % 2 == 0))
                    {
                        machineState = machineState with { PC = machineState.PC + Convert.ToInt32(arg1) };
                        continue;
                    }
                }

                machineState = machineState with { PC = machineState.PC + 1 };

                if (instruction == "inc")
                {
                    var register = arg0;
                    if (register == "a") machineState = machineState with { A = machineState.A + 1 };
                    else machineState = machineState with { B = machineState.B + 1 };
                }
                else if (instruction == "hlf")
                {
                    var register = arg0;
                    if (register == "a") machineState = machineState with { A = machineState.A / 2 };
                    else machineState = machineState with { B = machineState.B / 2 };
                }
                else if (instruction == "tpl")
                {
                    var register = arg0;
                    if (register == "a") machineState = machineState with { A = machineState.A * 3 };
                    else machineState = machineState with { B = machineState.B * 3 };
                }
            }

            return machineState;
        }
    }

    public record MachineState(int A, int B, int PC);
}