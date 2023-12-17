namespace AoC2023.Common;

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
        return (Direction)((int)(@this + 4 - times % 4) % 4);
    }

    public static Direction TurnRight(this Direction @this, int times = 1)
    {
        return (Direction)((int)(@this + times) % 4);
    }
}
