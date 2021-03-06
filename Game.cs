using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Swing.Engine;
using Swing.Engine.Components;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace Swing
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static Game Instance { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }
        public int DisplayWidth => _graphics.PreferredBackBufferWidth;
        public int DisplayHeight => _graphics.PreferredBackBufferHeight;
        public SpriteFont DebugFont { get; private set; }
        public Texture2D DebugPixel { get; private set; }

        private GraphicsDeviceManager _graphics;
        private List<Actor> actors;
        private List<IDestroyable> toDestroy;
        private List<RectangleCollider> colliders;
        private bool contentLoaded = false;
        private float timeSinceFixedUpdate = 0;
        private FrameCounter _frameCounter = new FrameCounter();

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = true;
            //Window.IsBorderless = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Bannanarang";

            Instance = this;
        }

        protected override void Initialize()
        {
            actors = new List<Actor>();
            toDestroy = new List<IDestroyable>();
            colliders = new List<RectangleCollider>();

            //SoundEffect.MasterVolume = 0;

            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            DebugFont = Content.Load<SpriteFont>("debug");
            DebugPixel = Content.Load<Texture2D>("1x1WhitePixel");

            foreach (Actor actor in actors)
                actor.EngineLoadContent(Content);

            contentLoaded = true;
        }

        protected override void Update(GameTime gameTime)
        {
            Time.UpdateTime(gameTime);
            InputManager.Update();

            if (InputManager.Exit)
                Exit();

            Time.IsInFixedUpdate = true;
            timeSinceFixedUpdate += Time.UpdateDeltaTime;
            while (timeSinceFixedUpdate > Time.FixedDeltaTime)
            {
                timeSinceFixedUpdate -= Time.FixedDeltaTime;
                for (int i = 0; i < actors.Count; i++)
                {
                    Actor actor = actors[i];
                    if (!actor.IsDestroyed)
                        actor.EngineFixedUpdate();
                }

                CollisionHelper.CheckCollision(colliders);
            }
            Time.IsInFixedUpdate = false;

            for (int i = 0; i < actors.Count; i++)
            {
                Actor actor = actors[i];
                if (!actor.IsDestroyed)
                    actor.EngineUpdate();
            }

            while (toDestroy.Count > 0)
            {
                IDestroyable d = toDestroy[0];

                d.FinalDestroy();

                toDestroy.RemoveAt(0);
                if (d is Actor a)
                {
                    actors.Remove(a);
                } 
                else if (d is RectangleCollider rc)
                {
                    colliders.Remove(rc);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //_frameCounter.Draw(gameTime, SpriteBatch, DebugFont);

            foreach (Actor actor in actors)
            {
                if (!actor.IsDestroyed)
                    actor.EngineDraw();
            }
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        public Actor Instantiate(Actor a)
        {
            actors.Add(a);

            a.Start();

            if (contentLoaded)
                a.EngineLoadContent(Content);

            return a;
        }

        public void Destroy(IDestroyable d)
        {
            toDestroy.Add(d);

            if (!d.IsDestroyed)
                d.Destroy();
        }

        public void AddCollider(RectangleCollider collider)
        {
            colliders.Add(collider);
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

        public T GetActor<T>() where T : Actor
        {
            return actors.Find(a => a is T) as T;
        }

        public T[] GetActors<T>() where T : Actor
        {
            return actors.Where(a => a is T).Select(a => a as T).ToArray();
        }
    }
}
