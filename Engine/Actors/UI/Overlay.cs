using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine.Actors.UI
{
    class Overlay : Actor
    {
        private Color color;
        private Rectangle screen;

        public Overlay() : this(new Color(0.5f, 0.5f, 0.5f, 0.25f)) { }

        public Overlay(Color color) : base(Vector2.Zero)
        {
            this.color = color;
            screen = new Rectangle(0, 0, MainGame.Instance.DisplayWidth, MainGame.Instance.DisplayHeight);
        }

        protected override void Draw()
        {
            base.Draw();

            RenderSpriteScreenspace(screen, Screen.ScreenManager.DebugPixel, color);
        }
    }
}
