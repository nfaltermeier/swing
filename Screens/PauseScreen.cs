using Swing.Engine;
using Swing.Engine.StateManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Screens
{
    class PauseScreen : GameScreen
    {
        public PauseScreen()
        {
            
        }

        protected override void Activate()
        {
            base.Activate();

            Time.TimeScale = 0;
        }

        protected override void Deactivate()
        {
            base.Deactivate();

            Time.TimeScale = 1;
        }
    }
}
