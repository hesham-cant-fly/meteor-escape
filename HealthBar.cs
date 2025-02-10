using Raylib_cs;

namespace meteor_escape;

public class HealthBar
{
    private float _hp;
    public float MaxHP { get; set; }

    public float HP
    {
        get => _hp;
        set
        {
            _hp = value;
            if (_hp < 0)
            {
                _hp = 0;
            }
        }
    }
    public float Racio { get => _hp / MaxHP; }

    public HealthBar(float maxHP)
    {
        _hp = MaxHP = maxHP;
    }

    public void Draw(Vec2 offset)
    {
        const int maxWidth = 60;
        const int maxHeight = 15;
        const int gap = 4;
        /// Background
        Raylib.DrawRectangle(
            (int)(offset.X - maxWidth / 2), (int)(offset.Y - maxHeight / 2),
            maxWidth, maxHeight,
            Color.Gray
        );

        /// Forground
        Raylib.DrawRectangle(
            (int)(offset.X - maxWidth / 2 + gap / 2), (int)(offset.Y - maxHeight / 2 + gap / 2),
            (int)((maxWidth - gap) * Racio), maxHeight - gap,
            Color.Green
        );
    }
}
