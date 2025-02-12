using System;
using Raylib_cs;

namespace meteor_escape;

public class Player : Sprite
{
    protected Vec2 _acc = Vec2.One * 60;
    protected float _friction = 0.95F;
    protected Texture2D _texture = Raylib.LoadTexture("Assets/space_ship.png");
    public HealthBar Health { get; private set; }
    public ImmunityFrames Imf { get; private set; }
    private float AttackSpeed = 0.1F;
    private float AttackT = 0.1F;
    public bool Active = false;

    public override System.Drawing.RectangleF Rect => new(Pos.X, Pos.Y, 100, 100);
    public override SpriteKind Kind => SpriteKind.Player;

    public override System.Drawing.RectangleF HitBox => new(Pos.X - 70 / 2, Pos.Y - 70 / 2, 70, 70);

    public Player()
    {
        Console.WriteLine(HitBox.ToString());
        this.Health = new HealthBar(100);
        this.Imf = new ImmunityFrames(20);
        this.Pos = new Vec2(
            Raylib.GetScreenWidth() / 2 - Rect.Width,
            Raylib.GetScreenHeight() / 2 - Rect.Height
        );
    }

    public override void Update()
    {
        float dt = Raylib.GetFrameTime();
        Vec2 mPos = Raylib.GetMousePosition();
        Vec2 newRot = Vec2.FromPolar(1, _pos.AngleTo(mPos) + float.Pi / 2);
        _rot = Vec2.Lerp(_rot, newRot, (float)0.05);

        AttackT -= dt;

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
            new Raylib_cs.Rectangle(HitBox.X, HitBox.Y, HitBox.Width, HitBox.Height),
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

        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            if (AttackT <= 0)
            {
                Bullet bullet = new Bullet(
                    Pos,
                    _rot.Angle - float.Pi / 2
                );
                var normVel = bullet.Vel.ToNormalize();
                bullet.Pos += normVel * 27.0F;
                Globals.world.AddBullet(bullet);
                AttackT = AttackSpeed;
            }
        }
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
