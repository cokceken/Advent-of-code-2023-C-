using System.Collections.Generic;
using System.Linq;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day8Solver : ISolver
    {
        public string SolveFirst(string[] input)
        {
            var commands = input[0];

            var paths = input[2..]
                .ToDictionary(
                    s => s.Split("=")[0].Trim(),
                    s => s.Split("=")[1].Trim()
                        .Replace("(", "")
                        .Replace(")", "")
                        .Split(",")
                        .Select(x => x.Trim())
                        .ToArray()
                );

            var steps = 0;
            const string goal = "ZZZ";
            const string startingPoint = "AAA";
            var nextPoints = paths[startingPoint];
            while (true)
            {
                var nextIndex = commands[steps % commands.Length].Equals('L') ? 0 : 1;
                steps++;
                if (nextPoints[nextIndex].Equals(goal)) return steps.ToString();
                nextPoints = paths[nextPoints[nextIndex]];
            }
        }

        public string SolveSecond(string[] input)
        {
            var commands = input[0];

            var paths = input[2..]
                .ToDictionary(
                    s => s.Split("=")[0].Trim(),
                    s => s.Split("=")[1].Trim()
                        .Replace("(", "")
                        .Replace(")", "")
                        .Split(",")
                        .Select(x => x.Trim())
                        .ToArray()
                );

            var steps = 0;
            var startingPoints = paths.Where(x => x.Key.EndsWith("A")).ToArray();
            var nextPoints = startingPoints.Select(x => paths[x.Key]).ToArray();
            var circleTimes = new Dictionary<int, int>();
            while (true)
            {
                var nextIndex = commands[steps % commands.Length].Equals('L') ? 0 : 1;
                steps++;
                if (nextPoints.All(x => x[nextIndex].EndsWith("Z"))) return steps.ToString();

                for (var index = 0; index < nextPoints.Length; index++)
                {
                    var nextPoint = nextPoints[index];
                    if (nextPoint[nextIndex].EndsWith("Z"))
                    {
                        circleTimes.TryAdd(index, steps);
                    }
                }

                if (circleTimes.Count == startingPoints.Length)
                {
                    return FindLeastCommonMultiplier(circleTimes.Select(x => x.Value).ToArray()).ToString();
                }

                for (var i = 0; i < nextPoints.Length; i++)
                {
                    nextPoints[i] = paths[nextPoints[i][nextIndex]];
                }
            }
        }

        private static long FindLeastCommonMultiplier(IReadOnlyList<int> arr)
        {
            long result = arr[0];
            for (var i = 1; i < arr.Count; i++)
            {
                result = LeastCommonMultiplier(result, arr[i]);
            }

            return result;
        }

        private static long GreatestCommonFactor(long a, long b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        private static long LeastCommonMultiplier(long a, long b)
        {
            return (a / GreatestCommonFactor(a, b)) * b;
        }
    }
}