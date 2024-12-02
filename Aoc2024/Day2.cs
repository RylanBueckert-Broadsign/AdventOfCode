using Aoc2024.Common;

namespace Aoc2024;

public class Day2 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath);

        var reports = input.Select(r => r.Split(' ').Select(int.Parse));

        var safeReports = reports.Select(r => IsSafe(r) || IsSafe(r.Reverse()));

        Console.WriteLine(safeReports.Count(i => i));
    }

    private IEnumerable<int> GetDiff(IReadOnlyList<int> report)
    {
        return report.Skip(1).Select((level, idx) => level - report[idx]);
    }

    private bool IsSafe(IEnumerable<int> report)
    {
        var reportList = report.ToList();

        var diffReport = GetDiff(reportList);

        var problemIdx = diffReport.ToList().FindIndex(i => i is < 1 or > 3);

        if (problemIdx == -1)
        {
            return true;
        }

        diffReport = GetDiff(reportList.Where((_, idx) => idx != problemIdx).ToList());

        if (!diffReport.Any(i => i is < 1 or > 3))
        {
            return true;
        }

        diffReport = GetDiff(reportList.Where((_, idx) => idx != problemIdx + 1).ToList());

        return !diffReport.Any(i => i is < 1 or > 3);
    }
}
