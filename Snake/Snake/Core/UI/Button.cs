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
        if (_hovered) {
            spriteBatch.Draw(_texture, _bounds, Color.DarkGray);
            Mouse.SetCursor(MouseCursor.Hand);
        } else {
            spriteBatch.Draw(_texture, _bounds, Color.White);
            Mouse.SetCursor(MouseCursor.Arrow);
        }

        Vector2 textSize = _font.MeasureString(_name);
        Vector2 textPosition = new Vector2(
            _bounds.Center.X - textSize.X / 2,
            _bounds.Center.Y - textSize.Y / 2
        );

        spriteBatch.DrawString(_font, _name, textPosition, Color.Red);
    }

    // public static Texture2D CreateRoundedRectTexture(GraphicsDevice graphicsDevice, int width, int height, int radius)
    // {
    //     var texture = new Texture2D(graphicsDevice, width, height);
    //     var data = new Color[width * height];
    //     int r2 = radius * radius;
    //     int w = width - 1;
    //     int h = height - 1;
    //     for (int y = 0; y < height; y++) {
    //         for (int x = 0; x < width; x++) {
    //             int dx = 0, dy = 0;
    //             if (x < radius && y < radius) {
    //                 dx = x - radius;
    //                 dy = y - radius;
    //             } else if (x > w - radius && y < radius) {
    //                 dx = x - (w - radius);
    //                 dy = y - radius;
    //             } else if (x < radius && y > h - radius) {
    //                 dx = x - radius;
    //                 dy = y - (h - radius);
    //             } else if (x > w - radius && y > h - radius) {
    //                 dx = x - (w - radius);
    //                 dy = y - (h - radius);
    //             }
    //             bool inside = dx * dx + dy * dy <= r2;
    //             data[y * width + x] = inside ? Color.White : Color.Transparent;
    //         }
    //     }
    //     texture.SetData(data);
    //     return texture;
    // }
}