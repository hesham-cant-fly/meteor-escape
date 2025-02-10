using System.Drawing;
using Raylib_cs;

namespace meteor_escape;

public static class Extensions
{
    public static void Draw(this RectangleF rect, Raylib_cs.Color color)
    {
        Raylib.DrawRectangle(
            (int)rect.X, (int)rect.Y,
            (int)rect.Width, (int)rect.Height,
            color
        );
    }

    public static void Draw(this RectangleF rect)
    {
        Raylib.DrawRectangle(
            (int)rect.X, (int)rect.Y,
            (int)rect.Width, (int)rect.Height,
            Raylib_cs.Color.Black
        );
    }

    public static void DrawLines(this RectangleF rect, Raylib_cs.Color color)
    {
        Raylib.DrawRectangleLinesEx(
            new Raylib_cs.Rectangle(rect.X, rect.Y, rect.Width, rect.Height),
            (float)2.0, color
        );
    }

    public static void DrawLines(this RectangleF rect)
    {
        Raylib.DrawRectangleLines(
            (int)rect.X, (int)rect.Y,
            (int)rect.Width, (int)rect.Height,
            Raylib_cs.Color.Black
        );
    }
}

