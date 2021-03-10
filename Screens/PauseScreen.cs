using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Swing.Actors;
using Swing.Engine;
using Swing.Engine.Actors.UI;
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
            float halfWidth = MainGame.Instance.DisplayWidth / 2f;

            Instantiate(new Overlay());
            Instantiate(new TextRenderer(new Vector2(halfWidth, 200), "Paused"));
            Instantiate(new Button(new Vector2(halfWidth, 300), "Resume")).Activated += Resume_Activated;
            Instantiate(new SettingsButton<PauseScreen>(new Vector2(halfWidth, 500)));
            Instantiate(new Button(new Vector2(halfWidth, 700), "Main Menu")).Activated += MainMenu_Activated;
            Instantiate(new PauseListener());
        }

        private void MainMenu_Activated()
        {
            ScreenManager.QueueAddScreen(new MainMenu());

            if (ScreenManager.GetScreen<MainGameScreen>() is MainGameScreen mgs)
            {
                ScreenManager.QueueRemoveScreen(mgs);
            }
            else
            {
                Debug.LogError("Could not find MainGameScreen from PauseScreen");
            }
            ExitScreen();
        }

        private void Resume_Activated()
        {
            ScreenManager.GetScreen<MainGameScreen>().Active = true;
            ExitScreen();
        }

        protected override void Activate()
        {
            base.Activate();

            Time.TimeScale = 0;
        }

        internal override void Unload()
        {
            base.Unload();

            Time.TimeScale = 1;
        }
    }
}
