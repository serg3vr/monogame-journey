using System;
using Asteroids.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.GameObjects;

public class Spaceship : Sprite
{
    public Vector2 Velocity { get; set; }
    public float Speed { get; set; }
    private Color _debugColor = Color.Green;
    private readonly Texture2D _debugTexture;
    public float Acceleration { get; set; }
    public float Rotation { get; set; }
    public float MaxAcceleration { get; set; }
    public float MaxVelocity { get; set; }

    public Spaceship(Texture2D texture, Vector2 position, Texture2D debugTexture)
    : base(texture, position)
    {
        // _borderColor = Color.LightGreen;
        _debugTexture = debugTexture;
    }

    // public void Update(GameTime gameTime)
    // {
    //     float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
    //     Velocity += Acceleration * dt;
    //     Position += Velocity * dt;
    // }

    public new void Draw(SpriteBatch spriteBatch)
    {
        var origin = new Vector2(Bounds.Width / 2f, Bounds.Height / 2f);
        var size = new Vector2(Bounds.Width, Bounds.Height) * 0.5f;        

        spriteBatch.Draw(Texture, Position, null, SpriteColor, Rotation, origin, 0.5f, SpriteEffects.None, 0f);

        if (false) { // TODO - Add debug var    
            var topLeft = Position - size / 2f;
            var topRight = Position + new Vector2(size.X, -size.Y) / 2f;
            var bottomRight = Position + size / 2f;
            var bottomLeft = Position + new Vector2(-size.X, size.Y) / 2f;

            DrawDebugLine(spriteBatch, topLeft, topRight);
            DrawDebugLine(spriteBatch, topRight, bottomRight);
            DrawDebugLine(spriteBatch, bottomRight, bottomLeft);
            DrawDebugLine(spriteBatch, bottomLeft, topLeft);

            spriteBatch.Draw(_debugTexture, new Rectangle((int)Position.X - 2, (int)Position.Y - 2, 4, 4), _debugColor);
        }
    }

    private void DrawDebugLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end)
    {
        var difference = end - start;
        spriteBatch.Draw(
            _debugTexture,
            start,
            null,
            _debugColor,
            (float)Math.Atan2(difference.Y, difference.X),
            Vector2.Zero,
            new Vector2(difference.Length(), 2f),
            SpriteEffects.None,
            0f
        );
    }
}
