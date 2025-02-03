using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
// using nkast.Aether.Physics2D.Dynamics;

namespace meteor_escape;

internal class TheGame : Game
{
    // private float _enemieSummonTimer = 0F;
    public static ulong _collisionDetection = 0L;
    private float _timer = 0;

    public TheGame()
    {
        Globals.game = this;
        Globals.graphics = new GraphicsDeviceManager(this);
        Globals.frameCounter = new FrameCounter();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Window.AllowUserResizing = true;
        Window.Title = "Meteor Escape";
    }

    protected override void Initialize()
    {
        Globals.basicEffect = new BasicEffect(GraphicsDevice)
        {
            VertexColorEnabled = true
        };

        // NOTE: ALWAYS DEFER THIS LITTLE SH*T
        base.Initialize();
    }

    protected override void LoadContent()
    {
        Globals.spriteBatch = new SpriteBatch(GraphicsDevice);
        Globals.hudFont = Content.Load<SpriteFont>("Hud");
        Globals.graphicsDevice = GraphicsDevice;
        Globals.world = new World();

        var shipTexture = Content.Load<Texture2D>("space_ship");
        {
            Globals.enemieTexture = new(GraphicsDevice, 50, 50);
            Color[] data = new Color[2500];

            for (int i = 0; i < 2500; i++)
            {
                data[i] = Color.White;
            }
            Globals.enemieTexture.SetData(data);
        }
        var rnd = new Random();
        for (int i = 0; i < 100; i++)
        {
            var spr = new Sprite(Globals.enemieTexture);
            spr.Pos = new Vec2(
                rnd.Next((int)(Globals.graphicsDevice.Viewport.Bounds.Width * 2.4)), rnd.Next((int)(Globals.graphicsDevice.Viewport.Bounds.Height * 2.1)));
            Globals.world.AddSprite(
                spr
            );
        }

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        _collisionDetection = 0L;
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        var mState = Mouse.GetState();
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Globals.frameCounter.Update(dt);
        _timer -= dt;

        Globals.world.Step(gameTime);

        var rnd = new Random();
        if (_timer <= 0 && mState.LeftButton == ButtonState.Pressed)
        {
            var spr = new Sprite(Globals.enemieTexture);
            spr.Pos = new Vec2(
                mState.X, mState.Y
            );
            Globals.world.AddSprite(
                spr
            );
            _timer = 0.1F;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        Globals.spriteBatch.Begin(
            samplerState: SamplerState.PointClamp,
            transformMatrix: Matrix.CreateScale(Globals.scaleFactor, Globals.scaleFactor, 1)
        );
        Globals.world.Draw(gameTime);
        Globals.spriteBatch.End();

        Globals.spriteBatch.Begin();
        // var debugText = $"Avg FPS: {Globals.frameCounter.AverageFramesPerSecond}\nBullets Count: {_bullets.Count}\nEnemies Count: {_enemies.Count}\nCollision Detection: {_collisionDetection}\nPlayer Velosity: {{X: {Math.Floor(player.Vel.X)}, Y: {Math.Floor(player.Vel.Y)}}}";
        // var debugText = $"Avg FPS: {Globals.frameCounter.AverageFramesPerSecond}\nCollision Detection: {_collisionDetection}";
        // Globals.spriteBatch.DrawString(
        //     spriteFont: Globals.hudFont,
        //     debugText,
        //     new Vector2(5, 5),
        //     Color.Black
        // );

        Globals.spriteBatch.End();


        base.Draw(gameTime);
    }

    public static Vector2 GetRandomInScreenPosition()
    {
        Random rnd = new Random();
        return new Vector2(
            rnd.Next(Globals.graphicsDevice.Viewport.Width),
            rnd.Next(Globals.graphicsDevice.Viewport.Height)
        );
    }
}
