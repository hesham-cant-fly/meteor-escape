using Raylib_cs;

namespace meteor_escape;

public abstract class Timer
{
    public float T { get; set; }
    public float Delay { get; set; }

    public abstract void Caller();

    public Timer(float delay, bool callOnStart)
    {
        this.T = callOnStart ? delay : 0;
        this.Delay = delay;
    }

    public void Progress()
    {
        T += Raylib.GetFrameTime();
        if (T >= Delay)
        {
            Caller();
            T = 0;
        }
    }
}
