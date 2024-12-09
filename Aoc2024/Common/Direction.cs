namespace Aoc2024.Common;

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}

public static class DirectionExtensions
{
    public static Direction TurnLeft(this Direction @this, int times = 1)
    {
        return @this.TurnRight(4 - times % 4);
    }

    public static Direction TurnRight(this Direction @this, int times = 1)
    {
        return (Direction)((int)(@this + times) % 4);
    }
}
