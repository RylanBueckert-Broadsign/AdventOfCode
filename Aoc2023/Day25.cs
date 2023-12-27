using AoC2023.Utils;

namespace AoC2023;

public static class Day25
{
    public static void Run()
    {
        var input = InputHelper.ReadLines(@"Day25\input.txt");
        var graph = GenerateGraph(input);

        var inside = graph.Nodes.First();
        var outside = graph.Nodes.First(n => MaxFlow(graph, inside, n) <= 3);
        for (var i = 0; i < 3; i++)
        {
            FindPath(graph, inside, outside, (_, _) => true, out var parent);
            
            for (var to = outside; to != inside; to = parent[to])
            {
                graph.RemoveEdge(parent[to], to);
            }
        }
        
        // var flow = graph.Nodes.Select((n, i) =>
        // {
        //     Console.WriteLine(i);
        //     return MaxFlow(graph, inside, n);
        // }).ToList();

        var insideNodes = new HashSet<string>();
        var queue = new Queue<string>();
        queue.Enqueue(inside);
        insideNodes.Add(inside);

        while (queue.TryDequeue(out var currentNode))
        {
            foreach (var edge in graph.GetEdges(currentNode))
            {
                if (!insideNodes.Contains(edge))
                {
                    queue.Enqueue(edge);
                    insideNodes.Add(edge);
                }
            }
        }

        var group1 = insideNodes.Count;
        var group2 = graph.Nodes.Count() - group1;
        Console.WriteLine(group1 * group2);
    }

    private static int MaxFlow(UndirectedGraph graph, string source, string sink)
    {
        if (source == sink)
        {
            return int.MaxValue;
        }

        var residualCapacity = graph.Nodes
            .ToDictionary(
                from => from,
                from =>
                {
                    var edges = graph.GetEdges(from);
                    return graph.Nodes.ToDictionary(to => to, to => edges.Contains(to) ? 1 : 0);
                });

        var maxFlow = 0;

        while (FindPath(graph, source, sink, (f, t) => residualCapacity[f][t] > 0, out var parent))
        {
            var pathFlow = int.MaxValue;

            for (var to = sink; to != source; to = parent[to])
            {
                var from = parent[to];
                pathFlow = Math.Min(pathFlow, residualCapacity[from][to]);
            }

            for (var to = sink; to != source; to = parent[to])
            {
                var from = parent[to];
                residualCapacity[from][to] -= pathFlow;
                residualCapacity[to][from] += pathFlow;
            }

            maxFlow += pathFlow;
        }

        return maxFlow;
    }

    private static bool FindPath(UndirectedGraph graph, string source, string sink, Func<string, string, bool> allowedFunc, out Dictionary<string, string> parent)
    {
        parent = new Dictionary<string, string>();

        var visited = new HashSet<string>();

        var queue = new Queue<string>();
        queue.Enqueue(source);
        visited.Add(source);

        while (queue.TryDequeue(out var currentNode))
        {
            foreach (var edge in graph.GetEdges(currentNode))
            {
                if (!visited.Contains(edge) && allowedFunc(currentNode, edge))
                {
                    parent[edge] = currentNode;

                    if (edge == sink)
                        return true;

                    queue.Enqueue(edge);
                    visited.Add(edge);
                }
            }
        }

        return false;
    }

    private static UndirectedGraph GenerateGraph(IEnumerable<string> input)
    {
        return input
            .Select(i =>
            {
                var x = i.Split(':', StringSplitOptions.TrimEntries);

                var from = x[0];
                var to = x[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                return new { from, to };
            })
            .Aggregate(new UndirectedGraph(), (g, next) =>
            {
                foreach (var to in next.to)
                {
                    g.AddEdge(next.from, to);
                }

                return g;
            });
    }

    private class UndirectedGraph
    {
        private readonly Dictionary<string, Node> _nodes = [];

        public IEnumerable<string> Nodes => _nodes.Keys;

        public void AddEdge(string from, string to)
        {
            GetOrCreateNode(from).AddEdge(to);
            GetOrCreateNode(to).AddEdge(from);
        }

        public void RemoveEdge(string from, string to)
        {
            GetOrCreateNode(from).RemoveEdge(to);
            GetOrCreateNode(to).RemoveEdge(from);
        }

        public IEnumerable<string> GetEdges(string node)
        {
            return GetOrCreateNode(node).Edges;
        }

        private Node GetOrCreateNode(string name)
        {
            if (_nodes.TryGetValue(name, out var existingNode))
            {
                return existingNode;
            }

            var newNode = new Node();
            _nodes.Add(name, newNode);
            return newNode;
        }

        private class Node
        {
            private readonly HashSet<string> _edges = [];

            public IEnumerable<string> Edges => _edges;

            public void AddEdge(string to)
            {
                _edges.Add(to);
            }

            public void RemoveEdge(string to)
            {
                _edges.Remove(to);
            }
        }
    }
}
