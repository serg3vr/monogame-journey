using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Core;
using Pong.Core.Scenes;
using Pong.GameObjects;

namespace Pong.Scenes;

public class GameScene : Scene
{
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

    // private Vector2 _ballVelocity;
    private float _ballAngle;

    private bool _isPause = false;
    private float _pauseTimer = 0;

    public GameScene(
        ContentManager content,
        GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch,
        SceneManager sceneManager)
        : base(content, graphicsDevice, spriteBatch, sceneManager)
    {
    }

    public override void Initialize()
    {
        _screenWidth = GraphicsDevice.Viewport.Width;
        _screenHeight = GraphicsDevice.Viewport.Height;
    }

    public override void LoadContent()
    {
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
        _ball.Speed = 600;
        _ball.SpriteColor = Color.White;
        _ball.Velocity = new Vector2(MathF.Cos(MathHelper.ToRadians(_ballAngle)), MathF.Sin(MathHelper.ToRadians(_ballAngle))) * _ball.Speed;

        _spriteFont = ContentManager.Load<SpriteFont>("myfont");
        _leftScorePosition = new Vector2(_screenWidth / 2 - PIXEL_WIDTH * 3, PIXEL_WIDTH * 3);
        _rightScorePosition = new Vector2(_screenWidth / 2 + PIXEL_WIDTH * 3, PIXEL_WIDTH * 3);
    }

    public override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        KeyboardState ks = Keyboard.GetState();

        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape)) {
        //     Exit();
        // }
        
        if (_isPause) {
            _pauseTimer += dt;

            if (_pauseTimer >= 1) {
                _isPause = false;
                _pauseTimer = 0;
            }
            return;
        }

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

        int top = (int)_ball.Position.Y;
        int bottom = (int)_ball.Position.Y + _ball.Height;
        int right = (int)_ball.Position.X + _ball.Width;
        int left = (int)_ball.Position.X;

        if (top <= 0) {
            _ball.Velocity = new Vector2(_ball.Velocity.X, _ball.Velocity.Y * -1);
        }
        if (bottom >= _screenHeight) {
            _ball.Velocity = new Vector2(_ball.Velocity.X, _ball.Velocity.Y * -1);
        }
        if (left <= 0) {
            _ball.Velocity = new Vector2(_ball.Velocity.X * -1, _ball.Velocity.Y);
            _rightScore++;
            _isPause = true;
            _ball.Position = new Vector2(_screenWidth / 2, _screenHeight / 2);
        }
        if (right >= _screenWidth) {
            _ball.Velocity = new Vector2(_ball.Velocity.X * -1, _ball.Velocity.Y);
            _leftScore++;
            _isPause = true;
            _ball.Position = new Vector2(_screenWidth / 2, _screenHeight / 2);
        }

        Rectangle ballRect = new Rectangle((int)_ball.Position.X, (int)_ball.Position.Y, _ball.Width, _ball.Height);

        if (_leftPaddle.Bounds.Intersects(ballRect)) {
            if (ballRect.X < _leftPaddle.Right) {
                float ballCenter = _ball.Position.Y + _ball.Height / 2;
                float paddleCenter = _leftPaddle.Position.Y + _leftPaddle.Height / 2;

                _ballAngle = ballCenter - paddleCenter;
                _ball.Velocity = new Vector2(MathF.Cos(MathHelper.ToRadians(_ballAngle)), MathF.Sin(MathHelper.ToRadians(_ballAngle))) * _ball.Speed;
            }
        }

        if (_rightPaddle.Bounds.Intersects(ballRect)) {
            if (ballRect.X < _rightPaddle.Right) {
                float ballCenter = _ball.Position.Y + _ball.Height / 2;
                float paddleCenter = _rightPaddle.Position.Y + _rightPaddle.Height / 2;

                _ballAngle = ballCenter - paddleCenter;
                _ball.Velocity = new Vector2(-MathF.Cos(MathHelper.ToRadians(_ballAngle)), MathF.Sin(MathHelper.ToRadians(_ballAngle))) * _ball.Speed;
            }
        }

        _ball.Position += _ball.Velocity * dt;
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp);

        Color objectsColor = new Color(100, 100, 100);

        int size = _screenHeight / 20;

        for (int i = 0; i <= size; i++) {
            SpriteBatch.Draw(_square, new Rectangle(_screenWidth / 2, i * size, PIXEL_WIDTH / 2, PIXEL_WIDTH), objectsColor);
        }

        _leftPaddle.Draw(SpriteBatch);

        _rightPaddle.Draw(SpriteBatch);

        _ball.Draw(SpriteBatch);        

        string leftText = _leftScore.ToString();
        Vector2 fontOrigin = _spriteFont.MeasureString(leftText) / 2;
        SpriteBatch.DrawString(_spriteFont, leftText, _leftScorePosition, objectsColor, 0, fontOrigin, 5.0f, SpriteEffects.None, 0.5f);

        string rightText = _rightScore.ToString();
        Vector2 fontOriginRight = _spriteFont.MeasureString(rightText) / 2;
        SpriteBatch.DrawString(_spriteFont, rightText, _rightScorePosition, objectsColor, 0, fontOriginRight, 5.0f, SpriteEffects.None, 0.5f);

        SpriteBatch.End();
    }
}
