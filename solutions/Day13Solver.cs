using System.Collections.Generic;
using System.Linq;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day13Solver : ISolver
    {
        private static string Solve(string[] input, int smudge)
        {
            var inputs = SplitInput(input);
            var result = 0;
            foreach (var s in inputs)
            {
                var verticalMirror = FindVerticalMirror(s, smudge);
                var horizontalMirror = FindHorizontalMirror(s, smudge);

                if (verticalMirror != -1)
                    result += verticalMirror;

                if (horizontalMirror != -1)
                    result += horizontalMirror * 100;
            }

            return result.ToString();
        } 
        
        public string SolveFirst(string[] input)
        {
            return Solve(input, 0);
        }

        public string SolveSecond(string[] input)
        {
            return Solve(input, 1);
        }

        private static int FindVerticalMirror(string[] input, int smudge = 0)
        {
            var possibleMirrors = new Dictionary<int, bool>();
            for (var i = 0; i < input.Length; i++)
            {
                var t = input[i];
                for (var j = 1; j < t.Length; j++)
                {
                    var found = possibleMirrors.TryGetValue(j, out var smudgeAlreadyUsed);
                    if (i > 0 && !found) continue;

                    var (isMirror, smudgeUsed) = IsMirror(t.ToCharArray(), j, smudgeAlreadyUsed ? 0 : smudge);

                    if (isMirror)
                    {
                        if (i == 0)
                            possibleMirrors.TryAdd(j, smudgeUsed);
                        else
                            possibleMirrors[j] = smudgeUsed;
                    }
                    else possibleMirrors.Remove(j);
                }
            }

            var smudgedMirrors = possibleMirrors.Where(x => x.Value).ToArray();
            return smudgedMirrors.Length == 1 ? smudgedMirrors[0].Key : -1;
        }

        private static int FindHorizontalMirror(string[] input, int smudge = 0)
        {
            var possibleMirrors = new Dictionary<int, bool>();
            for (var j = 0; j < input[0].Length; j++)
            {
                for (var i = 1; i < input.Length; i++)
                {
                    var toCheck = input.Select(x => x[j]).ToArray();

                    var found = possibleMirrors.TryGetValue(i, out var smudgeAlreadyUsed);
                    if (j > 0 && !found) continue;

                    var newSmudge = smudgeAlreadyUsed ? 0 : smudge;
                    var (isMirror, smudgeUsed) = IsMirror(toCheck, i, newSmudge);
                    if (isMirror)
                    {
                        if (j == 0)
                            possibleMirrors.TryAdd(i, smudgeUsed);
                        else
                            possibleMirrors[i] = smudgeUsed;
                    }
                    else possibleMirrors.Remove(i);
                }
            }

            var smudgedMirrors = possibleMirrors.Where(x => x.Value).ToArray();
            return smudgedMirrors.Length == 1 ? smudgedMirrors[0].Key : -1;
        }

        private static (bool, bool) IsMirror(char[] input, int mirrorPoint, int smudge = 0)
        {
            var currentMistakes = 0;
            var direction = input.Length / 2 >= mirrorPoint;
            var length = direction ? mirrorPoint : input.Length - mirrorPoint;
            var skip = direction ? 0 : input.Length - length - length;
            for (var i = skip; i < mirrorPoint; i++)
            {
                if (input[i] != input[2 * mirrorPoint - i - 1]) currentMistakes++;

                if (currentMistakes > smudge) return (false, false);
            }

            return (true, currentMistakes == smudge);
        }

        private static string[][] SplitInput(string[] input)
        {
            var result = new List<string[]>();
            var traversal = 0;
            for (var i = 0; i < input.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(input[i])) continue;

                result.Add(input[traversal..i]);
                traversal = i + 1;
                i++;
            }

            result.Add(input[traversal..]);

            return result.ToArray();
        }
    }
}