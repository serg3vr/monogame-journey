using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Core;

namespace Pong;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private int _screenWidth;
    private int _screenHeight;

    private Texture2D _square;

    private Sprite _leftPaddle;
    private Sprite _ball;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        _screenWidth = _graphics.GraphicsDevice.Viewport.Width;
        _screenHeight = _graphics.GraphicsDevice.Viewport.Height;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        _square = new Texture2D(GraphicsDevice, 1, 1);
        // _square.SetData(Enumerable.Repeat(Color.White, 400).ToArray());
        _square.SetData(new[] { Color.White });

        _leftPaddle = new Sprite(_square, new Vector2(20, _screenHeight / 2), 20, 100);
        _leftPaddle.Speed = 5;
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState ks = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape)) {
            Exit();
        }

        // TODO: Add your update logic here

        if (ks.IsKeyDown(Keys.W)) {
            _leftPaddle.Position -= new Vector2(0, _leftPaddle.Speed);
        } else if (ks.IsKeyDown(Keys.S)) {
            _leftPaddle.Position += new Vector2(0, _leftPaddle.Speed);
        }

        if (_leftPaddle.Position.Y <= 0) {
            _leftPaddle.Position = new Vector2(_leftPaddle.Position.X, 0);
        }

        if ((_leftPaddle.Position.Y + _leftPaddle.Height) > _screenHeight) {
            _leftPaddle.Position = new Vector2(_leftPaddle.Position.X, _screenHeight - _leftPaddle.Height);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here

        _spriteBatch.Begin();

        // _spriteBatch.Draw(_square, new Rectangle(_leftPaddle.X, _leftPaddle.Y, _leftPaddle.W, _leftPaddle.H), Color.Cyan);
        _leftPaddle.Draw(_spriteBatch);

        _spriteBatch.Draw(_square, new Rectangle(_screenWidth - 20 * 2, _screenHeight / 2 - 20, 20, 100), Color.DarkCyan);

        _spriteBatch.Draw(_square, new Rectangle(_screenWidth / 2, _screenHeight / 2, 20, 20), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
