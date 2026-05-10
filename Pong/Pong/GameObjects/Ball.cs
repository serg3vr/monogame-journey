using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Core;

namespace Pong.GameObjects;

public class Ball : Sprite
{
    public Vector2 Velocity { get; set; }

    public Ball(Texture2D texture, Vector2 position, int width, int height)
        : base(texture, position, width, height)
    {
        // Speed = 5;
    }

    public void Update(GameTime gameTime)
    {
        // Position = new Vector2(Position.X + Speed, Position.Y);
    }
}