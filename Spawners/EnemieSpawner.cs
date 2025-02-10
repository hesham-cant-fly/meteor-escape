namespace meteor_escape.Spawners;

public class EnemieSpawner : Timer
{
    public EnemieSpawner(float delay, bool callOnStart = false) : base(delay, callOnStart)
    {
    }

    public override void Caller()
    {
        Globals.world.AddSprite(new Enemie());
    }
}
