using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine.Actors
{
    class QuitOnMenu : Actor
    {
        protected override void Update()
        {
            base.Update();

            if (InputManager.MenuButton)
                MainGame.Instance.Exit();
        }
    }
}
