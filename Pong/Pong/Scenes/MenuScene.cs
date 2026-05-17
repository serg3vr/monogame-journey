using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Core;
using Pong.Core.UI;
using Pong.GameObjects;

namespace Pong.Scenes;

public class MenuScene
{
    private GraphicsDevice _graphicsDevice;
    private SpriteBatch _spriteBatch;
    private ContentManager _contentManager;

    private int _screenWidth;
    private int _screenHeight;

    private Texture2D _square;

    private Sprite _leftPaddle;
    private Sprite _rightPaddle;
    private Ball _ball;
    private const int PIXEL_WIDTH = 20;

    private SpriteFont _spriteFont;
    private SpriteFont _uiFont;

    private int _leftScore;
    private Vector2 _leftScorePosition;
    private int _rightScore;
    private Vector2 _rightScorePosition;

    // private Vector2 _ballVelocity;
    private float _ballAngle;

    private bool _isPause = false;
    private float _pauseTimer = 0;

    private Button _button;
    private Texture2D _buttonTexture;

    public MenuScene(GraphicsDevice graphicsDevice, ContentManager contentManager)
    {
        _graphicsDevice = graphicsDevice;
        // _spriteBatch = spriteBatch;
        _contentManager = contentManager;
    }

    public void Initialize()
    {
        // TODO: Add your initialization logic here

        _screenWidth = _graphicsDevice.Viewport.Width;
        _screenHeight = _graphicsDevice.Viewport.Height;

        // _leftScore = 0;
        // _rightScore = 0;

        // _ballAngle = -30;

        // base.Initialize();
    }

    public void LoadContent(SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;

        // TODO: use this.Content to load your game content here

        // _square = new Texture2D(_graphicsDevice, 1, 1);
        // _square.SetData(new[] { Color.White });

        _square = Button.CreateRoundedRectTexture(_graphicsDevice, 200, 60, 10);

        _spriteFont = _contentManager.Load<SpriteFont>("myfont");
        _uiFont = _contentManager.Load<SpriteFont>("myuifont");
        // _leftScorePosition = new Vector2(_screenWidth / 2 - PIXEL_WIDTH * 3, PIXEL_WIDTH * 3);
        _rightScorePosition = new Vector2(_screenWidth / 2 + PIXEL_WIDTH * 3, PIXEL_WIDTH * 3);

        _button = new Button(_square, _uiFont, new Rectangle(_screenWidth / 2, _screenHeight / 2, 200, 60), "START");

        _button.OnClick = () =>
        {
            // gameStarted = true;
            // throw new Exception();
            // System.Diagnostics.Debug.WriteLine("Message");
        };
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        KeyboardState ks = Keyboard.GetState();

        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape)) {
        //     Exit();
        // }
        
        if (_isPause) {
            _pauseTimer += dt;

            if (_pauseTimer >= 1) {
                _isPause = false;
                _pauseTimer = 0;
            }
            return;
        }

        // TODO: Add your update logic here

        // base.Update(gameTime);
        _button.Update();
    }

    public void Draw(GameTime gameTime)
    {
        // _graphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp);

        Color objectsColor = new Color(100, 100, 100);

        string rightText = "Pong";
        Vector2 fontOriginRight = _spriteFont.MeasureString(rightText) / 2;
        _spriteBatch.DrawString(_spriteFont, rightText, _rightScorePosition, objectsColor, 0, fontOriginRight, 5.0f, SpriteEffects.None, 0.5f);

        _button.Draw(_spriteBatch);
        _spriteBatch.End();

        // base.Draw(gameTime);
    }
}
