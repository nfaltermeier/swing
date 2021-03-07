using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Swing.Engine;
using Swing.Engine.Components;
using System;
using Microsoft.Xna.Framework.Audio;
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

        private GraphicsDeviceManager _graphics;
        private FrameCounter _frameCounter = new FrameCounter();
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
            World = new World(Vector2.UnitY * -9);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            screenManager.AddScreen(new Background(), null);
            screenManager.AddScreen(new MainGameScreen(), null);

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
            /*SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _frameCounter.Draw(gameTime, SpriteBatch, DebugFont);
            SpriteBatch.End();*/

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
