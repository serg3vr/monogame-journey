using System;
using Asteroids.Core;
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

    public void Update(GameTime gameTime)
    {
        if (Bounds.Bottom < 0) {
            Position = new Vector2(Position.X, Globals.ScreenHeight);
        }

        if (Bounds.Top > Globals.ScreenHeight) {
            Position = new Vector2(Position.X, 0);
        }

        if (Bounds.Right < 0) {
            Position = new Vector2(Globals.ScreenWidth, Position.Y);
        }

        if (Bounds.Left > Globals.ScreenWidth) {
            Position = new Vector2(0, Position.Y);
        }
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Bounds, null, SpriteColor, Rotation, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
    }
}
