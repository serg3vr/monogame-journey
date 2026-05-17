using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Core.Scenes;
using Pong.Core.UI;
using Pong.Scenes;

public class MenuScene : Scene
{

    private int _screenWidth;
    private int _screenHeight;

    private Texture2D _square;

    private const int PIXEL_WIDTH = 20;

    private SpriteFont _spriteFont;
    private SpriteFont _uiFont;

    private Vector2 _rightScorePosition;

    private bool _isPause = false;
    private float _pauseTimer = 0;

    private Button _button;

    public MenuScene(
        ContentManager content,
        GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch,
        SceneManager sceneManager)
        : base(content, graphicsDevice, spriteBatch, sceneManager)
    {
    }

    public override void Initialize()
    {
        _screenWidth = GraphicsDevice.Viewport.Width;
        _screenHeight = GraphicsDevice.Viewport.Height;
    }

    public override void LoadContent()
    {
        _square = Button.CreateRoundedRectTexture(GraphicsDevice, 200, 60, 10);

        _spriteFont = ContentManager.Load<SpriteFont>("myfont");
        _uiFont = ContentManager.Load<SpriteFont>("myuifont");
        _rightScorePosition = new Vector2(_screenWidth / 2 + PIXEL_WIDTH * 3, PIXEL_WIDTH * 3);

        _button = new Button(_square, _uiFont, new Rectangle(_screenWidth / 2, _screenHeight / 2, 200, 60), "START");

        _button.OnClick = () =>
        {
            SceneManager.ChangeScene(
                new GameScene(ContentManager, GraphicsDevice, SpriteBatch, SceneManager)
            );
        };
    }

    public override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        KeyboardState ks = Keyboard.GetState();
        
        if (_isPause) {
            _pauseTimer += dt;

            if (_pauseTimer >= 1) {
                _isPause = false;
                _pauseTimer = 0;
            }
            return;
        }

        _button.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp);

        Color objectsColor = new Color(100, 100, 100);

        string rightText = "Pong";
        Vector2 fontOriginRight = _spriteFont.MeasureString(rightText) / 2;
        SpriteBatch.DrawString(_spriteFont, rightText, _rightScorePosition, objectsColor, 0, fontOriginRight, 5.0f, SpriteEffects.None, 0.5f);

        _button.Draw(SpriteBatch);
        
        SpriteBatch.End();
    }
}
