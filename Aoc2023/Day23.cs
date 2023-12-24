using AoC2023.Common;
using AoC2023.Utils;

namespace AoC2023;

public static class Day23
{
    public static void Run()
    {
        var grid = InputHelper.ReadGrid(@"Day23\input.txt");

        var height = grid.Length;
        var width = grid[0].Length;

        var start = new Coords2D(0, Array.IndexOf(grid[0], '.'));
        var end = new Coords2D(height - 1, Array.IndexOf(grid[height - 1], '.'));

        var graph = CreateGraph(grid);
        PruneGraph(graph);

        var result = LongestPath(graph, graph[start], graph[end], new HashSet<Node>());

        Console.WriteLine(result);
    }

    private class Node
    {
        private readonly Dictionary<Node, int> _edges = new();

        public IReadOnlyDictionary<Node, int> Edges => _edges;

        public void AddEdge(Node to, int weight)
        {
            _edges.Add(to, weight);
        }

        public void RemoveEdge(Node to)
        {
            if (!_edges.Remove(to))
                throw new Exception("Could not remove edge");
        }
    }

    private static Dictionary<Coords2D, Node> CreateGraph(char[][] grid)
    {
        var graph = grid
            .SelectMany((row, rowIdx) => row.Select((c, colIdx) => new { coords = new Coords2D(rowIdx, colIdx), c }))
            .Where(i => i.c != '#')
            .ToDictionary(i => i.coords, _ => new Node());

        foreach (var node in graph)
        {
            var options = Enum.GetValues<Direction>()
                .Select(d => node.Key.Move(d));

            foreach (var opt in options)
            {
                if (graph.TryGetValue(opt, out var connectedNode))
                {
                    node.Value.AddEdge(connectedNode, 1);
                }
            }
        }

        return graph;
    }

    private static void PruneGraph(Dictionary<Coords2D, Node> graph)
    {
        foreach (var node in graph)
        {
            if (node.Value.Edges.Count == 2)
            {
                var edge1 = node.Value.Edges.First();
                var edge2 = node.Value.Edges.Last();

                var combinedWeight = edge1.Value + edge2.Value;

                edge1.Key.AddEdge(edge2.Key, combinedWeight);
                edge2.Key.AddEdge(edge1.Key, combinedWeight);

                edge1.Key.RemoveEdge(node.Value);
                edge2.Key.RemoveEdge(node.Value);

                graph.Remove(node.Key);
            }
        }
    }

    private static int? LongestPath(Dictionary<Coords2D, Node> graph, Node start, Node end, HashSet<Node> visited)
    {
        if (start == end)
            return 0;

        visited.Add(start);

        var options = start.Edges
            .Where(i => !visited.Contains(i.Key))
            .Select(i => i.Value + LongestPath(graph, i.Key, end, visited))
            .Where(i => i.HasValue)
            .Select(i => i!.Value)
            .ToList();

        visited.Remove(start);

        if (!options.Any())
            return null;

        return options.Max();
    }
}
