using Microsoft.Xna.Framework;
using Swing.Engine.StateManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Screens
{
    class Background : GameScreen
    {
        // Make the background always draw
        public override void UpdateTransitions(bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.UpdateTransitions(otherScreenHasFocus, false);
        }

        public override void Draw()
        {
            ScreenManager.GraphicsDevice.Clear(Color.Black);

            base.Draw();
        }
    }
}
