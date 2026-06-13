using Snake.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake.GameObjects;

public class PowerUp1 : Sprite
{
    public Vector2 Velocity { get; set; }
    public float Speed { get; set; }
    public bool ShouldBeDestroyed { get; set; }

    public PowerUp1(Texture2D texture, Vector2 position, Vector2 size)
    : base(texture, position, size)
    {
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Bounds, new Color(155, 155, 155));
        spriteBatch.Draw(Texture, new Rectangle(Bounds.X + 1, Bounds.Y + 1, Bounds.Width - 2, Bounds.Height - 2), SpriteColor);
    }
}