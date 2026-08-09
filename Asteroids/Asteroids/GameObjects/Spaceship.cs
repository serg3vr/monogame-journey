using System;
using Asteroids.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.GameObjects;

public class Spaceship : Sprite
{
    public float Speed { get; set; }
    public Vector2 Acceleration { get; set; }
    public float MaxAcceleration { get; set; }
    public float MaxVelocity { get; set; }

    public Spaceship(Texture2D texture, Vector2 position, Vector2 size)
    : base(texture, position, size)
    {
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Bounds, null, SpriteColor, Rotation, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
        DrawDebugBounds(spriteBatch);
    }

    private void DrawDebugBounds(SpriteBatch spriteBatch)
    {
        var topLeft = new Vector2(Bounds.Left, Bounds.Top);
        var topRight = new Vector2(Bounds.Right, Bounds.Top);
        var bottomRight = new Vector2(Bounds.Right, Bounds.Bottom);
        var bottomLeft = new Vector2(Bounds.Left, Bounds.Bottom);

        DrawDebugLine(spriteBatch, topLeft, topRight);
        DrawDebugLine(spriteBatch, topRight, bottomRight);
        DrawDebugLine(spriteBatch, bottomRight, bottomLeft);
        DrawDebugLine(spriteBatch, bottomLeft, topLeft);
        DrawDebugLine(spriteBatch, topLeft, bottomRight);
    }

    private void DrawDebugLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end)
    {
        var direction = end - start;
        spriteBatch.Draw(
            Texture,
            start,
            null,
            Color.LimeGreen,
            MathF.Atan2(direction.Y, direction.X),
            Vector2.Zero,
            new Vector2(direction.Length(), 1f),
            SpriteEffects.None,
            0f
        );
    }
}
