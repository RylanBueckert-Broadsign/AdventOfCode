using AoC2023.Common;
using AoC2023.Utils;

namespace AoC2023;

public static class Day24
{
    public static void Run()
    {
        var input = InputHelper.ReadLines(@"Day24\input.txt");

        var hail = input
            .Select(i => i.Replace(",", "").Replace("@", "").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList())
            .Select(i => new Hail((i[0], i[1], i[2]), (i[3], i[4], i[5])))
            .ToList();

        var intercepts = hail.Select((h1, i) => hail.Skip(i + 1).Select(h2 => Intercept2D(h1, h2)));

        const double lower = 200000000000000;
        const double upper = 400000000000000;

        var realIntercepts = intercepts.Select(h => h.Where(i => i != null).Select(i => i!.Value));
        var futureIntercepts = realIntercepts.Select(h => h.Where(i => i.t is { t1: >= 0, t2: >= 0 }));
        var testAreaIntercepts = futureIntercepts.Select(h => h.Where(i => i.position.X is >= lower and <= upper && i.position.Y is >= lower and <= upper));

        var result = testAreaIntercepts
            .SelectMany(i => i)
            .Count();

        Console.WriteLine(result);
    }

    private static (Vec2D<double> position, (double t1, double t2) t)? Intercept2D(Hail h1, Hail h2)
    {
        double c1 = h1.Velocity.Y * h1.Position.X - h1.Velocity.X * h1.Position.Y;
        double c2 = h2.Velocity.Y * h2.Position.X - h2.Velocity.X * h2.Position.Y;

        var determinant = h1.Velocity.Y * -h2.Velocity.X - h2.Velocity.Y * -h1.Velocity.X;

        if (determinant == 0)
        {
            return null;
        }

        var x = (-h2.Velocity.X * c1 + h1.Velocity.X * c2) / determinant;
        var y = (h1.Velocity.Y * c2 - h2.Velocity.Y * c1) / determinant;

        var t1 = (x - h1.Position.X) / h1.Velocity.X;
        var t2 = (x - h2.Position.X) / h2.Velocity.X;

        return ((x, y), (t1,t2));
    }

    private record Hail(Vec3D<long> Position, Vec3D<long> Velocity)
    {
    }
}
