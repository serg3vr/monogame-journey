using Breakout.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.GameObjects;

public class Brick : Sprite
{
    public Vector2 Velocity { get; set; }
    public float Speed { get; set; }
    public bool Destroyed { get; set; }
    public bool HasPowerUp { get; set; }

    public Brick(Texture2D texture, Vector2 position, Vector2 size)
    : base(texture, position, size)
    {
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Bounds, SpriteColor);
    }
}