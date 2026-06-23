using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FlappyBird.Core.Scenes;

namespace FlappyBird;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SceneManager _sceneManager;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;

        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        _sceneManager = new SceneManager();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _sceneManager.ChangeScene(
            new GameScene(Content, GraphicsDevice, _spriteBatch, _sceneManager)
        );
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState ks = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape)) {
            Exit();
        }

        _sceneManager.Update(gameTime);

        if (_sceneManager.WantsToExit) {
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SteelBlue);

        _sceneManager.Draw(gameTime);

        base.Draw(gameTime);
    }
}
