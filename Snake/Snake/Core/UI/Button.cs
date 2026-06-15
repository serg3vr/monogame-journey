using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake.Core.UI;

public class Button
{
    private Texture2D _texture;
    private SpriteFont _font;
    private Rectangle _bounds;
    private string _name;

    private MouseState _previousMouseState;

    public Action OnClick;
    public Action OnHover;

    private bool _hovered = false;

    public Button(Texture2D texture, SpriteFont font, Rectangle bounds, string name)
    {
        _texture = texture;
        _font = font;
        _bounds = bounds;
        _name = name;
    }

    public void Update()
    {
        MouseState currentMouseState = Mouse.GetState();
        _hovered = _bounds.Contains(currentMouseState.Position);

        bool clicked = _hovered &&
            currentMouseState.LeftButton == ButtonState.Released
            && _previousMouseState.LeftButton == ButtonState.Pressed;

        if (clicked) {
            OnClick.Invoke();
        }

        _previousMouseState = currentMouseState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // var prevCursor = MouseCursor.Arrow
        var b = 1;
        // spriteBatch.Draw(_texture, new Rectangle(_bounds.Top - b, _bounds.Left - b, _bounds.Width + b * 2, _bounds.Height + b * 2), Color.Red);


        // spriteBatch.Draw(Texture, new Rectangle(Bounds.X + 2, Bounds.Y + 2, Bounds.Width - 4, Bounds.Height - 4), SpriteColor);

        spriteBatch.Draw(_texture, _bounds, new Color(55, 71, 79));

        if (_hovered) {
            spriteBatch.Draw(_texture, new Rectangle(_bounds.X + 2, _bounds.Y + 2, _bounds.Width - 4, _bounds.Height - 4), new Color(55, 71, 79));
            Mouse.SetCursor(MouseCursor.Hand);
        } else {
            spriteBatch.Draw(_texture, new Rectangle(_bounds.X + 2, _bounds.Y + 2, _bounds.Width - 4, _bounds.Height - 4), new Color(38, 50, 56));
            Mouse.SetCursor(MouseCursor.Arrow);
        }

        Vector2 textSize = _font.MeasureString(_name);
        Vector2 textPosition = new Vector2(
            _bounds.Center.X - textSize.X / 2,
            _bounds.Center.Y - textSize.Y / 2
        );
        
        spriteBatch.DrawString(_font, _name, textPosition, Color.White);
    }
}