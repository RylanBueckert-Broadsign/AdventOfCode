using Aoc2024.Common;

namespace Aoc2024;

public class Day5 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath).ToList();

        var rules = input.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).ToList();

        var mustBeBefore = rules.Select(line =>
        {
            var numArr = line.Split('|').Select(int.Parse).ToArray();

            return (numArr[0], numArr[1]);
        }).Aggregate(new Dictionary<int, HashSet<int>>(), (acc, next) =>
        {
            if (acc.TryGetValue(next.Item1, out var list))
            {
                list.Add(next.Item2);
            }
            else
            {
                acc[next.Item1] = [next.Item2];
            }

            return acc;
        });

        var manuals = input.Skip(rules.Count + 1).Select(m => m.Split(',').Select(int.Parse).ToList());

        var incorrect = manuals.Where(pages =>
        {
            for (var idx = 1; idx < pages.Count; idx++)
            {
                if (mustBeBefore.TryGetValue(pages[idx], out var mustBeBeforeList))
                {
                    if (pages.Take(idx).Any(mustBeBeforeList.Contains))
                    {
                        return true;
                    }
                }
            }

            return false;
        });

        var ordered = incorrect.Select(manual =>
            manual.OrderBy(
                i => i,
                Comparer<int>.Create((a, b) =>
                {
                    if (mustBeBefore.TryGetValue(a, out var mustBeBeforeList1))
                    {
                        if (mustBeBeforeList1.Contains(b))
                        {
                            return -1;
                        }
                    }

                    if (mustBeBefore.TryGetValue(b, out var mustBeBeforeList2))
                    {
                        if (mustBeBeforeList2.Contains(a))
                        {
                            return 1;
                        }
                    }

                    return 0;
                })
            ).ToList()
        );

        var middleValues = ordered.Select(manual => manual[manual.Count / 2]);

        Console.WriteLine(middleValues.Sum());
    }
}
