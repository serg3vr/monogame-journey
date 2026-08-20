using System;
using Asteroids.Core;
using Asteroids.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.GameObjects;

public class Asteroid : Sprite
{
    public bool ShouldBeDeleted { get; set; }
    public float RotationSpeed { get; set; } = 1f;
    public int Health = 2;

    public Asteroid(Texture2D texture, Vector2 position, Vector2 size, float rotation)
    : base(texture, position, size)
    {
        Rotation = rotation;
        ShouldBeDeleted = false;

        var direction = Vector2.Transform(Direction.Up, Matrix.CreateRotationZ(Rotation));
        Velocity = direction;
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Rotation += RotationSpeed * dt;
        Position += Velocity * Speed * dt;
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Bounds, null, SpriteColor, Rotation, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
    }
}