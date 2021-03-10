using Swing.Engine;
using Swing.Screens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Actors
{
    class PauseListener : Actor
    {
        protected override void Update()
        {
            base.Update();

            if (InputManager.MenuButton)
            {
                if (Screen is PauseScreen ps)
                {
                    ps.ExitScreen();
                    if (Screen.ScreenManager.GetScreen<MainGameScreen>() is MainGameScreen mgs)
                    {
                        Screen.ScreenManager.QueueActivateScreen(mgs);
                    }
                    else
                    {
                        Debug.LogError($"Could not find MainGameScreen from {Screen}");
                    }
                }
                else
                {
                    Screen.Active = false;
                    Screen.ScreenManager.QueueAddScreen(new PauseScreen());
                }
            }
        }
    }
}
