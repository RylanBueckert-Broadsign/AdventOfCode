using System.Diagnostics;

namespace Aoc2025;

internal static class Program
{
    public static void Main(string[] args)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        new Day3().Run(@"Day3\input.txt");

        stopWatch.Stop();
        Console.WriteLine($"Ran in {stopWatch.ElapsedMilliseconds} ms");
    }
}
