using System;
using Raylib_cs;

namespace meteor_escape;

public class Sprite
{
    protected Vec2 _pos = Vec2.Zero;
    protected Vec2 _vel = Vec2.Zero;
    protected Vec2 _rot = Vec2.Zero;

    public Vec2 Pos { get => _pos; set => _pos = value; }
    public Vec2 Vel { get => _vel; set => _vel = value; }
    public Vec2 Rot { get => _rot; set => _rot = value; }
    public virtual Vec2 Origin { get => new(Rect.Width / 2, Rect.Height / 2); }
    public virtual System.Drawing.RectangleF Rect { get => new(Pos.X, Pos.Y, 10, 10); }

    public virtual System.Drawing.RectangleF OriginRect { get => new(Pos.X - Origin.X, Pos.Y - Origin.Y, Rect.Width, Rect.Height); }

    public Sprite()
    {
    }

    public virtual void Update()
    {
        Pos += Vel * Raylib.GetFrameTime();
        InputHandling();
    }

    public virtual void InputHandling() { }

    public virtual void Draw()
    {
        Raylib.DrawRectanglePro(
            new(Rect.X, Rect.Y, Rect.Width, Rect.Height),
            new(Rect.Width / 2, Rect.Height / 2),
            _rot.Angle.ToDeg(),
            Color.Black
        );
    }

    public bool IsCollidingWith(Sprite other)
    {
        return OriginRect.IntersectsWith(other.OriginRect);
    }

    /// Events
    public virtual void OnCollide(int index, int otherIndex, Sprite other)
    {
    }
}
