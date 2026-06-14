using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.Core.Scenes;
using Snake.GameObjects;
using System.Collections.Generic;
using Snake.Core.UI;

namespace Snake;

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

    // private List<Brick> _bricks;
    private Color[] _colors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Purple, Color.Cyan };
    private List<Wall> _walls;

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

    private List<PowerUp1> _powerUps1;
    private Color[] _powerUpsColors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Purple, Color.Cyan };

    private List<Tile> _tiles;
    private List<Body> _bodies;
    private bool _canMove;
    private float _moveTimer;
    private Vector2 _newPosition;
    private float _speed = 64f;
    private Apple _apple;

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
        _walls = new List<Wall>();

        _lives = 3;
        _isPause = false;
        _isGameOver = false;
        _youWon = false;

        // _balls = new List<Ball>();
        _powerUps1 = new List<PowerUp1>();

        _tiles = new List<Tile>();
        _bodies = new List<Body>();

        _canMove = true;
        _moveTimer = 0f;

        _newPosition = new Vector2(1, 0);
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

        for (int i = 0; i < 19; i++) {
            isLight = !isLight;
            for (int j = 0; j < 10; j++) {
                var newTile = new Tile(_texture, new Vector2(32 + i * 64, 64 + j * 64), new Vector2(ww, ww));
                if (isLight) {
                    newTile.SpriteColor = new Color(1, 1, 1);
                } else {
                    newTile.SpriteColor = new Color(10, 10, 10);
                }
                isLight = !isLight;

                _tiles.Add(newTile);
            }
        }

        var body = new Body(_texture, new Vector2(32 + 64 * 9, 128), new Vector2(64, 64));
        body.SpriteColor = new Color(108, 187, 60);
        var body2 = new Body(_texture, new Vector2(32 + 64 * 9, 128), new Vector2(64, 64));
        body2.SpriteColor = new Color(108, 187, 60);
        var body3 = new Body(_texture, new Vector2(32 + 64 * 9, 128), new Vector2(64, 64));
        body3.SpriteColor = new Color(108, 187, 60);
        _bodies.Add(body);
        _bodies.Add(body2);
        _bodies.Add(body3);

        float pos = _random.Next(19);
        float pos2 = _random.Next(10);
        _apple = new Apple(_texture, new Vector2(pos * 64, pos2 * 64), new Vector2(64, 64));
        _apple.SpriteColor = Color.DarkRed;

        _walls.Add(new Wall(_texture, new Vector2(32, 64), new Vector2(19 * 64, 2)));
        _walls.Add(new Wall(_texture, new Vector2(32, 64 + 10 * 64), new Vector2(19 * 64, 2)));
        _walls.Add(new Wall(_texture, new Vector2(32, 64), new Vector2(2, 10 * 64)));
        _walls.Add(new Wall(_texture, new Vector2(32 + 19 * 64, 64), new Vector2(2, 10 * 64)));

        foreach (var wall in _walls) {
            wall.SpriteColor = Color.White;
        }

        _spriteFont = ContentManager.Load<SpriteFont>("fonts/myfont");

        _panel = new Panel(_texture, _spriteFont, new Rectangle((int)_usableScreenWidth, 32, (int)_usableScreenWidth * 2, (int)_usableScreenWidth * 2));
        _restartButton = new Button(_texture, _spriteFont, new Rectangle(200, 100, 100, 100), "Restart");

        _gameOverText = new Text(_spriteFont, new Vector2(200, 200));
        _gameOverText.Scale = Vector2.One;
        _gameOverText.Content = "GAMER OVER";

        _youWonText = new Text(_spriteFont, new Vector2(200, 200));
        _youWonText.Scale = Vector2.One;
        _youWonText.Content = "YOU WON";

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

        // _paddle.Velocity = Vector2.Zero;

        _moveTimer += dt;

        if (_moveTimer >= 0.1f) {
            _canMove = true;
            _moveTimer = 0f;
        }

        if (_canMove) {
            for (int i = _bodies.Count - 1; i > 0; i--) {
                _bodies[i].Position = _bodies[i - 1].Position;
            }
            _bodies[0].Position += _newPosition * _speed;
            _canMove = false;
        }

        if (ks.IsKeyDown(Keys.W)) {
            _newPosition = new Vector2(0, -1);
            // _canMove = true;
        }

        if (ks.IsKeyDown(Keys.S)) {
            _newPosition = new Vector2(0, 1);
            // _canMove = true;
        }

        if (ks.IsKeyDown(Keys.A)) {
            _newPosition = new Vector2(-1, 0);
            // _canMove = true;
        }

        if (ks.IsKeyDown(Keys.D)) {
            _newPosition = new Vector2(1, 0);
        }

        if (_bodies[0].Bounds.Intersects(_apple.Bounds)) {
            float pos = _random.Next(19);
            float pos2 = _random.Next(10);
            _apple.Position = new Vector2(pos * 64, pos2 * 64);

            var body = new Body(_texture, _bodies[0].Position, new Vector2(64, 64));
            body.SpriteColor = new Color(108, 187, 60);
            _bodies.Add(body);
        }

        foreach (var wall in _walls) {
            if (_bodies[0].Bounds.Intersects(wall.Bounds)) {
                _isGameOver = true;
                _isPause = true;
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();

        foreach (var tile in _tiles) {
            tile.Draw(SpriteBatch);
        }

        foreach (var body in _bodies) {
            body.Draw(SpriteBatch);
        }

        _apple.Draw(SpriteBatch);

        foreach (var wall in _walls) {
            wall.Draw(SpriteBatch);
        }

        // SpriteBatch.DrawString(_spriteFont, "Ball angle: " + currentDeg.ToString(), new Vector2(100, 100), Color.Red);

        // _heart.Draw(SpriteBatch);
        SpriteBatch.DrawString(_spriteFont, _bodies.Count.ToString(), new Vector2(32, 16), Color.Red, 0f, Vector2.Zero, new Vector2(3f, 3f), SpriteEffects.None, 0f);

        foreach (var powerUp in _powerUps1) {
            powerUp.Draw(SpriteBatch);
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

    // private void ResetPositions()
    // {
    //     GenerateExtraBalls();

    //     _paddle.Position = new Vector2(_screenWidth / 2, (_screenHeight / 32) * 30);
    //     _lives -= 1;

    //     if (_lives <= 0) {
    //         _isGameOver = true;
    //     }
    // }

    // private void GenerateExtraBalls(int extra = 1)
    // {
    //     for (int i = 0; i < extra; i++) {
    //         var ball = new Ball(_texture, new Vector2(_screenWidth / 2, (_screenHeight / 32) * 29), new Vector2(16, 16));
    //         ball.SpriteColor = Color.White;
    //         ball.Speed = 400f;
    //         ball.Velocity = -Vector2.One;
    //         _balls.Add(ball);
    //     }
    // }

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
