using System.Text;
using Aoc2024.Common;

namespace Aoc2024;

public class Day21 : IAocDay
{
    private static readonly char?[][] Numpad =
    [
        ['7', '8', '9'],
        ['4', '5', '6'],
        ['1', '2', '3'],
        [null, '0', 'A']
    ];

    private static readonly char?[][] Arrowpad =
    [
        [null, '^', 'A'],
        ['<', 'v', '>'],
    ];

    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath);

        var sequence = input.Select(pwd => GetCombination(Arrowpad, GetCombination(Arrowpad, GetCombination(Numpad, pwd))));
        // var sequence = input.Select(pwd => GetCombination(arrowpad, GetCombination(numpad, pwd)));
    }

    private static string GetCombination(char?[][] pad, string targetSequence)
    {
        return targetSequence.Aggregate(
            (str: new StringBuilder(), pos: 'A'),
            (curr, next) => (curr.str.Append(GetCombination(pad, curr.pos, next)), next),
            i => i.str.ToString()
        );
    }

    private static string GetCombination(char?[][] pad, char start, char end)
    {
        var startCoord = Find(pad, start);
        var endCoord = Find(pad, end);

        var deltaX = endCoord.X - startCoord.X;
        var deltaY = endCoord.Y - startCoord.Y;

        var xInstruction = deltaX > 0 ? new string('v', deltaX) : new string('^', -deltaX);
        var yInstruction = deltaY > 0 ? new string('>', deltaY) : new string('<', -deltaY);

        return xInstruction + yInstruction + 'A';
    }

    private static Vec2D<int> Find(char?[][] pad, char c)
    {
        var pos = pad.SelectMany((row, x) => row.Select((cell, y) => (cell, x, y))).First(v => v.cell == c);

        return (pos.x, pos.y);
    }
}
