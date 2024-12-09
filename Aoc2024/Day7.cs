using Aoc2024.Common;

namespace Aoc2024;

public class Day7 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath);

        var equations = input.Select(line =>
        {
            var splitLine = line.Split(' ').ToList();

            var target = long.Parse(splitLine[0][..^1]);

            var values = splitLine.Skip(1).Select(long.Parse).ToList();

            return new
            {
                target,
                values
            };
        });

        var validEquations = equations.Where(eq =>
        {
            var possibleValuesLess = eq.values.Skip(1).Aggregate(
                new[] { eq.values[0] }.AsEnumerable(),
                ((possibleTotals, next) => possibleTotals.SelectMany(pt => new[] { pt + next, pt * next, long.Parse($"{pt}{next}") }.Where(newTotal => newTotal <= eq.target))));

            return possibleValuesLess.Contains(eq.target);
        });

        var result = validEquations.Select(eq => eq.target).Sum();

        Console.WriteLine(result);
    }
}
