using System;
using System.Collections.Generic;
using System.Linq;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day16Solver : ISolver
    {
        private static Dictionary<string, bool> _cache = new();
        private static int Solve(Beam initialBeam, IReadOnlyList<string> input)
        {
            _cache = new Dictionary<string, bool>();
            var energizedMap = input.Select(x => x.Select(_ => '.').ToArray()).ToArray();
            energizedMap[initialBeam.Y][initialBeam.X] = '#';

            var beams = new List<Beam>
            {
                initialBeam
            };

            while (!beams.All(x => x.IsDone))
            {
                var beamsToRemove = new List<Beam>();
                var beamsToAdd = new List<Beam>();
                foreach (var beam in beams)
                {
                    var move = beam.Direction switch
                    {
                        Direction.N => (0, -1),
                        Direction.E => (1, 0),
                        Direction.S => (0, 1),
                        Direction.W => (-1, 0),
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    var newX = beam.X + move.Item1;
                    var newY = beam.Y + move.Item2;
                    if (newX < 0 || newY < 0 || newX >= input[0].Length || newY >= input.Count)
                    {
                        beamsToRemove.Add(beam);
                        continue;
                    }

                    energizedMap[newY][newX] = '#';
                    switch (input[newY][newX])
                    {
                        case '|' when beam.Direction is Direction.E or Direction.W:
                        {
                            beamsToAdd.Add(new Beam(newX, newY, Direction.N));
                            beam.Direction = Direction.S;
                            break;
                        }
                        case '-' when beam.Direction is Direction.N or Direction.S:
                        {
                            beamsToAdd.Add(new Beam(newX, newY, Direction.E));

                            beam.Direction = Direction.W;
                            break;
                        }
                        case '/':
                            beam.Direction = beam.Direction switch
                            {
                                Direction.N => Direction.E,
                                Direction.E => Direction.N,
                                Direction.S => Direction.W,
                                Direction.W => Direction.S,
                                _ => throw new ArgumentOutOfRangeException()
                            };
                            break;
                        case '\\':
                            beam.Direction = beam.Direction switch
                            {
                                Direction.N => Direction.W,
                                Direction.E => Direction.S,
                                Direction.S => Direction.E,
                                Direction.W => Direction.N,
                                _ => throw new ArgumentOutOfRangeException()
                            };
                            break;
                    }
                    beam.SetPosition(newX, newY);
                }

                beamsToRemove.AddRange(beams.Where(beam => beam.IsDone));
                beamsToRemove.ForEach(x => beams.Remove(x));
                beamsToAdd.ForEach(x => beams.Add(x));
            }

            return energizedMap.Sum(x => x.Count(y => y == '#'));
        }

        public string SolveFirst(string[] input)
        {
            return Solve(new Beam(0, 0, Direction.E), input).ToString();
        }

        public string SolveSecond(string[] input)
        {
            var beams = new List<Beam>();
            for (var i = 0; i < input.Length; i++)
            {
                beams.Add(new Beam(0, i, Direction.E));
                beams.Add(new Beam(input[0].Length - 1, i, Direction.W));
            }

            for (var i = 0; i < input[0].Length; i++)
            {
                beams.Add(new Beam(i, 0, Direction.S));
                beams.Add(new Beam(i, input.Length - 1, Direction.N));
            }

            return beams.Max(x => Solve(x, input)).ToString();
        }

        private class Beam
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public Direction Direction { get; set; }

            public void SetPosition(int x, int y)
            {
                X = x;
                Y = y;
                var key = $"{X}-{Y}-{Direction}";
                if (!_cache.TryAdd(key, true))
                {
                    IsDone = true;
                }
            }

            public Beam(int x, int y, Direction direction)
            {
                Direction = direction;
                SetPosition(x, y);
            }

            public bool IsDone { get; private set; }
        }

        private enum Direction
        {
            N,
            E,
            S,
            W
        }
    }
}