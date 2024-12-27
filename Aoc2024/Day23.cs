using System.Diagnostics.CodeAnalysis;
using Aoc2024.Common;

namespace Aoc2024;

public class Day23 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath);

        var graph = new Dictionary<string, HashSet<string>>();

        foreach (var line in input.Select(i => i.Split('-')))
        {
            CreateEdge(line[0], line[1]);
        }

        // Part 1
        var triangles = new List<(string, string, string)>();

        foreach (var (node, edges) in graph)
        {
            foreach (var edge in edges)
            {
                var common = edges.Intersect(graph[edge]);

                triangles.AddRange(common.Select(c => (node, edge, c)));
            }
        }

        var tTriangles = triangles.Where(t => t.Item1.StartsWith('t') || t.Item2.StartsWith('t') || t.Item3.StartsWith('t'));

        Console.WriteLine(tTriangles.Count() / 6);

        // Part 2
        List<HashSet<string>> biggestNetworks = new();
        BronKerbosch(new HashSet<string>(), graph.Keys.ToHashSet(), new HashSet<string>());

        var biggestNetwork = biggestNetworks.MaxBy(n => n.Count)!;
        
        var password = string.Join(',', biggestNetwork.Order());

        Console.WriteLine(password);
        return;

        void BronKerbosch(HashSet<string> r, HashSet<string> p, HashSet<string> x)
        {
            if (p.Count == 0 && x.Count == 0)
            {
                biggestNetworks.Add(r);
                return;
            }

            foreach (var v in p)
            {
                var newR = new HashSet<string>(r) { v };
                var newP = p.Intersect(graph[v]).ToHashSet();
                var newX = x.Intersect(graph[v]).ToHashSet();

                BronKerbosch(newR, newP, newX);

                p.Remove(v);
                x.Add(v);
            }
        }

        void CreateEdge(string from, string to)
        {
            if (graph.TryGetValue(from, out var set1))
            {
                set1.Add(to);
            }
            else
            {
                graph[from] = [to];
            }

            if (graph.TryGetValue(to, out var set2))
            {
                set2.Add(from);
            }
            else
            {
                graph[to] = [from];
            }
        }
    }
}
