using Aoc2024.Common;

namespace Aoc2024;

public class Day1 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath);

        var left = new List<long>();
        var right = new Dictionary<long, long>();

        foreach (var line in input)
        {
            var nums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();
            left.Add(nums[0]);

            if (right.TryGetValue(nums[1], out var timesSeen))
            {
                right[nums[1]] = timesSeen + 1;
            }
            else
            {
                right[nums[1]] = 1;
            }
        }

        var similarity = left.Select(i => i * (right.GetValueOrDefault(i, 0)));

        Console.WriteLine(similarity.Sum());
    }
}
