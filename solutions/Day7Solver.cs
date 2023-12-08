using System;
using System.Collections.Generic;
using System.Linq;
using Advent_2023.shared;

namespace Advent_2023.solutions
{
    public class Day7Solver : ISolver
    {
        private static List<(string, int)> ParseInput(IEnumerable<string> input)
        {
            return input.Select(x =>
            {
                var parts = x.Split(" ");
                return (parts[0], int.Parse(parts[1]));
            }).ToList();
        }

        public string SolveFirst(string[] input)
        {
            var lines = ParseInput(input);
            lines.Sort(delegate((string, int) b1, (string, int) b2)
            {
                var h1 = new Hand(b1.Item1);
                var h2 = new Hand(b2.Item1);
                return h1.Compare(h2) ? -1 : 1;
            });

            var result = 0;
            for (var i = 0; i < lines.Count; i++)
            {
                result += lines[i].Item2 * (lines.Count - i);
            }

            return result.ToString();
        }


        public string SolveSecond(string[] input)
        {
            var lines = ParseInput(input);
            lines.Sort(delegate((string, int) b1, (string, int) b2)
            {
                var h1 = new Hand(b1.Item1, true);
                var h2 = new Hand(b2.Item1, true);
                return h1.Compare(h2) ? -1 : 1;
            });

            var result = 0;
            for (var i = 0; i < lines.Count; i++)
            {
                result += lines[i].Item2 * (lines.Count - i);
            }

            return result.ToString();
        }

        private class Hand
        {
            private readonly char[] _possibleCards =
                { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };

            private readonly char[] _jokerCards =
                { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' };

            private readonly string _cards;
            private HandType _type;
            private readonly bool _isJokersActive;

            public Hand(string cards, bool isJokersActive = false)
            {
                _cards = cards;
                _isJokersActive = isJokersActive;
                SetType();
            }
            
            private void SetType()
            {
                var distinctCount = _cards.Distinct().Count();
                var groupByMax = _cards.GroupBy(x => x).Select(x => x.Count()).Max();
                var jokerCount = _isJokersActive ? _cards.Count(x => x == 'J') : 0;

                _type = (distinctCount, groupByMax, jokerCount) switch
                {
                    (1, _, _) => HandType.FiveOfAKind,
                    (2, 4, 0) => HandType.FourOfAKind,
                    (2, 4, _) => HandType.FiveOfAKind,
                    (2, 3, 0) => HandType.FullHouse,
                    (2, 3, _) => HandType.FiveOfAKind,
                    (3, 3, 0) => HandType.ThreeOfAKind,
                    (3, 3, _) => HandType.FourOfAKind,
                    (3, 2, 0) => HandType.TwoPair,
                    (3, 2, 1) => HandType.FullHouse,
                    (3, 2, 2) => HandType.FourOfAKind,
                    (4, _, 0) => HandType.OnePair,
                    (4, _, _) => HandType.ThreeOfAKind,
                    (5, _, 0) => HandType.HighCard,
                    (5, _, 1) => HandType.OnePair,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            public bool Compare(Hand other)
            {
                var possibleCards = _isJokersActive ? _jokerCards : _possibleCards;
                
                if (_type != other._type) return _type < other._type;
                
                for (var i = 0; i < 5; i++)
                {
                    if (_cards[i].Equals(other._cards[i]))
                        continue;
                    foreach (var t in possibleCards)
                    {
                        if (_cards[i].Equals(t))
                            return true;

                        if (other._cards[i].Equals(t))
                            return false;
                    }
                }

                return false;
            }
        }

        private enum HandType
        {
            FiveOfAKind,
            FourOfAKind,
            FullHouse,
            ThreeOfAKind,
            TwoPair,
            OnePair,
            HighCard
        }
    }
}