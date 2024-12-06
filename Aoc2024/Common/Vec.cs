namespace AoC2024.Common;

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
