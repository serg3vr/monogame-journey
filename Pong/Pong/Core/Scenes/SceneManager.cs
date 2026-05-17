using Microsoft.Xna.Framework;

namespace Pong.Core.Scenes;

public class SceneManager
{
    public Scene CurrentScene { get; private set; }

    public void ChangeScene(Scene newScene)
    {
        CurrentScene?.Unload();

        CurrentScene = newScene;
        CurrentScene.Initialize();
        CurrentScene.LoadContent();
    }

    public void Update(GameTime gameTime)
    {
        CurrentScene?.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        CurrentScene?.Draw(gameTime);
    }
}