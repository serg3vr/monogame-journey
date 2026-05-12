using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Core;
using Pong.GameObjects;

namespace Pong;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private int _screenWidth;
    private int _screenHeight;

    private Texture2D _square;

    private Sprite _leftPaddle;
    private Sprite _rightPaddle;
    private Ball _ball;
    private const int PIXEL_WIDTH = 20;

    private SpriteFont _spriteFont;

    private int _leftScore;
    private Vector2 _leftScorePosition;
    private int _rightScore;
    private Vector2 _rightScorePosition;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;

        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        _screenWidth = _graphics.GraphicsDevice.Viewport.Width;
        _screenHeight = _graphics.GraphicsDevice.Viewport.Height;

        _leftScore = 0;
        _rightScore = 0;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        _square = new Texture2D(GraphicsDevice, 1, 1);
        // _square.SetData(Enumerable.Repeat(Color.White, 400).ToArray());
        _square.SetData(new[] { Color.White });

        _leftPaddle = new Sprite(_square, new Vector2(PIXEL_WIDTH * 2, _screenHeight / 2), PIXEL_WIDTH, 100);
        _leftPaddle.Speed = 800;
        _leftPaddle.SpriteColor = Color.Cyan;

        _rightPaddle = new Sprite(_square, new Vector2(_screenWidth - PIXEL_WIDTH * 3, _screenHeight / 2), PIXEL_WIDTH, 100);
        _rightPaddle.Speed = 400;
        _rightPaddle.SpriteColor = Color.DarkCyan;

        _ball = new Ball(_square, new Vector2(_screenWidth / 2, _screenHeight / 2), PIXEL_WIDTH, PIXEL_WIDTH);
        _ball.Speed = 800;
        _ball.SpriteColor = Color.White;
        _ball.Velocity = new Vector2(_ball.Speed, _ball.Speed);

        _spriteFont = Content.Load<SpriteFont>("myfont");
        _leftScorePosition = new Vector2(_screenWidth / 2 - PIXEL_WIDTH * 3, PIXEL_WIDTH * 3);
        _rightScorePosition = new Vector2(_screenWidth / 2 + PIXEL_WIDTH * 3, PIXEL_WIDTH * 3);

    }

    protected override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        KeyboardState ks = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape)) {
            Exit();
        }

        // TODO: Add your update logic here

        if (ks.IsKeyDown(Keys.W)) {
            _leftPaddle.Position -= new Vector2(0, _leftPaddle.Speed) * dt;
        } else if (ks.IsKeyDown(Keys.S)) {
            _leftPaddle.Position += new Vector2(0, _leftPaddle.Speed) * dt;
        }

        _leftPaddle.Position = new Vector2(
            _leftPaddle.Position.X,
            MathHelper.Clamp(_leftPaddle.Position.Y, 0, _screenHeight - _leftPaddle.Height)
        );

        if (_ball.Position.Y < _rightPaddle.Position.Y) {
            _rightPaddle.Position -= new Vector2(0, _rightPaddle.Speed) * dt;
        } else if (_ball.Position.Y > _rightPaddle.Position.Y) {
            _rightPaddle.Position += new Vector2(0, _rightPaddle.Speed) * dt;
        }

        _rightPaddle.Position = new Vector2(
            _rightPaddle.Position.X,
            MathHelper.Clamp(_rightPaddle.Position.Y, 0, _screenHeight - _rightPaddle.Height)
        );

        _ball.Position += _ball.Velocity * dt;

        int top = (int)_ball.Position.Y;
        int bottom = (int)_ball.Position.Y + _ball.Height;
        int right = (int)_ball.Position.X + _ball.Width;
        int left = (int)_ball.Position.X;

        if (top <= 0) {
            _ball.Velocity = new Vector2(_ball.Velocity.X, _ball.Speed);
        }
        if (bottom >= _screenHeight) {
            _ball.Velocity = new Vector2(_ball.Velocity.X, -_ball.Speed);
        }
        if (left <= 0) {
            _ball.Velocity = new Vector2(_ball.Speed, _ball.Velocity.Y);
            _rightScore++;
        }
        if (right >= _screenWidth) {
            _ball.Velocity = new Vector2(-_ball.Speed, _ball.Velocity.Y);
            _leftScore++;
        }

        Rectangle ballRect = new Rectangle((int)_ball.Position.X, (int)_ball.Position.Y, _ball.Width, _ball.Height);

        if (_leftPaddle.Bounds.Intersects(ballRect)) {
            if (ballRect.X < _leftPaddle.Right) {
                _ball.Velocity = new Vector2(_ball.Speed, _ball.Velocity.Y);
            }
        }

        if (_rightPaddle.Bounds.Intersects(ballRect)) {
            if (ballRect.X + ballRect.Width > _rightPaddle.Left) {
                _ball.Velocity = new Vector2(-_ball.Speed, _ball.Velocity.Y);
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp);

        Color objectsColor = new Color(100, 100, 100);

        int size = _screenHeight / 20;

        for (int i = 0; i <= size; i++) {
            _spriteBatch.Draw(_square, new Rectangle(_screenWidth / 2, i * size, PIXEL_WIDTH / 2, PIXEL_WIDTH), objectsColor);
        }

        _leftPaddle.Draw(_spriteBatch);

        _rightPaddle.Draw(_spriteBatch);

        _ball.Draw(_spriteBatch);        

        string leftText = _leftScore.ToString();
        Vector2 fontOrigin = _spriteFont.MeasureString(leftText) / 2;
        _spriteBatch.DrawString(_spriteFont, leftText, _leftScorePosition, objectsColor, 0, fontOrigin, 5.0f, SpriteEffects.None, 0.5f);

        string rightText = _rightScore.ToString();
        Vector2 fontOriginRight = _spriteFont.MeasureString(rightText) / 2;
        _spriteBatch.DrawString(_spriteFont, rightText, _rightScorePosition, objectsColor, 0, fontOriginRight, 5.0f, SpriteEffects.None, 0.5f);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
