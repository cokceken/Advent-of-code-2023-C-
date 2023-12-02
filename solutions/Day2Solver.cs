using System.Collections.Generic;
using System.Linq;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day2Solver : ISolver
    {
        public string SolveFirst(string[] input)
        {
            var limits = new Dictionary<string, int>()
            {
                { "red", 12 },
                { "green", 13 },
                { "blue", 14 }
            };

            var response = 0;
            foreach (var s in input)
            {
                var isPossible = true;
                var parts = s.Split(":");
                var gameNumber = int.Parse(parts[0].Split(" ")[1]);
                var sets = parts[1].Split(";");
                foreach (var set in sets)
                {
                    var cubes = set.Split(",").Select(x => x.Trim());
                    var currentCubes = new Dictionary<string, int>();
                    foreach (var cube in cubes)
                    {
                        var number = int.Parse(cube.Split(" ")[0]);
                        var name = cube.Split(" ")[1];
                        if (currentCubes.ContainsKey(name))
                            currentCubes[name] += number;
                        else
                            currentCubes[name] = number;
                    }

                    if (currentCubes.Any(x => x.Value > limits[x.Key]))
                    {
                        isPossible = false;
                        break;
                    }
                }

                if (isPossible)
                    response += gameNumber;
            }

            return response.ToString();
        }

        public string SolveSecond(string[] input)
        {
            var response = 0;
            foreach (var s in input)
            {
                var requiredAmount = new Dictionary<string, int>
                {
                    { "red", 0 },
                    { "green", 0 },
                    { "blue", 0 }
                };
                
                var parts = s.Split(":");
                var gameNumber = int.Parse(parts[0].Split(" ")[1]);
                var sets = parts[1].Split(";");
                foreach (var set in sets)
                {
                    var cubes = set.Split(",").Select(x => x.Trim());
                    var currentCubes = new Dictionary<string, int>();
                    foreach (var cube in cubes)
                    {
                        var number = int.Parse(cube.Split(" ")[0]);
                        var name = cube.Split(" ")[1];
                        if (currentCubes.ContainsKey(name))
                            currentCubes[name] += number;
                        else
                            currentCubes[name] = number;
                    }

                    foreach (var cube in currentCubes)
                    {
                        if (requiredAmount[cube.Key] < cube.Value)
                            requiredAmount[cube.Key] = cube.Value;
                    }
                }
                
                response += requiredAmount["red"] * requiredAmount["green"] * requiredAmount["blue"];
            }

            return response.ToString();
        }
    }
}