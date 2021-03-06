using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Swing.Screens;
using tainicom.Aether.Physics2D.Dynamics;

namespace Swing.Engine.StateManagement
{
    /// <summary>
    /// The ScreenManager is a component which manages one or more GameScreen instance.
    /// It maintains a stack of screens, calls their Update and Draw methods when 
    /// appropriate, and automatically routes input to the topmost screen.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        private readonly List<GameScreen> _screens = new List<GameScreen>();
        private readonly List<GameScreen> _tmpScreensList = new List<GameScreen>();
        private readonly List<GameScreen> screensToDestroy = new List<GameScreen>();
        private readonly List<GameScreen> screensToDestroyAfterDraw = new List<GameScreen>();
        private readonly List<(GameScreen, bool)> screensToChangeActiveAfterDraw = new List<(GameScreen, bool)>();
        private readonly List<GameScreen> screensToAddAfterDraw = new List<GameScreen>();
        private readonly List<Action> postPhysicsStepActions = new List<Action>();

        private readonly InputState _input = new InputState();

        /// <summary>
        /// A SpriteBatch shared by all GameScreens
        /// </summary>
        public SpriteBatch SpriteBatch { get; private set; }

        public SpriteFont DebugFont { get; private set; }
        public Texture2D DebugPixel { get; private set; }
        public ContentManager Content { get; private set; }

        private bool contentLoaded = false;
        private float timeSinceFixedUpdate = 0;
        private FrameCounter _frameCounter = new FrameCounter();

        /// <summary>
        /// Constructs a new ScreenManager
        /// </summary>
        /// <param name="game">The game this ScreenManager belongs to</param>
        public ScreenManager(MainGame game, GameScreen[] initialScreens) : base(game)
        {
            Content = new ContentManager(game.Services, "Content");
            foreach (GameScreen screen in initialScreens)
            {
                AddScreen(screen, null);
            }
        }

        /// <summary>
        /// Initializes the ScreenManager
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Loads content for the ScreenManager and its screens
        /// </summary>
        protected override void LoadContent()
        {
            contentLoaded = true;
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            DebugFont = Content.Load<SpriteFont>("Engine/debug");
            DebugPixel = Content.Load<Texture2D>("Engine/1x1WhitePixel");

            // Tell each of the screens to load thier content 
            foreach (var screen in _screens)
            {
                screen.LoadContent(Content);
            }
        }

        /// <summary>
        /// Unloads content for the ScreenManager's screens
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (var screen in _screens)
            {
                screen.Unload();
            }
        }

        /// <summary>
        /// Updates all screens managed by the ScreenManager
        /// </summary>
        /// <param name="gameTime">An object representing time in the game</param>
        public override void Update(GameTime gameTime)
        {
            Time.UpdateTime(gameTime);
            InputManager.Update();

            // Read in the keyboard and gamepad
            _input.Update();

            timeSinceFixedUpdate += Time.UpdateDeltaTime;
            Time.IsInFixedUpdate = true;
            while (timeSinceFixedUpdate > Time.FixedDeltaTime)
            {
                timeSinceFixedUpdate -= Time.FixedDeltaTime;

                _tmpScreensList.Clear();
                _tmpScreensList.AddRange(_screens);

                while (_tmpScreensList.Count > 0)
                {
                    // Pop the topmost screen 
                    var screen = _tmpScreensList[_tmpScreensList.Count - 1];
                    _tmpScreensList.RemoveAt(_tmpScreensList.Count - 1);

                    if (screen.Active)
                        screen.FixedUpdate();
                }

                MainGame.Instance.World.Step(Time.DeltaTime);

                while (postPhysicsStepActions.Count > 0)
                {
                    postPhysicsStepActions[0]();
                    postPhysicsStepActions.RemoveAt(0);
                }
            }
            Time.IsInFixedUpdate = false;


            // Make a copy of the screen list, to avoid confusion if 
            // the process of updating a screen adds or removes others
            _tmpScreensList.Clear();
            _tmpScreensList.AddRange(_screens);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            while (_tmpScreensList.Count > 0)
            {
                // Pop the topmost screen 
                var screen = _tmpScreensList[_tmpScreensList.Count - 1];
                _tmpScreensList.RemoveAt(_tmpScreensList.Count - 1);

                screen.UpdateTransitions(otherScreenHasFocus, coveredByOtherScreen);

                if (screen.Active)
                {
                    screen.Update();

                    if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                    {
                        // if this is the first active screen, let it handle input 
                        if (!otherScreenHasFocus)
                        {
                            screen.HandleInput(_input);
                            otherScreenHasFocus = true;
                        }

                        // if this is an active non-popup, all subsequent 
                        // screens are covered 
                        if (!screen.IsPopup) coveredByOtherScreen = true;
                    }
                }
            }

            while (screensToDestroy.Count > 0)
            {
                var screen = screensToDestroy[0];
                screensToDestroy.RemoveAt(0);
                screen.FinalDestory();
            }
        }

        /// <summary>
        /// Draws the appropriate screens managed by the SceneManager
        /// </summary>
        /// <param name="gameTime">An object representing time in the game</param>
        public override void Draw(GameTime gameTime)
        {
            MainGameScreen mgs = null;
            foreach (var screen in _screens)
            {
                if (screen is MainGameScreen main)
                    mgs = main;
                if (screen.ScreenState == ScreenState.Hidden) continue;

                screen.Draw();
            }

            /*SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _frameCounter.Draw(gameTime, SpriteBatch, DebugFont);
            SpriteBatch.End();*/

            while (screensToDestroyAfterDraw.Count > 0)
            {
                RemoveScreen(screensToDestroyAfterDraw[0]);
                screensToDestroyAfterDraw.RemoveAt(0);
            }

            while (screensToDestroy.Count > 0)
            {
                screensToDestroy[0].FinalDestory();
                screensToDestroy.RemoveAt(0);
            }

            while(screensToChangeActiveAfterDraw.Count > 0)
            {
                (GameScreen screen, bool enable) = screensToChangeActiveAfterDraw[0];
                screen.Active = enable;
                screensToChangeActiveAfterDraw.RemoveAt(0);
            }

            while (screensToAddAfterDraw.Count > 0)
            {
                AddScreen(screensToAddAfterDraw[0], null);
                screensToAddAfterDraw.RemoveAt(0);
            }

            if (Debug.DISPLAY_COLLIDERS)
            {
                Matrix proj = MainGame.Instance.PhysicsProjectionMatrix;
                Matrix view = MainGame.Instance.ViewMatrix;
                if (mgs != null)
                {
                    proj = Matrix.CreateTranslation(mgs.CameraOffset.X / MainGame.PhysicsScale, mgs.CameraOffset.Y / MainGame.PhysicsScale, 0) * proj;
                }

                MainGame.Instance.DebugView.RenderDebugData(proj, view);
            }


            if (Debug.DISPLAY_PLAYER_TOUCHING_COLLIDERS)
            {
                if (Debug.playerTouchingColliders.Count > 0)
                {
                    MainGame.Instance.DebugView.BeginCustomDraw(MainGame.Instance.PhysicsProjectionMatrix, MainGame.Instance.ViewMatrix);

                    Dictionary<Fixture, bool>.KeyCollection fixtures = Debug.playerTouchingColliders.Keys;
                    foreach (Fixture f in fixtures)
                    {
                        MainGame.Instance.DebugView.DrawShape(f, f.Body.GetTransform(), Color.White);
                    }

                    MainGame.Instance.DebugView.EndCustomDraw();
                }
            }
        }

        /// <summary>
        /// Adds a screen to the ScreenManager
        /// </summary>
        /// <param name="screen">The screen to add</param>
        private void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer)
        {
            screen.ControllingPlayer = controllingPlayer;
            screen.ScreenManager = this;
            screen.IsExiting = false;

            screen.Active = true;

            if (contentLoaded)
                screen.LoadContent(Content);

            _screens.Add(screen);
        }

        private void RemoveScreen(GameScreen screen)
        {
            if (screen.IsDestroyed)
                return;

            screen.Active = false;
            screen.Unload();
            screensToDestroy.Add(screen);

            _screens.Remove(screen);
            _tmpScreensList.Remove(screen);
        }

        /// <summary>
        /// Exposes an array holding all the screens managed by the ScreenManager
        /// </summary>
        /// <returns>An array containing references to all current screens</returns>
        public GameScreen[] GetScreens()
        {
            return _screens.ToArray();
        }

        public T GetScreen<T>() where T : GameScreen
        {
            return _screens.Find(a => a is T) as T;
        }

        // Helper draws a translucent black fullscreen sprite, used for fading
        // screens in and out, and for darkening the background behind popups.
        public void FadeBackBufferToBlack(float alpha)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(DebugPixel, GraphicsDevice.Viewport.Bounds, Color.Black * alpha);
            SpriteBatch.End();
        }

        public void Deactivate()
        {
        }

        public bool Activate()
        {
            return false;
        }

        public void QueueActivateScreen(GameScreen screen)
        {
            screensToChangeActiveAfterDraw.Add((screen, true));
        }

        public void QueueDeactivateScreen(GameScreen screen)
        {
            screensToChangeActiveAfterDraw.Add((screen, false));
        }

        public void QueueRemoveScreen(GameScreen screen)
        {
            screensToDestroyAfterDraw.Add(screen);
        }

        public void QueueAddScreen(GameScreen screen)
        {
            screensToAddAfterDraw.Add(screen);
        }

        public void AddPostPhysicsStepAction(Action a)
        {
            postPhysicsStepActions.Add(a);
        }
    }
}
