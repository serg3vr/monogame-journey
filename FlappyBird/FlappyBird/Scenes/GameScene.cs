using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FlappyBird.Core.Scenes;
using FlappyBird.GameObjects;
using System.Collections.Generic;
using FlappyBird.Core.UI;

namespace FlappyBird;

public class GameScene : Scene
{
    private static readonly Random _random = new();
    private const float GRAVITY = 1000f;
    private const float IMPULSE = 650f;

    private int _screenWidth;
    private int _screenHeight;
    private float _usableScreenWidth;

    private Texture2D _texture;

    private const int Rows = 6;
    private const int Columns = 12;
    private const float RECT_WIDTH = 16;

    // private List<Brick> _bricks;
    private Color[] _colors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Purple, Color.Cyan };
    // private List<Wall> _walls;

    // private List<Ball> _balls;
    // private Paddle _paddle;

    private SpriteFont _spriteFont;
    private float currentDeg;

    // private Heart _heart;
    private int _lives;

    private bool _isPause;
    private float _timerToUnpause;
    private bool _isGameOver;
    private bool _youWon;

    private Panel _panel;
    private Text _gameOverText;
    private Button _restartButton;
    private Text _youWonText;

    // private List<PowerUp1> _powerUps1;
    private Color[] _powerUpsColors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Purple, Color.Cyan };

    // private List<Tile> _tiles;
    // private List<Body> _bodies;
    private bool _canMove;
    private float _moveTimer;
    private Vector2 _newPosition;
    private float _speed = 64f;
    // private Apple _apple;
    private string _direction;

    private SpriteFont _hudFont;

    private Body _bird;

    private KeyboardState _previousKeyboardState;

    private List<Pipe> _pipes;

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
        _screenWidth = GraphicsDevice.Viewport.Width;
        _screenHeight = GraphicsDevice.Viewport.Height;

        _usableScreenWidth = _screenWidth / 4f;

        _lives = 3;
        _isPause = false;
        _isGameOver = false;
        _youWon = false;

        _canMove = true;
        _moveTimer = 0f;

        _newPosition = new Vector2(1, 0);
        _direction = "Right";

        _pipes = new List<Pipe>();
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

        float ww = _screenWidth / 20;

        _spriteFont = ContentManager.Load<SpriteFont>("fonts/ingame");
        _hudFont = ContentManager.Load<SpriteFont>("fonts/hud");

        _panel = new Panel(_texture, _spriteFont, new Rectangle((int)_usableScreenWidth, 32, (int)_usableScreenWidth * 2, (int)_usableScreenWidth * 2));
        _restartButton = new Button(_texture, _hudFont, new Rectangle(_screenWidth / 2 - 140 / 2, 240, 140, 60), "Restart");

        var scale = 2;
        var text = "GAME OVER";
        var stringSize = _hudFont.MeasureString(text) * scale;
        _gameOverText = new Text(_hudFont, new Vector2(_screenWidth / 2 - stringSize.X / 2, 100));
        _gameOverText.Scale = Vector2.One * scale;
        _gameOverText.Content = text;

        text = "YOU WON";
        stringSize = _hudFont.MeasureString(text) * scale;
        _youWonText = new Text(_hudFont, new Vector2(_screenWidth / 2 - stringSize.X / 2, 100));
        _youWonText.Scale = Vector2.One * scale;
        _youWonText.Content = text;

        _restartButton.OnClick = () => {
            SceneManager.ChangeScene(new GameScene(ContentManager, GraphicsDevice, SpriteBatch, SceneManager));
        };

        _bird = new Body(_texture, new Vector2(100, 100), new Vector2(64, 64));
        _bird.SpriteColor = Color.Yellow;
        _bird.Speed = 1f;

        var pipe1 = new Pipe(_texture, new Vector2(100, 0), new Vector2(64, 64));
        var pipe2 = new Pipe(_texture, new Vector2(200, 0), new Vector2(64, 64));
        var pipe3 = new Pipe(_texture, new Vector2(300, 0), new Vector2(64, 64));
        var pipe4 = new Pipe(_texture, new Vector2(400, 0), new Vector2(64, 64));
        _pipes.Add(pipe1);
        _pipes.Add(pipe2);
        _pipes.Add(pipe3);
        _pipes.Add(pipe4);
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

        SpriteBatch.End();
    }
}
