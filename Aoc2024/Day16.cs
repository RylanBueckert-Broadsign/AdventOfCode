using Aoc2024.Common;

namespace Aoc2024;

public class Day16 : IAocDay
{
    public void Run(string inputPath)
    {
        var grid = InputHelper.ReadGrid(inputPath);

        Console.WriteLine(LowestCostPathCellsCount());

        return;

        int LowestCostPathCellsCount()
        {
            var start = grid
                .SelectMany((row, x) => row.Select((cell, y) => (cell, x, y))).Where(i => i.cell == 'S')
                .Select(i => new Vec2D<int>(i.x, i.y))
                .First();

            var end = grid
                .SelectMany((row, x) => row.Select((cell, y) => (cell, x, y))).Where(i => i.cell == 'E')
                .Select(i => new Vec2D<int>(i.x, i.y))
                .First();

            Dictionary<(Vec2D<int> pos, Direction dir), (int bestCost, HashSet<(Vec2D<int> pos, Direction dir)> bestParents)> foundPaths = new()
            {
                [(start, Direction.Right)] = (0, new())
            };

            PriorityQueue<((Vec2D<int> pos, Direction dir) self, (Vec2D<int> pos, Direction dir)? parent), int> candidates = new();
            candidates.Enqueue(((start, Direction.Right), null), 0);

            var endCost = int.MaxValue;

            while (candidates.TryDequeue(out var curr, out var cost))
            {
                if (cost > endCost)
                    break;

                switch (grid[curr.self.pos.X][curr.self.pos.Y])
                {
                    case '#':
                        continue;
                    case 'E':
                        endCost = cost;
                        break;
                }

                if (foundPaths.TryGetValue(curr.self, out var found))
                {
                    if (found.bestCost < cost)
                        continue;

                    if (curr.parent is not null)
                        foundPaths[curr.self].bestParents.Add(curr.parent.Value);
                }
                else
                {
                    if (curr.parent is not null)
                        foundPaths[curr.self] = (cost, [curr.parent.Value]);
                }

                candidates.Enqueue(((curr.self.pos.Move(curr.self.dir), curr.self.dir), curr.self), cost + 1);
                candidates.Enqueue(((curr.self.pos, curr.self.dir.TurnRight()), curr.self), cost + 1000);
                candidates.Enqueue(((curr.self.pos, curr.self.dir.TurnLeft()), curr.self), cost + 1000);
            }

            var endPoints = foundPaths.Where(p => p.Key.pos == end).ToList();

            var bestCells = endPoints.Select(ep => ep.Key.pos).ToHashSet();
            Queue<(Vec2D<int> pos, Direction dir)> parentsQueue = new();

            foreach (var parent in endPoints.SelectMany(ep => ep.Value.bestParents))
            {
                parentsQueue.Enqueue(parent);
            }

            while (parentsQueue.TryDequeue(out var parent))
            {
                bestCells.Add(parent.pos);

                foreach (var newParent in foundPaths[parent].bestParents)
                {
                    parentsQueue.Enqueue(newParent);
                }
            }

            return bestCells.Count;
        }
    }
}
