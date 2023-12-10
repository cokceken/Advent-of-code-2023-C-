using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day10Solver : ISolver
    {
        private readonly Dictionary<(char pipe, Direction inFrom), Direction> _pipeDirectionChange = new()
        {
            { ('|', Direction.N), Direction.N },
            { ('|', Direction.S), Direction.S },
            { ('-', Direction.E), Direction.E },
            { ('-', Direction.W), Direction.W },
            { ('L', Direction.S), Direction.E },
            { ('L', Direction.W), Direction.N },
            { ('J', Direction.E), Direction.N },
            { ('J', Direction.S), Direction.W },
            { ('7', Direction.E), Direction.S },
            { ('7', Direction.N), Direction.W },
            { ('F', Direction.N), Direction.E },
            { ('F', Direction.W), Direction.S }
        };

        private Location _startingPosition;

        public string SolveFirst(string[] input)
        {
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[i].Length; j++)
                {
                    if (input[i][j] == 'S')
                    {
                        _startingPosition = new Location
                        {
                            X = j,
                            Y = i,
                            Key = 'S'
                        };
                    }
                }
            }

            var possiblePaths = new[]
            {
                (0, 1),
                (1, 0),
                (0, -1),
                (-1, 0)
            };

            foreach (var path in possiblePaths)
            {
                var location = new Location
                {
                    X = _startingPosition.X + path.Item1,
                    Y = _startingPosition.Y + path.Item2,
                };

                if (!location.IsInMap(input)) continue;

                location.Key = input[location.Y][location.X];
                var direction = location.GetRelativeDirection(_startingPosition);

                if (!_pipeDirectionChange.ContainsKey((location.Key, direction))) continue;

                var solutionPath = Traverse(new[] { _startingPosition, location }, input, direction);
                if (solutionPath != null) return ((solutionPath.Length - 1) / 2).ToString();
            }

            return string.Empty;
        }

        public string SolveSecond(string[] input)
        {
            var map = new string[input.Length * 2];

            for (var i = 0; i < input.Length; i++)
            {
                var line1 = new StringBuilder();
                var line2 = new StringBuilder();
                for (var j = 0; j < input[i].Length; j++)
                {
                    var c = input[i][j];
                    var line1Addition = c switch
                    {
                        'S' => input[i][j + 1] switch
                        {
                            '7' => "S-",
                            'J' => "S-",
                            '-' => "S-",
                            _ => "S."
                        },
                        '-' => "--",
                        'F' => "F-",
                        'L' => "L-",
                        _ => $"{c}."
                    };
                    line1.Append(line1Addition);

                    var line2Addition = c switch
                    {
                        'S' => input[i + 1][j] switch
                        {
                            '|' => "|.",
                            'J' => "|.",
                            'L' => "|.",
                            _ => ".."
                        },
                        '|' => "|.",
                        'F' => "|.",
                        '7' => "|.",
                        _ => ".."
                    };
                    line2.Append(line2Addition);
                }

                map[2 * i] = line1.ToString();
                map[2 * i + 1] = line2.ToString();
            }

            for (var i = 0; i < map.Length; i++)
            {
                for (var j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == 'S')
                    {
                        _startingPosition = new Location
                        {
                            X = j,
                            Y = i,
                            Key = 'S'
                        };
                    }
                }
            }

            var possiblePaths = new[]
            {
                (0, 1),
                (1, 0),
                (0, -1),
                (-1, 0)
            };

            foreach (var path in possiblePaths)
            {
                var location = new Location
                {
                    X = _startingPosition.X + path.Item1,
                    Y = _startingPosition.Y + path.Item2,
                };

                if (!location.IsInMap(map)) continue;

                location.Key = map[location.Y][location.X];
                var direction = location.GetRelativeDirection(_startingPosition);

                if (!_pipeDirectionChange.ContainsKey((location.Key, direction))) continue;

                var solutionPath = Traverse(new[] { _startingPosition, location }, map, direction);
                if (solutionPath == null) continue;

                //clean unnecessary characters
                for (var i = 0; i < map.Length; i++)
                {
                    var sb = new StringBuilder(map[i]);
                    for (var j = 0; j < map[i].Length; j++)
                    {
                        if (solutionPath.Any(x => x.X == j && x.Y == i)) continue;
                        sb[j] = '.';
                    }

                    map[i] = sb.ToString();
                }

                for (var i = 0; i < map.Length; i++)
                {
                    for (var j = 0; j < map[i].Length ; j++)
                    {
                        if (i == 0 || j == 0 || j == map[i].Length - 1 || i == map.Length - 1)
                        {
                            map = MarkNotEnclosed(map, j, i);
                        }
                    }
                }

                //this is nice to visualize in debugger
                // var test = new StringBuilder();
                // foreach (var m in map)
                // {
                //     test.Append(m + "\n");
                // }
                //
                // var b = test.ToString();
                return CountSquares(map).ToString();
            }

            return string.Empty;
        }

        private static string[] MarkNotEnclosed(string[] map, int x, int y)
        {
            var visited = new Dictionary<(int, int), bool>();
            var stack = new Stack<(int, int)>();
            stack.Push((x, y));

            while (stack.TryPop(out var t))
            {
                if (t.Item1 < 0 || t.Item1 >= map[0].Length || t.Item2 < 0 || t.Item2 >= map.Length) continue;
                if (visited.ContainsKey(t)) continue;

                visited[t] = false;

                if (map[t.Item2][t.Item1] != '.') continue;

                visited[t] = true;

                stack.Push((t.Item1 - 1, t.Item2));
                stack.Push((t.Item1 + 1, t.Item2));
                stack.Push((t.Item1, t.Item2 - 1));
                stack.Push((t.Item1, t.Item2 + 1));
            }

            for (var i = 0; i < map.Length; i++)
            {
                var sb = new StringBuilder(map[i]);
                for (var j = 0; j < map[i].Length; j++)
                {
                    if (visited.ContainsKey((j, i)) && visited[(j, i)]) sb[j] = '+';
                }

                map[i] = sb.ToString();
            }

            return map;
        }

        private static int CountSquares(IReadOnlyList<string> map)
        {
            var result = 0;
            for (var i = 0; i < map.Count; i += 2)
            {
                for (var j = 0; j < map[i].Length; j += 2)
                {
                    if (map[i][j] == '.' && map[i + 1][j] == '.' && map[i][j + 1] == '.' &&
                        map[i + 1][j + 1] == '.') result++;
                }
            }

            return result;
        }

        private Location[] Traverse(Location[] currentPath, IReadOnlyList<string> map, Direction currentDirection)
        {
            while (true)
            {
                var last = currentPath.Last();
                switch (last.Key)
                {
                    case '.':
                        return null;
                    case 'S':
                        return currentPath;
                }

                var move = last.GetMove(currentDirection);
                var nextLocation = new Location { X = last.X + move.Item1, Y = last.Y + move.Item2 };
                if (!nextLocation.IsInMap(map)) return null;
                nextLocation.Key = map[nextLocation.Y][nextLocation.X];

                var nextDirection = _pipeDirectionChange[(last.Key, currentDirection)];

                currentPath = currentPath.Append(nextLocation).ToArray();
                currentDirection = nextDirection;
            }
        }

        private enum Direction
        {
            N,
            W,
            S,
            E
        }

        private class Location
        {
            public int X { get; set; }
            public int Y { get; set; }
            public char Key { get; set; }

            public bool IsInMap(IReadOnlyList<string> map)
            {
                return X >= 0 && X < map[0].Length && Y >= 0 && Y < map.Count;
            }

            public Direction GetRelativeDirection(Location other)
            {
                if (other.X > X) return Direction.W;
                if (other.X < X) return Direction.E;
                if (other.Y > Y) return Direction.N;
                if (other.Y < Y) return Direction.S;

                throw new Exception("Could not get relative direction");
            }

            public (int, int) GetMove(Direction direction)
            {
                return direction switch
                {
                    Direction.N => Key switch
                    {
                        '|' => (0, -1),
                        '7' => (-1, 0),
                        'F' => (1, 0),
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    Direction.W => Key switch
                    {
                        '-' => (-1, 0),
                        'F' => (0, 1),
                        'L' => (0, -1),
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    Direction.S => Key switch
                    {
                        '|' => (0, 1),
                        'J' => (-1, 0),
                        'L' => (1, 0),
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    Direction.E => Key switch
                    {
                        '-' => (1, 0),
                        'J' => (0, -1),
                        '7' => (0, 1),
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }
}