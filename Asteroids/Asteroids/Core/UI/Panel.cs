using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.Core.UI;

public class Panel
{
    private Texture2D _texture;
    private SpriteFont _font;
    private Rectangle _bounds;

    private MouseState _previousMouseState;

    public Action OnClick;
    public Action OnHover;

    private bool _hovered = false;

    public Panel(Texture2D texture, SpriteFont font, Rectangle bounds)
    {
        _texture = texture;
        _font = font;
        _bounds = bounds;
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
        // if (_hovered) {
        //     spriteBatch.Draw(_texture, _bounds, Color.DarkGray);
        //     Mouse.SetCursor(MouseCursor.Hand);
        // } else {
        //     spriteBatch.Draw(_texture, _bounds, Color.White);
        //     Mouse.SetCursor(MouseCursor.Arrow);
        // }
        spriteBatch.Draw(_texture, new Rectangle(_bounds.X + 2, _bounds.Y + 2, _bounds.Width - 4, _bounds.Height - 4), new Color(10, 10, 10, 100));
        spriteBatch.Draw(_texture, _bounds, new Color(10, 10, 10, 100));
    }
}