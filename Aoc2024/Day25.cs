using Aoc2024.Common;

namespace Aoc2024;

public class Day25 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath);

        var schematics = input.Where(i => !string.IsNullOrWhiteSpace(i)).Chunk(7);

        var locks = new List<int[]>();
        var keys = new List<int[]>();

        foreach (var schematic in schematics)
        {
            var isLock = schematic[0][0] == '#';

            var heights = new int[5];

            for (
                var i = isLock ? 1 : schematic.Length - 2;
                isLock ? i < schematic.Length - 1 : i > 0;
                i += isLock ? 1 : -1
            )
            {
                for (var j = 0; j < schematic[i].Length; j++)
                {
                    if (schematic[i][j] == '#')
                    {
                        heights[j]++;
                    }
                }
            }

            if (isLock)
            {
                locks.Add(heights);
            }
            else
            {
                keys.Add(heights);
            }
        }

        var matches = (from @lock in locks
                       from key in keys
                       where @lock.Zip(key).All(p => p.First + p.Second <= 5)
                       select @lock).Count();
        
        Console.WriteLine(matches);
    }
}
