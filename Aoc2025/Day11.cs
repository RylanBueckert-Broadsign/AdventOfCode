using Aoc2025.Common;

namespace Aoc2025;

public class Day11 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath);

        var graph = input.Select(l =>
        {
            var node = l[..3];

            var connections = l[5..].Split(' ').ToHashSet();

            return (node, connections);
        }).ToDictionary();

        Console.WriteLine($"Part 1: {CountPaths(graph, "you", "out", new())}");
        
        var cache = new Dictionary<(string start, string end), long>();
        var svrToFft = CountPaths(graph, "svr", "fft", cache);
        var fftToDac = CountPaths(graph, "fft", "dac", cache);
        var dacToOut = CountPaths(graph, "dac", "out", cache);
        
        var svrToDac = CountPaths(graph, "svr", "dac", cache);
        var dacToFft = CountPaths(graph, "dac", "fft", cache);
        var fftToOut = CountPaths(graph, "fft", "out", cache);
        
        var part2Paths = svrToFft * fftToDac * dacToOut + svrToDac * dacToFft * fftToOut;
        
        Console.WriteLine($"Part 2: {part2Paths}");
    }

    private static long CountPaths(Dictionary<string, HashSet<string>> graph, string start, string end, Dictionary<(string start, string end), long> cache)
    {
        if (start == end)
        {
            return 1;
        }

        if (cache.TryGetValue((start, end), out var cachedPaths))
        {
            return cachedPaths;
        }

        if (!graph.TryGetValue(start, out var connections))
            return 0;

        var pathCount = connections.Select(c => CountPaths(graph, c, end, cache)).Sum();
        cache[(start, end)] = pathCount;
        return pathCount;
    }
}
