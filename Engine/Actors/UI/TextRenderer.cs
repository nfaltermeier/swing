using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine.Actors.UI
{
    public class TextRenderer : Actor
    {
        protected string text;
        protected SpriteFont font;
        private Style style;

        public override RenderType RenderType => RenderType.UI;

        /// <summary>
        /// Takes a screen space position
        /// </summary>
        /// <param name="position">a screen space position</param>
        public TextRenderer(Vector2 position, string text) : this(position, text, Style.Medium) { }

        /// <summary>
        /// Takes a screen space position
        /// </summary>
        /// <param name="position">a screen space position</param>
        public TextRenderer(Vector2 position, string text, Style style) : base(position)
        {
            this.text = text;
            this.style = style;
        }

        protected override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            switch (style)
            {
                case Style.Medium:
                    font = content.Load<SpriteFont>("MediumFont");
                    break;
                case Style.Large:
                    font = content.Load<SpriteFont>("LargeFont");
                    break;
                case Style.Debug:
                    font = Screen.ScreenManager.DebugFont;
                    break;
            }
        }

        protected override void Draw()
        {
            base.Draw();

            RenderCenteredTextScreenspace(Position, text, font);
        }

        public enum Style
        {
            Medium,
            Large,
            Debug,
        }
    }

}
