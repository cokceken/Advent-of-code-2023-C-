using System.Collections.Generic;
using System.Linq;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day6Solver : ISolver
    {
        private static (long[], long[]) ParseInput(IReadOnlyList<string> input)
        {
            var times = input[0].Split(":")[1]
                .Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(x => long.Parse(x.Trim())).ToArray();

            var distances = input[1].Split(":")[1]
                .Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(x => long.Parse(x.Trim())).ToArray();
            return (times, distances);
        }

        private (long, long) ParseInput2(IReadOnlyList<string> input)
        {
            var time = long.Parse(input[0].Split(":")[1].Replace(" ", ""));
            var distance = long.Parse(input[1].Split(":")[1].Replace(" ", ""));
            return (time, distance);
        }

        public string SolveFirst(string[] input)
        {
            var (times, distances) = ParseInput(input);
            var result = 1L;
            for (var i = 0L; i < times.Length; i++)
            {
                var distance = distances[i];
                var time = times[i];
                var waysToWin = 0;
                for (var j = 0L; j < time; j++)
                {
                    if ((j) * (times[i] - j) > distance)
                        waysToWin++;
                }

                result *= waysToWin;
            }

            return result.ToString();
        }

        public string SolveSecond(string[] input)
        {
            var (time, distance) = ParseInput2(input);
            var waysToWin = 0;
            for (var j = 0L; j < time; j++)
            {
                if ((j) * (time - j) > distance)
                    waysToWin++;
            }

            return waysToWin.ToString();
        }
    }
}