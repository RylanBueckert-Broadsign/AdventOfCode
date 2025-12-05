using Aoc2025.Common;

namespace Aoc2025;

public class Day5 : IAocDay
{
    public void Run(string inputPath)
    {
        var lines = InputHelper.ReadLines(inputPath).ToList();

        var ranges = lines.TakeWhile(l => l != string.Empty).Select(r =>
        {
            var v = r.Split('-');

            return (lower: long.Parse(v[0]), upper: long.Parse(v[1]));
        }).ToList();

        var ids = lines.Skip(ranges.Count + 1).Select(long.Parse).ToList();

        var fresh = ids.Where(id => ranges.Any(r => id >= r.lower && id <= r.upper));

        Console.WriteLine($"Part 1: {fresh.Count()}");

        var sortedRanges = ranges.OrderBy(r => r.lower);

        var mergedRanges = new List<(long lower, long upper)>();
        foreach (var range in sortedRanges)
        {
            if (mergedRanges.Count == 0 || range.lower > mergedRanges[^1].upper + 1)
            {
                mergedRanges.Add(range);
            }
            else
            {
                var last = mergedRanges[^1];
                mergedRanges[^1] = (last.lower, Math.Max(last.upper, range.upper));
            }
        }

        var totalFreshIds = mergedRanges.Sum(r => r.upper - r.lower + 1);
        Console.WriteLine($"Part 2: {totalFreshIds}");
    }
}
