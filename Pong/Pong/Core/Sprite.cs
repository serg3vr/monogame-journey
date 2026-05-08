using System;

namespace Pong.Core;

public class Sprite
{
    public int X;
    public int Y;
    public int W;
    public int H;
    public int Speed;

    public Sprite(int x, int y, int w, int h)
    {
        X = x;
        Y = y;
        W = w;
        H = h;
    }
}
