using System;
using System.Drawing;
using Microsoft.Xna.Framework;

namespace meteor_escape;

public struct Vec2(float x, float y)
{
    public float X { get; set; } = x;
    public float Y { get; set; } = y;
    public float Length2
    {
        get => X * X + Y * Y;
        set => this.Length = MathF.Sqrt(value);
    }
    public float Length
    {
        get => MathF.Sqrt(this.Length2);
        set
        {
            float angle = this.Angle;
            this.X = MathF.Cos(angle) * value;
            this.Y = MathF.Sin(angle) * value;
        }
    }
    public float Angle
    {
        get => MathF.Atan2(X, Y);
        set
        {
            float length = this.Length;
            this.X = MathF.Cos(value) * length;
            this.Y = MathF.Sin(value) * length;
        }
    }

    public static Vec2 FromPolar(float length, float angle = 0)
    {
        return new Vec2(
            length * MathF.Cos(angle),
            length * MathF.Sin(angle)
        );
    }

    public float DistanceTo2(float vx, float vy)
    {
        vx -= this.X;
        vy -= this.Y;
        return (vx * vx + vy * vy);
    }

    public float DistanceTo2(Vec2 v)
    {
        float vx = v.X - this.X,
              vy = v.Y - this.Y;
        return (vx * vx + vy * vy);
    }

    public float DistanceTo(float vx, float vy)
    {
        return MathF.Sqrt(DistanceTo2(vx, vy));
    }

    public float DistanceTo(Vec2 v)
    {
        return MathF.Sqrt(DistanceTo2(v));
    }

    public void Normalize()
    {
        float length = this.Length;
        X /= length;
        Y /= length;
    }

    public Vec2 ToNormalize()
    {
        float length = this.Length;
        return new Vec2(
            X / length,
            Y / length
        );
    }

    public float Dot(float vx, float vy) => (X * vx) + (Y * vy);
    public float Dot(Vec2 v) => (v.X * X) + (v.Y * Y);
    public float Cross(float vx, float vy) => (vx * Y) - (vy * X);

    public static Vec2 operator +(Vec2 self) => new Vec2(self.X, self.Y);
    public static Vec2 operator -(Vec2 self) => new Vec2(-self.X, -self.Y);
    public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.X + b.X, a.Y + b.Y);
    public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.X - b.X, a.Y - b.Y);
    public static Vec2 operator /(Vec2 a, Vec2 b) => new Vec2(a.X / b.X, a.Y / b.Y);
    public static Vec2 operator /(Vec2 a, float divider) => new Vec2(a.X / divider, a.Y / divider);
    public static Vec2 operator *(Vec2 a, Vec2 b) => new Vec2(a.X * b.X, a.Y * b.Y);
    public static Vec2 operator *(float scaleFactor, Vec2 b) => new Vec2(scaleFactor * b.X, scaleFactor * b.Y);
    public static Vec2 operator *(Vec2 a, float scaleFactor) => new Vec2(a.X * scaleFactor, a.Y * scaleFactor);
    public static bool operator ==(Vec2 a, Vec2 b) => a.X == b.X && b.Y == b.Y;
    public static bool operator !=(Vec2 a, Vec2 b) => a.X != b.X && b.Y != b.Y;
    public static implicit operator Vector2(Vec2 self) => new Vector2(self.X, self.Y);
    public static implicit operator Vec2(Vector2 self) => new Vec2(self.X, self.Y);
    public static implicit operator Vec2(System.Numerics.Vector2 a) => new Vec2(a.X, a.Y);
    public static implicit operator PointF(Vec2 self) => new PointF(self.X, self.Y);

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        float x, y;
        if (obj is Vec2)
        {
            x = ((Vec2)obj).X;
            y = ((Vec2)obj).Y;
        }
        else if (obj is Vector2)
        {
            x = ((Vector2)obj).X;
            y = ((Vector2)obj).Y;
        }
        else if (obj is System.Numerics.Vector2)
        {
            x = ((System.Numerics.Vector2)obj).X;
            y = ((System.Numerics.Vector2)obj).Y;
        }
        else
        {
            return false;
        }
        return (X == x) && (y == Y);
    }

    public override string ToString()
    {
        return $"{{ X: {X}, Y: {Y} }}";
    }
}
