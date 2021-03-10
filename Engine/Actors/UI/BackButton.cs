using Microsoft.Xna.Framework;
using Swing.Engine.StateManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine.Actors.UI
{
    public class BackButton<ScreenToGoTo> : Button where ScreenToGoTo : GameScreen
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
             if (Screen.ScreenManager.GetScreen<ScreenToGoTo>() is ScreenToGoTo backTo)
             {
                Screen.ScreenManager.QueueActivateScreen(backTo);
                Screen.ExitScreen();
            }
             else
             {
                Debug.LogError($"Could not find a screen of {typeof(ScreenToGoTo)} from {Screen}");
             }
        }
    }
}
