using System;
using Asteroids.Core;
using Asteroids.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.GameObjects;

public class Bullet : Sprite
{
    private int _lifetime = 2;
    private float _lifetimeCounter = 1;
    public bool ShouldBeDeleted { get; set; }

    public Bullet(Texture2D texture, Vector2 position, Vector2 size, float rotation)
    : base(texture, position, size)
    {
        Rotation = rotation;
        ShouldBeDeleted = false;
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_lifetimeCounter >= _lifetime) {
            ShouldBeDeleted = true;
        } else {
            _lifetimeCounter += dt;
        }

        var direction = Vector2.Transform(Direction.Up, Matrix.CreateRotationZ(Rotation));
        Position += direction * Speed * dt;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, new Rectangle(Bounds.X + 2, Bounds.Y + 2, Bounds.Width - 4, Bounds.Height - 4), SpriteColor);
        base.DrawDebug(spriteBatch);
    }
}