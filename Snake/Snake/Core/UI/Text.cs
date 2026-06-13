using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake.Core.UI;

public class Text
{
    private SpriteFont _font;
    public Vector2 Position { get; set; }
    public Vector2 Scale { get; set; }
    public string Content { get; set; }

    public Text(SpriteFont font, Vector2 position)
    {
        _font = font;
        Position = position;
    }

    public void Update()
    {
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(
            _font, 
            Content, 
            Position, 
            Color.Red, 
            0f, 
            Vector2.Zero, 
            Scale, 
            SpriteEffects.None, 
            0f
        );
    }
}