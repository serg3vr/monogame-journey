using Snake.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake.GameObjects;

public class Body : Sprite
{
    public Vector2 Velocity { get; set; }
    public float Speed { get; set; }
    private Color _border;

    public Body(Texture2D texture, Vector2 position, Vector2 size)
    : base(texture, position, size)
    {
        _border =  new Color(255, 255, 255);
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Bounds, _border);
        spriteBatch.Draw(Texture, new Rectangle(Bounds.X + 2, Bounds.Y + 2, Bounds.Width - 4, Bounds.Height - 4), SpriteColor);
    }
}