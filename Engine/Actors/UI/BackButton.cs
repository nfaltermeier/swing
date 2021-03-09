using Microsoft.Xna.Framework;
using Swing.Engine.StateManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine.Actors.UI
{
    public class BackButton<T> : Button where T : GameScreen
    {
        /// <summary>
        /// Takes a screen space position
        /// </summary>
        /// <param name="position">a screen space position</param>
        public BackButton(Vector2 position) : base(position, "Back")
        {
            Activated += BackButton_Activated;
        }

        private void BackButton_Activated()
        {
             if (Screen.ScreenManager.GetScreen<T>() is T backTo)
             {
                backTo.Active = true;
                Screen.ExitScreen();
             }
        }
    }
}
