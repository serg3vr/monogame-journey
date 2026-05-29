using Microsoft.Xna.Framework;

namespace Breakout.Core.Scenes;

public class SceneManager
{
    public bool WantsToExit { get; private set; }

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

    public void Exit()
    {
        WantsToExit = true;
    }
}