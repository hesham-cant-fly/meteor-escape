using System;
using Raylib_cs;

namespace meteor_escape;

public class Player : Sprite
{
    protected Vec2 _acc = Vec2.One * 70;
    protected float _friction = 0.95F;
    protected Texture2D _texture = Raylib.LoadTexture("Assets/space_ship.png");
    public HealthBar Health { get; private set; }
    public ImmunityFrames Imf { get; private set; }
    public bool Active = false;

    public override System.Drawing.RectangleF Rect { get => new(Pos.X, Pos.Y, 100, 100); }

    public Player()
    {
        Console.WriteLine(OriginRect.ToString());
        this.Health = new HealthBar(100);
        this.Imf = new ImmunityFrames(20);
        this.Pos = new Vec2(
            Raylib.GetScreenWidth() / 2 - Rect.Width,
            Raylib.GetScreenHeight() / 2 - Rect.Height
        );
    }

    public override void Update()
    {
        Active = false;
        float dt = Raylib.GetFrameTime();
        Vec2 mPos = Raylib.GetMousePosition();
        Vec2 newRot = Vec2.FromPolar(1, _pos.AngleTo(mPos) + float.Pi / 2);
        /// Lerping it this way makes it always takes the shortest path
        _rot = Vec2.Lerp(_rot, newRot, (float)0.05);
        // This one is equivalent to the code above
        // _rot = new Vec2(
        //     float.Lerp(_rot.X, newRot.X, (float)0.1),
        //     float.Lerp(_rot.Y, newRot.Y, (float)0.1)
        // );

        Vel *= _friction;
        Imf.Progress();
        base.Update();
    }

    public override void Draw()
    {
        Raylib.DrawTexturePro(
            _texture,
            new(0, 0, _texture.Width, _texture.Height),
            new(Rect.X, Rect.Y, Rect.Width, Rect.Height),
            Origin,
            _rot.Angle.ToDeg(), Color.White
        );

        Raylib.DrawRectangleLinesEx(
            new Raylib_cs.Rectangle(Rect.X - Origin.X, Rect.Y - Origin.Y, Rect.Width, Rect.Height),
            (float)2.0, Color.Lime
        );

        if (Health.Racio != 1)
        {
            Health.Draw(new Vec2(Pos.X, Pos.Y - Rect.Height / 2));
        }
    }

    public override void InputHandling()
    {
        /// AZERTY Layout
        ///     [W]
        /// [A] [S] [D]
        ///

        if (Raylib.IsKeyDown(KeyboardKey.W))
        {
            Vel += _acc * -Vec2.UnitY;
        }
        if (Raylib.IsKeyDown(KeyboardKey.S))
        {
            Vel += _acc * Vec2.UnitY;
        }
        if (Raylib.IsKeyDown(KeyboardKey.A))
        {
            Vel += _acc * -Vec2.UnitX;
        }
        if (Raylib.IsKeyDown(KeyboardKey.D))
        {
            Vel += _acc * Vec2.UnitX;
        }
        // TODO: Bullets
    }

    public void Damage(float amount)
    {
        /// With Immunity Frames
        if (!Imf.IsImmune())
        {
            Health.HP -= amount;
            Imf.Activate();
        }
    }

    ~Player()
    {
        Raylib.UnloadTexture(_texture);
    }
}
