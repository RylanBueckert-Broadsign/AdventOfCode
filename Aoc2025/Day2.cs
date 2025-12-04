using Aoc2025.Common;

namespace Aoc2025;

public class Day2 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadWholeFile(inputPath);

        var ranges = input.Split(',').Select(r =>
        {
            var parts = r.Split('-');
            return (Start: long.Parse(parts[0]), End: long.Parse(parts[1]));
        }).ToList();

        Part1(ranges);

        Part2(ranges);
    }

    private static void Part1(List<(long Start, long End)> ranges)
    {
        var invalidIds1 = ranges.SelectMany(r =>
        {
            var startStr = r.Start.ToString();

            string start;

            if (startStr.Length % 2 == 1)
            {
                // Odd length, always valid. Skip to next even length.
                start = '1' + new string('0', startStr.Length / 2);
            }
            else
            {
                var firstHalf = long.Parse(startStr[..(startStr.Length / 2)]);
                var secondHalf = long.Parse(startStr[(startStr.Length / 2)..]);

                start = secondHalf > firstHalf ? (firstHalf + 1).ToString() : firstHalf.ToString();
            }

            var endStr = r.End.ToString();

            string end;

            if (endStr.Length % 2 == 1)
            {
                // Odd length, always valid. Skip to previous even length.
                end = new string('9', endStr.Length / 2);
            }
            else
            {
                var firstHalf = long.Parse(endStr[..(endStr.Length / 2)]);
                var secondHalf = long.Parse(endStr[(endStr.Length / 2)..]);

                end = secondHalf < firstHalf ? (firstHalf - 1).ToString() : firstHalf.ToString();
            }

            var startNum = int.Parse(start);
            var endNum = int.Parse(end);

            return Enumerable.Range(startNum, endNum - startNum + 1).Select(v => long.Parse(v.ToString() + v.ToString()));
        });

        Console.WriteLine($"Part 1: {invalidIds1.Sum()}");
    }

    private static void Part2(List<(long Start, long End)> ranges)
    {
        var invalidIds2 = ranges.SelectMany(r =>
            {
                var results = new HashSet<long>();
                var minLen = r.Start.ToString().Length;
                var maxLen = r.End.ToString().Length;

                for (var totalLen = minLen; totalLen <= maxLen; totalLen++)
                {
                    for (var seqLen = 1; seqLen <= totalLen / 2; seqLen++)
                    {
                        if (totalLen % seqLen != 0) continue;
                        var repeats = totalLen / seqLen;

                        var minSeq = long.Parse(1 + new string('0', seqLen - 1));
                        var maxSeq = long.Parse(new string('9', seqLen));

                        for (var baseSeq = minSeq; baseSeq <= maxSeq; baseSeq++)
                        {
                            var repeated = string.Concat(Enumerable.Repeat(baseSeq.ToString(), repeats));
                            var num = long.Parse(repeated);

                            if (num < r.Start) continue;
                            if (num > r.End) break;

                            results.Add(num);
                        }
                    }
                }

                return results;
            }
        );

        Console.WriteLine($"Part 2: {invalidIds2.Sum()}");
    }
}
