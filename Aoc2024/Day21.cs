using Aoc2024.Common;

namespace Aoc2024;

public class Day21 : IAocDay
{
    private static readonly string[] Numpad =
    [
        "789",
        "456",
        "123",
        " 0A",
    ];

    private static readonly string[] Arrowpad =
    [
        " ^A",
        "<v>",
    ];

    private const int TOTAL_ROBOTS = 26;

    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath);

        var results = input.Select(i => (presses: GetPresses(i, 0), num: int.Parse(i[..^1])));

        var complexity = results.Select(r => r.presses * r.num);

        Console.WriteLine(complexity.Sum());
    }

    private readonly Dictionary<(char from, char to, int robot), long> _pressCache = new();

    private long GetPresses(char from, char to, int robot)
    {
        if (!_pressCache.TryGetValue((from, to, robot), out var result))
        {
            var pad = robot == 0 ? Numpad : Arrowpad;

            var paths = GetPaths(pad, from, to).ToList();

            result = paths.Select(path => GetPresses(path, robot + 1)).Min();
        }

        _pressCache[(from, to, robot)] = result;

        return result;
    }

    private long GetPresses(string code, int robot)
    {
        if (robot == TOTAL_ROBOTS)
            return code.Length;

        var presses = 0L;
        var curr = 'A';

        foreach (var next in code)
        {
            presses += GetPresses(curr, next, robot);
            curr = next;
        }

        return presses;
    }

    private static IEnumerable<string> GetPaths(string[] pad, char start, char end)
    {
        var startCoord = Find(pad, start);
        var endCoord = Find(pad, end);

        var deltaX = endCoord.X - startCoord.X;
        var deltaY = endCoord.Y - startCoord.Y;

        var xInstruction = deltaX > 0 ? new string('v', deltaX) : new string('^', -deltaX);
        var yInstruction = deltaY > 0 ? new string('>', deltaY) : new string('<', -deltaY);

        if (pad[startCoord.X + deltaX][startCoord.Y] != ' ')
            yield return xInstruction + yInstruction + 'A';

        if (xInstruction.Length == 0 || yInstruction.Length == 0)
            yield break;

        if (pad[startCoord.X][startCoord.Y + deltaY] != ' ')
            yield return yInstruction + xInstruction + 'A';
    }

    private static Vec2D<int> Find(string[] pad, char c)
    {
        var pos = pad.SelectMany((row, x) => row.Select((cell, y) => (cell, x, y))).First(v => v.cell == c);

        return (pos.x, pos.y);
    }
}
