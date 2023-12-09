using System;
using System.Collections.Generic;
using System.Linq;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day9Solver : ISolver
    {
        public string SolveFirst(string[] input)
        {
            return Solve(input, list => list.Select(x => x.Last()).Aggregate((l, l1) => l + l1));
        }

        public string SolveSecond(string[] input)
        {
            return Solve(input, list =>
            {
                var firsts = list.Select(x => x.First()).ToArray();
                var result = 0L;
                for (var i = firsts.Length-1; i >= 0 ; i--)
                {
                    result = firsts[i] - result;
                }

                return result;
            });
        }

        private static string Solve(IEnumerable<string> input, Func<List<long[]>, long> resultSelector)
        {
            var lines = input.Select(x => x.Split(" ").Select(long.Parse).ToArray());

            var result = 0L;
            foreach (var line in lines)
            {
                var history = new List<long[]> { line };
                do
                {
                    var newHistoryItem = new List<long>();
                    for (var index = 0; index < history.Last().Length - 1; index++)
                    {
                        var t = history.Last()[index];
                        var n = history.Last()[index + 1];
                        newHistoryItem.Add(n - t);
                    }

                    if (newHistoryItem.All(x => x == 0)) break;
                    history.Add(newHistoryItem.ToArray());
                } while (true);

                result += resultSelector.Invoke(history);
            }

            return result.ToString();
        }
    }
}