using System.Diagnostics;

namespace AoC2023;

internal static class Program
{
    public static void Main()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        Day18.Run();

        stopWatch.Stop();
        Console.WriteLine($"Ran in {stopWatch.ElapsedMilliseconds} ms");
    }
}
