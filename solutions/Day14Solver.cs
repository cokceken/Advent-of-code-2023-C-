using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day14Solver : ISolver
    {
        private static char[][] Transpose(IReadOnlyList<char[]> matrix)
        {
            var h = matrix.Count;
            var w = matrix[0].Length;

            var result = new char[h][];
            for (var i = 0; i < h; i++)
            {
                result[i] = new char[w];
            }

            for (var i = 0; i < h; i++)
            {
                for (var j = 0; j < w; j++)
                {
                    result[j][w - i - 1] = matrix[i][j];
                }
            }

            return result;
        }

        private static char[][] MoveMapToNorth(char[][] map)
        {
            for (var i = 0; i < map.Length; i++)
            {
                for (var j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] != 'O') continue;
                    for (var k = i - 1; k >= 0; k--)
                    {
                        if (map[k][j] == '.')
                        {
                            map[k][j] = 'O';
                            map[k + 1][j] = '.';
                        }
                        else
                            break;
                    }
                }
            }

            return map;
        }

        private static string CalculateResult(IReadOnlyList<char[]> map)
        {
            var result = 0;
            for (var i = 0; i < map.Count; i++)
            {
                for (var j = 0; j < map[0].Length; j++)
                {
                    if (map[i][j] == 'O')
                    {
                        result += map.Count - i;
                    }
                }
            }

            return result.ToString();
        }

        private static string GetDictionaryKey(IEnumerable<char[]> map)
        {
            var sb = new StringBuilder();
            foreach (var s in map)
            {
                sb.Append(s);
            }

            return sb.ToString();
        }

        private static char[][] DoCycle(char[][] map)
        {
            for (var j = 0; j < 4; j++)
            {
                map = MoveMapToNorth(map);
                map = Transpose(map);
            }

            return map;
        }

        public string SolveFirst(string[] input)
        {
            var map = input.Select(x => x.ToArray()).ToArray();

            map = MoveMapToNorth(map);

            return CalculateResult(map);
        }

        public string SolveSecond(string[] input)
        {
            var map = input.Select(x => x.ToArray()).ToArray();
            var cache = new Dictionary<string, long>();

            for (long i = 0; i < 1000000000; i++)
            {
                var key = GetDictionaryKey(map);
                if (cache.TryGetValue(key, out var oldCycle))
                {
                    var step = i - oldCycle;
                    var remainingCycles = (1000000000 - oldCycle) % step;

                    for (var k = 0; k < remainingCycles; k++)
                        map = DoCycle(map);

                    return CalculateResult(map);
                }

                cache.Add(key, i);

                map = DoCycle(map);
            }

            return CalculateResult(map);
        }
    }
}