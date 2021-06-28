using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2015.Utils;
using FluentAssertions;
using Newtonsoft.Json;

namespace AdventOfCode2015.Days.Day12
{
    public static class Day12
    {
        //private static Day9Input Parse(string input) => StructuredRx.Parse<Day9Input>(input);
        private static string Input => File.ReadAllText("Days/Day12/Day12Input.txt");

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1("[1,2,3]").Should().Be(6);
            Do1("{\"a\":2,\"b\":4}").Should().Be(6);
            Do1("[[[3]]]").Should().Be(3);
            Do1("{\"a\":{\"b\":4},\"c\":-1}").Should().Be(3);
            Do1("{\"a\":[-1,1]}").Should().Be(0);
            Do1("[-1,{\"a\":1}]").Should().Be(0);
            Do1("[]").Should().Be(0); 
            Do1("{}").Should().Be(0);
            Do1(Input).Should().Be(156366);
        }
        

        private static void Part2()
        {
            Do2("[1,{\"c\":\"red\",\"b\":2},3]").Should().Be(4);
            Do2(Input).Should().Be(96852);
        }
        

        private static int Do1(string original)
        {
            var json = ReadJson(original, out original);
            original.Should().Be("");
            return Count(json, false);
        }

        private static int Do2(string original)
        {
            var json = ReadJson(original, out original);
            original.Should().Be("");
            return Count(json, true);
        }

        private static int Count(MyJsonObject json, bool ignoreRed)
        {
            if (json.IntValue is { } i) return i;
            if (json.StringValue is { } j) return 0;
            if (json.Array is { } a)
            {
                return a.Sum(it => Count(it, ignoreRed));
            }

            if (json.Object is { } o)
            {
                if (ignoreRed && o.Values.Any(it => it.StringValue == "red")) return 0;
                return o.Values.Sum(it => Count(it, ignoreRed));
            }

            return 0;
        }

        public static MyJsonObject ReadJson(string original, out string remainder)
        {
            original = original.Trim();
            if (original[0] == '[')
            {
                return ReadArray(original.Substring(1), out remainder);
            }
            if (original[0] == '{')
            {
                return ReadObject(original.Substring(1), out remainder);
            }

            if (original[0] == '"')
            {
                return ReadString(original.Substring(1), out remainder);
            }

            if (Regex.IsMatch($"original[0]", @"[-0-9]"))
            {
                return ReadNumber(original, out remainder);
            }

            throw new ApplicationException();
        }

        private static MyJsonObject ReadObject(string substring, out string remainder)
        {
            var result = new MyJsonObject
            {
                Object = new Dictionary<string, MyJsonObject>()
            };

            while (true)
            {
                substring = substring.Trim();
                if (substring[0] == '}')
                {
                    remainder = substring.Substring(1);
                    return result;
                }

                if (substring[0] == ',')
                {
                    substring = substring.Substring(1).Trim();
                }

                if (substring[0] != '"') throw new ApplicationException();
                substring = substring.Substring(1);
                var index = substring.IndexOf('"');
                var key = substring.Substring(0, index);
                substring = substring.Substring(index + 1).Trim();
                if (substring[0] != ':') throw new ApplicationException();
                substring = substring.Substring(1);

                result.Object[key] = ReadJson(substring, out substring);
            }
        }

        private static MyJsonObject ReadArray(string substring, out string remainder)
        {
            var result = new MyJsonObject
            {
                Array = new List<MyJsonObject>()
            };
            while (true)
            {
                substring = substring.Trim();
                if (substring[0] == ']')
                {
                    remainder = substring.Substring(1);
                    return result;
                }

                if (substring[0] == ',')
                {
                    substring = substring.Substring(1).Trim();
                }

                result.Array.Add(ReadJson(substring, out substring));
            }
        }

        private static MyJsonObject ReadString(string substring, out string remainder)
        {
            var index = substring.IndexOf('"');
            remainder = substring.Substring(index + 1);
            return new MyJsonObject
            {
                StringValue = substring.Substring(0, index)
            };
        }

        private static MyJsonObject ReadNumber(string substring, out string remainder)
        {
            var match = Regex.Match(substring, @"^-?\d+");
            remainder = substring.Substring(match.Length);
            return new MyJsonObject
            {
                IntValue = Convert.ToInt32(substring.Substring(0, match.Length))
            };
        }
    }

    


    public class MyJsonObject
    {
        public List<MyJsonObject>? Array { get; set; }
        public Dictionary<string, MyJsonObject>? Object { get; set; }
        public int? IntValue { get; set; }
        public string StringValue { get; set; }
    }
}