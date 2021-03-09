using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine.Actors.UI
{
    class TextRenderer : Actor
    {
        private string text;
        /// <summary>
        /// Takes a screen space position
        /// </summary>
        /// <param name="position">a screen space position</param>
        public TextRenderer(Vector2 position, string text) : base(position)
        {
            this.text = text;
        }

        protected override void Draw()
        {
            base.Draw();

            RenderCenteredTextScreenspace(Position, text, Screen.ScreenManager.DebugFont);
        }
    }
}
