using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Graphics.Core;

public class Sprite
{
    public static bool DebugMode { get; set; } = true;
    public static Texture2D DebugPixel { get; set; }
    private static readonly Color DebugColor = Color.LimeGreen;

    public Texture2D Texture { get; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Speed  { get; set; }
    public float Rotation { get; set; }
    public Vector2 Size { get; set; }
    public Color SpriteColor { get; set; } = Color.White;
    public Rectangle Bounds => new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

    public Sprite(Texture2D texture, Vector2 position, Vector2 size)
    {
        Texture = texture;
        Position = position;
        Size = size;
    }

    public Sprite(Texture2D texture, Vector2 position)
    {
        Texture = texture;
        Position = position;
        Size = new Vector2(texture.Width, texture.Height);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Bounds, SpriteColor);
        DrawDebug(spriteBatch);
    }

    protected void DrawDebug(SpriteBatch spriteBatch)
    {
        if (!DebugMode || DebugPixel is null) return;

        var bounds = Bounds;

        spriteBatch.Draw(DebugPixel, new Rectangle(bounds.X, bounds.Y, bounds.Width, 1), DebugColor);
        spriteBatch.Draw(DebugPixel, new Rectangle(bounds.X, bounds.Bottom, bounds.Width, 1), DebugColor);
        spriteBatch.Draw(DebugPixel, new Rectangle(bounds.X, bounds.Y, 1, bounds.Height), DebugColor);
        spriteBatch.Draw(DebugPixel, new Rectangle(bounds.Right, bounds.Y, 1, bounds.Height), DebugColor);

        var start = new Vector2(bounds.Left, bounds.Bottom);
        var end = new Vector2(bounds.Right, bounds.Top);
        var delta = end - start;
        var angle = MathF.Atan2(delta.Y, delta.X);
        var origin = new Vector2(0.5f, 0.5f);

        spriteBatch.Draw(
            DebugPixel,
            start + delta * 0.5f,
            null,
            DebugColor,
            angle,
            origin,
            new Vector2(delta.Length(), 1f),
            SpriteEffects.None,
            0f
        );
    }
}
