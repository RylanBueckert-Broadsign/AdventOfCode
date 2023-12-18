namespace AoC2023.Common;

public record Coords2D(int Row, int Col)
{
    public static implicit operator Coords2D((int, int) tuple) =>
        new Coords2D(tuple.Item1, tuple.Item2);

    public static implicit operator (int, int)(Coords2D coords) =>
        (coords.Row, coords.Col);
};

public static class CoordsExtensions
{
    public static Coords2D Move(this Coords2D curr, Direction dir, int count = 1)
    {
        return dir switch
        {
            Direction.Up => (curr.Row - count, curr.Col),
            Direction.Down => (curr.Row + count, curr.Col),
            Direction.Left => (curr.Row, curr.Col - count),
            Direction.Right => (curr.Row, curr.Col + count),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, "Provided direction is not valid.")
        };
    }
}
