using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Snake.Core.Scenes;

public abstract class Scene
{
    protected ContentManager ContentManager;
    protected GraphicsDevice GraphicsDevice;
    protected SpriteBatch SpriteBatch;
    protected SceneManager SceneManager;

    protected Scene(
        ContentManager contentManager,
        GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch,
        SceneManager sceneManager
    )
    {
        ContentManager = contentManager;
        GraphicsDevice = graphicsDevice;
        SpriteBatch = spriteBatch;
        SceneManager = sceneManager;
    }

    public virtual void Initialize()
    {
    }

    public virtual void LoadContent()
    {
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void Draw(GameTime gameTime)
    {
    }

    public virtual void Unload()
    {
    }
}