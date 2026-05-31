using Breakout.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.GameObjects;

public class Ball : Sprite
{
    public Vector2 Velocity { get; set; }
    public float Speed { get; set; }

    private Texture2D _circle;

    public Ball(Texture2D texture, Vector2 position, Vector2 size)
    : base(texture, position, size)
    {
        var diameter = (int)size.X;
        var radius = diameter / 2f;
        _circle = new Texture2D(texture.GraphicsDevice, diameter, diameter);
        var data = new Color[diameter * diameter];

        for (var y = 0; y < diameter; y++)
        {
            for (var x = 0; x < diameter; x++)
            {
                var dx = x - radius;
                var dy = y - radius;
                data[y * diameter + x] = dx * dx + dy * dy <= radius * radius
                    ? Color.White
                    : Color.Transparent;
            }
        }

        _circle.SetData(data);
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_circle, Bounds, SpriteColor);
    }
}