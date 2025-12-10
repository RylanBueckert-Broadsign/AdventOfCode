using Aoc2025.Common;

namespace Aoc2025;

public class Day9 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath);

        var outerShape = input
            .Select(line => line.Split(',').Select(long.Parse).ToArray())
            .Select(coords => new Vec2D<long>(coords[0], coords[1]))
            .ToList();

        var biggestArea = 0L;

        for (var first = 0; first < outerShape.Count; first++)
        {
            for (var second = first + 1; second < outerShape.Count; second++)
            {
                var area = GetArea(outerShape[first], outerShape[second]);
                if (area > biggestArea)
                {
                    biggestArea = area;
                }
            }
        }

        Console.WriteLine($"Biggest area: {biggestArea}");
    }

    private static long GetArea(Vec2D<long> corner1, Vec2D<long> corner2)
    {
        var width = Math.Abs(corner2.X - corner1.X) + 1;
        var height = Math.Abs(corner2.Y - corner1.Y) + 1;
        return width * height;
    }
}
