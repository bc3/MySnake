using System;

namespace MonoSnake.Game;

public enum Direction
{
    North,
    South,
    East,
    West
}

public static class DirectionExtension
{
    public static float ToFloat(this Direction direction)
    {
        switch (direction)
        {
            case Direction.North: return 0;
            case Direction.South: return (float)Math.PI;
            case Direction.East: return (float)Math.PI/2;
            case Direction.West: return (float)-Math.PI/2;
        }

        return 0;
    }
}