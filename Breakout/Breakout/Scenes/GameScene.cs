using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Breakout.Core.Scenes;
using Breakout.Graphics.Core;
using Breakout.GameObjects;

namespace Breakout;

public class GameScene : Scene
{
    private int _screenWidth;
    private int _screenHeight;

    private Texture2D _texture;

    private const int Rows = 6;
    private const int Columns = 12;
    private Sprite[,] _blocks = new Sprite[Columns, Rows];
    private Color[] _colors = {Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Purple, Color.Cyan};

    private Ball _ball;
    private Paddle _paddle;

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
        
    }

    public override void LoadContent()
    {
        _texture = new Texture2D(GraphicsDevice, 1, 1);
        _texture.SetData(new[] { Color.White });

        int blockWidth = _screenWidth / 13;
        int space = blockWidth / 12;

        for (int y = 0; y < Rows; y++) {
            for (int x = 0; x < Columns; x++) {
                _blocks[x, y] = new Sprite(_texture, new Vector2(x * (blockWidth + space) + space, y * (32 + space) + space), new Vector2(blockWidth, 32));
                _blocks[x, y].SpriteColor = _colors[y];
            }
        }

        _ball = new Ball(_texture, new Vector2(_screenWidth / 2, (_screenHeight / 32) * 29), new Vector2(32, 32));
        _ball.SpriteColor = Color.White;
        _ball.Speed = 400f;
        _ball.Velocity = Vector2.One;

        _paddle = new Paddle(_texture, new Vector2(_screenWidth / 2, (_screenHeight / 32) * 30), new Vector2(blockWidth, 32));
        _paddle.SpriteColor = Color.White;
        _paddle.Speed = 400f;
    }

    public override void Update(GameTime gameTime)
    {
        KeyboardState ks = Keyboard.GetState();
        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        // Exit();

        // TODO: Add your update logic here

        // base.Update(gameTime);

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
    
        if (_ball.Bounds.Top < 0) {
            _ball.Velocity = new Vector2(_ball.Velocity.X, 1);
        }
        if (_ball.Bounds.Center.Y > _screenHeight) {
            _ball.Velocity = new Vector2(_ball.Velocity.X, -1);
        }
        if (_ball.Bounds.Left < 0) {
            _ball.Velocity = new Vector2(1, _ball.Velocity.Y);
        }
        if (_ball.Bounds.Center.X > _screenWidth) {
            _ball.Velocity = new Vector2(-1, _ball.Velocity.Y);
        }

        _ball.Position += _ball.Velocity * _ball.Speed * dt;
        _ball.Velocity.Normalize();
        
        _paddle.Velocity = Vector2.Zero;

        if (ks.IsKeyDown(Keys.A)) {
           _paddle.Velocity = new Vector2(-1, 0); 
        }

        if (ks.IsKeyDown(Keys.D)) {
           _paddle.Velocity = new Vector2(1, 0); 
        }

        _paddle.Position += _paddle.Velocity * _paddle.Speed * dt;
        _paddle.Position = new Vector2(
            MathHelper.Clamp(_paddle.Position.X, 0, _screenWidth), 
            _paddle.Position.Y
        );
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();

        // TODO: Add your drawing code here

        // base.Draw(gameTime);

        // SpriteBatch.Draw(_texture, new Rectangle(100, 100, 100, 100), Color.Red);


        for (int y = 0; y < Rows; y++) {
            for (int x = 0; x < Columns; x++) {
                _blocks[x, y].Draw(SpriteBatch);
            }
        }

        _ball.Draw(SpriteBatch);

        _paddle.Draw(SpriteBatch);

        SpriteBatch.End();
    }
}
