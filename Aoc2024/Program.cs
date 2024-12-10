using System.Diagnostics;

namespace Aoc2024;

internal static class Program
{
    public static void Main(string[] args)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        new Day9().Run(@"Day9\sample.txt");
        new Day9().Run(@"Day9\input.txt");

        stopWatch.Stop();
        Console.WriteLine($"Ran in {stopWatch.ElapsedMilliseconds} ms");
    }
}
