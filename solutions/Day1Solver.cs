using System.Collections.Generic;
using System.Linq;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day1Solver : ISolver
    {
        public string SolveFirst(string[] input)
        {
            var elfTotals = new List<int> {0};
            foreach (var s in input)
            {
                if (int.TryParse(s, out var i))
                    elfTotals[^1] += i;
                else
                    elfTotals.Add(0);
            }

            return elfTotals.Max().ToString();
        }

        public string SolveSecond(string[] input)
        {
            var elfTotals = new List<int> {0};
            foreach (var s in input)
            {
                if (int.TryParse(s, out var i))
                    elfTotals[^1] += i;
                else
                    elfTotals.Add(0);
            }

            return elfTotals.OrderByDescending(i => i).Take(3).Sum().ToString();        
        }
    }
}