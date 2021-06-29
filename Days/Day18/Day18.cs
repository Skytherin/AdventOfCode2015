using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2015.Utils;
using FluentAssertions;
using JetBrains.Annotations;

namespace AdventOfCode2015.Days.Day18
{
    [UsedImplicitly]
    public static class Day18
    {
        private static HashSet<Position> Parse(string input)
        {
            var hs = new HashSet<Position>();
            foreach (var (line, row) in input.SplitIntoLines().WithIndices())
            {
                foreach (var (c, col) in line.WithIndices())
                {
                    if (c == '#')
                    {
                        hs.Add(new Position(col, row));
                    }
                }
            }

            return hs;
        }
        private static HashSet<Position> Input => Parse(File.ReadAllText("Days/Day18/Day18Input.txt"));

        [UsedImplicitly]
        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            Do1(6, Parse(@".#.#.#
...##.
#....#
..#...
#.#..#
####.."), 4).Should().Be(4);
            Do1(100, Input, 100).Should().Be(768);
        }
        

        private static void Part2()
        {
            Do2(6, Parse(@".#.#.#
...##.
#....#
..#...
#.#..#
####.."), 5).Should().Be(17);
            Do2(100, Input, 100).Should().Be(781);
        }
        

        private static int Do1(int gridSize, HashSet<Position> grid, int steps)
        {
            foreach (var _ in Enumerable.Range(0, steps))
            {
                var adjancencies = grid.SelectMany(AdjacentPositions)
                    .Where(it => it.X >= 0 && it.Y >= 0 && it.X < gridSize && it.Y < gridSize)
                    .GroupBy(it => it)
                    .ToDictionary(it => it.Key, it => it.Count());

                var newGrid = new HashSet<Position>();

                foreach (var (position, count) in adjancencies)
                {
                    if (grid.Contains(position))
                    {
                        if (count == 2 || count == 3)
                        {
                            newGrid.Add(position);
                        }
                    }
                    else if (count == 3)
                    {
                        newGrid.Add(position);
                    }
                }

                grid = newGrid;
            }

            return grid.Count;
        }

        private static int Do2(int gridSize, HashSet<Position> grid, int steps)
        {
            grid.Add(new Position(0, 0));
            grid.Add(new Position(0, gridSize - 1));
            grid.Add(new Position(gridSize - 1, 0));
            grid.Add(new Position(gridSize - 1, gridSize - 1));
            foreach (var _ in Enumerable.Range(0, steps))
            {
                var adjancencies = grid.SelectMany(AdjacentPositions)
                    .Where(it => it.X >= 0 && it.Y >= 0 && it.X < gridSize && it.Y < gridSize)
                    .GroupBy(it => it)
                    .ToDictionary(it => it.Key, it => it.Count());

                var newGrid = new HashSet<Position>();

                foreach (var (position, count) in adjancencies)
                {
                    if (grid.Contains(position))
                    {
                        if (count == 2 || count == 3)
                        {
                            newGrid.Add(position);
                        }
                    }
                    else if (count == 3)
                    {
                        newGrid.Add(position);
                    }
                }

                grid = newGrid;

                grid.Add(new Position(0, 0));
                grid.Add(new Position(0, gridSize - 1));
                grid.Add(new Position(gridSize - 1, 0));
                grid.Add(new Position(gridSize - 1, gridSize - 1));
            }

            return grid.Count;
        }

        private static IEnumerable<Position> AdjacentPositions(Position p)
        {
            yield return p + Vector.North;
            yield return p + Vector.North + Vector.East;
            yield return p + Vector.East;
            yield return p + Vector.South + Vector.East;
            yield return p + Vector.South;
            yield return p + Vector.South + Vector.West;
            yield return p + Vector.West;
            yield return p + Vector.North + Vector.West;
        }

        private static void PrintGrid(HashSet<Position> p)
        {
            foreach (var row in Enumerable.Range(0, 6))
            {
                foreach (var col in Enumerable.Range(0, 6))
                {
                    Console.Write(p.Contains(new Position(col, row)) ? "#" : ".");
                }
                Console.WriteLine();
            }
        }
    }
}