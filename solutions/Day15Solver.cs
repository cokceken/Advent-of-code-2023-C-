using System.Collections.Generic;
using System.Linq;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day15Solver : ISolver
    {
        public string SolveFirst(string[] input)
        {
            var steps = input[0].Split(",");

            var result = 0L;
            foreach (var step in steps)
            {
                var stepResult = 0;
                foreach (var c in step)
                {
                    stepResult += c;
                    stepResult *= 17;
                    stepResult %= 256;
                }

                result += stepResult;
            }

            return result.ToString();
        }

        public string SolveSecond(string[] input)
        {
            var boxes = new Box[256];
            for (var index = 0; index < boxes.Length; index++)
            {
                boxes[index] = new Box();
            }
            
            var steps = input[0].Split(",");
            foreach (var step in steps)
            {
                var stepResult = 0;
                var label = step.Split("-")[0].Split("=")[0];
                foreach (var c in label)
                {
                    stepResult += c;
                    stepResult *= 17;
                    stepResult %= 256;
                }

                if (step.EndsWith('-'))
                {
                    var box = boxes[stepResult];
                    box.Content = box.Content.Where(x => !x.StartsWith(label)).ToList();
                }
                else
                {
                    var box = boxes[stepResult];
                    var index = box.Content.FindIndex(x => x.StartsWith(label));

                    if (index == -1)
                        box.Content.Add(step);
                    else
                        box.Content[index] = step;
                }
            }

            var result = 0L;
            for (var index = 0; index < boxes.Length; index++)
            {
                var box = boxes[index];
                if (!box.Content.Any()) continue;

                for (var i = 0; i < box.Content.Count; i++)
                {
                    var number = int.Parse(box.Content[i].Split("=")[1]);
                    result += (index+1) * number * (i + 1);
                }
            }

            return result.ToString();
        }

        private class Box
        {
            public List<string> Content = new();
        }
    }
}