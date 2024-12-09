using Aoc2024.Common;
using AoC2024.Common;

namespace Aoc2024;

public class Day8 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadGrid(inputPath);

        var nodes = ReadNodes(input);

        Vec2D<int> gridSize = (input.Length, input[0].Length);

        bool InBounds(Vec2D<int> pos) => pos.X >= 0 && pos.X < gridSize.X && pos.Y >= 0 && pos.Y < gridSize.Y;

        var antiNodes = new HashSet<Vec2D<int>>();

        foreach (var frequency in nodes)
        {
            var freqNodes = frequency.Value.ToList();

            for (var i = 0; i < freqNodes.Count - 1; i++)
            {
                var node1 = freqNodes[i];

                for (var j = i + 1; j < freqNodes.Count; j++)
                {
                    var node2 = freqNodes[j];

                    var gap = node2.Subtract(node1);

                    for (var k = 0;; k++)
                    {
                        var candidate = node1.Subtract(gap.Multiply(k));

                        if (!InBounds(candidate))
                            break;

                        antiNodes.Add(candidate);
                    }

                    for (var k = 0;; k++)
                    {
                        var candidate = node2.Add(gap.Multiply(k));

                        if (!InBounds(candidate))
                            break;

                        antiNodes.Add(candidate);
                    }
                }
            }
        }

        Console.WriteLine(antiNodes.Count);
    }

    private static Dictionary<char, HashSet<Vec2D<int>>> ReadNodes(char[][] grid)
    {
        var nodes = new Dictionary<char, HashSet<Vec2D<int>>>();

        for (var x = 0; x < grid.Length; x++)
        {
            for (var y = 0; y < grid[x].Length; y++)
            {
                var frequency = grid[x][y];

                if (frequency == '.')
                    continue;

                if (!nodes.TryGetValue(frequency, out var positions))
                {
                    positions = [];
                    nodes[frequency] = positions;
                }

                positions.Add((x, y));
            }
        }

        return nodes;
    }
}
