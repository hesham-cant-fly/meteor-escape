using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// using nkast.Aether.Physics2D.Dynamics;

namespace meteor_escape;

internal class Globals
{
    public static SpriteFont hudFont;
    public static Texture2D enemieTexture;
    public static Texture2D pointTexture;

    public static FrameCounter frameCounter;
    public static GraphicsDeviceManager graphics;
    public static GraphicsDevice graphicsDevice;
    public static SpriteBatch spriteBatch;
    public static BasicEffect basicEffect;

    public static TheGame game;
    public static World world;

    public static readonly int scaleFactor = 1;

    public static void DrawOutlineRectangle(Rectangle rectangle, Color color, int lineWidth)
    {
        if (pointTexture == null)
        {
            pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pointTexture.SetData(new Color[] { Color.White });
        }

        // Draw the top line
        Globals.spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, lineWidth), color);

        // Draw the left line
        Globals.spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height), color);

        // Draw the right line
        Globals.spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X + rectangle.Width - lineWidth, rectangle.Y, lineWidth, rectangle.Height), color);

        // Draw the bottom line
        Globals.spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - lineWidth, rectangle.Width, lineWidth), color);
    }
}
