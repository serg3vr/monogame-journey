using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Breakout.Core.Scenes;
using Breakout.GameObjects;
using System.Collections.Generic;
using Breakout.Core.UI;

namespace Breakout;

public class GameScene : Scene
{
    private static readonly Random _random = new();

    private int _screenWidth;
    private int _screenHeight;
    private float _usableScreenWidth;

    private Texture2D _texture;

    private const int Rows = 6;
    private const int Columns = 12;
    private const float RECT_WIDTH = 16;

    private List<Brick> _bricks;
    private Color[] _colors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Purple, Color.Cyan };
    private List<Wall> _walls;

    private List<Ball> _balls;
    private Paddle _paddle;

    private SpriteFont _spriteFont;
    private float currentDeg;

    private Heart _heart;
    private int _lives;

    private bool _isPause;
    private float _timerToUnpause;
    private bool _isGameOver;

    private Panel _panel;
    private Text _gameOverText;
    private Button _restartButton;

    private List<PowerUp1> _powerUps1;
    private Color[] _powerUpsColors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Purple, Color.Cyan };

    public GameScene(
        ContentManager contentManager,
        GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch,
        SceneManager sceneManager
    ) : base(contentManager, graphicsDevice, spriteBatch, sceneManager)
    {
    }

    public override void Initialize()
    {
        // TODO: Add your initialization logic here
        _screenWidth = GraphicsDevice.Viewport.Width;
        _screenHeight = GraphicsDevice.Viewport.Height;

        _usableScreenWidth = _screenWidth / 4f;

        _bricks = new List<Brick>();
        _walls = new List<Wall>();

        _lives = 3;
        _isPause = false;
        _isGameOver = false;

        _balls = new List<Ball>();
        _powerUps1 = new List<PowerUp1>();
    }

    public override void LoadContent()
    {
        _texture = new Texture2D(GraphicsDevice, 1, 1);
        _texture.SetData(new[] { Color.White });

        float blockWidth = (_usableScreenWidth - 16f) * 2f / 13f;
        float blockSpace = blockWidth / 14f;

        float startingPointX = _usableScreenWidth + RECT_WIDTH + blockSpace;
        float startingPointY = RECT_WIDTH * 7 + blockSpace;

        float paddleWidth = blockWidth * 1.5f;

        for (int y = 0; y < Rows; y++) {
            for (int x = 0; x < Columns; x++) {
                var newBrick = new Brick(
                    _texture,
                    new Vector2(
                        startingPointX + x * (blockWidth + blockSpace),
                        startingPointY + y * (16 + blockSpace)
                    ),
                    new Vector2(blockWidth, 16)
                );
                newBrick.SpriteColor = _colors[y];
                newBrick.HasPowerUp = _random.Next(5) == 1;
                _bricks.Add(newBrick);
            }
        }

        GenerateExtraBalls();

        _paddle = new Paddle(_texture, new Vector2(_screenWidth / 2, (_screenHeight / 32) * 30), new Vector2(paddleWidth, 16));
        _paddle.SpriteColor = Color.White;
        _paddle.Speed = 400f;

        _walls.Add(new Wall(_texture, new Vector2(_usableScreenWidth, RECT_WIDTH * 2), new Vector2(_usableScreenWidth * 2, RECT_WIDTH)));
        _walls.Add(new Wall(_texture, new Vector2(_usableScreenWidth, _screenHeight - RECT_WIDTH), new Vector2(_usableScreenWidth * 2, RECT_WIDTH)));
        _walls.Add(new Wall(_texture, new Vector2(_usableScreenWidth, RECT_WIDTH * 3), new Vector2(RECT_WIDTH, _screenHeight - RECT_WIDTH * 5)));
        _walls.Add(new Wall(_texture, new Vector2(_usableScreenWidth * 3 - RECT_WIDTH, RECT_WIDTH * 3), new Vector2(RECT_WIDTH, _screenHeight - RECT_WIDTH * 5)));

        foreach (var wall in _walls) {
            wall.SpriteColor = Color.White;
        }
        _walls[1].SpriteColor = new Color(21, 21, 21);

        _spriteFont = ContentManager.Load<SpriteFont>("fonts/myfont");

        _heart = new Heart(_texture, new Vector2(32, 32), new Vector2(32, 32));
        _heart.SpriteColor = Color.DarkRed;

        _panel = new Panel(_texture, _spriteFont, new Rectangle((int)_usableScreenWidth, 32, (int)_usableScreenWidth * 2, (int)_usableScreenWidth * 2));
        _restartButton = new Button(_texture, _spriteFont, new Rectangle(200, 100, 100, 100), "Restart");

        _gameOverText = new Text(_spriteFont, new Vector2(200, 200));
        _gameOverText.Scale = Vector2.One;
        _gameOverText.Content = "GAMER OVER";

        _restartButton.OnClick = () => {
            SceneManager.ChangeScene(new GameScene(ContentManager, GraphicsDevice, SpriteBatch, SceneManager));
        };
    }

    public override void Update(GameTime gameTime)
    {
        KeyboardState ks = Keyboard.GetState();
        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        // Exit();
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_isGameOver) {
            _restartButton.Update();
        }

        if (_isPause) {
            if (!_isGameOver) {
                _timerToUnpause -= dt;

                if (_timerToUnpause <= 0) {
                    _isPause = false;
                }
            }

            return;
        }

        _paddle.Velocity = Vector2.Zero;

        if (ks.IsKeyDown(Keys.A)) {
            _paddle.Velocity = new Vector2(-1, 0);
        }

        if (ks.IsKeyDown(Keys.D)) {
            _paddle.Velocity = new Vector2(1, 0);
        }

        _paddle.Position += _paddle.Velocity * _paddle.Speed * dt;
        _paddle.Position = new Vector2(
            MathHelper.Clamp(_paddle.Position.X, _walls[2].Bounds.Right, _walls[3].Bounds.Left - _paddle.Bounds.Width),
            _paddle.Position.Y
        );

        _powerUps1.ForEach(elm => {
            elm.Position += elm.Velocity * elm.Speed * dt;
        });

        Ball? hittedBall = null;

        foreach (var ball in _balls) {
            if (ball.Bounds.Top < _walls[0].Bounds.Bottom) {
                ball.Velocity = new Vector2(ball.Velocity.X, MathF.Abs(ball.Velocity.Y));
            }
            if (ball.Bounds.Bottom > _walls[1].Bounds.Top) {
                // ball.Velocity = new Vector2(ball.Velocity.X, -MathF.Abs(ball.Velocity.Y));
                hittedBall = ball;
            }
            if (ball.Bounds.Left < _walls[2].Bounds.Right) {
                ball.Velocity = new Vector2(MathF.Abs(ball.Velocity.X), ball.Velocity.Y);
            }
            if (ball.Bounds.Right > _walls[3].Bounds.Left) {
                ball.Velocity = new Vector2(-MathF.Abs(ball.Velocity.X), ball.Velocity.Y);
            }

            Brick? hittedBrick = null;

            foreach (var brick in _bricks) {
                if (ball.Bounds.Intersects(brick.Bounds)) {
                    if (ball.Bounds.Top > brick.Bounds.Top) {
                        ball.Velocity = new Vector2(ball.Velocity.X, MathF.Abs(ball.Velocity.Y));
                    }
                    if (ball.Bounds.Bottom < brick.Bounds.Bottom) {
                        ball.Velocity = new Vector2(ball.Velocity.X, -MathF.Abs(ball.Velocity.Y));
                    }
                    if (ball.Bounds.Left > brick.Bounds.Right) {
                        ball.Velocity = new Vector2(MathF.Abs(ball.Velocity.X), ball.Velocity.Y);
                    }
                    if (ball.Bounds.Right < brick.Bounds.Left) {
                        ball.Velocity = new Vector2(-MathF.Abs(ball.Velocity.X), ball.Velocity.Y);
                    }
                    hittedBrick = brick;
                    if (brick.HasPowerUp) {
                        CreatePowerUp(brick.Position);
                    }
                    break;
                }
            }
            _bricks.Remove(hittedBrick);

            if (_paddle.Bounds.Intersects(ball.Bounds)) {
                var isMovingToLeft = ks.IsKeyDown(Keys.A);
                var isMovingToRight = ks.IsKeyDown(Keys.D);

                float hitPos = MathHelper.Clamp((ball.Bounds.Center.X - _paddle.Bounds.Center.X) / (_paddle.Bounds.Width / 2f), -1, 1);
                float angleDeg = MathHelper.Lerp(-150f, -30f, (hitPos + 1f) / 2f);
                currentDeg = angleDeg;
                float angleRad = MathHelper.ToRadians(angleDeg);
                float moveX = Math.Abs(MathF.Cos(angleRad));

                if (isMovingToLeft) {
                    moveX = -Math.Abs(MathF.Cos(angleRad));
                } else if (isMovingToRight) {
                    moveX = Math.Abs(MathF.Cos(angleRad));
                }

                ball.Velocity = new Vector2(moveX, MathF.Sin(angleRad));
                ball.Position = new Vector2(ball.Position.X, _paddle.Bounds.Top - ball.Bounds.Height);
            }

            ball.Position += ball.Velocity * ball.Speed * dt;
        }

        foreach (var powerUp in _powerUps1) {
            if (_paddle.Bounds.Intersects(powerUp.Bounds)) {
                powerUp.ShouldBeDestroyed = true;
                GenerateExtraBalls(2);
            }
        }

        _powerUps1.RemoveAll(elm => elm.ShouldBeDestroyed);

        _balls.Remove(hittedBall);

        if (_balls.Count == 0) {
            _isPause = true;
            _timerToUnpause = 1f;

            ResetPositions();
        }
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();

        foreach (var block in _bricks) {
            block.Draw(SpriteBatch);
        }

        foreach (var ball in _balls) {
            ball.Draw(SpriteBatch);
        }

        _paddle.Draw(SpriteBatch);

        foreach (var wall in _walls) {
            wall.Draw(SpriteBatch);
        }

        SpriteBatch.DrawString(_spriteFont, "Ball angle: " + currentDeg.ToString(), new Vector2(100, 100), Color.Red);

        _heart.Draw(SpriteBatch);
        SpriteBatch.DrawString(_spriteFont, _lives.ToString(), new Vector2(64, 32), Color.Red, 0f, Vector2.Zero, new Vector2(2f, 2f), SpriteEffects.None, 0f);

        foreach (var powerUp in _powerUps1) {
            powerUp.Draw(SpriteBatch);
        }

        if (_isGameOver) {
            _panel.Draw(SpriteBatch);
            _gameOverText.Draw(SpriteBatch);
            _restartButton.Draw(SpriteBatch);
        }

        SpriteBatch.End();
    }

    private void ResetPositions()
    {
        GenerateExtraBalls();

        _paddle.Position = new Vector2(_screenWidth / 2, (_screenHeight / 32) * 30);
        _lives -= 1;

        if (_lives <= 0) {
            _isGameOver = true;
        }
    }

    private void GenerateExtraBalls(int extra = 1)
    {
        for (int i = 0; i < extra; i++) {
            var ball = new Ball(_texture, new Vector2(_screenWidth / 2, (_screenHeight / 32) * 29), new Vector2(16, 16));
            ball.SpriteColor = Color.White;
            ball.Speed = 400f;
            ball.Velocity = -Vector2.One;
            _balls.Add(ball);
        }
    }

    private void CreatePowerUp(Vector2 pos)
    {
        var powerUp = new PowerUp1(_texture, pos, new Vector2(32, 16));
        // powerUp.SpriteColor = new Color(
        //     _random.Next(255),
        //     _random.Next(255),
        //     _random.Next(255)
        // );
        powerUp.SpriteColor = _powerUpsColors[_random.Next(_powerUpsColors.Length)];
        powerUp.Speed = 200f;
        powerUp.Velocity = new Vector2(0f, 1f);
        _powerUps1.Add(powerUp);
    }
}
