using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Graphics.Core;

public class Sprite
{
    public Texture2D Texture { get; }
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public Color SpriteColor { get; set; }
    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

    public Sprite(Texture2D texture, Vector2 position, Vector2 size)
    {
        Texture = texture;
        Position = position;
        Size = size;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Bounds, SpriteColor);
    }
}
