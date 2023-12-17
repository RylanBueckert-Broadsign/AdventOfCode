﻿using AoC2023.Common;
using AoC2023.Utils;

namespace AoC2023;

public static class Day10
{
    public static void Run()
    {
        var lines = InputHelper.ReadLines(@"Day10\input.txt");

        var grid = lines.Select(l => l.ToList()).ToList();

        var start = GetStartingPosition(grid);

        var pipeGrid = grid.Select(i => i.Select(GetPipe).ToList()).ToList();

        var startPipe = InferPipe(pipeGrid, start.Row, start.Col);
        pipeGrid[start.Row][start.Col] = startPipe;

        var currLoc = start;
        var loopLength = 0;

        var from = pipeGrid[currLoc.Row][currLoc.Col]!.D2;

        var loopEdge = new List<Coords2D>();

        do
        {
            var to = pipeGrid[currLoc.Row][currLoc.Col]!.Travel(from);

            currLoc = currLoc.Move(to);
            from = InvertDirection(to);
            loopLength++;
            loopEdge.Add(currLoc);
        } while (currLoc != start);

        // Console.WriteLine(loopLength / 2);

        for (var row = 0; row < pipeGrid.Count; row++)
        {
            for (var col = 0; col < pipeGrid[row].Count; col++)
            {
                if (!loopEdge.Contains((row, col)))
                {
                    pipeGrid[row][col] = null;
                }
            }
        }

        // var firstPipe = GetFirstPipe(pipeGrid);

        currLoc = start;
        from = pipeGrid[currLoc.Row][currLoc.Col]!.D2;
        var dir1 = from + 1 % 4;
        var region1 = new HashSet<Coords2D>();
        var region2 = new HashSet<Coords2D>();

        do
        {
            var cell1 = currLoc.Move(dir1);
            var cell2 = currLoc.Move(InvertDirection(dir1));

            if (cell1.Row >= 0 && cell1.Row < pipeGrid.Count && cell1.Col >= 0 && cell1.Col < pipeGrid[cell1.Row].Count && pipeGrid[cell1.Row][cell1.Col] == null)
            {
                region1.Add(cell1);
            }

            if (cell2.Row >= 0 && cell2.Row < pipeGrid.Count && cell2.Col >= 0 && cell2.Col < pipeGrid[cell2.Row].Count && pipeGrid[cell2.Row][cell2.Col] == null)
            {
                region2.Add(cell2);
            }

            var to = pipeGrid[currLoc.Row][currLoc.Col]!.Travel(from);

            if (pipeGrid[currLoc.Row][currLoc.Col]!.IsCorner())
            {
                var isLeftTurn = InvertDirection(from).TurnLeft() == to;

                dir1 = isLeftTurn ? dir1.TurnLeft() : dir1.TurnRight();

                cell1 = currLoc.Move(dir1);
                cell2 = currLoc.Move(InvertDirection(dir1));

                if (cell1.Row >= 0 && cell1.Row < pipeGrid.Count && cell1.Col >= 0 && cell1.Col < pipeGrid[cell1.Row].Count && pipeGrid[cell1.Row][cell1.Col] == null)
                {
                    region1.Add(cell1);
                }

                if (cell2.Row >= 0 && cell2.Row < pipeGrid.Count && cell2.Col >= 0 && cell2.Col < pipeGrid[cell2.Row].Count && pipeGrid[cell2.Row][cell2.Col] == null)
                {
                    region2.Add(cell2);
                }
            }

            currLoc = currLoc.Move(to);
            from = InvertDirection(to);
        } while (currLoc != start);

        Grow(region1, pipeGrid);
        Grow(region2, pipeGrid);

        // Two possible answers (inside and outside). Smaller answer is probably correct
        Console.WriteLine(region1.Count);
        Console.WriteLine(region2.Count);
    }

    private static void Grow(HashSet<Coords2D> region, List<List<Pipe?>> grid)
    {
        bool doAgain;

        do
        {
            doAgain = false;
            var toAdd = new List<Coords2D>();

            foreach (var loc in region)
            {
                if (loc.Row > 0 && grid[loc.Row - 1][loc.Col] == null)
                {
                    toAdd.Add((loc.Row - 1, loc.Col));
                }

                if (loc.Col > 0 && grid[loc.Row][loc.Col - 1] == null)
                {
                    toAdd.Add((loc.Row, loc.Col - 1));
                }

                if (loc.Row + 1 < grid.Count && grid[loc.Row + 1][loc.Col] == null)
                {
                    toAdd.Add((loc.Row + 1, loc.Col));
                }

                if (loc.Col + 1 < grid[loc.Row].Count && grid[loc.Row][loc.Col + 1] == null)
                {
                    toAdd.Add((loc.Row, loc.Col + 1));
                }
            }

            foreach (var x in toAdd)
            {
                if (region.Add(x)) doAgain = true;
            }
        } while (doAgain);
    }

    private static Direction InvertDirection(Direction dir)
    {
        return dir.TurnRight(2);
        // return dir switch
        // {
        //     Direction.Up => Direction.Down,
        //     Direction.Down => Direction.Up,
        //     Direction.Left => Direction.Right,
        //     Direction.Right => Direction.Left,
        //     _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        // };
    }

    private static Pipe InferPipe(List<List<Pipe?>> grid, int row, int col)
    {
        var connections = new List<Direction>();

        if (row > 0 && (grid[row - 1][col]?.Has(Direction.Down) ?? false))
        {
            connections.Add(Direction.Up);
        }

        if (row + 1 < grid.Count && (grid[row + 1][col]?.Has(Direction.Up) ?? false))
        {
            connections.Add(Direction.Down);
        }

        if (col > 0 && (grid[row][col - 1]?.Has(Direction.Right) ?? false))
        {
            connections.Add(Direction.Left);
        }

        if (col + 1 < grid[row].Count && (grid[row][col + 1]?.Has(Direction.Left) ?? false))
        {
            connections.Add(Direction.Right);
        }

        return new Pipe(connections[0], connections[1]);
    }

    private static Pipe? GetPipe(char c)
    {
        return c switch
        {
            '|' => new Pipe(Direction.Up, Direction.Down),
            '-' => new Pipe(Direction.Left, Direction.Right),
            'L' => new Pipe(Direction.Up, Direction.Right),
            'J' => new Pipe(Direction.Up, Direction.Left),
            '7' => new Pipe(Direction.Down, Direction.Left),
            'F' => new Pipe(Direction.Down, Direction.Right),
            _ => null
        };
    }

    private static Coords2D GetStartingPosition(List<List<char>> grid)
    {
        for (var row = 0; row < grid.Count; row++)
        {
            for (var col = 0; col < grid[row].Count; col++)
            {
                if (grid[row][col] == 'S') return (row, col);
            }
        }

        throw new Exception("Cannot find start");
    }

    private static Coords2D GetFirstPipe(List<List<Pipe?>> grid)
    {
        for (var row = 0; row < grid.Count; row++)
        {
            for (var col = 0; col < grid[row].Count; col++)
            {
                if (grid[row][col] == null) return (row, col);
            }
        }

        throw new Exception("Cannot find pipe");
    }

    private record Pipe(Direction D1, Direction D2)
    {
        public Direction Travel(Direction from)
        {
            if (from == D1) return D2;
            if (from == D2) return D1;

            throw new ArgumentException($"Cannot enter pipe ({D1} {D2}) from {from}");
        }

        public bool Has(Direction dir) =>
            dir == D1 || dir == D2;

        public bool IsCorner() =>
            !((Has(Direction.Up) && Has(Direction.Down)) || (Has(Direction.Left) && Has(Direction.Right)));
    }
}
