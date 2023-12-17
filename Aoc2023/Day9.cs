using AoC2023.Utils;

namespace AoC2023;

public static class Day9
{
    public static void Run()
    {
        var lines = InputHelper.ReadLines(@"Day9\input.txt").ToList();

        var histories = lines.Select(l => l.Split().Select(int.Parse).ToList()).ToList();

        var result = histories.Select(PreviousValue).Sum();

        Console.WriteLine(result);
    }

    private static List<int> GenerateDiff(IReadOnlyList<int> input)
    {
        return input.Skip(1).Select((value, index) => value - input[index]).ToList();
    }

    private static int NextValue(IReadOnlyList<int> input)
    {
        if (input.All(i => i == 0))
        {
            return 0;
        }

        var diff = GenerateDiff(input);

        return input[^1] + NextValue(diff);
    }

    private static int PreviousValue(IReadOnlyList<int> input)
    {
        if (input.All(i => i == 0))
        {
            return 0;
        }

        var diff = GenerateDiff(input);

        return input[0] - PreviousValue(diff);
    }
}
