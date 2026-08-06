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
        spriteBatch.Draw(Texture, Position, null, SpriteColor, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
    }
}