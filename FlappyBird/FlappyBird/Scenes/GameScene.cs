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
    private const float GRAVITY = 9.81f;

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

        // _bricks = new List<Brick>();
        // _walls = new List<Wall>();

        _lives = 3;
        _isPause = false;
        _isGameOver = false;
        _youWon = false;

        // _balls = new List<Ball>();
        // _powerUps1 = new List<PowerUp1>();

        // _tiles = new List<Tile>();
        // _bodies = new List<Body>();

        _canMove = true;
        _moveTimer = 0f;

        _newPosition = new Vector2(1, 0);
        _direction = "Right";
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
        var isLight = true;

        // for (int i = 0; i < 19; i++) {
        //     isLight = !isLight;
        //     for (int j = 0; j < 10; j++) {
        //         var newTile = new Tile(_texture, new Vector2(32 + i * 64, 64 + j * 64), new Vector2(ww, ww));
        //         if (isLight) {
        //             newTile.SpriteColor = new Color(1, 1, 1);
        //         } else {
        //             newTile.SpriteColor = new Color(10, 10, 10);
        //         }
        //         isLight = !isLight;

        //         _tiles.Add(newTile);
        //     }
        // }

        var body = new Body(_texture, new Vector2(32 + 64 * 9, 128), new Vector2(64, 64));
        body.SpriteColor = new Color(88, 187, 10);
        var body2 = new Body(_texture, new Vector2(32 + 64 * 9, 128), new Vector2(64, 64));
        body2.SpriteColor = new Color(108, 187, 60);
        var body3 = new Body(_texture, new Vector2(32 + 64 * 9, 128), new Vector2(64, 64));
        body3.SpriteColor = new Color(108, 187, 60);
        // _bodies.Add(body);
        // _bodies.Add(body2);
        // _bodies.Add(body3);

        // float pos = _random.Next(19);
        // float pos2 = _random.Next(10);
        // _apple = new Apple(_texture, new Vector2(0, 0), new Vector2(64, 64));
        // _apple.SpriteColor = Color.DarkRed;
        // RepositionApple();

        // _walls.Add(new Wall(_texture, new Vector2(32, 62), new Vector2(19 * 64, 2)));
        // _walls.Add(new Wall(_texture, new Vector2(32, 64 + 10 * 64), new Vector2(19 * 64, 2)));
        // _walls.Add(new Wall(_texture, new Vector2(30, 64), new Vector2(2, 10 * 64)));
        // _walls.Add(new Wall(_texture, new Vector2(32 + 19 * 64, 64), new Vector2(2, 10 * 64)));

        // foreach (var wall in _walls) {
        //     wall.SpriteColor = Color.White;
        // }

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
        _youWonText.Content = "YOU WON";

        _restartButton.OnClick = () => {
            SceneManager.ChangeScene(new GameScene(ContentManager, GraphicsDevice, SpriteBatch, SceneManager));
        };

        _bird = new Body(_texture, new Vector2(100, 100), new Vector2(64, 64));
        _bird.SpriteColor = Color.Yellow;
        _bird.Speed = 1f;
    }

    public override void Update(GameTime gameTime)
    {
        KeyboardState ks = Keyboard.GetState();
        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        // Exit();
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
            _bird.Velocity += new Vector2(0, -1000f);
        }

        _bird.Velocity += new Vector2(0, GRAVITY);
        _bird.Position += _bird.Velocity * _bird.Speed * dt;
        
        _previousKeyboardState = ks;
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();

        _bird.Draw(SpriteBatch);

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
