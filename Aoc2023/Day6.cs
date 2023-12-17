using System.Diagnostics;

namespace AoC2023;

public static class Day6
{
    public static void Run()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var time = 40709879;
        var distance = 215105121471005;

        long waysToWin = 0; // Enumerable.Range(0, time).Select(charge => (time - charge) * charge).Count(d => d > distance);

        for (long charge = 0; charge <= time; charge++)
        {
            var d = (time - charge) * charge;

            waysToWin += d > distance ? 1 : 0;
        }

        // var product = waysToWin.Aggregate((curr, next) => curr * next);
        stopWatch.Stop();

        Console.WriteLine(waysToWin);
        Console.WriteLine($"Ran in {stopWatch.ElapsedMilliseconds} ms");
    }
}
