using AoC2023.Common;
using AoC2023.Utils;

namespace AoC2023;

public static class Day17
{
    public static void Run()
    {
        var grid = InputHelper.ReadGrid(@"Day17\input.txt", i => int.Parse(i.ToString()));

        var height = grid.Length;
        var width = grid[0].Length;
        var dest = new Coords2D(height - 1, width - 1);

        var shortestPaths = new Dictionary<Node, int>();
        var candidates = new PriorityQueue<Node, int>();

        candidates.Enqueue(new Node((0, 0), Direction.Right, 0), 0);
        candidates.Enqueue(new Node((0, 0), Direction.Down, 0), 0);

        while (candidates.TryDequeue(out var bestNode, out var shortestPath))
        {
            if (!shortestPaths.TryAdd(bestNode, shortestPath))
            {
                continue;
            }

            //Early Exit
            if (bestNode.Location == dest && bestNode.StraightCount >= 4)
            {
                Console.WriteLine(shortestPath);
                break;
            }

            // New candidates

            // Turn Left
            if (bestNode.StraightCount >= 4)
            {
                var left = bestNode.StraightDirection.TurnLeft();
                var leftLoc = bestNode.Location.Move(left);
                if (IsValidCoords(leftLoc))
                {
                    var leftNode = new Node(leftLoc, left, 1);
                    var leftPath = shortestPath + grid[leftLoc.Row][leftLoc.Col];

                    if (!shortestPaths.ContainsKey(leftNode))
                    {
                        candidates.Enqueue(leftNode, leftPath);
                    }
                }
            }

            // Turn Right
            if (bestNode.StraightCount >= 4)
            {
                var right = bestNode.StraightDirection.TurnRight();
                var rightLoc = bestNode.Location.Move(right);
                if (IsValidCoords(rightLoc))
                {
                    var rightNode = new Node(rightLoc, right, 1);
                    var rightPath = shortestPath + grid[rightLoc.Row][rightLoc.Col];

                    if (!shortestPaths.ContainsKey(rightNode))
                    {
                        candidates.Enqueue(rightNode, rightPath);
                    }
                }
            }

            // Go straight
            if (bestNode.StraightCount < 10)
            {
                var straightLoc = bestNode.Location.Move(bestNode.StraightDirection);
                if (IsValidCoords(straightLoc))
                {
                    var straightNode = new Node(straightLoc, bestNode.StraightDirection, bestNode.StraightCount + 1);
                    var straightPath = shortestPath + grid[straightLoc.Row][straightLoc.Col];

                    if (!shortestPaths.ContainsKey(straightNode))
                    {
                        candidates.Enqueue(straightNode, straightPath);
                    }
                }
            }
        }

        return;

        bool IsValidCoords(Coords2D loc) => loc.Row >= 0 && loc.Row < height && loc.Col >= 0 && loc.Col < width;
    }

    private record Node(Coords2D Location, Direction StraightDirection, int StraightCount);
}
