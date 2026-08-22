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

    private float _moveTimer;
    private SpriteFont _mediumFont;

    private Spaceship _spaceship;

    private KeyboardState _previousKeyboardState;
    private int _score = 0;
    private Text _scoreText;

    private float _accelerationForce = 300f;
    private static readonly float _maxVelocity = 150f;

    private List<Bullet> _bulletlist;
    private List<Asteroid> _asteroidList;
    private List<Asteroid> _asteroidsToAdd;

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

        _bulletlist = new List<Bullet>();
        _asteroidList = new List<Asteroid>();
        _asteroidsToAdd = new List<Asteroid>();
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

        var size = new Vector2(32, 32);
        var center = new Vector2(Globals.ScreenWidth / 2 - size.X / 2, Globals.ScreenHeight / 2 - size.Y / 2);
        _spaceship = new Spaceship(_texture, center, size);
        _spaceship.SpriteColor = Color.White;

        _scoreText = new Text(_mediumFont, new Vector2(Globals.ScreenWidth / 2, 100));
        _scoreText.Scale = Vector2.One * scale;
        _scoreText.Value = _score.ToString();

        var asteroidPos = new Vector2(_random.Next(50, 400), _random.Next(50, 400));
        var asteroidRot = _random.Next(0, 40);

        var obj = new Asteroid(_texture, asteroidPos, new Vector2(128, 128), asteroidRot);
        obj.Speed = 30f;
        _asteroidList.Add(obj);
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
            // _canMove = true;
            _moveTimer = 0f;
        }

        if (ks.IsKeyDown(Keys.A)) {
            _spaceship.Rotation -= 10 * dt; // _rotationSpeed * dt;
        }

        if (ks.IsKeyDown(Keys.D)) {
            _spaceship.Rotation += 10 * dt; // _rotationSpeed * dt;
        }

        var rotation = Vector2.Transform(Direction.Up, Matrix.CreateRotationZ(_spaceship.Rotation));

        if (ks.IsKeyDown(Keys.W)) {
            _spaceship.Acceleration = rotation * _accelerationForce;
            _spaceship.Velocity += _spaceship.Acceleration * dt;
        } else {
            _spaceship.Velocity *= MathF.Pow(0.01f, dt);
        }

        float speed = _spaceship.Velocity.Length();
        if (speed > _maxVelocity) {
            _spaceship.Velocity = _spaceship.Velocity / speed * _maxVelocity;
        }

        _spaceship.Update(gameTime);

        _spaceship.Position += _spaceship.Velocity * dt;

        if (ks.IsKeyDown(Keys.Space) && !_previousKeyboardState.IsKeyDown(Keys.Space)) {
            var bullet = new Bullet(_texture, _spaceship.Position, new Vector2(8, 8), _spaceship.Rotation);
            bullet.Speed = 650f;
            _bulletlist.Add(bullet);
        }

        foreach (var bl in _bulletlist) {
            bl.Update(gameTime);
        }

        foreach (var asteroid in _asteroidList) {
            asteroid.Update(gameTime);

            foreach (var bullet in _bulletlist) {
                if (asteroid.ShouldBeDeleted || bullet.ShouldBeDeleted)
                    continue;

                if (asteroid.Bounds.Intersects(bullet.Bounds)) {
                    asteroid.ShouldBeDeleted = true;
                    bullet.ShouldBeDeleted = true;
                    asteroid.Health -= 1;

                    if (asteroid.Health > 0) {
                        var rotationPlus = asteroid.Rotation + 10;
                        var rotationMinus = asteroid.Rotation - 10;
                        
                        var obj = new Asteroid(_texture, asteroid.Position, asteroid.Size * 0.5f, rotationPlus);
                        obj.Speed = asteroid.Speed * 1.5f;
                        obj.Health = asteroid.Health;
                        _asteroidsToAdd.Add(obj);
                        
                        var obj2 = new Asteroid(_texture, asteroid.Position, asteroid.Size * 0.5f, rotationMinus);
                        obj2.Speed = asteroid.Speed * 1.5f;
                        obj2.Health = asteroid.Health;
                        _asteroidsToAdd.Add(obj2);
                    }
                }
            }
        }

        if (_asteroidsToAdd.Count > 0) {
            _asteroidList.AddRange(_asteroidsToAdd);
            _asteroidsToAdd.Clear();
        }

        _bulletlist.RemoveAll(bl => bl.ShouldBeDeleted);
        _asteroidList.RemoveAll(bl => bl.ShouldBeDeleted);

        if (_asteroidList.Count == 0) {
            _isGameOver = true;
            _isPause = true;
        }

        _previousKeyboardState = ks;
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();

        _spaceship.Draw(SpriteBatch);

        // Debug lines
        // SpriteBatch.Draw(_texture, new Rectangle(Globals.ScreenWidth / 2, 0, 1, Globals.ScreenHeight), Color.Green);
        // SpriteBatch.Draw(_texture, new Rectangle(0, Globals.ScreenHeight / 2, Globals.ScreenWidth, 1), Color.Green);

        foreach (var bl in _bulletlist) {
            bl.Draw(SpriteBatch);
        }

        foreach (var al in _asteroidList) {
            al.Draw(SpriteBatch);
            SpriteBatch.DrawString(_smallFont, $"HP: {al.Health}", new Vector2(al.Bounds.Left, al.Bounds.Top), Color.Red);
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

    // private int GetNextGap()
    // {
    //     return _random.Next(2, 7);
    // }
}
