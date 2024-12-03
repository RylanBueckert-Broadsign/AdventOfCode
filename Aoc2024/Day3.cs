using System.Text.RegularExpressions;
using Aoc2024.Common;

namespace Aoc2024;

public class Day3 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadWholeFile(inputPath);

        const string instructionRegex = @"mul\((\d{1,3}),(\d{1,3})\)";

        const string dontIns = "don't()";
        const string doIns = "do()";

        while (true)
        {
            var eraseStart = input.IndexOf(dontIns, StringComparison.Ordinal);

            if (eraseStart < 0) break;

            var eraseEnd = input.IndexOf(doIns, eraseStart, StringComparison.Ordinal);

            if (eraseEnd >= 0)
            {
                input = input[..eraseStart] + input[(eraseEnd + doIns.Length)..];
            }
            else
            {
                input = input[..eraseStart];
            }
        }

        var instructions = Regex.Matches(input, instructionRegex);

        var results = instructions.Select(i =>
            {
                var nums = i.Groups.Values.Skip(1).Select(v => int.Parse(v.Value)).ToArray();

                return nums[0] * nums[1];
            }
        );

        Console.WriteLine(results.Sum());
    }
}
