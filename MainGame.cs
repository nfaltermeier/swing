using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Swing.Engine;
using Swing.Engine.Components;
using System;
using Microsoft.Xna.Framework.Audio;
using tainicom.Aether.Physics2D.Diagnostics;
using tainicom.Aether.Physics2D.Dynamics;
using Swing.Engine.StateManagement;
using Swing.Screens;

namespace Swing
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        public static MainGame Instance { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }
        public int DisplayWidth => _graphics.PreferredBackBufferWidth;
        public int DisplayHeight => _graphics.PreferredBackBufferHeight;
        public World World { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }
        public Matrix ViewMatrix { get; private set; }
        public DebugView DebugView { get; private set; }

        private GraphicsDeviceManager _graphics;
        private ScreenManager screenManager;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = true;
            //Window.IsBorderless = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Swing";

            Instance = this;
        }

        protected override void Initialize()
        {
            World = new World(Vector2.UnitY * -90);
            DebugView = new DebugView(World);
            DebugView.DefaultShapeColor = Color.White;
            DebugView.SleepingShapeColor = Color.LightGray;
            DebugView.LoadContent(GraphicsDevice, Content);
            if (Debug.DISPLAY_COLLIDERS)
            {
                DebugView.AppendFlags(DebugViewFlags.DebugPanel);
            }

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            screenManager.AddScreen(new Background(), null);
            screenManager.AddScreen(new MainMenu(), null);

            ProjectionMatrix = Matrix.CreateOrthographicOffCenter(0, DisplayWidth, DisplayHeight, 0, 0, -100) *
                                Matrix.CreateScale(1, -1, 1) *
                                Matrix.CreateTranslation(1f, 1f, 0);
            ViewMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up);

            //SoundEffect.MasterVolume = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            if (InputManager.Exit)
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public Rectangle GetPlayBounds(int inset)
        {
            int displayLeft = 64 + inset;
            int displayRight = DisplayWidth - 64 - inset;
            int widthRange = displayRight - displayLeft;
            int displayTop = 56 + 64 + inset;
            int displayBottom = DisplayHeight - 64 - inset;
            int heightRange = displayBottom - displayTop;

            return new Rectangle(displayLeft, displayTop, widthRange, heightRange);
        }

        private Vector2 GetPositionAwayFromPoint(Vector2 point, float distanceFromPlayer)
        {
            Vector2 pos;
            do
            {
                Rectangle bounds = GetPlayBounds(32);
                // Prevent them from spawning on the border 5% of the screen
                pos.X = ((float)Utilities.random.NextDouble() * bounds.Width) + bounds.Left;
                pos.Y = ((float)Utilities.random.NextDouble() * bounds.Height) + bounds.Top;
                pos = pos.ScreenToWorldspace();
            } while (Vector2.Distance(point, pos) < distanceFromPlayer);

            return pos;
        }
    }
}
