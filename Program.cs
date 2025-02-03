using System;
using System.Drawing;
using Raylib_cs;

namespace meteor_escape
{
    public class Program
    {
        public static Random rnd = new Random();
        public static Game game = new Game();
        private static void Main(string[] args)
        {
            Raylib.InitWindow(800, 700, "Meteor Escape");
            Raylib.SetTargetFPS(60);

            game.Initialize();

            while (!Raylib.WindowShouldClose())
            {
                game.Update();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib_cs.Color.RayWhite);
                game.Draw();
                Raylib.DrawFPS(0, 0);
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
            // using var game = new meteor_escape.TheGame();
            // game.Run();
        }
    }

    public static class Entensions
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
                (float)4.0, color
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
}
