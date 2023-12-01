using System.IO;

namespace Advent_2023.shared
{
    public static class FileHelper
    {
        public static string[] ReadInputFile(int day)
        {
            return File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(),
                $"../../../inputs/day-{day}/input.txt"));
        }

        public static string[] ReadSampleFile(int day, int? step = null)
        {
            var filename = step.HasValue ? "sample-" + step : "sample";
            return File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(),
                $"../../../inputs/day-{day}/{filename}.txt"));
        }
    }
}