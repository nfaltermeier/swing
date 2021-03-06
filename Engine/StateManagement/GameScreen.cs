using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Swing.Engine.StateManagement
{
    /// <summary>
    /// A screen is a single layer of game content that has
    /// its own update and draw logic and can be combined 
    /// with other layers to create complex menus or game
    /// experiences
    /// </summary>
    public abstract class GameScreen
    {
        /// <summary>
        /// Indicates if this screen is a popup
        /// </summary>
        /// <remarks>
        /// Normally when a new screen is brought over another, 
        /// the covered screen will transition off.  However, this
        /// bool indicates the covering screen is only a popup, and 
        /// the covered screen will remain partially visible
        /// </remarks>
        public bool IsPopup { get; protected set; }

        /// <summary>
        /// The amount of time taken for this screen to transition on
        /// </summary>
        protected TimeSpan TransitionOnTime { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// The amount of time taken for this screen to transition off
        /// </summary>
        protected TimeSpan TransitionOffTime { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// The screen's position in the transition
        /// </summary>
        /// <value>Ranges from 0 to 1 (fully on to fully off)</value>
        protected float TransitionPosition { get; set; } = 1;

        /// <summary>
        /// The alpha value based on the current transition position
        /// </summary>
        public float TransitionAlpha => 1f - TransitionPosition;

        /// <summary>
        /// The current state of the screen
        /// </summary>
        public ScreenState ScreenState { get; set; } = ScreenState.TransitionOn;

        /// <summary>
        /// Indicates the screen is exiting for good (not simply obscured)
        /// </summary>
        /// <remarks>
        /// There are two possible reasons why a screen might be transitioning
        /// off. It could be temporarily going away to make room for another
        /// screen that is on top of it, or it could be going away for good.
        /// This property indicates whether the screen is exiting for real:
        /// if set, the screen will automatically remove itself as soon as the
        /// transition finishes.
        /// </remarks>
        public bool IsExiting { get; protected internal set; }

        /// <summary>
        /// Indicates if this screen is active
        /// </summary>
        public bool IsActive => !_otherScreenHasFocus && (
            ScreenState == ScreenState.TransitionOn ||
            ScreenState == ScreenState.Active);

        private bool _otherScreenHasFocus;

        /// <summary>
        /// The ScreenManager in charge of this screen
        /// </summary>
        public ScreenManager ScreenManager { get; internal set; }

        /// <summary>
        /// Gets the index of the player who is currently controlling this screen,
        /// or null if it is accepting input from any player. 
        /// </summary>
        /// <remarks>
        /// This is used to lock the game to a specific player profile. The main menu 
        /// responds to input from any connected gamepad, but whichever player makes 
        /// a selection from this menu is given control over all subsequent screens, 
        /// so other gamepads are inactive until the controlling player returns to the 
        /// main menu.
        /// </remarks>
        public PlayerIndex? ControllingPlayer { protected get; set; }

        private bool _active = false;
        public bool Active {
            get => _active;
            set
            {
                _active = value;
                if (_active)
                    Activate();
                else
                    Deactivate();
            }
        }

        public bool IsDestroyed { get; private set; }

        private List<Actor> actors;
        private List<IDestroyable> toDestroy;
        private bool contentLoaded = false;

        public Vector2 CameraOffset { get; protected set; }

        public GameScreen()
        {
            actors = new List<Actor>();
            toDestroy = new List<IDestroyable>();
        }

        /// <summary>
        /// Activates the screen.  Called when the screen is added to the screen manager 
        /// or the game returns from being paused.
        /// </summary>
        protected virtual void Activate() { }

        public virtual void LoadContent(ContentManager content)
        {
            contentLoaded = true;
            foreach (Actor a in actors)
                a.EngineLoadContent(ScreenManager.Content);
        }

        /// <summary>
        /// Deactivates the screen.  Called when the screen is removed from the screen manager 
        /// or when the game is paused.
        /// </summary>
        protected virtual void Deactivate() { }

        /// <summary>
        /// Unloads content for the screen. Called when the screen is removed from the screen manager
        /// Should only be called by the screen manager.
        /// </summary>
        internal virtual void Unload() {
            if (IsDestroyed)
                return;

            IsDestroyed = true;
            foreach (Actor a in actors)
                a.Destroy();
        }

        public virtual void Update()
        {
            for (int i = 0; i < actors.Count; i++)
            {
                Actor actor = actors[i];
                if (!actor.IsDestroyed)
                    actor.EngineUpdate();
            }

            FinalDestroyThings();
        }

        /// <summary>
        /// Updates the screen. Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        public virtual void UpdateTransitions(bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _otherScreenHasFocus = otherScreenHasFocus;

            if (IsExiting)
            {
                // If the screen is going away forever, it should transition off
                ScreenState = ScreenState.TransitionOff;

                if (!UpdateTransitionPosition(TransitionOffTime, 1))
                    ScreenManager.QueueRemoveScreen(this);
            }
            else if (coveredByOtherScreen)
            {
                // if the screen is covered by another, it should transition off
                ScreenState = UpdateTransitionPosition(TransitionOffTime, 1)
                    ? ScreenState.TransitionOff
                    : ScreenState.Hidden;
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                ScreenState = UpdateTransitionPosition(TransitionOnTime, -1)
                    ? ScreenState.TransitionOn
                    : ScreenState.Active;
            }
        }

        public virtual void FixedUpdate()
        {
            for (int i = 0; i < actors.Count; i++)
            {
                Actor actor = actors[i];
                if (!actor.IsDestroyed)
                    actor.EngineFixedUpdate();
            }
        }

        private void FinalDestroyThings()
        {
            while (toDestroy.Count > 0)
            {
                IDestroyable d = toDestroy[0];

                d.FinalDestroy();

                toDestroy.RemoveAt(0);
                if (d is Actor a)
                {
                    actors.Remove(a);
                }
            }
        }

        /// <summary>
        /// Updates the TransitionPosition property based on the time
        /// </summary>
        /// <param name="time">The amount of time the transition should take</param>
        /// <param name="direction">The direction of the transition</param>
        /// <returns>true if still transitioning, false if the transition is done</returns>
        private bool UpdateTransitionPosition(TimeSpan time, int direction)
        {
            // How much should we move by?
            float transitionDelta = (time == TimeSpan.Zero)
                ? 1
                : (float)(Time.DeltaTime / time.TotalSeconds);

            // Update the transition time
            TransitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if (direction < 0 && TransitionPosition <= 0 || direction > 0 && TransitionPosition >= 0)
            {
                TransitionPosition = MathHelper.Clamp(TransitionPosition, 0, 1);
                return false;
            }

            // if not, we are still transitioning
            return true;
        }

        /// <summary>
        /// Handles input for this screen.  Only called when the screen is active,
        /// and not when another screen has taken focus.
        /// </summary>
        /// <param name="input">An object representing input</param>
        public virtual void HandleInput(InputState input) { }

        /// <summary>
        /// Draws the GameScreen.  Only called with the screen is active, and not 
        /// when another screen has taken the focus.
        /// </summary>
        public virtual void Draw()
        {
            List<Actor> uiElements = new List<Actor>();
            List<Actor> customElements = new List<Actor>();

            Matrix transform = GetStandardTransformWithCamera();

            ScreenManager.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack, transformMatrix: transform, rasterizerState: RasterizerState.CullClockwise);
            foreach (Actor actor in actors)
            {
                if (!actor.IsDestroyed)
                {
                    switch (actor.RenderType)
                    {
                        case RenderType.Standard:
                        default:
                            actor.EngineDraw();
                            break;
                        case RenderType.UI:
                            uiElements.Add(actor);
                            break;
                        case RenderType.Custom:
                            customElements.Add(actor);
                            break;
                    }
                }
            }
            ScreenManager.SpriteBatch.End();

            if (uiElements.Count > 0)
            {
                ScreenManager.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
                foreach (Actor actor in uiElements)
                {
                    actor.EngineDraw();
                }
                ScreenManager.SpriteBatch.End();
            }

            foreach (Actor actor in customElements)
            {
                actor.EngineDraw();
            }
        }

        public virtual void FinalDestory()
        {
            FinalDestroyThings();
        }

        /// <summary>
        /// This method tells the screen to exit, allowing it time to transition off
        /// </summary>
        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
                ScreenManager.QueueRemoveScreen(this);    // If the screen has a zero transition time, remove it immediately
            else
                IsExiting = true;    // Otherwise flag that it should transition off and then exit.
        }

        public T Instantiate<T>(T a) where T : Actor
        {
            actors.Add(a);

            a.Screen = this;

            a.EngineStart();

            if (contentLoaded)
                a.EngineLoadContent(ScreenManager.Content);

            return a;
        }

        public void Destroy(IDestroyable d)
        {
            toDestroy.Add(d);

            if (!d.IsDestroyed)
                d.Destroy();
        }

        public T GetActor<T>() where T : Actor
        {
            return actors.Find(a => a is T) as T;
        }

        public T[] GetActors<T>() where T : Actor
        {
            return actors.Where(a => a is T).Select(a => a as T).ToArray();
        }

        public Matrix GetStandardTransformWithCamera()
        {
            return MainGame.Instance.StandardTransformMatrix * GetCameraTranslation();
        }

        public Matrix GetCameraTranslation()
        {
            return Matrix.CreateTranslation(CameraOffset.X, -CameraOffset.Y, 0);
        }
    }
}
