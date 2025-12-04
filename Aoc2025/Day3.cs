using System.Text;
using Aoc2025.Common;

namespace Aoc2025;

public class Day3 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath);

        var banks = input.Select(i => i.Select(j => int.Parse(j.ToString())).ToList());

        var maxJoltage = banks.Select(b =>
        {
            var omitStart = 0;
            var strJoltage = new StringBuilder(12);

            for (var omitEnd = 11; omitEnd >= 0; omitEnd--)
            {
                var digit = LargestValueIndex(b.Skip(omitStart).SkipLast(omitEnd));

                strJoltage.Append(digit.value);
                omitStart += digit.index + 1;
            }

            return long.Parse(strJoltage.ToString());
        });

        Console.WriteLine($"Part 2: {maxJoltage.Sum()}");
    }

    private static (int value, int index) LargestValueIndex(IEnumerable<int> numbers)
    {
        return numbers
            .Select((value, index) => (value, index))
            .Aggregate((max, current) => current.value > max.value ? current : max);
    }
}
