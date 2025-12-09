using Aoc2025.Common;

namespace Aoc2025;

public class Day8 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath);

        var junctionBoxes = input
            .Select(line => line.Split(",", StringSplitOptions.TrimEntries))
            .Select(parts => parts.Select(long.Parse).ToArray())
            .Select(parts => new JunctionBox((parts[0], parts[1], parts[2])))
            .ToList();

        var possibleConnections = junctionBoxes
            .SelectMany((first, fIdx) =>
            {
                return junctionBoxes
                    .Skip(fIdx + 1)
                    .Select(second => (first, second, distance: SquareDistance(first, second)));
            })
            .OrderBy(c => c.distance)
            .ToList();

        var totalNetworks = junctionBoxes.Count;

        foreach (var connection in possibleConnections)
        {
            var (first, second, _) = connection;

            if (first.Network != second.Network)
            {
                JoinNetworks(first, second);
                totalNetworks--;

                if (totalNetworks == 1)
                {
                    Console.WriteLine("All junction boxes are now connected.");
                    Console.WriteLine(first.Position.X * second.Position.X);
                    break;
                }
            }
        }
    }

    private static long SquareDistance(JunctionBox first, JunctionBox second)
    {
        var deltaX = second.Position.X - first.Position.X;
        var deltaY = second.Position.Y - first.Position.Y;
        var deltaZ = second.Position.Z - first.Position.Z;

        return deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ;
    }

    private class JunctionBox
    {
        public JunctionBox(Vec3D<long> position)
        {
            Position = position;
            Network = [this];
        }

        public Vec3D<long> Position { get; }

        public HashSet<JunctionBox> Network { get; set; }
    }

    private static void JoinNetworks(JunctionBox first, JunctionBox second)
    {
        if (first.Network == second.Network)
        {
            Console.WriteLine("Already in the same network");
            return;
        }

        var (larger, smaller) = first.Network.Count >= second.Network.Count ? (first.Network, second.Network) : (second.Network, first.Network);

        foreach (var box in smaller)
        {
            larger.Add(box);
            box.Network = larger;
        }
    }
}
