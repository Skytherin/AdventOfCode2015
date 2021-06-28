using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;

namespace AdventOfCode2015.Days.Day14
{
    public static class Day14
    {
        private static Day14Input Parse(string input) => StructuredRx.Parse<Day14Input>(input);
        private static Day14Input[] Input => File.ReadAllText("Days/Day14/Day14Input.txt").SplitIntoLines()
            .Select(Parse)
            .ToArray();

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1(1000, Parse("Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.")).Should()
                .Be(1120);
            Do1(1000, Parse("Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds.")).Should()
                .Be(1056);
            Do1(2503, Input).Should().Be(2640);
        }
        

        private static void Part2()
        {
            Do2(1000,
                    Parse("Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds."),
                    Parse("Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds."))
                .Should().Be(689);
            Do2(2503, Input).Should().Be(1102);
        }
        

        private static int Do1(int seconds, params Day14Input[] lines)
        {
            return lines.Select(line => TravelDistance(line, seconds)).Max();
        }

        private static int Do2(int seconds, params Day14Input[] lines)
        {
            var scores = lines.ToDictionary(it => it.Subject, _ => 0);
            foreach (var second in Enumerable.Range(1, seconds))
            {
                var distances = lines.ToDictionary(line => line.Subject, line => TravelDistance(line, second));
                var maxDistance = distances.Values.Max();
                foreach (var subject in distances.Where(it => it.Value == maxDistance))
                {
                    scores[subject.Key] += 1;
                }
            }

            return scores.Values.Max();
        }

        private static int TravelDistance(Day14Input line, int seconds)
        {
            var totalTime = line.TravelTime + line.RestTime;
            var fullSegments = seconds / totalTime;
            var finalTravelTime = Math.Min(line.TravelTime, seconds % totalTime);

            return line.Velocity * (fullSegments * line.TravelTime + finalTravelTime);
        }
    }

    internal class Day14Input
    {
        [RxFormat(After = "can fly")]
        public string Subject { get; set; } = "";

        [RxFormat(After = "km/s for")]
        public int Velocity { get; set; }

        [RxFormat(After = @"seconds, but then must rest for")]
        public int TravelTime { get; set; }

        [RxFormat(After = @"seconds.")]
        public int RestTime { get; set; }
    }
}