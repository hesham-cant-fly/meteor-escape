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
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
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
}
