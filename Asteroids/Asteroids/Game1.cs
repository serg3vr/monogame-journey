using Asteroids.Core;
using Asteroids.Core.Scenes;
using Asteroids.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids;

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

        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 768;

        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        Globals.ScreenWidth = _graphics.PreferredBackBufferWidth;
        Globals.ScreenHeight = _graphics.PreferredBackBufferHeight;

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
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _sceneManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _sceneManager.Draw(gameTime);

        base.Draw(gameTime);
    }
}
