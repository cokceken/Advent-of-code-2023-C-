using System.Diagnostics;
using System.Linq;
using Advent_2023.shared;
using Advent_2023.solutions;
using Xunit;
using Xunit.Abstractions;

namespace Advent_2023
{
    public class Runner
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ISolver[] _solvers;

        public Runner(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _solvers = new ISolver[]
                { new Day1Solver(), new Day2Solver(), new Day3Solver(), 
                    new Day4Solver(), new Day5Solver(), new Day6Solver(),
                    new Day7Solver(), new Day8Solver(), new Day9Solver(),
                    new Day10Solver(), new Day11Solver(),
                };
        }
        

        [Theory]
        [InlineData(1, "142", "281", true)]
        [InlineData(2, "8", "2286", false)]
        [InlineData(3, "4361", "467835", false)]
        [InlineData(4, "13", "30", false)]
        [InlineData(5, "35", "46", false)]
        [InlineData(6, "288", "71503", false)]
        [InlineData(7, "6440", "5905", false)]
        [InlineData(8, "6", "6", true)]
        [InlineData(9, "114", "2", false)]
        [InlineData(10, "22", "4", false)]
        [InlineData(11, "374", "82000210", false)]
        public void Run(int day, string firstAnswer, string secondAnswer, bool isSeparateStepFile)
        {
            var solver = _solvers[day - 1];
            SolveWithLogs(solver, day, 1, firstAnswer, isSeparateStepFile);
            SolveWithLogs(solver, day, 2, secondAnswer, isSeparateStepFile);
        }

        private void SolveWithLogs(ISolver solver, int day, int step, string expectedAnswer,
            bool isSeparateStepFile = false)
        {
            var input = FileHelper.ReadSampleFile(day, isSeparateStepFile ? step : null);
            if (string.IsNullOrEmpty(expectedAnswer) || string.IsNullOrEmpty(input.FirstOrDefault()))
            {
                _testOutputHelper.WriteLine($"Skipping day {day} - step {step}");
                return;
            }

            _testOutputHelper.WriteLine($"Solving day {day} - step {step}");

            var watch = Stopwatch.StartNew();
            var response = step == 1 ? solver.SolveFirst(input) : solver.SolveSecond(input);
            watch.Stop();
            Assert.Equal(expectedAnswer, response);

            _testOutputHelper.WriteLine($"\tTest passed in {watch.ElapsedMilliseconds} ms");

            input = FileHelper.ReadInputFile(day);
            if (string.IsNullOrEmpty(input.FirstOrDefault()))
            {
                _testOutputHelper.WriteLine($"Skipping solution for day {day} - step {step}");
                return;
            }

            watch = Stopwatch.StartNew();
            response = step == 1 ? solver.SolveFirst(input) : solver.SolveSecond(input);
            watch.Stop();
            _testOutputHelper.WriteLine($"\tYour answer: `{response}` in {watch.ElapsedMilliseconds} ms");
        }
    }
}