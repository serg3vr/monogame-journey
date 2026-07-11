using System;

namespace Asteroids.Core;

public static class Globals
{
    public static int ScreenWidth { get; set; }
    public static int ScreenHeight { get; set; }
    
    public static readonly Random Random = new();
}
