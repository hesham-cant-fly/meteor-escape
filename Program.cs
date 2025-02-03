using Raylib_cs;

internal class Program
{
    private static void Main(string[] args)
    {
        Raylib.InitWindow(500, 500, "Meteor Escape");
        Raylib.SetTargetFPS(60);


        Raylib.CloseWindow();
        // using var game = new meteor_escape.TheGame();
        // game.Run();
    }
}
