using AoC2023.Common;
using AoC2023.Utils;

namespace AoC2023;

public static class Day16
{
    public static void Run()
    {
        var grid = InputHelper.ReadGrid(@"Day16\input.txt");

        var leftStart = Enumerable.Range(0, grid.Length).Select(i => new
        {
            loc = new Vec2D<int>(i, 0),
            dir = Direction.Right
        });

        var rightStart = Enumerable.Range(0, grid.Length).Select(i => new
        {
            loc = new Vec2D<int>(i, grid[0].Length - 1),
            dir = Direction.Left
        });

        var topStart = Enumerable.Range(0, grid[0].Length).Select(i => new
        {
            loc = new Vec2D<int>(0, i),
            dir = Direction.Down
        });

        var bottomStart = Enumerable.Range(0, grid[0].Length).Select(i => new
        {
            loc = new Vec2D<int>(grid.Length - 1, i),
            dir = Direction.Up
        });

        var allStarts = leftStart.Concat(rightStart).Concat(topStart).Concat(bottomStart);

        var energized = allStarts.Select(s =>
        {
            var history = new HashSet<(Vec2D<int> loc, Direction dir)>();
            ShineBeam(grid, s.loc, s.dir, history);

            return history.Select(i => i.loc).Distinct().Count();
        });

        Console.WriteLine(energized.Max());
    }

    private static void ShineBeam(char[][] grid, Vec2D<int> location, Direction direction, HashSet<(Vec2D<int> loc, Direction dir)> history)
    {
        while (true)
        {
            if (location.X < 0 || location.X >= grid.Length || location.Y < 0 || location.Y >= grid[location.X].Length) return;

            if (!history.Add((location, direction))) return;

            switch (grid[location.X][location.Y])
            {
                case '.':
                    location = location.Move(direction);
                    break;
                case '\\':
                    direction = direction is Direction.Right or Direction.Left ? direction.TurnRight() : direction.TurnLeft();
                    location = location.Move(direction);
                    break;
                case '/':
                    direction = direction is Direction.Right or Direction.Left ? direction.TurnLeft() : direction.TurnRight();
                    location = location.Move(direction);
                    break;
                case '|':
                    if (direction is Direction.Right or Direction.Left)
                    {
                        var otherDir = direction.TurnRight();
                        direction = direction.TurnLeft();
                        ShineBeam(grid, location.Move(otherDir), otherDir, history);
                        location = location.Move(direction);
                    }
                    else
                    {
                        location = location.Move(direction);
                    }

                    break;
                case '-':
                    if (direction is Direction.Up or Direction.Down)
                    {
                        var otherDir = direction.TurnRight();
                        direction = direction.TurnLeft();
                        ShineBeam(grid, location.Move(otherDir), otherDir, history);
                        location = location.Move(direction);
                    }
                    else
                    {
                        location = location.Move(direction);
                    }

                    break;
                default:
                    throw new Exception("Unexpected value");
            }
        }
    }
}