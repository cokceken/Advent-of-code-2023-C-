using System;
using System.Collections.Generic;
using System.Linq;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day4Solver : ISolver
    {
        private (int[], int[]) ParseLine(string s)
        {
            var halves = s.Split("|");
            var winningNumbers = halves[0].Split(":")[1].Split(" ").Select(x =>
            {
                if (int.TryParse(x, out var number))
                    return number;
                return -1;
            }).Where(x => x != -1).ToArray();
            var ourNumbers = halves[1].Trim().Split(" ").Select(x =>
            {
                if (int.TryParse(x, out var number))
                    return number;
                return -1;
            }).Where(x => x != -1).ToArray();
            return (winningNumbers, ourNumbers);
        }
        
        public string SolveFirst(string[] input)
        {
            var result = 0;

            foreach (var s in input)
            {
                var (winningNumbers, ourNumbers) = ParseLine(s);

                var intersect = winningNumbers.Intersect(ourNumbers).ToArray();
                result += intersect.Any() ? intersect.Select(_ => 2).Aggregate((a, b) => a * b) / 2 : 0;
            }

            return result.ToString();
        }

        public string SolveSecond(string[] input)
        {
            var result = 0;
            var countMap = new Dictionary<int, int>();
            for (var index = 1; index <= input.Length; index++)
                countMap[index] = 0;

            for (var index = 1; index <= input.Length; index++)
            {
                countMap[index] += 1;
                var multiplier = countMap[index];
                
                var s = input[index-1];
                var (winningNumbers, ourNumbers) = ParseLine(s);

                var intersectCount = winningNumbers.Intersect(ourNumbers).Count();
                for (var i = 1; i <= intersectCount; i++)
                    countMap[index + i] += multiplier;
            }

            return countMap.Select(x => x.Value).Aggregate((a, b) => a+b).ToString();
        }
    }
}