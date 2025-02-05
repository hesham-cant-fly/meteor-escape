using System;
using Raylib_cs;

namespace meteor_escape;

public class Game
{
    // private float _enemieSummonTimer = 0F;
    public static ulong _collisionDetection = 0L;
    private float _timer = 0;

    public Game()
    {
    }

    public void Initialize()
    {
        Globals.frameCounter = new();
        LoadContent();
    }

    public void LoadContent()
    {
        Globals.world = new World();

        var rnd = new Random();
        for (int i = 0; i < 1000; i++)
        {
            var spr = new Sprite();
            spr.Pos = new Vec2(rnd.Next(Raylib.GetScreenWidth()), rnd.Next(Raylib.GetScreenHeight()));
            Globals.world.AddSprite(
                spr
            );
        }
    }

    public void Update()
    {
        _collisionDetection = 0L;
        float dt = Raylib.GetFrameTime();
        // Globals.frameCounter.Update(dt);
        _timer -= dt;

        Globals.world.Update();
    }

    public void Draw()
    {
        Globals.world.Draw();
    }

    public static Vec2 GetRandomInScreenPosition()
    {
        Random rnd = new Random();
        return new Vec2(
            rnd.Next(Raylib.GetScreenWidth()),
            rnd.Next(Raylib.GetScreenHeight())
        );
    }
}
