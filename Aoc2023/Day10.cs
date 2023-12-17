using AoC2023.Common;
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

        var startPipe = InferPipe(pipeGrid, start.row, start.col);
        pipeGrid[start.row][start.col] = startPipe;

        var currLoc = start;
        var loopLength = 0;

        var from = pipeGrid[currLoc.row][currLoc.col]!.D2;

        var loopEdge = new List<(int row, int col)>();

        do
        {
            var to = pipeGrid[currLoc.row][currLoc.col]!.Travel(from);

            currLoc = Move(currLoc, to);
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
        from = pipeGrid[currLoc.row][currLoc.col]!.D2;
        var dir1 = from + 1 % 4;
        var region1 = new HashSet<(int row, int col)>();
        var region2 = new HashSet<(int row, int col)>();

        do
        {
            var cell1 = Move(currLoc, dir1);
            var cell2 = Move(currLoc, InvertDirection(dir1));

            if (cell1.row >= 0 && cell1.row < pipeGrid.Count && cell1.col >= 0 && cell1.col < pipeGrid[cell1.row].Count && pipeGrid[cell1.row][cell1.col] == null)
            {
                region1.Add(cell1);
            }

            if (cell2.row >= 0 && cell2.row < pipeGrid.Count && cell2.col >= 0 && cell2.col < pipeGrid[cell2.row].Count && pipeGrid[cell2.row][cell2.col] == null)
            {
                region2.Add(cell2);
            }

            var to = pipeGrid[currLoc.row][currLoc.col]!.Travel(from);

            if (pipeGrid[currLoc.row][currLoc.col]!.IsCorner())
            {
                var isLeftTurn = InvertDirection(from).TurnLeft() == to; //(to - from + 4) % 4 == 1;

                dir1 = isLeftTurn ? dir1.TurnLeft() : dir1.TurnRight(); //(Direction)((int)(dir1 + (isLeftTurn ? 3 : 1)) % 4);

                cell1 = Move(currLoc, dir1);
                cell2 = Move(currLoc, InvertDirection(dir1));

                if (cell1.row >= 0 && cell1.row < pipeGrid.Count && cell1.col >= 0 && cell1.col < pipeGrid[cell1.row].Count && pipeGrid[cell1.row][cell1.col] == null)
                {
                    region1.Add(cell1);
                }

                if (cell2.row >= 0 && cell2.row < pipeGrid.Count && cell2.col >= 0 && cell2.col < pipeGrid[cell2.row].Count && pipeGrid[cell2.row][cell2.col] == null)
                {
                    region2.Add(cell2);
                }
            }

            currLoc = Move(currLoc, to);
            from = InvertDirection(to);
        } while (currLoc != start);

        Grow(region1, pipeGrid);
        Grow(region2, pipeGrid);

        // Two possible answers (inside and outside). Smaller answer is probably correct
        Console.WriteLine(region1.Count);
        Console.WriteLine(region2.Count);
    }

    private static void Grow(HashSet<(int row, int col)> region, List<List<Pipe?>> grid)
    {
        bool doAgain;

        do
        {
            doAgain = false;
            var toAdd = new List<(int row, int col)>();

            foreach (var loc in region)
            {
                if (loc.row > 0 && grid[loc.row - 1][loc.col] == null)
                {
                    toAdd.Add((loc.row - 1, loc.col));
                }

                if (loc.col > 0 && grid[loc.row][loc.col - 1] == null)
                {
                    toAdd.Add((loc.row, loc.col - 1));
                }

                if (loc.row + 1 < grid.Count && grid[loc.row + 1][loc.col] == null)
                {
                    toAdd.Add((loc.row + 1, loc.col));
                }

                if (loc.col + 1 < grid[loc.row].Count && grid[loc.row][loc.col + 1] == null)
                {
                    toAdd.Add((loc.row, loc.col + 1));
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

    private static (int row, int col) Move(this (int row, int col) curr, Direction dir)
    {
        return dir switch
        {
            Direction.Up => (curr.row - 1, curr.col),
            Direction.Down => (curr.row + 1, curr.col),
            Direction.Left => (curr.row, curr.col - 1),
            Direction.Right => (curr.row, curr.col + 1),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
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

    private static (int row, int col) GetStartingPosition(List<List<char>> grid)
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

    private static (int row, int col) GetFirstPipe(List<List<Pipe?>> grid)
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
