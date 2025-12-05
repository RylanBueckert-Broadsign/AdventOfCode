using Aoc2025.Common;

namespace Aoc2025;

public class Day4 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadGrid(inputPath);

        var paperLocations = input
            .SelectMany((row, rIdx) => row.Select((cell, cIdx) => (cell, rIdx, cIdx)))
            .Where(loc => loc.cell == '@')
            .Select(loc => new Vec2D<int>(loc.rIdx, loc.cIdx))
            .ToHashSet();

        var totalAccessible = 0;

        while (true)
        {
            var accessible = paperLocations.Where(loc =>
                SurroundingLocations(loc).Count(surLoc => paperLocations.Contains(surLoc)) < 4).ToList();

            if (accessible.Count == 0)
                break;

            totalAccessible += accessible.Count;

            foreach (var loc in accessible)
            {
                paperLocations.Remove(loc);
            }
        }

        Console.WriteLine(totalAccessible);
    }

    private static IEnumerable<Vec2D<int>> SurroundingLocations(Vec2D<int> loc)
    {
        yield return loc.Move(Direction.Up);
        yield return loc.Move(Direction.Down);
        yield return loc.Move(Direction.Left);
        yield return loc.Move(Direction.Right);
        yield return loc.Move(Direction.Up).Move(Direction.Left);
        yield return loc.Move(Direction.Up).Move(Direction.Right);
        yield return loc.Move(Direction.Down).Move(Direction.Left);
        yield return loc.Move(Direction.Down).Move(Direction.Right);
    }
}
