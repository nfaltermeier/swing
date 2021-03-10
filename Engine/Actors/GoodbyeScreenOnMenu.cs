using Swing.Engine.StateManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine.Actors
{
    class GoodbyeScreenOnMenu<ScreenToGoTo> : Actor where ScreenToGoTo : GameScreen
    {
        protected override void Update()
        {
            base.Update();

            if (InputManager.MenuButton)
            {
                GameScreen gameScreen = Screen.ScreenManager.GetScreen<ScreenToGoTo>();
                if (gameScreen != null)
                {
                    Screen.ScreenManager.QueueActivateScreen(gameScreen);
                    Screen.ExitScreen();
                }
                else
                {
                    Debug.LogError($"Could not find a screen of {typeof(ScreenToGoTo)} from {Screen}");
                }
            }
        }
    }
}
