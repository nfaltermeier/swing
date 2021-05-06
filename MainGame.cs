using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Swing.Engine;
using System;
using Microsoft.Xna.Framework.Audio;
using tainicom.Aether.Physics2D.Diagnostics;
using tainicom.Aether.Physics2D.Dynamics;
using Swing.Engine.StateManagement;
using Swing.Screens;
using Microsoft.Xna.Framework.Media;

namespace Swing
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        public static MainGame Instance { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }
        public int DisplayWidth => _graphics.PreferredBackBufferWidth;
        public int DisplayHeight => _graphics.PreferredBackBufferHeight;
        public World World { get; private set; }
        public Matrix PhysicsProjectionMatrix { get; private set; }
        public Matrix ViewMatrix { get; private set; }
        public Matrix StandardTransformMatrix { get; private set; }
        public DebugView DebugView { get; private set; }
        public ScreenManager ScreenManager { get; private set; }
        public int Deaths { get; set; }
        public TimeSpan RunTime { get; set; }

        /// <summary>
        /// Pixels are 1 / PhysicsScale meters
        /// </summary>
        public static readonly float PhysicsScale = 10;

        private GraphicsDeviceManager _graphics;

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
            SoundEffect.MasterVolume = 0.5f;
            MediaPlayer.Volume = 0.5f;

            Instance = this;
        }

        protected override void Initialize()
        {
            World = new World(Vector2.UnitY * -10 * PhysicsScale);
            DebugView = new DebugView(World);
            DebugView.DefaultShapeColor = Color.White;
            DebugView.SleepingShapeColor = Color.LightGray;
            DebugView.LoadContent(GraphicsDevice, Content);
            if (Debug.DISPLAY_COLLIDERS)
            {
                DebugView.AppendFlags(DebugViewFlags.DebugPanel);
            }

            ScreenManager = new ScreenManager(this, new GameScreen[] { new Background(), new MainMenu() });
            Components.Add(ScreenManager);

            PhysicsProjectionMatrix = Matrix.CreateOrthographicOffCenter(0, DisplayWidth, DisplayHeight, 0, 0, -100) *
                                Matrix.CreateScale(PhysicsScale, -PhysicsScale, 1) *
                                Matrix.CreateTranslation(PhysicsScale, PhysicsScale, 0);
            StandardTransformMatrix = Matrix.CreateTranslation(DisplayWidth / 2, -DisplayHeight / 2, 0) * Matrix.CreateScale(1, -1f, 1);
            ViewMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
