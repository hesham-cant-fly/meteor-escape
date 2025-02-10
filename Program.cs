using System;
using Raylib_cs;

// (setq catppuccin-flavor 'frappe)
// (catppuccin-reload)

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
            Raylib.HideCursor();

            game.Initialize();
            Globals.state = new();

            while (!Raylib.WindowShouldClose())
            {
                game.Update();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib_cs.Color.RayWhite);
                game.Draw();
                Raylib.DrawFPS(0, 0);
                Raylib.EndDrawing();

                game.PostProcess();
            }

            Raylib.CloseWindow();
            // using var game = new meteor_escape.TheGame();
            // game.Run();
        }
    }
}
