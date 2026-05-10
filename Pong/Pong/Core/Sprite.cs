using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Core;

public class Sprite
{
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public int Speed { get; set; }
    public int Width;
    public int Height;
    public Color SpriteColor { get; set; }

    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
    public int Top => (int)Position.Y;
    public int Bottom => (int)Position.Y + Height;
    public int Left => (int)Position.X;
    public int Right => (int)Position.X + Width;

    public Sprite(Texture2D texture, Vector2 position, int width, int height)
    {
        Texture = texture;
        Position = position;
        Width = width;
        Height = height;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Bounds, SpriteColor);
    }
}
