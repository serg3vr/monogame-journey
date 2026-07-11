using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Asteroids.Core;
using Asteroids.Core.Scenes;
using Asteroids.GameObjects;
using System.Collections.Generic;
using Asteroids.Core.UI;

namespace Asteroids.Scenes;

public class GameScene : Scene
{
    private static readonly Random _random = new();
    private const float GRAVITY = 1500f;
    private const float IMPULSE = 480f;
    private const float PIPE_WIDTH = 52f;

    private float _usableScreenWidth;

    private Texture2D _texture;

    private const float RECT_WIDTH = 16;


    private SpriteFont _smallFont;

    private bool _isPause;
    private float _timerToUnpause;
    private bool _isGameOver;
    private bool _youWon;

    private Panel _panel;
    private Text _gameOverText;
    private Button _restartButton;
    private Text _youWonText;

    private Color[] _powerUpsColors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Purple, Color.Cyan };

    private bool _canMove;
    private float _moveTimer;
    private Vector2 _newPosition;
    private float _speed = 64f;
    // private Apple _apple;
    private string _direction;

    private SpriteFont _mediumFont;

    private Body _bird;

    private KeyboardState _previousKeyboardState;

    private List<Pipe> _pipes;
    private int _score = 0;
    private Text _scoreText;

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
        _usableScreenWidth = Globals.ScreenWidth / 4f;

        _isPause = false;
        _isGameOver = false;
        _youWon = false;

        _canMove = true;
        _moveTimer = 0f;

        _newPosition = new Vector2(1, 0);
        _direction = "Right";

        _pipes = new List<Pipe>();
        // _column1 = new PipesColumn();
        // _column2 = new PipesColumn();
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

        float ww = Globals.ScreenWidth / 20;

        _smallFont = ContentManager.Load<SpriteFont>("fonts/small");
        _mediumFont = ContentManager.Load<SpriteFont>("fonts/medium");

        _panel = new Panel(_texture, _smallFont, new Rectangle((int)_usableScreenWidth, 32, (int)_usableScreenWidth * 2, (int)_usableScreenWidth * 2));
        _restartButton = new Button(_texture, _mediumFont, new Rectangle(Globals.ScreenWidth / 2 - 140 / 2, 240, 140, 60), "Restart");

        var scale = 2;
        var text = "GAME OVER";
        var stringSize = _mediumFont.MeasureString(text) * scale;
        _gameOverText = new Text(_mediumFont, new Vector2(Globals.ScreenWidth / 2 - stringSize.X / 2, 100));
        _gameOverText.Scale = Vector2.One * scale;
        _gameOverText.Content = text;

        text = "YOU WON";
        stringSize = _mediumFont.MeasureString(text) * scale;
        _youWonText = new Text(_mediumFont, new Vector2(Globals.ScreenWidth / 2 - stringSize.X / 2, 100));
        _youWonText.Scale = Vector2.One * scale;
        _youWonText.Content = text;

        _restartButton.OnClick = () => {
            SceneManager.ChangeScene(new GameScene(ContentManager, GraphicsDevice, SpriteBatch, SceneManager));
        };

        _bird = new Body(_texture, new Vector2(100, 100), new Vector2(32, 32));
        _bird.SpriteColor = Color.Yellow;
        _bird.Speed = 1f;

        var size = GetNextGap();
        var otherSize = 11 - size - 3;

        var initialX = 256;

        var pipe1 = new Pipe(_texture, new Vector2(initialX + 100, 0), new Vector2(PIPE_WIDTH, size * 64));
        pipe1.SpriteColor = Color.Green;
        var pipe2 = new Pipe(_texture, new Vector2(initialX + 100, Globals.ScreenHeight - otherSize * 64), new Vector2(PIPE_WIDTH, otherSize * 64));
        pipe2.SpriteColor = Color.Green;
        _pipes.Add(pipe1);
        _pipes.Add(pipe2);

        size = GetNextGap();
        otherSize = 11 - size - 3;

        var pipe3 = new Pipe(_texture, new Vector2(initialX + 356, 0), new Vector2(PIPE_WIDTH, size * 64));
        pipe3.SpriteColor = Color.Green;
        var pipe4 = new Pipe(_texture, new Vector2(initialX + 356, Globals.ScreenHeight - otherSize * 64), new Vector2(PIPE_WIDTH, otherSize * 64));
        pipe4.SpriteColor = Color.Green;
        _pipes.Add(pipe4);
        _pipes.Add(pipe3);

        _scoreText = new Text(_mediumFont, new Vector2(Globals.ScreenWidth /2, 100));
        _scoreText.Scale = Vector2.One * scale;
        _scoreText.Content = _score.ToString();
    }

    public override void Update(GameTime gameTime)
    {
        KeyboardState ks = Keyboard.GetState();
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_isGameOver || _youWon) {
            _restartButton.Update();
        }

        if (_isPause || _youWon) {
            if (!_isGameOver) {
                _timerToUnpause -= dt;

                if (_timerToUnpause <= 0) {
                    _isPause = false;
                }
            }

            return;
        }

        _moveTimer += dt;

        if (_moveTimer >= 0.1f) {
            _canMove = true;
            _moveTimer = 0f;
        }


        if (ks.IsKeyDown(Keys.Space) && !_previousKeyboardState.IsKeyDown(Keys.Space)) {
            _bird.Velocity = new Vector2(0, -IMPULSE);
        }

        _bird.Velocity += new Vector2(0, GRAVITY * dt);
        _bird.Velocity = new Vector2(
            _bird.Velocity.X,
            Math.Clamp(_bird.Velocity.Y, -IMPULSE, 1000f)
        );
        _bird.Position += _bird.Velocity * _bird.Speed * dt;


        foreach (var pipe in _pipes) {
            pipe.Update(gameTime);
        }

        if (_pipes[0].Bounds.Right < 0) {
            var size = GetNextGap();
            var otherSize = 11 - size - 3;
            _pipes[0].Position = new Vector2(Globals.ScreenWidth + _pipes[0].Bounds.Width, 0);
            _pipes[0].Size = new Vector2(PIPE_WIDTH, size * 64);

            _pipes[1].Position = new Vector2(Globals.ScreenWidth + _pipes[1].Bounds.Width, Globals.ScreenHeight - otherSize * 64);
            _pipes[1].Size = new Vector2(PIPE_WIDTH, otherSize * 64);
        }

        if (_pipes[2].Bounds.Right < 0) {
            var size = GetNextGap();
            var otherSize = 11 - size - 3;
            _pipes[2].Position = new Vector2(Globals.ScreenWidth + _pipes[2].Bounds.Width, 0);
            _pipes[2].Size = new Vector2(PIPE_WIDTH, size * 64);

            _pipes[3].Position = new Vector2(Globals.ScreenWidth + _pipes[3].Bounds.Width, Globals.ScreenHeight - otherSize * 64);
            _pipes[3].Size = new Vector2(PIPE_WIDTH, otherSize * 64);
        }

        var wasCollision = false;

        foreach (var pipe in _pipes) {
            if (_bird.Bounds.Intersects(pipe.Bounds)) {
                wasCollision =  true;
                break;
            }
        }

        if (_bird.Bounds.Top < 0 || _bird.Bounds.Bottom > Globals.ScreenHeight) {
            wasCollision =  true;
        }

        if (wasCollision) {
            _isPause = true;
            _isGameOver = true;
        }


        foreach (var pipe in _pipes) {
            if (_bird.Bounds.Left > pipe.Bounds.Right && pipe.IsScoreable) {
                pipe.IsScoreable = false;
                _score += 1;
                _scoreText.Content = (_score / 2).ToString();
                break; // Only need pass 1/4
            }
        }

        _previousKeyboardState = ks;
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();

        _bird.Draw(SpriteBatch);

        foreach (var pipe in _pipes) {
            pipe.Draw(SpriteBatch);
        }

        if (_isGameOver) {
            _panel.Draw(SpriteBatch);
            _gameOverText.Draw(SpriteBatch);
            _restartButton.Draw(SpriteBatch);
        }

        if (_youWon) {
            _panel.Draw(SpriteBatch);
            _youWonText.Draw(SpriteBatch);
            _restartButton.Draw(SpriteBatch);
        }

        _scoreText.Draw(SpriteBatch);

        SpriteBatch.End();
    }

    private int GetNextGap()
    {
        return _random.Next(2, 7);
    }
}
