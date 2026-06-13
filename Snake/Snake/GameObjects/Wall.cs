using Snake.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake.GameObjects;

public class Wall : Sprite
{
    public Vector2 Velocity { get; set; }
    public float Speed { get; set; }

    public Wall(Texture2D texture, Vector2 position, Vector2 size)
    : base(texture, position, size)
    {
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Bounds, SpriteColor);
    }
}