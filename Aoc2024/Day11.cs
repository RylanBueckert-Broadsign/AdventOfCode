using Aoc2024.Common;

namespace Aoc2024;

public class Day11 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadWholeFile(inputPath);

        var stones = input.Split(' ').Select(long.Parse);

        var stoneCounts = new Dictionary<long, long>();

        foreach (var stone in stones)
        {
            if (!stoneCounts.TryAdd(stone, 1))
            {
                stoneCounts[stone]++;
            }
        }

        for (var blink = 0; blink < 75; blink++)
        {
            var newStoneCounts = new Dictionary<long, long>();
            foreach (var (stone, count) in stoneCounts)
            {
                var str = stone.ToString();

                if (stone == 0)
                {
                    newStoneCounts.TryAdd(1, 0);

                    newStoneCounts[1] += count;
                }

                else if (str.Length % 2 == 0)
                {
                    var mid = str.Length / 2;

                    var newStones = new[] { str[..mid], str[mid..] }.Select(long.Parse);

                    foreach (var ns in newStones)
                    {
                        newStoneCounts.TryAdd(ns, 0);

                        newStoneCounts[ns] += count;
                    }
                }
                else
                {
                    var newStone = stone * 2024;

                    newStoneCounts.TryAdd(newStone, 0);

                    newStoneCounts[newStone] += count;
                }
            }

            stoneCounts = newStoneCounts;
        }

        Console.WriteLine(stoneCounts.Select(sc => sc.Value).Sum());
    }
}
