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

    public Sprite(Texture2D texture, Vector2 position, int width, int height)
    {
        Texture = texture;
        Position = position;
        Width = width;
        Height = height;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), SpriteColor);
    }
}
