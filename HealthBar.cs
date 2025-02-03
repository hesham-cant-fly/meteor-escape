
using Microsoft.Xna.Framework;

namespace meteor_escape;

internal class HealthBar
{
    private float _hp;
    private float _maxHP;

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
    public float MaxHP { get => _maxHP; set => _maxHP = value; }
    public float Percentage { get => _hp / _maxHP; }

    public HealthBar(float maxHP)
    {
        _hp = _maxHP = maxHP;
    }

    public void Draw(Vector2 offset)
    {
        var maxWidth = 60;
        var maxHeight = 15;
        var gap = 6;
        // Background
        Globals.spriteBatch.Draw(
            Globals.enemieTexture,
            new Rectangle(
                (int)(offset.X - maxWidth / 2), (int)(offset.Y - maxHeight / 2),
                maxWidth, maxHeight
            ),
            Color.Gray
        );

        // Forground
        Globals.spriteBatch.Draw(
            Globals.enemieTexture,
            new Rectangle(
                (int)(offset.X - maxWidth / 2 + gap / 2), (int)(offset.Y - maxHeight / 2 + gap / 2),
                (int)((maxWidth - gap) * Percentage), maxHeight - gap
            ),
            Color.Green
        );
    }
}
