using AoC2023.Utils;

namespace AoC2023;

public static class Day11
{
    public static void Run()
    {
        var lines = InputHelper.ReadLines(@"Day11\input.txt").ToList();

        var emptyRows = lines
            .Select((value, idx) => new { idx, value })
            .Where(i => i.value.All(c => c == '.'))
            .Select(i => i.idx)
            .ToList();

        var emptyColumns = Enumerable.Range(0, lines[0].Length)
            .Where(i => lines.All(l => l[i] == '.'))
            .ToList();

        var locations = lines
            .SelectMany((line, row) =>
                line.Select((c, col) => new { c, col })
                    .Where(i => i.c == '#')
                    .Select(i => (row, i.col)))
            .ToList();

        var distances = new List<long>();

        const int factor = 1000000;

        for (var from = 0; from < locations.Count; from++)
        {
            for (var to = from + 1; to < locations.Count; to++)
            {
                var fromLoc = locations[from];
                var toLoc = locations[to];

                var rowDistance = Math.Abs(fromLoc.row - toLoc.row);
                var colDistance = Math.Abs(fromLoc.col - toLoc.col);

                var rowExpansion = emptyRows.Count(i => i < fromLoc.row && i > toLoc.row || i > fromLoc.row && i < toLoc.row) * (factor - 1);
                var colExpansion = emptyColumns.Count(i => i < fromLoc.col && i > toLoc.col || i > fromLoc.col && i < toLoc.col) * (factor - 1);

                var distance = rowDistance + colDistance + rowExpansion + colExpansion;

                distances.Add(distance);
            }
        }

        Console.WriteLine(distances.Sum());
    }
}
