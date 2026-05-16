using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Scenes;

namespace Pong;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private GameScene _gameScene;
    private MenuScene _menuScene;

    private enum Scene { Menu, Game }

    private Scene _currentScene;

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
        // TODO: Add your initialization logic here

        _menuScene = new MenuScene(GraphicsDevice, Content);
        _menuScene.Initialize();

        _gameScene = new GameScene(GraphicsDevice, Content);
        _gameScene.Initialize();

        _currentScene = Scene.Menu;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        _gameScene.LoadContent(_spriteBatch);
        _menuScene.LoadContent(_spriteBatch);
    }

    protected override void Update(GameTime gameTime)
    {
        // float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        KeyboardState ks = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape)) {
            Exit();
        }

        // TODO: Add your update logic here

        if (_currentScene == Scene.Menu) {
            _menuScene.Update(gameTime);
        } if (_currentScene == Scene.Game) {
            _gameScene.Update(gameTime);
        } else {
            _menuScene.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here

        if (_currentScene == Scene.Menu) {
            _menuScene.Draw(gameTime);
        } if (_currentScene == Scene.Game) {
            _gameScene.Draw(gameTime);
        } else {
            _menuScene.Draw(gameTime);
        }

        base.Draw(gameTime);
    }
}
