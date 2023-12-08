using System.Collections.Generic;
using System.Linq;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day5Solver : ISolver
    {
        private static (Dictionary<string, List<(long, long, long)>>, long[]) ParseInput(IReadOnlyList<string> input)
        {
            var map = new Dictionary<string, List<(long, long, long)>>();
            var seeds = input[0].Split(":")[1].Trim().Split(" ").Select(long.Parse).ToArray();

            var currentKey = string.Empty;
            for (var i = 2; i < input.Count; i++)
            {
                var s = input[i];
                if (string.IsNullOrEmpty(currentKey) && s.Contains("map"))
                {
                    currentKey = input[i].Split(" ")[0];
                    continue;
                }

                if (string.IsNullOrEmpty(s))
                {
                    currentKey = string.Empty;
                    continue;
                }

                var numbers = s.Split(" ").Select(long.Parse).ToArray();
                if (map.ContainsKey(currentKey))
                    map[currentKey].Add((numbers[0], numbers[1], numbers[2]));
                else
                    map.Add(currentKey, new List<(long, long, long)> { (numbers[0], numbers[1], numbers[2]) });
            }

            return (map, seeds);
        }

        public string SolveFirst(string[] input)
        {
            var types = new[] { "seed", "soil", "fertilizer", "water", "light", "temperature", "humidity", "location" };
            var (map, seeds) = ParseInput(input);

            var min = long.MaxValue;
            foreach (var seed in seeds)
            {
                var traversal = seed;
                for (var i = 0; i < types.Length - 1; i++)
                {
                    var key = $"{types[i]}-to-{types[i + 1]}";
                    foreach (var (destination, source, range) in map[key])
                    {
                        if (source > traversal || traversal > source + range) continue;

                        traversal = destination + (traversal - source);
                        break;
                    }
                }

                if (traversal < min) min = traversal;
            }

            return min.ToString();
        }

        public string SolveSecond(string[] input)
        {
            var types = new[] { "seed", "soil", "fertilizer", "water", "light", "temperature", "humidity", "location" };
            var (map, inputSeeds) = ParseInput(input);

            var seeds = new List<Range>();
            for (var i = 0; i < inputSeeds.Length; i += 2)
            {
                var seed = inputSeeds[i];
                var steps = inputSeeds[i + 1];
                seeds.Add(new Range
                {
                    Min = seed,
                    Step = steps
                });
            }

            var index = 0L;
            while (true)
            {
                index++;
                var traversal = index;
                for (var i = types.Length - 1; i > 0; i--)
                {
                    var key = $"{types[i - 1]}-to-{types[i]}";
                    foreach (var (destination, source, range) in map[key])
                    {
                        if (destination > traversal || traversal > destination + range-1) continue;

                        traversal += source - destination;
                        break;
                    }
                }

                if (seeds.Any(x => x.Min <= traversal && x.Min + x.Step -1 >= traversal))
                {
                    return index.ToString();
                }
            }
        }

        private class Range
        {
            public long Min;
            public long Step;
        }
    }
}