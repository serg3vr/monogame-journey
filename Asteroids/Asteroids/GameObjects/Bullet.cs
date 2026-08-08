using System;
using Asteroids.Core;
using Asteroids.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.GameObjects;

public class Bullet : Sprite
{
    // public float Speed { get ; set; }
    // public Vector2 Direction { get; set; }

    public Bullet(Texture2D texture, Vector2 position, Vector2 size)
    : base(texture, position, size)
    {
        Velocity = new Vector2(-1, 0);
        // Speed = 100f;

    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position += Velocity * dt;
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, new Rectangle(Bounds.X + 2, Bounds.Y + 2, Bounds.Width - 4, Bounds.Height - 4), SpriteColor);
    }
}