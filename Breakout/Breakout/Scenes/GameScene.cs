using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Breakout.Core.Scenes;
using Breakout.Graphics.Core;

namespace Breakout;

public class GameScene : Scene
{
    // private GraphicsDeviceManager _graphics;
    // private SpriteBatch _spriteBatch;

    private Texture2D _texture;

    private Sprite[,] _blocks = new Sprite[2, 2];

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

        // base.Initialize();
    }

    public override void LoadContent()
    {
        _texture = new Texture2D(GraphicsDevice, 1, 1);
        _texture.SetData(new[] { Color.White });
    }

    public override void Update(GameTime gameTime)
    {
        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        // Exit();

        // TODO: Add your update logic here

        // base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();

        // TODO: Add your drawing code here

        // base.Draw(gameTime);

        SpriteBatch.Draw(_texture, new Rectangle(100, 100, 100, 100), Color.Red);

        SpriteBatch.End();
    }
}
