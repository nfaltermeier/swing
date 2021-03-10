using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine.Actors.UI
{
    public class Button : Actor
    {
        private Texture2D background;
        private string text;
        private Vector2 worldPosition;
        private Rectangle screenBounds;

        public event Action Activated;

        public override Vector2 Position {
            get => base.Position;
            set
            {
                base.Position = value;
                worldPosition = value.ScreenToWorldspace();

                if (background != null)
                {
                    int halfWidth = background.Width / 2;
                    int halfHeight = background.Height / 2;
                    int iX = (int)Position.X;
                    int iY = (int)Position.Y;
                    screenBounds = new Rectangle(iX - halfWidth, iY - halfHeight, background.Width, background.Height);
                }
            }
        }

        /// <summary>
        /// Takes a screen space position
        /// </summary>
        /// <param name="position">a screen space position</param>
        public Button(Vector2 position, string text) : base(position)
        {
            this.text = text;
        }

        protected void OnActivate()
        {
            Activated?.Invoke();
        }

        protected override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            background = content.Load<Texture2D>("Engine/UI/ButtonBase");
            // Called to update the bounds
            Position = Position;
        }

        protected override void Update()
        {
            base.Update();

            if (InputManager.MousePressed && screenBounds.Contains(InputManager.MouseLocation))
                OnActivate();
        }

        protected override void Draw()
        {
            base.Draw();

            RenderSpriteScreenspace(screenBounds, background, Color.White);
            RenderCenteredTextScreenspace(Position, text, Screen.ScreenManager.DebugFont);

            if (Debug.DISPLAY_UI_RECTANGLES)
                Debug.DrawRectangleOutlineScreenspace(screenBounds);
        }
    }
}
