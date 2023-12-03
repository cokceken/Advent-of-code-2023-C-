using System;
using System.Collections.Generic;
using System.Linq;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day3Solver : ISolver
    {
        public string SolveFirst(string[] input)
        {
            var result = 0;
            for (var i = 0; i < input.Length; i++)
            {
                var currentNumber = string.Empty;
                var successfulNumber = false;
                for (var k = 0; k < input[i].Length; k++)
                {
                    var value = input[i][k];
                    if (char.IsDigit(value))
                    {
                        currentNumber += value;
                        if (IsAdjacentToSymbol(i, k, input))
                            successfulNumber = true;
                    }
                    else
                    {
                        if (successfulNumber)
                        {
                            result += int.Parse(currentNumber);
                        }

                        successfulNumber = false;
                        currentNumber = string.Empty;
                    }
                }

                if (!string.IsNullOrEmpty(currentNumber) && successfulNumber)
                    result += int.Parse(currentNumber);
            }

            return result.ToString();
        }

        public string SolveSecond(string[] input)
        {
            var gears = new Dictionary<int, string>();
            for (var i = 0; i < input.Length; i++)
            {
                var currentGears = new Dictionary<int, int>();
                var currentNumber = string.Empty;
                for (var k = 0; k < input[i].Length; k++)
                {
                    var value = input[i][k];
                    if (char.IsDigit(value))
                    {
                        currentNumber += value;
                        if (IsAdjacentToSymbol(i, k, input))
                        {
                            var symbols = GetAdjacentSymbols(i, k, input);
                            foreach (var symbol in symbols)
                            {
                                if (symbol.Item3 != '*') continue;
                                var key = symbol.Item1 * input.Length + symbol.Item2;
                                if (currentGears.ContainsKey(key))
                                    currentGears[key] += 1;
                                else
                                    currentGears[key] = 1;
                            }
                        }
                    }
                    else
                    {
                        foreach (var key in currentGears.Select(currentGear => currentGear.Key))
                        {
                            if (gears.ContainsKey(key))
                                gears[key] = gears[key] + ";" + currentNumber;
                            else
                                gears[key] = currentNumber;
                        }

                        currentNumber = string.Empty;
                        currentGears = new Dictionary<int, int>();
                    }
                }

                foreach (var key in currentGears.Select(currentGear => currentGear.Key))
                {
                    if (gears.ContainsKey(key))
                        gears[key] = gears[key] + ";" + currentNumber;
                    else
                        gears[key] = currentNumber;
                }
            }

            return gears
                .Select(x => x.Value.Split(";"))
                .Where(x => x.Length == 2)
                .Select(Multiply)
                .Sum().ToString();
        }

        private static int Multiply(IEnumerable<string> numbers)
        {
            return numbers.Select(int.Parse).Aggregate((a, b) => a * b);
        }

        private static bool IsAdjacentToSymbol(int i, int k, string[] map)
        {
            var pointsToCheck = new Tuple<int, int>[]
            {
                new(i - 1, k - 1),
                new(i - 1, k),
                new(i - 1, k + 1),
                new(i, k - 1),
                new(i, k),
                new(i, k + 1),
                new(i + 1, k - 1),
                new(i + 1, k),
                new(i + 1, k + 1)
            };

            return pointsToCheck.Any(v =>
            {
                if (v.Item1 < 0 || v.Item1 >= map.Length || v.Item2 < 0 || v.Item2 >= map[v.Item1].Length)
                    return false;

                return !char.IsDigit(map[v.Item1][v.Item2]) && map[v.Item1][v.Item2] != '.';
            });
        }

        private static Tuple<int, int, char>[] GetAdjacentSymbols(int i, int k, string[] map)
        {
            var pointsToCheck = new Tuple<int, int>[]
            {
                new(i - 1, k - 1),
                new(i - 1, k),
                new(i - 1, k + 1),
                new(i, k - 1),
                new(i, k),
                new(i, k + 1),
                new(i + 1, k - 1),
                new(i + 1, k),
                new(i + 1, k + 1)
            };

            return pointsToCheck.Where(v =>
            {
                if (v.Item1 < 0 || v.Item1 >= map.Length || v.Item2 < 0 || v.Item2 >= map[v.Item1].Length)
                    return false;

                return !char.IsDigit(map[v.Item1][v.Item2]) && map[v.Item1][v.Item2] != '.';
            }).Select(x => new Tuple<int, int, char>(x.Item1, x.Item2, map[x.Item1][x.Item2])).ToArray();
        }
    }
}