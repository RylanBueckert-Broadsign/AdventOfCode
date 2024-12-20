using Aoc2024.Common;

namespace Aoc2024;

public class Day19 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath).ToList();

        var availableTowels = input[0].Split(',', StringSplitOptions.TrimEntries).ToList();

        var patterns = input.Skip(2);

        var cache = new Dictionary<string, long>();

        var possiblePatterns = patterns.Select(PatternCombinations);

        Console.WriteLine(possiblePatterns.Sum());
        return;

        long PatternCombinations(string pattern)
        {
            if (cache.TryGetValue(pattern, out var cached))
            {
                return cached;
            }

            if (pattern.Length == 0)
            {
                cache[pattern] = 1;
                return 1;
            }

            var matches = availableTowels.Where(pattern.StartsWith);

            var possibleCombinations = matches.Select(m =>
            {
                var subPattern = pattern[m.Length..];

                return PatternCombinations(subPattern);
            }).Sum();

            cache[pattern] = possibleCombinations;
            return possibleCombinations;
        }
    }
}