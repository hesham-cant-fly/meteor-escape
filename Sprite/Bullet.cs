using System;
using System.Diagnostics;
using Raylib_cs;

namespace meteor_escape;

public class Bullet : Sprite
{
    public override System.Drawing.RectangleF Rect => new(Pos.X, Pos.Y, 5, 5);
    public override SpriteKind Kind => SpriteKind.Bullet;
    public float AttackDamage { get; private set; }

    public Bullet(float x, float y, float angle)
    {
        this._pos.X = x;
        this._pos.Y = y;

        this._vel = Vec2.FromPolar(500, angle);
    }

    public override void Draw()
    {

        Raylib.DrawRectanglePro(
            new(Rect.X, Rect.Y, Rect.Width, Rect.Height),
            new(Rect.Width / 2, Rect.Height / 2),
            _rot.Angle.ToDeg(),
            Color.Orange
        );
    }

    public Bullet(Vec2 pos, float angle)
    {
        this._pos.X = pos.X;
        this._pos.Y = pos.Y;

        this._vel = Vec2.FromPolar(500, angle);
    }

    public override void OnCollide(int index, int otherIndex, Sprite other)
    {
        Console.WriteLine("Here");
        Debug.Assert(other.Kind == SpriteKind.Enemie, $"this bullet did collide with a non-enemie sprite: '{other.Kind}'");

        Globals.world.RemoveEnemieAt(otherIndex);
        Globals.world.RemoveBulletAt(index);
    }
}
