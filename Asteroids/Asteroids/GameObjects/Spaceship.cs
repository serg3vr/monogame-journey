using Asteroids.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.GameObjects;

public class Spaceship : Sprite
{
    public Vector2 Velocity { get; set; }
    public float Speed { get; set; }
    private Color _border;
    public Vector2 Acceleration { get; set; }

    public Spaceship(Texture2D texture, Vector2 position)
    : base(texture, position)
    {
        _border = new Color(255, 255, 255);
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Velocity += Acceleration * dt;
        Position += Velocity * dt;
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        // spriteBatch.Draw(Texture, Bounds, _border);
        // spriteBatch.Draw(Texture, new Rectangle(Bounds.X + 2, Bounds.Y + 2, Bounds.Width - 4, Bounds.Height - 4), SpriteColor);

        spriteBatch.Draw(Texture, Position, SpriteColor);
    }
}