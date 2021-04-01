using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Swing.Engine.StateManagement;

namespace Swing.Engine
{
    public abstract class Actor : IDestroyable
    {
        public virtual Vector2 Position { get; set; }
        public bool IsDestroyed { get; private set; }
        public GameScreen Screen { get; set; }
        public virtual RenderType RenderType => RenderType.Standard;

        private List<Component> components;
        private bool contentLoaded = false;

        public Actor() : this(Vector2.Zero) { }

        public Actor(Vector2 position)
        {
            Position = position;
            components = new List<Component>();
            IsDestroyed = false;
        }

        #region Render methods
        /// <summary>
        /// Renders sprites vertically flipped so they appear correctly when rendered with the standard transform
        /// </summary>
        /// <param name="position">Where to draw the center of the sprite</param>
        /// <param name="sprite"></param>
        /// <param name="depth">1f front, 0f back</param>
        public static void RenderSprite(Vector2 position, Texture2D sprite, float depth)
        {
            MainGame.Instance.ScreenManager.SpriteBatch.Draw(sprite, position, null, Color.White, 0,
                new Vector2(sprite.Width / 2, sprite.Height / 2), 1, SpriteEffects.FlipVertically, depth);
        }

        /// <summary>
        /// Renders sprites vertically flipped so they appear correctly when rendered with the standard transform
        /// </summary>
        /// <param name="position">Where to draw the center of the sprite</param>
        /// <param name="sprite">The spritesheet</param>
        /// <param name="spriteWidth"></param>
        /// <param name="spriteHeight"></param>
        /// <param name="cellX">The x coordinate of the sprite to draw, where 0,0 is the top left of the sheet</param>
        /// <param name="cellY">The y coordinate of the sprite to draw, where 0,0 is the top left of the sheet</param>
        /// <param name="frame">The frame of the sprite animation to draw, assuming the frames are laid horizontally</param>
        /// <param name="depth">1f front, 0f back</param>
        public static void RenderSpriteFromSheet(Vector2 position, Texture2D sprite, int spriteWidth, int spriteHeight, int cellX, int cellY, int frame, float depth)
        {
            Rectangle r = new Rectangle((cellX + frame) * spriteWidth, cellY * spriteHeight, spriteWidth, spriteHeight);
            MainGame.Instance.ScreenManager.SpriteBatch.Draw(sprite, position, r, Color.White, 0,
                new Vector2(spriteWidth / 2, spriteHeight / 2), 1, SpriteEffects.FlipVertically, depth);
        }

        /// <summary>
        /// Renders sprites unflipped so they appear correctly when rendered with the UI
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="sprite"></param>
        /// <param name="color"></param>
        /// <param name="depth">1f front, 0f back</param>
        public static void RenderSpriteUI(Rectangle destination, Texture2D sprite, Color color, float depth)
        {
            MainGame.Instance.ScreenManager.SpriteBatch.Draw(sprite, destination, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Renders text where the top left is at the given position
        /// </summary>
        /// <param name="position">The top left of the text to render</param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        public static void RenderTextScreenspace(Vector2 position, string text, SpriteFont font)
        {
            RenderTextScreenspace(position, text, font, Color.White);
        }

        /// <summary>
        /// Renders text where the top left is at the given position
        /// </summary>
        /// <param name="position">The top left of the text to render</param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="color"></param>
        public static void RenderTextScreenspace(Vector2 position, string text, SpriteFont font, Color color)
        {
            MainGame.Instance.ScreenManager.SpriteBatch.DrawString(font, text, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, RenderOrder.Text);
        }

        /// <summary>
        /// Renders text where the center is at the given position
        /// </summary>
        /// <param name="position">The center of the text to render</param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        public static void RenderCenteredTextScreenspace(Vector2 position, string text, SpriteFont font)
        {
            Vector2 textSize = font.MeasureString(text);
            RenderTextScreenspace(position - (textSize / 2f), text, font, Color.White);
        }

        public static void RenderDebugTextScreenspace(Vector2 position, string text)
        {
            RenderTextScreenspace(position, text, MainGame.Instance.ScreenManager.DebugFont);
        }
        #endregion

        public Actor Instantiate(Actor a)
        {
            return Screen.Instantiate(a);
        }

        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in components)
            {
                if (component is T casted)
                    return casted;
            }

            return null;
        }

        public T[] GetComponents<T>() where T : Component
        {
            List<T> ret = new List<T>();
            foreach (Component component in components)
            {
                if (component is T casted)
                    ret.Add(casted);
            }

            return ret.ToArray();
        }

        public T AddComponent<T>(T component) where T : Component
        {
            components.Add(component);

            component.Start();

            if (contentLoaded)
                component.LoadContent(MainGame.Instance.Content);

            return component;
        }

        public void Destroy()
        {
            if (IsDestroyed)
                return;

            IsDestroyed = true;
            Screen.Destroy(this);

            foreach (Component c in components)
            {
                if(!c.IsDestroyed)
                    c.Destroy();
            }
        }

        internal void EngineStart()
        {
            if (Screen == null)
                Debug.LogError($"Screen was null in {this}");

            Start();
        }

        internal void EngineLoadContent(ContentManager content)
        {
            LoadContent(content);

            contentLoaded = true;
            foreach (Component c in components)
            {
                if (!c.IsDestroyed)
                    c.LoadContent(content);
            }
        }

        internal void EngineUpdate()
        {
            Update();

            foreach (Component c in components)
            {
                if (!c.IsDestroyed)
                    c.Update();
            }
        }

        internal void EngineFixedUpdate()
        {
            FixedUpdate();

            foreach (Component c in components)
            {
                if (!c.IsDestroyed)
                    c.FixedUpdate();
            }
        }

        internal void EngineDraw()
        {
            Draw();

            foreach (Component c in components)
            {
                if (!c.IsDestroyed)
                    c.Draw();
            }
        }

        #region Virtual methods
        protected virtual void Start()
        {

        }

        protected virtual void LoadContent(ContentManager content)
        {

        }

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void Update()
        {

        }

        protected virtual void Draw()
        {

        }

        public virtual void FinalDestroy()
        {
            components = null;
        }
        #endregion
    }

    public enum RenderType
    {
        Standard,
        UI,
        Custom
    }
}
