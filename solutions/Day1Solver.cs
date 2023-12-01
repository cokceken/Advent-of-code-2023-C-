using System.Collections.Generic;
using System.Linq;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day1Solver : ISolver
    {
        public string SolveFirst(string[] input)
        {
            var result = 0;
            foreach (var s in input)
            {
                var calibrationValue = 0;
                for (var i = 0; i < s.Length; i++)
                {
                    if (!int.TryParse(s[i].ToString(), out var number)) continue;
                    
                    calibrationValue = number * 10;
                    break;
                }
                
                for (var i = s.Length-1; i >= 0; i--)
                {
                    if (!int.TryParse(s[i].ToString(), out var number)) continue;
                    
                    calibrationValue += number;
                    break;
                }

                result += calibrationValue;
            }

            return result.ToString();
        }

        public string SolveSecond(string[] input)
        {
            var digits = new Dictionary<string, int>
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
                { "six", 6 },
                { "seven", 7 },
                { "eight", 8 },
                { "nine", 9 }
            };
            
            var result = 0;
            foreach (var s in input)
            {
                var calibrationValue = 0;
                for (var i = 0; i < s.Length; i++)
                {
                    if (int.TryParse(s[i].ToString(), out var number))
                    {
                        calibrationValue = number * 10;
                        break;
                    }

                    var sub = s[i..];
                    var letterDigit = digits.FirstOrDefault(x => sub.StartsWith(x.Key));
                    if (!letterDigit.Equals(default(KeyValuePair<string, int>)))
                    {
                        calibrationValue = letterDigit.Value * 10;
                        break;
                    }
                }
                
                for (var i = s.Length-1; i >= 0; i--)
                {
                    if (int.TryParse(s[i].ToString(), out var number))
                    {
                        calibrationValue += number;
                        break;
                    }
                    
                    var sub = s[..(i+1)];
                    var letterDigit = digits.FirstOrDefault(x => sub.EndsWith(x.Key));
                    if (!letterDigit.Equals(default(KeyValuePair<string, int>)))
                    {
                        calibrationValue += letterDigit.Value;
                        break;
                    }
                }

                result += calibrationValue;
            }

            return result.ToString();
        }
    }
}