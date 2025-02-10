using System;
using Raylib_cs;

namespace meteor_escape;

public class Enemie : Sprite
{
    public float AttackDamage { get; private set; }
    public float Health { get; private set; }

    public Enemie()
    {
        var rnd = new Random();
        this.Pos = Game.GetRandomInScreenPosition();
        this.AttackDamage = 10;
        // _vel = Vec2.FromPolar(
        //     20, (float)rnd.NextDouble() * float.Pi * 2
        // );
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
        if (other is Player)
        {
            // ((Player)other).Active = true;
            ((Player)other).Damage(AttackDamage);
        }
    }
}
