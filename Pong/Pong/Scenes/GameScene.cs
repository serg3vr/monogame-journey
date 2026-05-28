using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Core;
using Pong.Core.Scenes;
using Pong.Core.UI;
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

    private bool _isPause = false;

    private float _leftPaddleMovementAccumulation;
    private const int SPEED_INCREMENT = 20; // 20 pixels per second

    private Panel _panel;
    private Button _resumeButton;
    private Button _backToMenuButton;


    private KeyboardState _previousKeyboardState;

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
        _ball.SpriteColor = Color.White;
        
        ResetBall();

        _spriteFont = ContentManager.Load<SpriteFont>("myfont");
        _leftScorePosition = new Vector2(_screenWidth / 2 - PIXEL_WIDTH * 3, PIXEL_WIDTH * 3);
        _rightScorePosition = new Vector2(_screenWidth / 2 + PIXEL_WIDTH * 3, PIXEL_WIDTH * 3);

        _panel = new Panel(_square, _spriteFont, new Rectangle(_screenWidth / 2 - 200, 150, 400, 500));

        _resumeButton = new Button(_square, _spriteFont, new Rectangle(_screenWidth / 2 - 100, _screenHeight / 2, 200, 60), "RESUME");
        _resumeButton.OnClick = () => {
            _isPause = !_isPause;
        };

        _backToMenuButton = new Button(_square, _spriteFont, new Rectangle(_screenWidth / 2 - 100, _screenHeight / 2 + 70, 200, 60), "BACK TO MENU");
        _backToMenuButton.OnClick = () =>
        {
            SceneManager.ChangeScene(
                new MenuScene(ContentManager, GraphicsDevice, SpriteBatch, SceneManager)
            );
        };
    }

    private void ResetBall(float right = 1)
    {
        _ball.Speed = 600;
        _ball.Position = new Vector2(_screenWidth / 2, _screenHeight / 2);
        _ball.Velocity = new Vector2(1f * right, 0f) * _ball.Speed;
    }

    public override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        KeyboardState ks = Keyboard.GetState();

        if (ks.IsKeyDown(Keys.Enter) && _previousKeyboardState.IsKeyUp(Keys.Enter)) {
            _isPause = !_isPause;
        }

        _previousKeyboardState = ks;
        
        if (_isPause) {
            _resumeButton.Update();
            _backToMenuButton.Update();
            return;
        }

        if (ks.IsKeyDown(Keys.W)) {
            _leftPaddle.Position -= new Vector2(0, _leftPaddle.Speed) * dt;

            _leftPaddleMovementAccumulation += dt;
        } else if (ks.IsKeyDown(Keys.S)) {
            _leftPaddle.Position += new Vector2(0, _leftPaddle.Speed) * dt;

            _leftPaddleMovementAccumulation += dt;
        } else {
            _leftPaddleMovementAccumulation = 0f;
        }

        _leftPaddleMovementAccumulation = MathHelper.Clamp(_leftPaddleMovementAccumulation, 0, 1);

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

        float top = _ball.Position.Y;
        float bottom = _ball.Position.Y + _ball.Height;
        float right = _ball.Position.X + _ball.Width;
        float left = _ball.Position.X;

        if (top <= 0) {
            _ball.Velocity = new Vector2(_ball.Velocity.X, MathF.Abs(_ball.Velocity.Y));
            _ball.Position = new Vector2(_ball.Position.X, 0);
        }
        if (bottom >= _screenHeight) {
            _ball.Velocity = new Vector2(_ball.Velocity.X, -MathF.Abs(_ball.Velocity.Y));
            _ball.Position = new Vector2(_ball.Position.X, _screenHeight - _ball.Height);
        }
        if (left <= 0) {
            _rightScore++;
            ResetBall(-1f);
        }
        if (right >= _screenWidth) {
            _leftScore++;
            ResetBall(1f);
        }

        if (_leftPaddle.Bounds.Intersects(_ball.Bounds)) {
            _ball.Speed += SPEED_INCREMENT;
            var leftPaddleIsMoving = ks.IsKeyDown(Keys.W) || ks.IsKeyDown(Keys.S);

            if (leftPaddleIsMoving) {
                float normalizedDis = MathF.Abs((_ball.Bounds.Center.Y - _leftPaddle.Bounds.Center.Y) / (_leftPaddle.Height / 2f));
                if (ks.IsKeyDown(Keys.W)) {
                    normalizedDis = -normalizedDis;
                }

                float angleInRad = normalizedDis * MathHelper.ToRadians(60f + (7.5f * 1f + _leftPaddleMovementAccumulation)); // Max 75f
                Vector2 dir = new Vector2(MathF.Cos(angleInRad), MathF.Sin(angleInRad));

                _ball.Velocity = dir * _ball.Speed * (1f + _leftPaddleMovementAccumulation);
            } else {
                _ball.Velocity = new Vector2(_ball.Velocity.X * -1, _ball.Velocity.Y);
            }
        }

        if (_rightPaddle.Bounds.Intersects(_ball.Bounds)) {
            _ball.Speed += SPEED_INCREMENT;
            
            float normalizedDis = (_ball.Bounds.Center.Y - _rightPaddle.Bounds.Center.Y) / (_rightPaddle.Height / 2f);
            float angleInRad = normalizedDis * MathHelper.ToRadians(60f);
            Vector2 dir = new Vector2(-MathF.Cos(angleInRad), MathF.Sin(angleInRad));
            _ball.Velocity = dir * _ball.Speed;
        }

        _ball.Position += _ball.Velocity * dt;
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();

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
        SpriteBatch.DrawString(_spriteFont, leftText, _leftScorePosition, objectsColor, 0, fontOrigin, 3.0f, SpriteEffects.None, 0.5f);

        string rightText = _rightScore.ToString();
        Vector2 fontOriginRight = _spriteFont.MeasureString(rightText) / 2;
        SpriteBatch.DrawString(_spriteFont, rightText, _rightScorePosition, objectsColor, 0, fontOriginRight, 3.0f, SpriteEffects.None, 0.5f);

        if (_isPause) {
            _panel.Draw(SpriteBatch);
            _resumeButton.Draw(SpriteBatch);
            _backToMenuButton.Draw(SpriteBatch);
        }

        SpriteBatch.End();
    }
}
