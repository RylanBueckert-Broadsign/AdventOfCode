using System.Text.RegularExpressions;
using Aoc2024.Common;
using AoC2024.Common;

namespace Aoc2024;

public partial class Day14 : IAocDay
{
    public void Run(string inputPath)
    {
        Vec2D<long> roomSize = (101, 103);
        const long targetSteps = 100;

        var input = InputHelper.ReadLines(inputPath);

        var robots = input.Select(ParseRobot).ToList();

        //Part 1
        var finalPositions = robots.Select(r => GetPosition(r, targetSteps));

        var quadrantCounts = new[] { 0, 0, 0, 0 };
        var midX = roomSize.X / 2;
        var midY = roomSize.Y / 2;

        foreach (var pos in finalPositions)
        {
            if (pos.X == midX || pos.Y == midY)
                continue;

            quadrantCounts[(pos.Y > midY ? 2 : 0) + (pos.X > midX ? 1 : 0)]++;
        }

        var safetyFactor = quadrantCounts.Aggregate((curr, next) => curr * next);

        Console.WriteLine(safetyFactor);

        // Part 2
        while (true)
        {
            var cmd = Console.ReadLine()!;

            if (cmd == "exit")
                break;

            var check = long.Parse(cmd);

            var positions = robots.Select(r => GetPosition(r, check)).ToHashSet();

            for (var y = 0; y < roomSize.Y; y++)
            {
                for (var x = 0; x < roomSize.X; x++)
                {
                    Console.Write(positions.Contains((x, y)) ? '#' : '.');
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        return;

        Vec2D<long> GetPosition((Vec2D<long> position, Vec2D<long> velocity) robot, long steps)
        {
            var (position, velocity) = robot;

            var unwrapped = position.Add(velocity.Multiply(steps));

            return new Vec2D<long>(PositiveModulo(unwrapped.X, roomSize.X), PositiveModulo(unwrapped.Y, roomSize.Y));
        }
    }

    private static (Vec2D<long> position, Vec2D<long> velocity) ParseRobot(string input)
    {
        var match = RobotRegex().Match(input);

        var groups = match.Groups;

        var values = new[] { groups["px"].Value, groups["py"].Value, groups["vx"].Value, groups["vy"].Value }
            .Select(long.Parse).ToArray();

        return ((values[0], values[1]), (values[2], values[3]));
    }

    [GeneratedRegex(@"p=(?<px>-?\d+),(?<py>-?\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)")]
    private static partial Regex RobotRegex();

    private static long PositiveModulo(long a, long b)
    {
        return a >= 0 ? a % b : (a % b + b) % b;
    }
}
