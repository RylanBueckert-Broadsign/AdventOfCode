using Aoc2024.Common;

namespace Aoc2024;

public class Day6 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadGrid(inputPath);

        var startPos = FindStartPos(input, '^');

        var guardPos = startPos;
        var guardFacing = Direction.Up;
        var pathOptions = new HashSet<Vec2D<int>>();

        while (true)
        {
            var nextPos = guardPos.Move(guardFacing);

            if (nextPos.X < 0 || nextPos.X >= input.Length || nextPos.Y < 0 || nextPos.Y >= input[nextPos.X].Length)
                break;

            if (input[nextPos.X][nextPos.Y] == '#')
            {
                guardFacing = guardFacing.TurnRight();
                continue;
            }

            guardPos = nextPos;
            pathOptions.Add(guardPos);
        }

        var loopsFound = 0;
        foreach (var option in pathOptions)
        {
            input[option.X][option.Y] = '#';

            guardPos = startPos;
            guardFacing = Direction.Up;

            if (HasLoop(input, startPos))
            {
                loopsFound++;
            }

            // Reset
            input[option.X][option.Y] = '.';
        }

        Console.WriteLine(loopsFound);
    }

    private static bool HasLoop(char[][] input, Vec2D<int> startPos)
    {
        var guardPos = startPos;
        var guardFacing = Direction.Up;
        var visited = new HashSet<(Vec2D<int>, Direction)> { (guardPos, guardFacing) };

        while (true)
        {
            var nextPos = guardPos.Move(guardFacing);

            if (visited.Contains((nextPos, guardFacing)))
                return true;

            if (nextPos.X < 0 || nextPos.X >= input.Length || nextPos.Y < 0 || nextPos.Y >= input[nextPos.X].Length)
                return false;

            if (input[nextPos.X][nextPos.Y] == '#')
            {
                guardFacing = guardFacing.TurnRight();
                continue;
            }

            guardPos = nextPos;
            visited.Add((guardPos, guardFacing));
        }
    }

    private static Vec2D<int> FindStartPos(char[][] input, char startChar)
    {
        for (var x = 0; x < input.Length; x++)
        {
            for (var y = 0; y < input[x].Length; y++)
            {
                if (input[x][y] == startChar)
                {
                    return new Vec2D<int>(x, y);
                }
            }
        }

        throw new Exception($"Could not find start: '{startChar}'");
    }
}