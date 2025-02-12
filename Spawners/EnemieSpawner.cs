namespace meteor_escape.Spawners;

public class EnemieSpawner : Timer
{
    public EnemieSpawner(float delay, bool callOnStart = false) : base(delay, callOnStart)
    {
    }

    public override void Caller()
    {
        Enemie enemie = new Enemie();
        enemie.Vel = Vec2.FromPolar(
            50, enemie.Pos.AngleTo(Globals.world.Player.Pos)
        );
        Globals.world.AddEnemie(enemie);
    }
}
