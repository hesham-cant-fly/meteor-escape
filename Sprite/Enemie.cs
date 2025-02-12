using System;
using Raylib_cs;

namespace meteor_escape;

public class Enemie : Sprite
{
    public float AttackDamage { get; private set; }
    public float Health { get; private set; }

    public override SpriteKind Kind => SpriteKind.Enemie;

    public Enemie()
    {
        this.Pos = Game.GetRandomInScreenPosition();
        this.AttackDamage = 10;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Draw()
    {
        base.Draw();
    }

    public override void OnCollide(int index, int otherIndex, Sprite other)
    {
        if (other.Kind == SpriteKind.Player)
        {
            // ((Player)other).Active = true;
            ((Player)other).Damage(AttackDamage);
        }
    }
}
