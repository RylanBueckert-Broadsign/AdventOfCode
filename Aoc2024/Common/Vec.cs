using System.Numerics;

namespace Aoc2024.Common;

public record Vec2D<T>(T X, T Y)
{
    public static implicit operator Vec2D<T>((T, T) tuple) =>
        new Vec2D<T>(tuple.Item1, tuple.Item2);

    public static implicit operator (T, T)(Vec2D<T> vec) =>
        (vec.X, vec.Y);
};

public record Vec3D<T>(T X, T Y, T Z)
{
    public static implicit operator Vec3D<T>((T, T, T) tuple) =>
        new Vec3D<T>(tuple.Item1, tuple.Item2, tuple.Item3);

    public static implicit operator (T, T, T)(Vec3D<T> vec) =>
        (vec.X, vec.Y, vec.Z);
};

public static class VecExtensions
{
    public static Vec2D<int> Move(this Vec2D<int> curr, Direction dir, int count = 1)
    {
        return dir switch
        {
            Direction.Up => (curr.X - count, curr.Y),
            Direction.Down => (curr.X + count, curr.Y),
            Direction.Left => (curr.X, curr.Y - count),
            Direction.Right => (curr.X, curr.Y + count),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, "Provided direction is not valid.")
        };
    }

    public static Vec2D<T> Add<T>(this Vec2D<T> a, Vec2D<T> b) where T : IAdditionOperators<T, T, T> =>
        (a.X + b.X, a.Y + b.Y);

    public static Vec2D<T> Subtract<T>(this Vec2D<T> a, Vec2D<T> b) where T : ISubtractionOperators<T, T, T> =>
        (a.X - b.X, a.Y - b.Y);

    public static Vec2D<T> Multiply<T>(this Vec2D<T> a, T b) where T : IMultiplyOperators<T, T, T> =>
        (a.X * b, a.Y * b);
}
