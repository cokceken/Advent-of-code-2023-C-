using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day11Solver : ISolver
    {
        public string SolveFirst(string[] input)
        {
            return SolveWithExpansion(input, 2);
        }

        public string SolveSecond(string[] input)
        {
            return SolveWithExpansion(input, 1000000);
        }

        private static string SolveWithExpansion(IReadOnlyList<string> map, long expansion)
        {
            var coordinates = new List<Coordinate>();
            var expandingRows = new List<int>();
            var expandingColumns = new List<int>();

            for (var index = 0; index < map.Count; index++)
            {
                var line = map[index];
                if (line.All(x => x.Equals('.')))
                    expandingRows.Add(index);
            }

            for (var i = 0; i < map[0].Length; i++)
            {
                if (map.All(t => t[i] == '.'))
                    expandingColumns.Add(i);
            }

            for (var i = 0; i < map.Count; i++)
            {
                for (var j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == '#')
                        coordinates.Add(new Coordinate
                        {
                            X = j,
                            Y = i
                        });
                }
            }

            var result = 0L;
            for (var i = 0; i < coordinates.Count; i++)
            {
                for (var j = i + 1; j < coordinates.Count; j++)
                {
                    var a = coordinates[i];
                    var b = coordinates[j];
                    var columnsToJump = expandingColumns.Count(x => x > Math.Min(a.X, b.X) && x < Math.Max(a.X, b.X));
                    var rowsToJump = expandingRows.Count(x => x > Math.Min(a.Y, b.Y) && x < Math.Max(a.Y, b.Y));
                    var normalDistance = Math.Abs(coordinates[i].X - coordinates[j].X) +
                                         Math.Abs(coordinates[i].Y - coordinates[j].Y);
                    result += (normalDistance - rowsToJump - columnsToJump)
                              + (rowsToJump * expansion) +
                              (columnsToJump * expansion);
                }
            }

            return result.ToString();
        }

        private class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}