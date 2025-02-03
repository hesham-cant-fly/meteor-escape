using System;
using Raylib_cs;

namespace meteor_escape;

public class Sprite
{
    protected Vec2 _pos;
    protected Vec2 _vel = new Vec2(Program.rnd.Next(-50, 50), Program.rnd.Next(-50, 50));
    protected float _rot;
    private bool active = false;

    public Vec2 Pos { get => _pos; set => _pos = value; }
    public Vec2 Vel { get => _vel; set => _vel = value; }
    public float Rot { get => _rot; set => _rot = value; }
    public System.Drawing.RectangleF Rect { get => new(Pos.X, Pos.Y, 20, 20); }

    public Sprite()
    {
    }

    public virtual void Update()
    {
        active = false;
        if (Pos.X < 0)
        {
            _vel.X *= -1;
            _pos.X = 0;
        }
        if (Pos.X + Rect.Width > Raylib.GetScreenWidth())
        {
            _vel.X *= -1;
            _pos.X = Raylib.GetScreenWidth() - Rect.Width;
        }
        if (Pos.Y < 0)
        {
            _vel.Y *= -1;
            _pos.Y = 0;
        }
        if (Pos.Y + Rect.Height > Raylib.GetScreenHeight())
        {
            _vel.Y *= -1;
            _pos.Y = Raylib.GetScreenHeight() - Rect.Height;
        }
        Pos += Vel * Raylib.GetFrameTime();
        InputHandling();
    }

    public virtual void InputHandling() { }

    public virtual void Draw()
    {
        Raylib.DrawRectangle(
            (int)Pos.X, (int)Pos.Y,
            (int)Rect.Width, (int)Rect.Height,
            active ? Color.Blue : Color.Black
        );
    }

    public bool IsCollidingWith(Sprite other)
    {
        return Rect.IntersectsWith(other.Rect);
    }

    // Events
    public virtual void OnCollide(Sprite other)
    {
        active = true;
        // Console.WriteLine("Collided");
    }
}
