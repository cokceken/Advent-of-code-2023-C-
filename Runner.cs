using System;
using Advent_2023.shared;
using Advent_2023.solutions;
using Xunit;
using Xunit.Abstractions;

namespace Advent_2023
{
    public class Runner
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Runner(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("142", "281")]
        public void Day1(string firstAnswer, string secondAnswer)
        {
            var solver = new Day1Solver();
            SolveWithLogs(solver, 1, 1, firstAnswer, true);
            SolveWithLogs(solver, 1, 2, secondAnswer, true);
        }

        private void SolveWithLogs(ISolver solver, int day, int step, string expectedAnswer, bool isSeparateStepFile = false)
        {
            _testOutputHelper.WriteLine($"Solving day {day} - step {step}");

            var input = FileHelper.ReadSampleFile(day, isSeparateStepFile ? step : null);
            var response = step == 1 ? solver.SolveFirst(input) : solver.SolveSecond(input);

            Assert.Equal(expectedAnswer, response);

            _testOutputHelper.WriteLine("\tTest passed");

            input = FileHelper.ReadInputFile(day);
            response = step == 1 ? solver.SolveFirst(input) : solver.SolveSecond(input);
            _testOutputHelper.WriteLine($"\tYour answer: `{response}`");
        }
    }
}