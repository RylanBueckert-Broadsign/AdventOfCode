using AoC2023.Utils;

namespace AoC2023;

public static class Day8
{
    public static void Run()
    {
        var lines = InputHelper.ReadLines(@"Day8\input.txt").ToList();

        var directions = lines.First();

        var graphInput = lines.Skip(2);

        var graph = ParseGraph(graphInput);

        var startingLocations = graph.Select(i => i.Key).Where(i => i.EndsWith('A'));

        var steps = startingLocations.Select(sl => (long)Traverse(graph, sl, loc => loc.EndsWith('Z'), directions));

        // var steps = Traverse(graph, startingLocations, loc => loc.EndsWith('Z'), directions);

        var lcm = steps.Aggregate((long)1, Lcm);

        Console.WriteLine(lcm);
    }

    private static long Gcf(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    private static long Lcm(long a, long b)
    {
        return (a / Gcf(a, b)) * b;
    }

    private static Dictionary<string, (string left, string right)> ParseGraph(IEnumerable<string> rawGraph)
    {
        return rawGraph.Select(str =>
        {
            var node = str[0..3];
            var left = str[7..10];
            var right = str[12..15];

            return new { node, left, right };
        }).ToDictionary(i => i.node, i => (i.left, i.right));
    }

    private static int Traverse(IReadOnlyDictionary<string, (string left, string right)> graph, string startLocation, Func<string, bool> endFunc, string instructions)
    {
        var location = startLocation;
        for (var step = 0;; step++)
        {
            if (endFunc(location))
            {
                return step;
            }

            var direction = instructions[step % instructions.Length];

            location = direction == 'L' ? graph[location].left : graph[location].right;
        }
    }
}
