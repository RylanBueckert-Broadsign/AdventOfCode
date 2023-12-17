using System.Text;

namespace AoC2023;

public static class Day13
{
    public static void Run()
    {
        var sr = new StreamReader(@"C:\Source\AoC2023\Day13\input.txt");
        var input = sr.ReadToEnd().Trim();
        var lines = input.Split('\n').Select(i => i.Trim()).ToList();

        var puzzlesRaw = new List<List<string>>();

        var puzzleStart = 0;

        while (true)
        {
            var puzzleEnd = lines.IndexOf("", puzzleStart);

            if (puzzleEnd < 0)
            {
                puzzlesRaw.Add(lines.Skip(puzzleStart).ToList());
                break;
            }

            var puzzle = lines.Skip(puzzleStart).Take(puzzleEnd - puzzleStart).ToList();

            puzzlesRaw.Add(puzzle);

            puzzleStart = puzzleEnd + 1;
        }

        var puzzles = puzzlesRaw.Select(p =>
        {
            var width = p[0].Length;

            var rows = p.Select(s => s.ToList()).ToList();
            var cols = Enumerable
                .Range(0, width)
                .Select(i => p.Aggregate(new StringBuilder(), (curr, next) => curr.Append(next[i])).ToString())
                .ToList();

            return new Puzzle(p, cols);
        });

        var summary = puzzles.Select(p =>
        {
            var verticalReflection = ReflectionIdx(p.Cols, null);
            var horizontalReflection = ReflectionIdx(p.Rows, null);

            for (var smudgeRow = 0; smudgeRow < p.Rows.Count; smudgeRow++)
            {
                for (var smudgeCol = 0; smudgeCol < p.Cols.Count; smudgeCol++)
                {
                    ToggleSmudge(p, smudgeRow, smudgeCol);

                    var sVerticalReflection = ReflectionIdx(p.Cols, verticalReflection);

                    if (sVerticalReflection is not null)
                        return sVerticalReflection;

                    var sHorizontalReflection = ReflectionIdx(p.Rows, horizontalReflection);

                    if (sHorizontalReflection is not null)
                        return sHorizontalReflection * 100;

                    ToggleSmudge(p, smudgeRow, smudgeCol);
                }
            }

            throw new Exception("None found");
        }).Sum();

        Console.WriteLine(summary);
    }

    private static void ToggleSmudge(Puzzle puzzle, int row, int col)
    {
        var newVal = puzzle.Rows[row][col] == '.' ? '#' : '.';

        puzzle.Rows[row] = new StringBuilder(puzzle.Rows[row])
        {
            [col] = newVal
        }.ToString();

        puzzle.Cols[col] = new StringBuilder(puzzle.Cols[col])
        {
            [row] = newVal
        }.ToString();
    }

    private static int? ReflectionIdx(List<string> list, int? notAllowed)
    {
        for (var i = 0; i < list.Count - 1; i++)
        {
            if (i + 1 != notAllowed && TestReflection(list, i))
                return i + 1;
        }

        return null;
    }

    private static bool TestReflection(List<string> list, int idx)
    {
        for (var offset = 0; idx - offset >= 0 && idx + 1 + offset < list.Count; offset++)
        {
            if (list[idx - offset] != list[idx + 1 + offset])
                return false;
        }

        return true;
    }

    private record Puzzle(List<string> Rows, List<string> Cols);
}
