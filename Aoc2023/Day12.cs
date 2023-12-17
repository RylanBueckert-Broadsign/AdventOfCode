using System.Text;
using AoC2023.Utils;

namespace AoC2023;

public static class Day12
{
    public static void Run()
    {
        var lines = InputHelper.ReadLines(@"Day12\input.txt");

        var record = lines.Select(l =>
        {
            var idx = l.IndexOf(' ');

            var text = l[..idx];
            var groups = l[(idx + 1)..].Split(',').Select(int.Parse).ToList();

            // return new { text, groups };

            var unfoldedText = text + '?' + text + '?' + text + '?' + text + '?' + text;
            var unfoldedGroups = groups.Concat(groups).Concat(groups).Concat(groups).Concat(groups).ToList();

            return new { text = unfoldedText, groups = unfoldedGroups };
        }).ToList();

        var possibleArrangements = record.Select(r => (long)PossibleArrangements(r.text, r.groups)).ToList();

        Console.WriteLine(possibleArrangements.Sum());
    }

    private static readonly Dictionary<(string text, string groups), long> Memory = new();

    private static long Memorize((string text, string groups) token, long result)
    {
        Memory[token] = result;
        return result;
    }

    private static long PossibleArrangements(string text, List<int> groups)
    {
        var token = (text, groups.Aggregate("", (curr, next) => curr + next + ','));

        if (Memory.TryGetValue(token, out var arrangements))
        {
            return arrangements;
        }

        var group = 0;
        var currGroupCount = 0;
        var currGroupIdx = -1;

        for (var i = 0; i < text.Length; i++)
        {
            var spring = text[i];
            switch (spring)
            {
                case '.':
                    if (currGroupCount > 0)
                    {
                        if (currGroupCount != groups[group])
                        {
                            return Memorize(token, 0);
                        }

                        group++;
                        currGroupCount = 0;
                        currGroupIdx = -1;
                    }

                    break;
                case '#':
                    if (group >= groups.Count)
                    {
                        return Memorize(token, 0);
                    }

                    if (currGroupCount == 0)
                    {
                        currGroupIdx = i;
                    }

                    currGroupCount++;
                    break;
                case '?':

                    var sb = new StringBuilder(text[(currGroupIdx >= 0 ? currGroupIdx : i)..]);
                    var newGroups = groups.Skip(group).ToList();

                    sb[currGroupIdx >= 0 ? i - currGroupIdx : 0] = '.';
                    var operationalPossibilities = PossibleArrangements(sb.ToString(), newGroups);

                    sb[currGroupIdx >= 0 ? i - currGroupIdx : 0] = '#';
                    var damagedPossibilities = PossibleArrangements(sb.ToString(), newGroups);

                    return Memorize(token, operationalPossibilities + damagedPossibilities);
                    break;
                default: throw new Exception();
            }
        }

        if (currGroupCount > 0)
        {
            if (currGroupCount != groups[group])
            {
                return Memorize(token, 0);
            }

            group++;
            currGroupCount = 0;
        }

        return Memorize(token, group == groups.Count ? 1 : 0);
    }
}
