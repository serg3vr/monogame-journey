using System;
using Asteroids.Core;
using Asteroids.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.GameObjects;

public class Pipe : Sprite
{
    public Vector2 Velocity { get; set; }
    public float Speed { get; set; }
    private Color _border;
    public bool IsScoreable { get; set; }

    public Pipe(Texture2D texture, Vector2 position, Vector2 size)
    : base(texture, position, size)
    {
        _border =  new Color(255, 255, 255);
        Velocity = new Vector2(-1, 0);
        Speed = 100f;
        IsScoreable = true;
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position += Velocity * Speed * dt;

        // if (Position.X < 0) {
        //     Position = new Vector2(Globals.ScreenWidth, Position.Y);
        // }

        if (Bounds.Right < 0) {
            IsScoreable = true;
        }
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Bounds, _border);
        spriteBatch.Draw(Texture, new Rectangle(Bounds.X + 2, Bounds.Y + 2, Bounds.Width - 4, Bounds.Height - 4), SpriteColor);
    }
}