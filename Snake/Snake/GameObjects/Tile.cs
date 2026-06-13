using Snake.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake.GameObjects;

public class Tile : Sprite
{
    public Vector2 Velocity { get; set; }
    public float Speed { get; set; }
    public bool Destroyed { get; set; }

    public Tile(Texture2D texture, Vector2 position, Vector2 size)
    : base(texture, position, size)
    {
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Bounds, SpriteColor);
    }
}