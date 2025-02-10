namespace meteor_escape;

public class ImmunityFrames
{
    private int _f;
    private int _maxF;

    public ImmunityFrames(int maxF)
    {
        this._f = 0;
        this._maxF = maxF;
    }

    public void Activate()
    {
        _f = _maxF;
    }

    public void Progress()
    {
        if (_f >= 0)
        {
            _f -= 1;
        }
    }

    public bool IsImmune()
    {
        return _f > 0;
    }
}
