using AoC2023.Common;
using AoC2023.Utils;

namespace AoC2023;

public static class Day22
{
    public static void Run()
    {
        var input = InputHelper.ReadLines(@"Day22\input.txt");

        var bricks = input.Select(ParseBrick).ToList();

        var lowestBricks = bricks.OrderBy(b => Math.Min(b.End1.Z, b.End2.Z)).ToList();

        var heightMap = new Dictionary<Coords2D, int>();

        var supports = new Dictionary<Brick, HashSet<Brick>>();

        foreach (var brick in lowestBricks)
        {
            var lowestZ = Math.Min(brick.End1.Z, brick.End2.Z);

            var below = brick.AllBlocks().Select(b => new Coords2D(b.X, b.Y)).Distinct().ToList();

            var newZ = below.Select(loc => heightMap.TryGetValue(loc, out var hmValue) ? hmValue : 0).Max() + 1;

            brick.MoveDown(lowestZ - newZ);

            foreach (var loc in below)
            {
                heightMap[loc] = Math.Max(brick.End1.Z, brick.End2.Z);
            }

            var belowBlocks = brick.AllBlocks().Select(b => b with { Z = b.Z - 1 });

            var supp = bricks.Where(b => b != brick && belowBlocks.Any(b.Contains)).ToHashSet();

            supports.Add(brick, supp);
        }

        // var cannotRemove = supports.Where(s => s.Value.Count == 1).Select(s => s.Value.Single()).Distinct();
        //
        // Console.WriteLine(bricks.Count - cannotRemove.Count());

        var supportsUp = bricks.ToDictionary(b => b, b => supports.Where(s => s.Value.Contains(b)).Select(s => s.Key).ToHashSet());

        var chainReaction = bricks.Select(disintegrate =>
        {
            var supportCounts = supports.ToDictionary(s => s.Key, s => s.Value.Count > 0 ? s.Value.Count : 1);

            var breaking = new Queue<Brick>();
            breaking.Enqueue(disintegrate);

            while (breaking.TryDequeue(out var smash))
            {
                foreach (var support in supportsUp[smash])
                {
                    supportCounts[support]--;
                    if (supportCounts[support] == 0)
                    {
                        breaking.Enqueue(support);
                    }
                }
            }

            return supportCounts.Count(sc => sc.Value == 0);
        });
        
        Console.WriteLine(chainReaction.Sum());
    }

    private static Brick ParseBrick(string s)
    {
        var r = s.Split('~').Select(i => i.Split(',').Select(int.Parse).ToArray()).ToArray();

        return new Brick((r[0][0], r[0][1], r[0][2]), (r[1][0], r[1][1], r[1][2]));
    }

    private record Brick
    {
        public Coords3D End1 { get; private set; }
        public Coords3D End2 { get; private set; }

        public Brick(Coords3D End1, Coords3D End2)
        {
            this.End1 = End1;
            this.End2 = End2;
        }

        public bool Contains(Coords3D block)
        {
            return IsBetween(block.X, End1.X, End2.X) &&
                IsBetween(block.Y, End1.Y, End2.Y) &&
                IsBetween(block.Z, End1.Z, End2.Z);
        }

        public IEnumerable<Coords3D> AllBlocks()
        {
            for (var x = Math.Min(End1.X, End2.X); x <= Math.Max(End1.X, End2.X); x++)
            {
                for (var y = Math.Min(End1.Y, End2.Y); y <= Math.Max(End1.Y, End2.Y); y++)
                {
                    for (var z = Math.Min(End1.Z, End2.Z); z <= Math.Max(End1.Z, End2.Z); z++)
                    {
                        yield return (x, y, z);
                    }
                }
            }
        }

        public void MoveDown(int times)
        {
            End1 = End1 with { Z = End1.Z - times };
            End2 = End2 with { Z = End2.Z - times };
        }

        private static bool IsBetween(int middle, int end1, int end2) =>
            middle >= end1 && middle <= end2 || middle >= end1 && middle <= end2;
    }
}
