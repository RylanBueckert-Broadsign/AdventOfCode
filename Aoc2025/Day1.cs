using Aoc2025.Common;

namespace Aoc2025;

public class Day1 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath);

        var instructions = input.Select(line => int.Parse(line[1..]) * (line[0] == 'R' ? 1 : -1));

        var current = 50;
        var zeroCount = 0;

        foreach (var instruction in instructions)
        {
            switch (instruction)
            {
                case > 0:
                    current += instruction;
                    zeroCount += current / 100;
                    current %= 100;
                    break;
                case < 0:
                {
                    if (current == 0)
                    {
                        // Prevent double counting when starting at zero
                        zeroCount--;
                    }

                    current += instruction;
                    while (current < 0)
                    {
                        var times = (-current + 99) / 100;
                        zeroCount += times;
                        current += times * 100;
                    }

                    if (current == 0)
                    {
                        zeroCount++;
                    }

                    break;
                }
            }
        }

        Console.WriteLine($"Position: {current}, ZeroCount: {zeroCount}");
    }
}
