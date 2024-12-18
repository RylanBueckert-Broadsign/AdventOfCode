using Aoc2024.Common;

namespace Aoc2024;

public class Day18 : IAocDay
{
    public void Run(string inputPath)
    {
        const int memorySize = 70;

        var input = InputHelper.ReadLines(inputPath);

        var start = new Vec2D<int>(0, 0);
        var end = new Vec2D<int>(memorySize, memorySize);

        var incomingBytes = input.Select(b =>
        {
            var coords = b.Split(",").Select(int.Parse).ToArray();

            return new Vec2D<int>(coords[0], coords[1]);
        }).ToList();

        var byteTimes = incomingBytes.Select((b, idx) => (b, idx)).ToDictionary(i => i.b, i => i.idx);

        var blockedTime = FindLastPath();

        Console.WriteLine(blockedTime);
        Console.WriteLine(incomingBytes[blockedTime]);

        return;

        int FindLastPath()
        {
            var visited = new HashSet<Vec2D<int>>();
            var queue = new PriorityQueue<Vec2D<int>, int>(Comparer<int>.Create((a, b) => b - a));
            queue.Enqueue(start, int.MaxValue);

            while (queue.TryDequeue(out var pos, out var time))
            {
                if (!visited.Add(pos))
                    continue;

                if (pos == end)
                    return time;

                foreach (var dir in Enum.GetValues<Direction>())
                {
                    var newPos = pos.Move(dir);

                    if (!OutOfBounds(newPos))
                    {
                        queue.Enqueue(newPos, byteTimes.TryGetValue(newPos, out var byteTime) ? Math.Min(time, byteTime) : time);
                    }
                }
            }

            throw new Exception("No path found");
        }

        bool OutOfBounds(Vec2D<int> pos) =>
            pos.X < 0 || pos.Y < 0 || pos.X > memorySize || pos.Y > memorySize;
    }
}
