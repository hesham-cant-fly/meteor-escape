using System;
using Raylib_cs;

namespace meteor_escape
{
    public class Program
    {
        public static Random rnd = new Random();
        public static Game game = new Game();
        public static Shader shader;
        private static void Main(string[] args)
        {
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.InitWindow(800, 700, "Meteor Escape");
            Raylib.SetTargetFPS(60);
            Raylib.HideCursor();
            Texture2D backgroundTexture = Raylib.LoadTexture("Assets/Space Background.png");

            game.Initialize();
            Globals.state = new();

            while (!Raylib.WindowShouldClose())
            {
                game.Update();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.RayWhite);

                Raylib.DrawTexturePro(
                    backgroundTexture,
                    new(0, 0, backgroundTexture.Width, backgroundTexture.Height),
                    new(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()),
                    new(0, 0),
                    0, Color.White
                );
                game.Draw();
                Raylib.DrawFPS(0, 0);
                Raylib.EndDrawing();

                game.PostProcess();
            }

            Raylib.CloseWindow();
        }
    }
}
