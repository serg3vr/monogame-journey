using System;
using Asteroids.Core;
using Asteroids.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.GameObjects;

public class Spaceship : Sprite
{
    public Vector2 Acceleration { get; set; }
    public float MaxAcceleration { get; set; }
    public float MaxVelocity { get; set; }
    public bool IsInvulnerable { get; set; } = false;
    public float _maxInvunerableDuration = 2f;
    public float _maxInvunerableTimer = 0;
    private const float BlinkInterval = 1f / 5f;
    public bool _isVisible = true;

    public Spaceship(Texture2D texture, Vector2 position, Vector2 size)
    : base(texture, position, size)
    {
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

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

        if (IsInvulnerable) {
            _maxInvunerableTimer += dt;
            
            if (_maxInvunerableTimer >= _maxInvunerableDuration) {
                IsInvulnerable = false;
                _maxInvunerableTimer = 0f;
            }
        }

        _isVisible = !IsInvulnerable || (int)((_maxInvunerableDuration - _maxInvunerableTimer) / BlinkInterval) % 2 == 0;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (_isVisible) {
            spriteBatch.Draw(Texture, Bounds, null, SpriteColor, Rotation, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
        }
        base.DrawDebug(spriteBatch);
    }
}
