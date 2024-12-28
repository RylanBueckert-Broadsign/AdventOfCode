using Aoc2024.Common;

#pragma warning disable CS8321 // Local function is declared but never used

namespace Aoc2024;

public class Day12 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadGrid(inputPath);

        var regions = new List<HashSet<Vec2D<int>>>();

        for (var x = 0; x < input.Length; x++)
        {
            for (var y = 0; y < input[x].Length; y++)
            {
                if (input[x][y] != '.')
                {
                    regions.Add(PullOutRegion((x, y)));
                }
            }
        }

        var price = regions.Select(r => CalculateSides(r) * r.Count).Sum();

        Console.WriteLine(price);

        return;

        int CalculateSides(HashSet<Vec2D<int>> region)
        {
            var sides = 0;

            foreach (var pos in region)
            {
                if (!region.Contains(pos.Move(Direction.Up)) &&
                    (!region.Contains(pos.Move(Direction.Left)) || region.Contains(pos.Move(Direction.Left).Move(Direction.Up))))
                    sides++;
                if (!region.Contains(pos.Move(Direction.Down)) &&
                    (!region.Contains(pos.Move(Direction.Right)) || region.Contains(pos.Move(Direction.Right).Move(Direction.Down))))
                    sides++;
                if (!region.Contains(pos.Move(Direction.Left)) &&
                    (!region.Contains(pos.Move(Direction.Down)) || region.Contains(pos.Move(Direction.Down).Move(Direction.Left))))
                    sides++;
                if (!region.Contains(pos.Move(Direction.Right)) &&
                    (!region.Contains(pos.Move(Direction.Up)) || region.Contains(pos.Move(Direction.Up).Move(Direction.Right))))
                    sides++;
            }

            return sides;
        }

        int CalculatePerimeter(HashSet<Vec2D<int>> region)
        {
            var perimeter = 0;

            foreach (var pos in region)
            {
                if (!region.Contains(pos.Move(Direction.Up)))
                    perimeter++;
                if (!region.Contains(pos.Move(Direction.Down)))
                    perimeter++;
                if (!region.Contains(pos.Move(Direction.Left)))
                    perimeter++;
                if (!region.Contains(pos.Move(Direction.Right)))
                    perimeter++;
            }

            return perimeter;
        }

        HashSet<Vec2D<int>> PullOutRegion(Vec2D<int> start)
        {
            var plant = input[start.X][start.Y];

            var region = new HashSet<Vec2D<int>>();

            var queue = new Queue<Vec2D<int>>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();

                if (!InBounds(curr) || input[curr.X][curr.Y] != plant || !region.Add(curr))
                    continue;

                input[curr.X][curr.Y] = '.';

                queue.Enqueue(curr.Move(Direction.Up));
                queue.Enqueue(curr.Move(Direction.Down));
                queue.Enqueue(curr.Move(Direction.Left));
                queue.Enqueue(curr.Move(Direction.Right));
            }

            return region;
        }

        bool InBounds(Vec2D<int> pos) =>
            pos.X >= 0 && pos.X < input.Length && pos.Y >= 0 && pos.Y < input[pos.X].Length;
    }
}
