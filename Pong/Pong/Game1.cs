using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Core;
using Pong.GameObjects;

namespace Pong;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private GameScene _gameScene;

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

        _gameScene = new GameScene(GraphicsDevice, Content);
        _gameScene.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        _gameScene.LoadContent(_spriteBatch);
    }

    protected override void Update(GameTime gameTime)
    {
        // float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        KeyboardState ks = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape)) {
            Exit();
        }

        // TODO: Add your update logic here

        _gameScene.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here

        _gameScene.Draw(gameTime);

        base.Draw(gameTime);
    }
}
