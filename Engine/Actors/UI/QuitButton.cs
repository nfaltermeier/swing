using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine.Actors.UI
{
    class QuitButton : Button
    {
        /// <summary>
        /// Takes a screen space position
        /// </summary>
        /// <param name="position">a screen space position</param>
        public QuitButton(Vector2 position) : base(position, "Quit")
        {
            Activated += QuitButton_Activated;
        }

        private void QuitButton_Activated()
        {
            MainGame.Instance.Exit();
        }
    }
}
