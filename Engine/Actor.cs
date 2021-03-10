using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Swing.Engine.Components;
using Swing.Engine.StateManagement;

namespace Swing.Engine
{
    public abstract class Actor : IDestroyable
    {
        public virtual Vector2 Position { get; set; }
        public bool IsDestroyed { get; private set; }
        public GameScreen Screen { get; set; }

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
        public static void RenderSprite(Vector2 position, Texture2D sprite)
        {
            RenderSpriteScreenspace(position.WorldToScreenspace(), sprite);
        }

        public static void RenderSpriteScreenspace(Vector2 position, Texture2D sprite)
        {
            MainGame.Instance.ScreenManager.SpriteBatch.Draw(sprite, position, null, Color.White, 0,
                new Vector2(sprite.Width / 2, sprite.Height / 2), 1, SpriteEffects.None, 0);
        }

        public static void RenderSprite(Rectangle destination, Texture2D sprite, Color color, float depth = 0f)
        {
            RenderSpriteScreenspace(destination.WorldToScreenspace(), sprite, color, depth);
        }

        public static void RenderSpriteScreenspace(Rectangle destination, Texture2D sprite, Color color, float depth = 0f)
        {
            MainGame.Instance.ScreenManager.SpriteBatch.Draw(sprite, destination, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        public static void RenderSpriteFromSheet(Vector2 position, Texture2D sprite, int spriteWidth, int spriteHeight, int cellX, int cellY, int frame)
        {
            Rectangle r = new Rectangle((cellX + frame) * spriteWidth, cellY * spriteHeight, spriteWidth, spriteHeight);
            MainGame.Instance.ScreenManager.SpriteBatch.Draw(sprite, position.WorldToScreenspace(), r, Color.White, 0,
                new Vector2(spriteWidth / 2, spriteHeight / 2), 1, SpriteEffects.None, 0);
        }

        public static void RenderTextScreenspace(Vector2 position, string text, SpriteFont font)
        {
            RenderTextScreenspace(position, text, font, Color.White);
        }

        public static void RenderTextScreenspace(Vector2 position, string text, SpriteFont font, Color color)
        {
            MainGame.Instance.ScreenManager.SpriteBatch.DrawString(font, text, position, color);
        }

        public static void RenderCenteredTextScreenspace(Vector2 position, string text, SpriteFont font)
        {
            Vector2 textSize = font.MeasureString(text);
            RenderTextScreenspace(position - (textSize / 2f), text, font, Color.White);
        }

        public static void RenderTextWorldspace(Vector2 position, string text, SpriteFont font)
        {
            RenderTextScreenspace(position.WorldToScreenspace(), text, font);
        }
        public static void RenderCenteredTextWorldspace(Vector2 position, string text, SpriteFont font)
        {
            Vector2 textSize = font.MeasureString(text);
            RenderTextWorldspace(position - (textSize / 2f), text, font);
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
}
