using Microsoft.Xna.Framework;
using Swing.Engine.StateManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Screens
{
    class MainGameScreen : GameScreen
    {
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(ScreenManager.DebugPixel, new Rectangle(10, 10, 10, 10), Color.White);
            ScreenManager.SpriteBatch.End();
        }
    }
}
