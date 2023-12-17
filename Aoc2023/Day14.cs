using System.Text;
using AoC2023.Utils;

namespace AoC2023;

public static class Day14
{
    public static void Run()
    {
        var lines = InputHelper.ReadLines(@"Day14\input.txt");

        var grid = lines.Select(i => i.ToArray()).ToArray();

        var stateHistory = new Dictionary<string, int>();
        var loadHistory = new List<int>();

        var cycleNum = 0;
        while (true) // We'll never get to 1 billion :(
        {
            var state = grid.Aggregate(new StringBuilder(), (curr, next) => curr.Append(next)).ToString();

            if (stateHistory.TryGetValue(state, out var dupeState))
            {
                var loadCycle = loadHistory.Skip(dupeState).ToList();

                var cycleLength = loadCycle.Count;

                var offset = (1000000000 - cycleNum) % cycleLength;

                var endLoad = loadCycle[offset];

                Console.WriteLine(endLoad);
                break;
            }

            stateHistory.Add(state, cycleNum);
            loadHistory.Add(CalculateLoad(grid));

            Cycle(grid);
            cycleNum++;
        }
    }

    private static void Cycle(char[][] grid)
    {
        TiltNorth(grid);
        TiltWest(grid);
        TiltSouth(grid);
        TiltEast(grid);
    }

    private static void TiltNorth(char[][] grid)
    {
        for (var row = 0; row < grid.Length; row++)
        {
            for (var col = 0; col < grid[0].Length; col++)
            {
                if (grid[row][col] == 'O')
                {
                    var newRow = row;
                    while (newRow - 1 >= 0 && grid[newRow - 1][col] == '.')
                    {
                        newRow--;
                    }

                    if (newRow != row)
                    {
                        grid[row][col] = '.';
                        grid[newRow][col] = 'O';
                    }
                }
            }
        }
    }

    private static void TiltWest(char[][] grid)
    {
        for (var col = 0; col < grid[0].Length; col++)
        {
            for (var row = 0; row < grid.Length; row++)
            {
                if (grid[row][col] == 'O')
                {
                    var newCol = col;
                    while (newCol - 1 >= 0 && grid[row][newCol - 1] == '.')
                    {
                        newCol--;
                    }

                    if (newCol != col)
                    {
                        grid[row][col] = '.';
                        grid[row][newCol] = 'O';
                    }
                }
            }
        }
    }

    private static void TiltSouth(char[][] grid)
    {
        for (var row = grid.Length - 1; row >= 0; row--)
        {
            for (var col = 0; col < grid[0].Length; col++)
            {
                if (grid[row][col] == 'O')
                {
                    var newRow = row;
                    while (newRow + 1 < grid.Length && grid[newRow + 1][col] == '.')
                    {
                        newRow++;
                    }

                    if (newRow != row)
                    {
                        grid[row][col] = '.';
                        grid[newRow][col] = 'O';
                    }
                }
            }
        }
    }

    private static void TiltEast(char[][] grid)
    {
        for (var col = grid[0].Length - 1; col >= 0; col--)
        {
            for (var row = 0; row < grid.Length; row++)
            {
                if (grid[row][col] == 'O')
                {
                    var newCol = col;
                    while (newCol + 1 < grid[0].Length && grid[row][newCol + 1] == '.')
                    {
                        newCol++;
                    }

                    if (newCol != col)
                    {
                        grid[row][col] = '.';
                        grid[row][newCol] = 'O';
                    }
                }
            }
        }
    }

    private static int CalculateLoad(char[][] grid)
    {
        return grid.Select((row, idx) => row.Count(i => i == 'O') * (grid.Length - idx)).Sum();
    }
}
