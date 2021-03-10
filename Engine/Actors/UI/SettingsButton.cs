using Microsoft.Xna.Framework;
using Swing.Engine.StateManagement;
using Swing.Screens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine.Actors.UI
{
    class SettingsButton<ScreenToGoBackTo> : Button where ScreenToGoBackTo : GameScreen
    {
        /// <summary>
        /// Takes a screen space position
        /// </summary>
        /// <param name="position">a screen space position</param>
        public SettingsButton(Vector2 position) : base(position, "Settings")
        {
            Activated += SettingsButton_Activated;
        }

        private void SettingsButton_Activated()
        {
            bool showOverlay = typeof(ScreenToGoBackTo) == typeof(PauseScreen);
            Screen.ScreenManager.QueueAddScreen(new Settings<ScreenToGoBackTo>(showOverlay));
            Screen.Active = false;
        }
    }
}
