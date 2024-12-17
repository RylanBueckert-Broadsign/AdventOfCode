using Aoc2024.Common;

namespace Aoc2024;

public class Day4 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadGrid(inputPath);

        var word = "MAS";
        var half = (word.Length - 1) / 2;

        var count = 0;

        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] != word[1]) continue;

                if ((Search(input, word, i - half, j - half, (1, 1)) || Search(input, word, i + half, j + half, (-1, -1))) &&
                    (Search(input, word, i - half, j + half, (1, -1)) || Search(input, word, i + half, j - half, (-1, 1))))
                {
                    count++;
                }
            }
        }

        Console.WriteLine(count);
    }

    private static bool Search(char[][] input, string word, int x, int y, Vec2D<int> dir)
    {
        for (var i = 0; i < word.Length; i++)
        {
            var xi = x + i * dir.X;
            var yi = y + i * dir.Y;
            if (!InBounds(input, xi, yi) || input[xi][yi] != word[i])
            {
                return false;
            }
        }

        return true;
    }

    private static bool InBounds(char[][] input, int x, int y) =>
        x >= 0 && x < input.Length && y >= 0 && y < input[x].Length;
}