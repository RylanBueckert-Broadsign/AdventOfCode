using System.Text.RegularExpressions;
using Aoc2024.Common;
using AoC2024.Common;

namespace Aoc2024;

public class Day13 : IAocDay
{
    private class Puzzle
    {
        public Dictionary<string, (Vec2D<long> move, long cost)> Buttons { get; } = new();
        public required Vec2D<long> Prize { get; init; }
    }

    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath);

        var puzzlesRaw = input.Where(s => !string.IsNullOrWhiteSpace(s)).Chunk(3);

        var puzzles = puzzlesRaw.Select(p =>
        {
            var puzzle = new Puzzle()
            {
                Prize = ParsePrize(p[2])
            };

            puzzle.Buttons.Add("A", (ParseButton(p[0]), 3));
            puzzle.Buttons.Add("B", (ParseButton(p[1]), 1));

            return puzzle;
        });

        var costs = puzzles.Select(PuzzleCost).ToList();

        Console.WriteLine(costs.Where(x => x.HasValue).Sum());
    }

    private static long? PuzzleCost(Puzzle puzzle)
    {
        var x = puzzle.Prize.X;
        var y = puzzle.Prize.Y;
        var xa = puzzle.Buttons["A"].move.X;
        var ya = puzzle.Buttons["A"].move.Y;
        var xb = puzzle.Buttons["B"].move.X;
        var yb = puzzle.Buttons["B"].move.Y;

        var t1 = y * xa;
        var t2 = x * ya;

        var t3 = -1 * xb * ya;
        var t4 = yb * xa;

        var b = (t1 - t2) / (t3 + t4);
        var a = (puzzle.Prize.X - b * puzzle.Buttons["B"].move.X) / puzzle.Buttons["A"].move.X;

        if (a < 0 || b < 0)
            return null;

        if (a * xa + b * xb != x)
            return null;

        if (a * ya + b * yb != y)
            return null;

        return a * puzzle.Buttons["A"].cost + b * puzzle.Buttons["B"].cost;
    }

    private static Vec2D<long> ParseButton(string buttonStr)
    {
        const string buttonPattern = @"Button .: X\+(?<X>\d+), Y\+(?<Y>\d+)";

        var r = Regex.Match(buttonStr, buttonPattern);

        return (long.Parse(r.Groups["X"].Value), long.Parse(r.Groups["Y"].Value));
    }

    private static Vec2D<long> ParsePrize(string prizeStr)
    {
        const string prizePattern = @"Prize: X=(?<X>\d+), Y=(?<Y>\d+)";

        var r = Regex.Match(prizeStr, prizePattern);

        var offset = 10000000000000;

        return (long.Parse(r.Groups["X"].Value) + offset, long.Parse(r.Groups["Y"].Value) + offset);
    }
}
