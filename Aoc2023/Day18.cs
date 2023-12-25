using System.Globalization;
using AoC2023.Common;
using AoC2023.Utils;

namespace AoC2023;

public static class Day18
{
    public static void Run()
    {
        var lines = InputHelper.ReadLines(@"Day18\input.txt");

        var instructions = lines.Select(ParseHexInstruction);

        var vertices = new List<Vec2D<int>>();

        var currentLocation = new Vec2D<int>(0, 0);
        var perimeter = 0;

        foreach (var instruction in instructions)
        {
            currentLocation = currentLocation.Move(instruction.Direction, instruction.Count);
            perimeter += instruction.Count;

            vertices.Add(currentLocation);
        }

        var size = ShoelaceArea(vertices) + perimeter / 2 + 1;

        Console.WriteLine(size);
    }

    private static long ShoelaceArea(List<Vec2D<int>> vertices)
    {
        var terms = vertices.Skip(1).Select((coords, i) => (long)(coords.X + vertices[i].X) * (coords.Y - vertices[i].Y));

        return Math.Abs(terms.Sum()) / 2;
    }

    private static DigInstruction ParseInstruction(string instruction)
    {
        var x = instruction.Split(' ');

        var dir = x[0] switch
        {
            "U" => Direction.Up,
            "R" => Direction.Right,
            "D" => Direction.Down,
            "L" => Direction.Left,
            _ => throw new Exception("Invalid direction")
        };

        var count = int.Parse(x[1]);

        return new DigInstruction(dir, count);
    }

    private static DigInstruction ParseHexInstruction(string instruction)
    {
        var x = instruction.Split(' ');

        var hex = x[2][2..^1];

        var dir = hex[^1] switch
        {
            '3' => Direction.Up,
            '0' => Direction.Right,
            '1' => Direction.Down,
            '2' => Direction.Left,
            _ => throw new Exception("Invalid direction")
        };

        var count = int.Parse(hex[..5], NumberStyles.HexNumber);

        return new DigInstruction(dir, count);
    }

    private record DigInstruction(Direction Direction, int Count);
}