using System;
using System.Collections.Generic;
using System.Linq;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day12Solver : ISolver
    {
        public string SolveFirst(string[] input)
        {
            var lines = (from s in input
                let line = s.Split(" ")[0]
                let conditions = s.Split(" ")[1].Split(",").Select(int.Parse).ToArray()
                select (line, conditions)).ToList();

            var cache = new Dictionary<string, long>();
            var result = lines.Sum(line => WaysToSolve(line.Item1.ToArray(), line.Item2, cache));

            return result.ToString();
        }

        public string SolveSecond(string[] input)
        {
            var lines = new List<(string, int[])>();
            foreach (var s in input)
            {
                var line = s.Split(" ")[0];
                line = $"{line}?{line}?{line}?{line}?{line}";
                var conditions = s.Split(" ")[1].Split(",").Select(int.Parse).ToArray();
                conditions = Enumerable.Repeat(conditions, 5).SelectMany(g => g).ToArray();
                lines.Add((line, conditions));
            }

            var cache = new Dictionary<string, long>();
            var result = lines.Sum(line => WaysToSolve(line.Item1.ToArray(), line.Item2, cache));

            return result.ToString();
        }

        private static long WaysToSolve(char[] part, int[] conditions, IDictionary<string, long> cache)
        {
            if (!part.Any() && conditions.Any()) return 0;
            if (!conditions.Any())
            {
                return part.Any(x => x == '#') ? 0 : 1;
            }

            var cacheKey = $"{new string(part)}-{string.Join(',', conditions)}";
            if (cache.TryGetValue(cacheKey, out var cached)) return cached;

            var result = 0L;
            var first = conditions[0];
            for (var i = 0; i < part.Length; i++)
            {
                if (part[i] == '.') continue;
                if (i + first > part.Length) break;

                var isFit = true;
                for (var j = 0; j < first; j++)
                {
                    if (part[j + i] != '.') continue;

                    isFit = false;
                    break;
                }

                if (i + first < part.Length && part[i + first] == '#')
                    isFit = false;

                for (var j = 0; j < i; j++)
                {
                    if (part[j] == '#') isFit = false;
                }

                if (!isFit) continue;

                var length = part.Length - first - i - 1;
                if (length <= 0) length = 0;
                if (length > 0)
                {
                    var newPart = new char[length];
                    for (var j = 0; j < length; j++)
                    {
                        newPart[j] = part[j + i + first + 1];
                    }

                    result += WaysToSolve(newPart, conditions[1..], cache);
                }
                else
                {
                    result += WaysToSolve(Array.Empty<char>(), conditions[1..], cache);
                }
            }

            cache.TryAdd(cacheKey, result);

            return result;
        }
    }
}