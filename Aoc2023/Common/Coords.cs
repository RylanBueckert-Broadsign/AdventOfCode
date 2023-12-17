namespace AoC2023.Common;

public record Coords2D(int Row, int Col)
{
    public static implicit operator Coords2D((int, int) tuple) =>
        new Coords2D(tuple.Item1, tuple.Item2);

    public static implicit operator (int, int)(Coords2D coords) =>
        (coords.Row, coords.Col);
};
