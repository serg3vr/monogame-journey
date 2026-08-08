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
    private float _usableScreenWidth;

    private Texture2D _texture;
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
    private SpriteFont _mediumFont;

    private Spaceship _spaceship;

    private KeyboardState _previousKeyboardState;
    private int _score = 0;
    private Text _scoreText;

    private Texture2D _spaceshipSprite;
    private float _maxAcceleration = 1000f;
    private float _acceleration = 10f;
    private float _accelerationForce = 300f;
    private float _maxSpeed = 1000f;

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
    }

    public override void LoadContent()
    {
        _texture = new Texture2D(GraphicsDevice, 1, 1);
        _texture.SetData(new[] { Color.White });

        _smallFont = ContentManager.Load<SpriteFont>("fonts/small");
        _mediumFont = ContentManager.Load<SpriteFont>("fonts/medium");

        _panel = new Panel(_texture, _smallFont, new Rectangle((int)_usableScreenWidth, 32, (int)_usableScreenWidth * 2, (int)_usableScreenWidth * 2));
        _restartButton = new Button(_texture, _mediumFont, new Rectangle(Globals.ScreenWidth / 2 - 140 / 2, 240, 140, 60), "Restart");

        var scale = 2;
        var text = "GAME OVER";
        var stringSize = _mediumFont.MeasureString(text) * scale;
        _gameOverText = new Text(_mediumFont, new Vector2(Globals.ScreenWidth / 2 - stringSize.X / 2, 100));
        _gameOverText.Scale = Vector2.One * scale;
        _gameOverText.Value = text;

        text = "YOU WON";
        stringSize = _mediumFont.MeasureString(text) * scale;
        _youWonText = new Text(_mediumFont, new Vector2(Globals.ScreenWidth / 2 - stringSize.X / 2, 100));
        _youWonText.Scale = Vector2.One * scale;
        _youWonText.Value = text;

        _restartButton.OnClick = () => {
            SceneManager.ChangeScene(new GameScene(ContentManager, GraphicsDevice, SpriteBatch, SceneManager));
        };

        _spaceshipSprite = ContentManager.Load<Texture2D>("images/Spaceship");
        _spaceship = new Spaceship(
            _spaceshipSprite,
            new Vector2(Globals.ScreenWidth / 2 - _spaceshipSprite.Width / 2, Globals.ScreenHeight / 2 - _spaceshipSprite.Height / 2)
        );
        _spaceship.Speed = 100f;

        _scoreText = new Text(_mediumFont, new Vector2(Globals.ScreenWidth /2, 100));
        _scoreText.Scale = Vector2.One * scale;
        _scoreText.Value = _score.ToString();
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

        if (ks.IsKeyDown(Keys.A)) {
            _spaceship.Rotation -= 10 * dt; // _rotationSpeed * dt;
        }

        if (ks.IsKeyDown(Keys.D)) {
            _spaceship.Rotation += 10 * dt; // _rotationSpeed * dt;
        }

        Vector2 direction = Vector2.Transform(new Vector2(0, -1), Matrix.CreateRotationZ(_spaceship.Rotation));

        if (ks.IsKeyDown(Keys.W)) {
            _spaceship.Velocity += direction * _accelerationForce * dt;
        }

        if (ks.IsKeyDown(Keys.S)) {
            _spaceship.Velocity -= direction * _accelerationForce * dt * 2;
        }

        _spaceship.Velocity = Vector2.Clamp(_spaceship.Velocity, new Vector2(-100, -100), new Vector2(100, 100));
        _scoreText.Value = _spaceship.Velocity.ToString();

        _spaceship.Position += _spaceship.Velocity * dt;

        _previousKeyboardState = ks;
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();

        _spaceship.Draw(SpriteBatch);

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

    // private int GetNextGap()
    // {
    //     return _random.Next(2, 7);
    // }
}
