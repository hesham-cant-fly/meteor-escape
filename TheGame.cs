using System;
using Raylib_cs;

namespace meteor_escape;

public class Game
{
    private float _timer = 0;
    private Spawners.EnemieSpawner _spawner = new(10, true);

    public Game()
    { }

    public void Initialize()
    {
        LoadContent();
    }

    public void LoadContent()
    {
        Globals.world = new World();

        Globals.world.AddSprite(new Player());
    }

    public void Update()
    {
        _timer -= Raylib.GetFrameTime();

        _spawner.Progress();
        Globals.world.Update();
    }

    public void Draw()
    {
        Globals.world.Draw();
        var mousePos = Raylib.GetMousePosition();
        Raylib.DrawCircle(
            (int)mousePos.X, (int)mousePos.Y,
            5, new Color(186, 186, 186)
        );
    }

    public void PostProcess() => Globals.world.PostProcess();

    public static Vec2 GetRandomInScreenPosition()
    {
        Random rnd = new Random();
        return new Vec2(
            rnd.Next(Raylib.GetScreenWidth()),
            rnd.Next(Raylib.GetScreenHeight())
        );
    }
}
