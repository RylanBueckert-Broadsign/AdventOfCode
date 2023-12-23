using AoC2023.Common;
using AoC2023.Utils;

namespace AoC2023;

public static class Day21
{
    public static void Run()
    {
        var grid = InputHelper.ReadGrid(@"Day21\input.txt");
        const int maxSteps = 16; //26501365;

        var start = grid
            .SelectMany((row, rowIdx) => row.Select((c, colIdx) => new { coords = new Coords2D(rowIdx, colIdx), c }))
            .Where(i => i.c == 'S').Select(i => i.coords)
            .Single();

        var shortestPaths = new Dictionary<Coords2D, int>();
        var candidates = new PriorityQueue<Coords2D, int>();

        candidates.Enqueue(start, 0);

        while (candidates.TryDequeue(out var bestLocation, out var shortestPath))
        {
            if (shortestPath > maxSteps)
                break;

            if (!shortestPaths.TryAdd(bestLocation, shortestPath))
            {
                continue;
            }

            for (var dir = (Direction)0; (int)dir < 4; dir++)
            {
                var newLoc = bestLocation.Move(dir);
                if (At(newLoc) != '#' && !shortestPaths.ContainsKey(newLoc))
                    candidates.Enqueue(newLoc, shortestPath + 1);
            }
        }

        var canReach = shortestPaths.Count(p => p.Value <= maxSteps && maxSteps % 2 == p.Value % 2);

        Console.WriteLine(canReach);

        return;

        bool IsValidCoords(Coords2D loc) => loc.Row >= 0 && loc.Row < Height() && loc.Col >= 0 && loc.Col < Width();

        int Height() => grid.Length;
        int Width() => grid[0].Length;

        char At(Coords2D loc)
        {
            var row = loc.Row % Height();
            var col = loc.Col % Height();

            return grid[row < 0 ? Height() + row : row][col < 0 ? Width() + col : col];
        }
    }
}
