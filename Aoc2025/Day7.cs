using Aoc2025.Common;

namespace Aoc2025;

public class Day7 : IAocDay
{
    public void Run(string inputPath)
    {
        var grid = InputHelper.ReadGrid(inputPath);

        var splitters = new List<Vec2D<int>>();
        var start = new Vec2D<int>(0, 0);

        for (var row = 0; row < grid.Length; row++)
        {
            for (var col = 0; col < grid[row].Length; col++)
            {
                var cell = grid[row][col];

                switch (cell)
                {
                    case 'S':
                        start = (row, col);
                        break;
                    case '^':
                        splitters.Add((row, col));
                        break;
                }
            }
        }

        Part1(start, grid.Length, splitters);

        // Part 2
        var timelineCache = new Dictionary<Vec2D<int>, long>();

        var totalTimelines = CountTimelines(start, grid.Length, splitters, timelineCache);

        Console.WriteLine($"Total timelines: {totalTimelines}");
    }

    private static long CountTimelines(Vec2D<int> position, int height, List<Vec2D<int>> splitters, Dictionary<Vec2D<int>, long> cache)
    {
        if (position.X == height - 1)
        {
            return 1;
        }

        if (cache.TryGetValue(position, out var timelines))
        {
            return timelines;
        }

        long totalTimelines;

        if (splitters.Contains(position))
        {
            totalTimelines =
                CountTimelines((position.X + 1, position.Y - 1), height, splitters, cache)
                + CountTimelines((position.X + 1, position.Y + 1), height, splitters, cache);
        }
        else
        {
            totalTimelines = CountTimelines((position.X + 1, position.Y), height, splitters, cache);
        }

        cache[position] = totalTimelines;
        return totalTimelines;
    }

    private static void Part1(Vec2D<int> start, int height, List<Vec2D<int>> splitters)
    {
        var beamPath = new List<HashSet<int>>();
        beamPath.Add([start.Y]);
        var totalSplits = 0;

        for (var row = 1; row < height; row++)
        {
            beamPath.Add(new HashSet<int>());

            foreach (var beam in beamPath[row - 1])
            {
                if (splitters.Contains((row, beam)))
                {
                    beamPath[row].Add(beam - 1);
                    beamPath[row].Add(beam + 1);
                    totalSplits++;
                }
                else
                {
                    beamPath[row].Add(beam);
                }
            }
        }

        Console.WriteLine($"Total splits: {totalSplits}");
    }
}
