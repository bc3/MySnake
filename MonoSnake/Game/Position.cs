using System;
using MonoGame.Extended;

namespace MonoSnake.Game;

public record Position
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public Position(int max)
    {
        Random r = new Random();
        X = r.Next(0, max);
        Y = r.Next(0, max);
    }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"[{X}-{Y}]";
    }
}