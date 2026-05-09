using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.GameObjects;

public class Ball
{
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public int Speed { get; set; }
    public int Width;
    public int Height;
    public Color SpriteColor { get; set; }

    public Vector2 Velocity { get; set; }

    public Ball(Texture2D texture, Vector2 position, int width, int height)
    {
        Texture = texture;
        Position = position;
        Width = width;
        Height = height;
    }

    public Ball(Texture2D texture, Vector2 position, int width, int height, int speed)
    {
        Texture = texture;
        Position = position;
        Width = width;
        Height = height;
        Speed = speed;
        Velocity = new Vector2(Speed, Speed);
    }

    /* public void Update(GameTime gameTime)
    {
        Position += Velocity;

        int top = (int)Position.Y;
        int bottom = (int)Position.Y + Height;
        int right = (int)Position.X + Width;
        int left = (int)Position.X;

        if (top <= 0) {
            _velocity.X = -Speed;
        }
        if (bottom >= 0) {
            _velocity.Y = -Speed;
        }
        if (left <= 0) {
            _velocity.X = Speed;
        }
        if (right >= 0) {
            _velocity.X = -Speed;
        }
    } */

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), SpriteColor);
    }
}
