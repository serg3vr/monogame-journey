using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Breakout.Core.Scenes;
using Breakout.Graphics.Core;
using Breakout.GameObjects;
using System.Collections.Generic;

namespace Breakout;

public class GameScene : Scene
{
    private int _screenWidth;
    private int _screenHeight;

    private Texture2D _texture;

    private const int Rows = 6;
    private const int Columns = 12;
    private List<Brick> _bricks;
    private Color[] _colors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Purple, Color.Cyan };

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

        _bricks = new List<Brick>();
    }

    public override void LoadContent()
    {
        _texture = new Texture2D(GraphicsDevice, 1, 1);
        _texture.SetData(new[] { Color.White });

        int blockWidth = _screenWidth / 13;
        int space = blockWidth / 12;

        for (int y = 0; y < Rows; y++) {
            for (int x = 0; x < Columns; x++) {
                var newBrick = new Brick(_texture, new Vector2(x * (blockWidth + space) + space, y * (32 + space) + space), new Vector2(blockWidth, 32));
                newBrick.SpriteColor = _colors[y];
                _bricks.Add(newBrick);
            }
        }

        _ball = new Ball(_texture, new Vector2(_screenWidth / 2, (_screenHeight / 32) * 29), new Vector2(32, 32));
        _ball.SpriteColor = Color.White;
        _ball.Speed = 600f;
        _ball.Velocity = Vector2.One;

        _paddle = new Paddle(_texture, new Vector2(_screenWidth / 2, (_screenHeight / 32) * 30), new Vector2(blockWidth, 32));
        _paddle.SpriteColor = Color.White;
        _paddle.Speed = 700f;
    }

    public override void Update(GameTime gameTime)
    {
        KeyboardState ks = Keyboard.GetState();
        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        // Exit();

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _paddle.Velocity = Vector2.Zero;

        if (ks.IsKeyDown(Keys.A)) {
            _paddle.Velocity = new Vector2(-1, 0);
        }

        if (ks.IsKeyDown(Keys.D)) {
            _paddle.Velocity = new Vector2(1, 0);
        }

        _paddle.Position += _paddle.Velocity * _paddle.Speed * dt;
        _paddle.Position = new Vector2(
            MathHelper.Clamp(_paddle.Position.X, 0, _screenWidth - _paddle.Bounds.Width),
            _paddle.Position.Y
        );

        if (_ball.Bounds.Top < 0) {
            _ball.Velocity = new Vector2(_ball.Velocity.X, MathF.Abs(_ball.Velocity.Y));
        }
        if (_ball.Bounds.Center.Y > _screenHeight) {
            _ball.Velocity = new Vector2(_ball.Velocity.X, -MathF.Abs(_ball.Velocity.Y));
        }
        if (_ball.Bounds.Left < 0) {
            _ball.Velocity = new Vector2(MathF.Abs(_ball.Velocity.X), _ball.Velocity.Y);
        }
        if (_ball.Bounds.Center.X > _screenWidth) {
            _ball.Velocity = new Vector2(-MathF.Abs(_ball.Velocity.X), _ball.Velocity.Y);
        }

        Brick? hittedBrick = null;

        foreach (var brick in _bricks) {
            if (_ball.Bounds.Intersects(brick.Bounds)) {
                if (_ball.Bounds.Top > brick.Bounds.Top) {
                    _ball.Velocity = new Vector2(_ball.Velocity.X, MathF.Abs(_ball.Velocity.Y));
                }
                if (_ball.Bounds.Bottom < brick.Bounds.Top) {
                    _ball.Velocity = new Vector2(_ball.Velocity.X, -MathF.Abs(_ball.Velocity.Y));
                }
                if (_ball.Bounds.Left > brick.Bounds.Right) {
                    _ball.Velocity = new Vector2(MathF.Abs(_ball.Velocity.X), _ball.Velocity.Y);
                }
                if (_ball.Bounds.Right < brick.Bounds.Left) {
                    _ball.Velocity = new Vector2(-MathF.Abs(_ball.Velocity.X), _ball.Velocity.Y);
                }
                hittedBrick = brick;
                break;
            }
        }
        _bricks.Remove(hittedBrick);

        if (_paddle.Bounds.Intersects(_ball.Bounds)) {
            var isMovingToLeft = ks.IsKeyDown(Keys.A);
            var isMovingToRight = ks.IsKeyDown(Keys.D);

            // if (isMovingToLeft || isMovingToRight) {
                float hitPos = (_ball.Bounds.Center.X - _paddle.Bounds.Center.X) / (_paddle.Bounds.Width / 2f);
                float angleDeg = MathHelper.Lerp(-120f, -60f, (hitPos + 1f) / 2f);
                float angleRad = MathHelper.ToRadians(angleDeg);
                float moveX = Math.Abs(MathF.Cos(angleRad)); // * (isMovingToLeft ? -1 : 1);

                if (isMovingToLeft) {
                    moveX = -Math.Abs(MathF.Cos(angleRad));
                } else if (isMovingToRight) {
                    moveX = Math.Abs(MathF.Cos(angleRad));
                }

                _ball.Velocity = new Vector2(moveX, MathF.Sin(angleRad)); ;
            // } else {
            //     _ball.Velocity = new Vector2(_ball.Velocity.X, _ball.Velocity.Y * -1);
            // }
            _ball.Position = new Vector2(_ball.Position.X, _paddle.Bounds.Top - _ball.Bounds.Height);
        }

        _ball.Position += _ball.Velocity * _ball.Speed * dt;
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();

        foreach (var block in _bricks) {
            block.Draw(SpriteBatch);
        }

        _ball.Draw(SpriteBatch);

        _paddle.Draw(SpriteBatch);

        SpriteBatch.End();
    }
}
