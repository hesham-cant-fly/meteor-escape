using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace meteor_escape;

public class Sprite
{
    protected Vec2 _pos;
    protected Vec2 _vel;
    protected float _rot;
    protected Texture2D _texture;

    public Vec2 Pos { get => _pos; set => _pos = value; }
    public Vec2 Vel { get => _vel; set => _vel = value; }
    public float Rot { get => _rot; set => _rot = value; }
    public Texture2D Texture { get => _texture; protected set => _texture = value; }
    public Vector2 Origin { get => _texture.Bounds.Size.ToVector2() * 0.5f; }
    public Rectangle Rect { get => new Rectangle((int)Pos.X, (int)Pos.Y, 10, 10); }
    public Vector2 Dim { get => new Vector2(Rect.Width, Rect.Height); }

    public Sprite(Texture2D texture)
    {
        this._texture = texture;
    }

    public virtual void Update(GameTime gameTime)
    {
        InputHandling(gameTime);
    }

    public virtual void InputHandling(GameTime gameTime) { }

    public virtual void Draw(GameTime gameTime)
    {
        Globals.spriteBatch.Draw(
            _texture,
            Rect,
            null,
            Color.Black,
            Rot,
            Origin,
            SpriteEffects.None,
            0F
        );
    }

    // Events
    public virtual void OnCollide(Sprite other)
    {
        Console.WriteLine("Collided");
    }
}
