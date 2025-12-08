using System.Text;
using Aoc2025.Common;

namespace Aoc2025;

public class Day6 : IAocDay
{
    public void Run(string inputPath)
    {
        var lines = InputHelper.ReadLines(inputPath, false).ToList();

        Part1(lines);
        Part2(lines);
    }

    private static void Part1(IEnumerable<string> lines)
    {
        var values = lines
            .Select(line => line
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            .ToList();

        var operations = values[^1];

        var numbers = values.SkipLast(1).Select(l => l.Select(long.Parse).ToList()).ToList();

        var results1 = operations.Select((op, idx) => op == "+" ? numbers.Sum(n => n[idx]) : numbers.Aggregate(1L, (acc, n) => acc * n[idx]));

        Console.WriteLine(results1.Sum());
    }

    private record Problem(string Operation, List<long> Numbers);

    private static void Part2(List<string> lines)
    {
        var currentProblem = new Problem("", new List<long>());
        var problems = new List<Problem>();

        for (var col = 0; col < lines[0].Length; col++)
        {
            var newOp = lines[^1][col];
            if (newOp != ' ')
            {
                problems.Add(currentProblem);
                currentProblem = new Problem(newOp.ToString(), new List<long>());
            }

            var sb = new StringBuilder();
            for (var row = 0; row < lines.Count - 1; row++)
            {
                var c = lines[row][col];
                if (c != ' ')
                    sb.Append(c);
            }

            if (sb.Length > 0)
                currentProblem.Numbers.Add(long.Parse(sb.ToString()));
        }

        problems.Add(currentProblem);

        var results2 = problems
            .Skip(1)
            .Select(p => p.Operation == "+" ? p.Numbers.Sum() : p.Numbers.Aggregate(1L, (acc, n) => acc * n));

        Console.WriteLine(results2.Sum());

        // Part 2
        // var numbersStr = values.SkipLast(1).ToList();
        //
        // var problems = Enumerable
        //     .Range(0, values[0].Length)
        //     .Select(idx =>
        //     {
        //         var operation = operations[idx];
        //         var problemNumsStr = numbersStr.Select(n => n[idx]).ToList();
        //
        //         var nums = new List<long>();
        //         for (var numCol = 0; true; numCol++)
        //         {
        //             var builtNum = problemNumsStr.Aggregate(new StringBuilder(), (curr, next) => next.Length > numCol ? curr.Append(next[numCol]) : curr).ToString();
        //
        //             if (builtNum == "")
        //                 break;
        //
        //             nums.Add(long.Parse(builtNum));
        //         }
        //
        //         return (operation, nums);
        //     });
        //
        // var results2 = problems.Select(p => p.operation == "+" ? p.nums.Sum() : p.nums.Aggregate(1L, (acc, n) => acc * n));
        //
        // Console.WriteLine(results2.Sum());
    }
}
